using System;
using System.Collections.Generic;
using System.Text;
using PM;
using PM.Collections;
using QuickFix.Util;

namespace QuickFix
{
    public class PersistentSession
    {
        public virtual string CreationTime { get; set; }
    }

    public class PersistentSeqnuns
    {
        public virtual int NextSenderMsgSeqNum { get; set; }
        public virtual int NextTargetMsgSeqNum { get; set; }
    }

    public class PersistentMessage
    {
        public virtual string Message { get; set; }
    }

    /// <summary>
    /// Persistent memory file store implementation
    /// </summary>
    public class PmFileStore : IMessageStore
    {
        private readonly PersistentSession _persistentSession;
        private readonly PersistentSeqnuns _persistentSeqnuns;
        private readonly PmList<PersistentMessage> _persistentMessages;
        private readonly IPersistentFactory _persistentFactory = new PersistentFactory();

        private MemoryStore cache_ = new MemoryStore();

        public static string Prefix(SessionID sessionID)
        {
            System.Text.StringBuilder prefix = new System.Text.StringBuilder(sessionID.BeginString)
                .Append('-').Append(sessionID.SenderCompID);
            if (SessionID.IsSet(sessionID.SenderSubID))
                prefix.Append('_').Append(sessionID.SenderSubID);
            if (SessionID.IsSet(sessionID.SenderLocationID))
                prefix.Append('_').Append(sessionID.SenderLocationID);
            prefix.Append('-').Append(sessionID.TargetCompID);
            if (SessionID.IsSet(sessionID.TargetSubID))
                prefix.Append('_').Append(sessionID.TargetSubID);
            if (SessionID.IsSet(sessionID.TargetLocationID))
                prefix.Append('_').Append(sessionID.TargetLocationID);

            if (SessionID.IsSet(sessionID.SessionQualifier))
                prefix.Append('-').Append(sessionID.SessionQualifier);

            return prefix.ToString();
        }

        public PmFileStore(string path, SessionID sessionID)
        {
            string prefix = Prefix(sessionID);

            var seqNumsFileName = System.IO.Path.Combine(path, prefix + ".seqnums");
            var msgFileName = System.IO.Path.Combine(path, prefix + ".body");
            var sessionFileName = System.IO.Path.Combine(path, prefix + ".session");

            _persistentMessages = new PmList<PersistentMessage>(msgFileName);
            _persistentSession = _persistentFactory.CreateRootObject<PersistentSession>(sessionFileName);
            _persistentSeqnuns = _persistentFactory.CreateRootObject<PersistentSeqnuns>(seqNumsFileName);

            Open();
        }

        private void Open()
        {
            ConstructFromFileCache();
            InitializeSessionCreateTime();
        }

        private void PurgeFileCache()
        {
            _persistentMessages.Clear();
        }

        private void ConstructFromFileCache()
        {
            if (_persistentSeqnuns.NextSenderMsgSeqNum != 0)
                cache_.NextSenderMsgSeqNum = _persistentSeqnuns.NextSenderMsgSeqNum;
            if (_persistentSeqnuns.NextTargetMsgSeqNum != 0)
                cache_.NextTargetMsgSeqNum = _persistentSeqnuns.NextTargetMsgSeqNum;
        }

        private void InitializeSessionCreateTime()
        {
            if (_persistentSession.CreationTime != null)
            {
                cache_.CreationTime = UtcDateTimeSerializer.FromString(_persistentSession.CreationTime);
            }
            else
            {
                _persistentSession.CreationTime = UtcDateTimeSerializer.ToString(cache_.CreationTime.Value);
            }
        }


        #region MessageStore Members

        /// <summary>
        /// Get messages within the range of sequence numbers
        /// </summary>
        /// <param name="startSeqNum"></param>
        /// <param name="endSeqNum"></param>
        /// <param name="messages"></param>
        public void Get(int startSeqNum, int endSeqNum, List<string> messages)
        {
            for (int i = startSeqNum; i <= endSeqNum; i++)
            {
                messages.Add(_persistentMessages[i - 1].Message);
            }
        }

        /// <summary>
        /// Store a message
        /// </summary>
        /// <param name="msgSeqNum"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Set(int msgSeqNum, string msg)
        {
            if (_persistentMessages.Count == msgSeqNum - 1)
            {
                _persistentMessages.AddPersistent(new PersistentMessage { Message = msg });
                return true;
            }
            return false;
        }

        public int NextSenderMsgSeqNum
        {
            get
            {
                return cache_.NextSenderMsgSeqNum;
            }
            set
            {
                cache_.NextSenderMsgSeqNum = value;
                _persistentSeqnuns.NextSenderMsgSeqNum = value;
            }
        }

        public int NextTargetMsgSeqNum
        {
            get
            {
                return cache_.NextTargetMsgSeqNum;
            }
            set
            {
                cache_.NextTargetMsgSeqNum = value;
                _persistentSeqnuns.NextTargetMsgSeqNum = value;
            }
        }

        public void IncrNextSenderMsgSeqNum()
        {
            cache_.IncrNextSenderMsgSeqNum();
            NextSenderMsgSeqNum = cache_.NextSenderMsgSeqNum;
        }

        public void IncrNextTargetMsgSeqNum()
        {
            cache_.IncrNextTargetMsgSeqNum();
            NextTargetMsgSeqNum = cache_.NextTargetMsgSeqNum;
        }

        public DateTime? CreationTime
        {
            get
            {
                return cache_.CreationTime;
            }
        }

        public void Reset()
        {
            cache_.Reset();
            PurgeFileCache();
            Open();
        }

        public void Refresh()
        {
            cache_.Reset();
            Open();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                // close();
            }
            _disposed = true;
        }

        ~PmFileStore() => Dispose(false);
        #endregion
    }
}
