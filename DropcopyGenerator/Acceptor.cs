using PM.Configs;
using QuickFix;

namespace DropcopyGenerator
{
    public class Acceptor : MessageCracker, IApplication, IDisposable
    {
        private readonly ThreadedSocketAcceptor _acceptor;
        private readonly OrderSender _orderSender = new OrderSender();
        private Thread? _managerThread;
        private volatile bool _managerThreadRunning = false;

        public Acceptor()
        {
            var settings = new SessionSettings("quickfix.cfg");

            PmGlobalConfiguration.PmInternalsFolder = "/mnt/nvram1/henguirelli";
            string os = Environment.OSVersion.Platform.ToString();

            if (os.StartsWith("Win"))
            {
                Console.WriteLine("Ambiente Windows, usando arquivos mapeados em memória tradicionais");
                PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.TraditionalMemoryMappedFile;
            }
            else
            {
                Console.WriteLine("Ambiente Linux, usando PM");
                PmGlobalConfiguration.PmTarget = PM.Core.PmTargets.PM;
            }

            //IMessageStoreFactory storeFactory = new PmFileStoreFactory(settings);
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
            _managerThreadRunning = true;
            _managerThread = new Thread(() =>
            {
                while (_managerThreadRunning)
                {
                    if (!_orderSender.Start(Session.LookupSession(sessionID)))
                    {
                        Thread.Sleep(5000);
                    }
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
            _managerThreadRunning = false;
        }
    }
}