using BepInEx;
using BepInEx.Configuration;
using EFT;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace TarkovShocker
{
    [BepInPlugin("nl.whitezom.tarkovshocker", "TarkovShocker", "1.0.0")]
    public class TarkovShockerPlugin : BaseUnityPlugin
    {
        private static TarkovShockerPlugin? instance;

        // Config entries
        private ConfigEntry<bool> pluginEnabled = null!;
        private ConfigEntry<string> apiKey = null!;
        private ConfigEntry<string> apiHost = null!;
        private ConfigEntry<string> defaultShockerId = null!;
        private ConfigEntry<int> durationMS = null!;
        private ConfigEntry<bool> shockerEnabled = null!;
        private ConfigEntry<bool> debugMode = null!;

        private void Awake()
        {
            instance = this;

            // Initialize configs
            pluginEnabled = Config.Bind("TarkovShocker", "Plugin Enabled", true, "Enable or disable plugin");
            apiKey = Config.Bind("TarkovShocker", "ApiKey", "", "Your OpenShock API Key");
            apiHost = Config.Bind("TarkovShocker", "ApiHost", "https://api.openshock.app", "OpenShock API Host URL");
            defaultShockerId = Config.Bind("TarkovShocker", "DefaultShockerId", "", "Default Shocker ID to trigger");
            durationMS = Config.Bind("TarkovShocker", "Duration MS", 500, "Duration in MS for shock or vibration");
            shockerEnabled = Config.Bind("TarkovShocker", "Shock or Vibration", true, "Enable for Shock, false for vibration");
            debugMode = Config.Bind("TarkovShocker", "Debug Mode", false, "Enable or disable console logging");

            if (debugMode.Value)
            {
                Logger.LogInfo($"✅ TarkovShocker plugin loaded. API Host: {apiHost.Value}");
            }

            // Apply Harmony patches
            new Harmony("nl.whitezom.tarkovshocker").PatchAll();
        }

        private void Update()
        {
            // Test key to confirm plugin works
            if (debugMode.Value && Input.GetKeyDown(KeyCode.F7))
            {
                Logger.LogInfo("⚡ F7 pressed - sending test feedback!");
                StartCoroutine(SendFeedback(25, 500));
            }
        }

        public static void HandleDamage(Player player, float damage)
        {
            if (instance == null || !instance.pluginEnabled.Value) return;

            int intensity = Mathf.Clamp(Mathf.RoundToInt(damage), 5, 100);

            if (instance.debugMode.Value)
            {
                instance.Logger.LogInfo($"💥 Player took {damage} damage — triggering feedback at intensity {intensity}!");
            }

            instance.StartCoroutine(instance.SendFeedback(intensity, instance.durationMS.Value));
        }

        private IEnumerator SendFeedback(int intensity = 20, int durationMs = 500)
        {
            if (string.IsNullOrEmpty(apiKey.Value) || string.IsNullOrEmpty(defaultShockerId.Value))
                yield break;

            string feedbackType = shockerEnabled.Value ? "Shock" : "Vibrate";

            var payload = new
            {
                shocks = new[]
                {
                    new
                    {
                        id = defaultShockerId.Value,
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

            using var request = new UnityWebRequest(apiHost.Value + "/2/shockers/control", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("OpenShockToken", apiKey.Value);

            yield return request.SendWebRequest();

            if (debugMode.Value)
            {
                if (request.result != UnityWebRequest.Result.Success)
                    Logger.LogError($"❌ Feedback failed: {request.error} | {request.downloadHandler.text}");
                else
                    Logger.LogInfo($"✅ Sent {feedbackType} feedback successfully!");
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.ApplyDamageInfo))]
        public class PlayerDamagePatch
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance, DamageInfoStruct damageInfo)
            {
                if (__instance == null || !__instance.IsYourPlayer) return;
                TarkovShockerPlugin.HandleDamage(__instance, damageInfo.Damage);
            }
        }
    }
}