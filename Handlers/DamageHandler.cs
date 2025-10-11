using EFT;
using UnityEngine;

namespace TarkovShocker.Handlers
{
    internal static class DamageHandler
    {
        public static void Handle(Player player, float damage)
        {
            if (TarkovShockerPlugin.Instance == null ||
                !TarkovShockerPlugin.Instance.ConfigManager.pluginEnabled.Value)
                return;

            int intensity = Mathf.Clamp(Mathf.RoundToInt(damage), 5, 100);

            if (TarkovShockerPlugin.Instance.ConfigManager.debugMode.Value)
            {
                TarkovShockerPlugin.Log.LogInfo(
                    $"💥 Player took {damage:F1} damage — triggering feedback at intensity {intensity}!"
                );
            }

            TarkovShockerPlugin.Instance.StartCoroutine(
                TarkovShockerPlugin.Instance.SendFeedback(
                    intensity,
                    TarkovShockerPlugin.Instance.ConfigManager.durationMS.Value
                )
            );
        }
    }
}