using QuickFix;

namespace DropcopyGenerator
{
    public class Acceptor : MessageCracker, IApplication, IDisposable
    {
        private readonly ThreadedSocketAcceptor _acceptor;
        private readonly OrderSender _orderSender = new OrderSender();
        private volatile int _ordersSended = 0;
        private const int MaxOrdersSendAtSameTime = 1;
        private Thread _managerThread;

        public Acceptor()
        {
            var settings = new SessionSettings("quickfix.cfg");
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);

            ILogFactory logFactory = new FileLogFactory(settings);
            _acceptor =
                new ThreadedSocketAcceptor(this, storeFactory, settings, logFactory);
            _acceptor.Start();
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
        }

        public void FromApp(Message message, SessionID sessionID)
        {
        }

        public void OnCreate(SessionID sessionID)
        {
        }

        public void OnLogon(SessionID sessionID)
        {
            Console.WriteLine("Logon " + sessionID);
            _managerThread = new Thread(() =>
            {
                while (true)
                {
                    _orderSender.Start(Session.LookupSession(sessionID));
                }
            });
            _managerThread.Name = "Manager thread";
            _managerThread.Start();
        }

        public void OnLogout(SessionID sessionID)
        {
            Console.WriteLine("Logout " + sessionID);
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
        }

        public void ToApp(Message message, SessionID sessionId)
        {
        }

        public void Dispose()
        {
            _acceptor.Dispose();
        }
    }
}