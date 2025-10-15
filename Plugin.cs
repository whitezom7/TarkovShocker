using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TarkovShocker.Configuration;
using TarkovShocker.Handlers;
using UnityEngine;

namespace TarkovShocker
{
    [BepInPlugin("nl.whitezom.tarkovshocker", "TarkovShocker", "1.0.2")]
    public class TarkovShockerPlugin : BaseUnityPlugin
    {
        public static TarkovShockerPlugin Instance { get; private set; } = null!;
        public static ManualLogSource Log { get; private set; } = null!;
        public TarkovConfig ConfigManager { get; private set; } = null!;



        private void Awake()
        {
            Instance = this;
            Log = Logger; // expose logger statically
            ConfigManager = new TarkovConfig();
            ConfigManager.Init(Config);

            Logger.LogInfo("Config initialized!");

            // Fix: Check for null before dereferencing ConfigManager
            if (ConfigManager != null && ConfigManager.debugMode.Value)
            {
                Logger.LogInfo($"✅ TarkovShocker plugin loaded. API Host: {ConfigManager.apiHost.Value}");
            }

            // Apply Harmony patches
            new Harmony("nl.whitezom.tarkovshocker").PatchAll();
        }

        private void Update()
        {
            // Fix: Check for null before dereferencing ConfigManager
            if (ConfigManager.pluginEnabled.Value && ConfigManager.debugMode.Value && Instance.ConfigManager.debugKey.Value.IsDown())
            {
                Logger.LogInfo($"⚡{Instance.ConfigManager.debugKey.Value}  pressed - sending test feedback!");
                int intensity = Instance.ConfigManager.fixedIntensity.Value;
                int durationMS = Instance.ConfigManager.durationMS.Value;
                StartCoroutine(OpenShockFeedBack.SendFeedBack.SendFeedback(intensity, durationMS));
            }

            if (ConfigManager.pluginEnabled.Value && ConfigManager.debugMode.Value && Instance.ConfigManager.killSwitch.Value.IsDown())
            {
                Logger.LogInfo($"⚡{Instance.ConfigManager.killSwitch.Value}  pressed -Disabling plugin!");
                Instance.ConfigManager.pluginEnabled.Value = false;
            }
        }

        

        


    }
}