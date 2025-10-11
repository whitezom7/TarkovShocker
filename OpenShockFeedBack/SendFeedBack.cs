using System.Collections;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;

namespace TarkovShocker.OpenShockFeedBack
{
    internal class SendFeedBack
    {
        public static IEnumerator SendFeedback(int intensity = 20, int durationMs = 500)
        {
            // Fix: Check for null before dereferencing ConfigManager
            if (TarkovShockerPlugin.Instance?.ConfigManager == null)
                yield break;

            if (string.IsNullOrEmpty(TarkovShockerPlugin.Instance.ConfigManager.apiKey.Value) || string.IsNullOrEmpty(TarkovShockerPlugin.Instance.ConfigManager.defaultShockerId.Value))
                yield break;

            string feedbackType = TarkovShockerPlugin.Instance.ConfigManager.shockerEnabled.Value ? "Shock" : "Vibrate";

            var payload = new
            {
                shocks = new[]
                {
                    new
                    {
                        id = TarkovShockerPlugin.Instance.ConfigManager.defaultShockerId.Value,
                        type = feedbackType,
                        intensity,
                        duration = durationMs,
                        exclusive = true
                    }
                },
                customName = "GameFeedback"
            };

            string json = JsonConvert.SerializeObject(payload);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using var request = new UnityEngine.Networking.UnityWebRequest(TarkovShockerPlugin.Instance.ConfigManager.apiHost.Value + "/2/shockers/control", "POST");
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("OpenShockToken", TarkovShockerPlugin.Instance.ConfigManager.apiKey.Value);

            yield return request.SendWebRequest();

            if (TarkovShockerPlugin.Instance.ConfigManager.debugMode.Value)
            {
                if (request.result != UnityWebRequest.Result.Success)
                    TarkovShockerPlugin.Log.LogError($"❌ Feedback failed: {request.error} | {request.downloadHandler.text}");
                else
                    TarkovShockerPlugin.Log.LogInfo($"✅ Sent {feedbackType} feedback successfully!");
            }
        }
    }
}
