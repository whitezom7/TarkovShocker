using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TarkovShocker.Configuration;
using UnityEngine;

namespace TarkovShocker
{
    [BepInPlugin("nl.whitezom.tarkovshocker", "TarkovShocker", "1.0.1")]
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
            if (ConfigManager != null && ConfigManager.debugMode.Value && Input.GetKeyDown(KeyCode.F7))
            {
                Logger.LogInfo("⚡ F7 pressed - sending test feedback!");
                StartCoroutine(OpenShockFeedBack.SendFeedBack.SendFeedback(25, 500));
            }
        }

        

        


    }
}