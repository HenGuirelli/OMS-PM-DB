namespace QuickFix
{
    /// <summary>
    /// Creates a message store that stores messages in a persistent memory
    /// </summary>
    public class PmFileStoreFactory : IMessageStoreFactory
    {
        private SessionSettings settings_;

        /// <summary>
        /// Create the factory with configuration in session settings
        /// </summary>
        /// <param name="settings"></param>
        public PmFileStoreFactory(SessionSettings settings)
        {
            settings_ = settings;
        }

        /// <summary>
        /// Creates a file-based message store
        /// </summary>
        /// <param name="sessionID">session ID for the message store</param>
        /// <returns></returns>
        public IMessageStore Create(SessionID sessionID)
        {
            return new PmFileStore(settings_.Get(sessionID).GetString(SessionSettings.FILE_STORE_PATH), sessionID);
        }
    }
}
