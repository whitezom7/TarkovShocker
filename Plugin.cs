using BepInEx;
using EFT;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using TarkovShocker.Configuration;
using UnityEngine;
using UnityEngine.Networking;

namespace TarkovShocker
{
    [BepInPlugin("nl.whitezom.tarkovshocker", "TarkovShocker", "1.0.0")]
    public class TarkovShockerPlugin : BaseUnityPlugin
    {
        public static TarkovShockerPlugin Instance { get; private set; } = null!;
        public TarkovConfig ConfigManager { get; private set; } = null!;



        private void Awake()
        {
            Instance = this;
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
                StartCoroutine(SendFeedback(25, 500));
            }
        }

        public static void HandleDamage(Player player, float damage)
        {
            if (Instance == null || !Instance.ConfigManager.pluginEnabled.Value) return;

            int intensity = Mathf.Clamp(Mathf.RoundToInt(damage), 5, 100);

            if (Instance.ConfigManager.debugMode.Value)
            {
                Instance.Logger.LogInfo($"💥 Player took {damage} damage — triggering feedback at intensity {intensity}!");
            }

            Instance.StartCoroutine(Instance.SendFeedback(intensity, Instance.ConfigManager.durationMS.Value));
        }

        private IEnumerator SendFeedback(int intensity = 20, int durationMs = 500)
        {
            // Fix: Check for null before dereferencing ConfigManager
            if (Instance?.ConfigManager == null)
                yield break;

            if (string.IsNullOrEmpty(Instance.ConfigManager.apiKey.Value) || string.IsNullOrEmpty(Instance.ConfigManager.defaultShockerId.Value))
                yield break;

            string feedbackType = Instance.ConfigManager.shockerEnabled.Value ? "Shock" : "Vibrate";

            var payload = new
            {
                shocks = new[]
                {
            new
            {
                id = Instance.ConfigManager.defaultShockerId.Value,
                type = feedbackType,
                intensity = intensity,
                duration = durationMs,
                exclusive = true
            }
        },
                customName = "GameFeedback"
            };

            string json = JsonConvert.SerializeObject(payload);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using var request = new UnityWebRequest(Instance.ConfigManager.apiHost.Value + "/2/shockers/control", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("OpenShockToken", Instance.ConfigManager.apiKey.Value);

            yield return request.SendWebRequest();

            if (Instance.ConfigManager.debugMode.Value)
            {
                if (request.result != UnityWebRequest.Result.Success)
                    Logger.LogError($"❌ Feedback failed: {request.error} | {request.downloadHandler.text}");
                else
                    Logger.LogInfo($"✅ Sent {feedbackType} feedback successfully!");
            }
        }


    }
}