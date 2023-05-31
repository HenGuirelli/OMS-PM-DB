namespace OMS
{
    public class SettingsSqlLite
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
        public SettingsSqlLite? SqlLite { get; set; }
        public SettingsPm? Pm { get; set; }
    }
}
