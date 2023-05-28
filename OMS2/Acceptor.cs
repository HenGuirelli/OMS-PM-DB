using QuickFix;
using QuickFix.Fields;

namespace OMS
{
    internal class Acceptor : MessageCracker, IApplication
    {
        //private const string HttpServerPrefix = "http://127.0.0.1:5080/";
        private Initiator _orderSender = new();

        public void Start()
        {
            SessionSettings settings = new SessionSettings("quickfix.cfg");
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);

            ILogFactory logFactory = new FileLogFactory(settings);
            ThreadedSocketAcceptor acceptor = new ThreadedSocketAcceptor(this, storeFactory, settings, logFactory);
            //HttpServer srv = new HttpServer(HttpServerPrefix, settings);

            _orderSender.Start();
            acceptor.Start();
            //srv.Start();
        }

        public void OnMessage(QuickFix.FIX44.NewOrderSingle n, SessionID s)
        {
            var order = new Order
            {
                Account = n.Account.getValue(),
                Symbol = n.Symbol.getValue(),
                Quantity = n.OrderQty.getValue(),
                Price = n.Price.getValue(),
                ClOrdId = n.ClOrdID.getValue(),
            };

            OrderRepository.Add(order);
            _orderSender.SendOrder(order);
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("IN:  " + message);
            Crack(message, sessionID);
        }

        public void OnCreate(SessionID sessionID)
        {
        }

        public void OnLogon(SessionID sessionID)
        {
        }

        public void OnLogout(SessionID sessionID)
        {
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
        }

        public void ToApp(Message message, SessionID sessionID)
        {
        }

    }
}
