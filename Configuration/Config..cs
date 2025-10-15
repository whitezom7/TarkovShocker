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
        public ConfigEntry<FeedbackType> shockerEnabled = null!;
        public ConfigEntry<bool> debugMode = null!;
        public ConfigEntry<IntensityType> intensityType  = null!;
        public ConfigEntry<int> fixedIntensity = null!;



        public enum IntensityType
        {
            Dynamic,
            Fixed
        }

        public enum FeedbackType
        {
            Shock,
            Vibrate
        }


        public void Init(ConfigFile Config)
        {
            // Initialize configs
            pluginEnabled = Config.Bind("General", "Plugin Enabled", true, "Enable or disable plugin");
            apiKey = Config.Bind("General", "ApiKey", "", "Your OpenShock API Key");
            apiHost = Config.Bind("General", "ApiHost", "https://api.openshock.app", "OpenShock API Host URL");
            defaultShockerId = Config.Bind("Shocker Config", "DefaultShockerId", "", "Default Shocker ID to trigger");
            durationMS = Config.Bind("Shocker Config", "Duration MS", 500, new ConfigDescription("Duration in MS for Shock or vibration (300-6000)", new AcceptableValueRange<int>(300, 6000)));
            shockerEnabled = Config.Bind<FeedbackType>("Shocker Config", "Feedback Type", FeedbackType.Shock, "Choose Shock or Vibration for feedback"); 
            intensityType = Config.Bind<IntensityType>("Shocker Config", "Intensity Type", IntensityType.Dynamic, "Choose Dynamic or Fixed intensity for feedback");
            fixedIntensity = Config.Bind("Shocker Config", "Fixed Intensity", 50, new ConfigDescription("Fixed intensity value (1-100)", new AcceptableValueRange<int>(1, 100)));
            debugMode = Config.Bind("Debug", "Debug Mode", false, "Enable or disable console logging");
        }
    }
}


