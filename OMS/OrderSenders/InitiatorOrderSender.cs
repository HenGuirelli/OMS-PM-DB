using QuickFix;
using QuickFix.Fields;
using QuickFix.Transport;

namespace OMS.OrderSenders
{
    internal class InitiatorOrderSender : MessageCracker, IApplication, IOrderSender
    {
        private readonly SocketInitiator _initiator;
        private readonly OrderProcess _orderProcess;
        private SessionID? _theOnlySession;

        public InitiatorOrderSender(OrderProcess orderProcess)
        {
            SessionSettings? settings;
            if (Environment.OSVersion.Platform.ToString().StartsWith("Win"))
            {
                settings = new SessionSettings("quickfix.win.cfg");
            }
            else
            {
                settings = new SessionSettings("quickfix.linux.cfg");
            }

            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            //IMessageStoreFactory storeFactory = new PmFileStoreFactory(settings);
            var logFactory = new FileLogFactory(settings);
            _initiator = new SocketInitiator(this, storeFactory, settings, logFactory);
            _orderProcess = orderProcess;
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

            _orderProcess.Process(er);
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
