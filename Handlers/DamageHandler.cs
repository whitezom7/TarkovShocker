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

            if (TarkovShockerPlugin.Instance.ConfigManager.intensityType.Value ==
                Configuration.TarkovConfig.IntensityType.Fixed)
            {
                int fixedIntensity = TarkovShockerPlugin.Instance.ConfigManager.fixedIntensity.Value;
                damage = fixedIntensity;
            }

            int intensity = Mathf.Clamp(Mathf.RoundToInt(damage), 5, 100);
            int duration = TarkovShockerPlugin.Instance.ConfigManager.durationMS.Value;

            if (TarkovShockerPlugin.Instance.ConfigManager.debugMode.Value)
            {
                TarkovShockerPlugin.Log.LogInfo(
                    $"💥 Player took {damage:F1} damage — triggering feedback at intensity {intensity} for {duration} !"
                );
            }

            TarkovShockerPlugin.Instance.StartCoroutine(
                OpenShockFeedBack.SendFeedBack.SendFeedback(
                    intensity,
                    duration
                )
            );
        }
    }
}