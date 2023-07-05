namespace OMS
{
    public class SettingsDB
    {
        public string ConnectionString { get; set; } = string.Empty;
    }    
    
    public class SettingsPm
    {
        public string InternalsFolder { get; set; } = string.Empty;
        public string OrdersFilePath { get; set; } = string.Empty;
    }

    public class Settings
    {
        public string Persistency { get; set; } = string.Empty;
        public bool UseTraditionalMemoryMappedFiles { get; set; }
        public SettingsDB? SqlLite { get; set; }
        public SettingsDB? PostgreSQL { get; set; }
        public SettingsPm? Pm { get; set; }
    }
}
