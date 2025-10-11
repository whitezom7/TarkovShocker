using EFT;
using HarmonyLib;

namespace TarkovShocker.Patches
{
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
