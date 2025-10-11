using BepInEx.Configuration;

namespace TarkovShocker.Configuration
{
    public class TarkovConfig
    {
        // Config entries
        public ConfigEntry<bool> pluginEnabled = null!;
        public ConfigEntry<string> apiKey = null!;
        public ConfigEntry<string> apiHost = null!;
        public ConfigEntry<string> defaultShockerId = null!;
        public ConfigEntry<int> durationMS = null!;
        public ConfigEntry<bool> shockerEnabled = null!;
        public ConfigEntry<bool> debugMode = null!;

        public void Init(ConfigFile Config)
        {
            // Initialize configs
            pluginEnabled = Config.Bind("TarkovShocker", "Plugin Enabled", true, "Enable or disable plugin");
            apiKey = Config.Bind("TarkovShocker", "ApiKey", "", "Your OpenShock API Key");
            apiHost = Config.Bind("TarkovShocker", "ApiHost", "https://api.openshock.app", "OpenShock API Host URL");
            defaultShockerId = Config.Bind("TarkovShocker", "DefaultShockerId", "", "Default Shocker ID to trigger");
            durationMS = Config.Bind("TarkovShocker", "Duration MS", 500, "Duration in MS for shock or vibration");
            shockerEnabled = Config.Bind("TarkovShocker", "Shock or Vibration", true, "Enable for Shock, false for vibration");
            debugMode = Config.Bind("TarkovShocker", "Debug Mode", false, "Enable or disable console logging");
        }
    }
}