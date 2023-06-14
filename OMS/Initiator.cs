using OMS.Repositories;
using QuickFix;
using QuickFix.Fields;
using QuickFix.Transport;

namespace OMS
{
    internal class Initiator : MessageCracker, IApplication
    {
        private readonly ExecutionReportCounter _executionReportCounter = new();
        private readonly SocketInitiator _initiator;
        private readonly IOrderRepository _orderRepository;
        private SessionID? _theOnlySession;

        public Initiator(IOrderRepository orderRepository)
        {
            var _settings = new SessionSettings("quickfix.cfg");
            IMessageStoreFactory storeFactory = new FileStoreFactory(_settings);
            //IMessageStoreFactory storeFactory = new PmFileStoreFactory(_settings);
            var logFactory = new FileLogFactory(_settings);
            _initiator = new SocketInitiator(this, storeFactory, _settings, logFactory);
            _orderRepository = orderRepository;
        }

        public void Start()
        {
            _initiator.Start();
            _theOnlySession = _initiator.GetSessionIDs().Single();
        }

        public void Stop()
        {
            _initiator.Stop();
        }

        public void SendOrder(Order order)
        {
            var newOrderSingle = new QuickFix.FIX44.NewOrderSingle
            {
                Account = new Account(order.Account),
                OrderQty = new OrderQty(order.Quantity),
                Symbol = new Symbol(order.Symbol),
                ClOrdID = new ClOrdID(order.ClOrdId),
                OrdType = new OrdType('2'),
                Price = new Price(order.Price),
            };

            Session.SendToTarget(newOrderSingle, _theOnlySession);
        }

        #region quickfix callbacks
        public void FromAdmin(Message message, SessionID sessionID)
        {
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            Crack(message, sessionID);
        }

        public void OnCreate(SessionID sessionID)
        {
        }

        public void OnLogon(SessionID sessionID)
        {
            Console.WriteLine("Logon " + sessionID);
        }

        public void OnLogout(SessionID sessionID)
        {
            Console.WriteLine("Logout " + sessionID);
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
        }

        public void ToApp(Message message, SessionID sessionID)
        {
        }
        #endregion

        public void OnMessage(QuickFix.FIX44.ExecutionReport n, SessionID s)
        {
            _executionReportCounter.AddEr();

            var er = new ExecutionReport
            {
                Account = n.Account.getValue(),
                ClOrdId = n.ClOrdID.getValue(),
                CumQty = n.CumQty.getValue(),
                LastQty = n.LastQty.getValue(),
                OrderId = n.OrderID.getValue(),
                Quantity = n.OrderQty.getValue(),
                ExecType = GetExecType(n.ExecType.getValue()),
                Price = n.Price.getValue(),
                Side = n.Side.getValue(),
                Symbol = n.Symbol.getValue(),
                Status = int.Parse(n.OrdStatus.getValue().ToString()),
            };

            var order = new Order
            {
                OrderId = er.OrderId,
                Account = er.Account,
                ClOrdId = er.ClOrdId,
                Quantity = er.Quantity,
                ExecutedQuantity = er.CumQty,
                Price = er.Price,
                Status = er.Status,
                Symbol = er.Symbol,
            };

            if (er.Status == 0) // NEW
            {
                _orderRepository.AddOrder(order);
            }
            else
            {
                _orderRepository.UpdateOrder(order);
            }
        }

        private string GetExecType(char execType)
        {
            if (execType == ExecType.NEW)
            {
                return "NEW";
            }

            if (execType == ExecType.TRADE)
            {
                return "TRADE";
            }

            return null;
        }
    }
}
