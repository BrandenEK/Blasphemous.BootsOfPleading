using HarmonyLib;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Framework.Managers;
using UnityEngine;

namespace BootsOfPleading
{
    [HarmonyPatch(typeof(CheckTrap), "SpikeTrapDamage")]
    public class SpikeDamage_Patch
    {
        public static bool Prefix()
        {
            if (Main.SpikeProtection.Protection == SpikeProtection.ProtectionStatus.None)
            {
                return true;
            }
            else
            {
                Main.SpikeProtection.InSpikes();
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(CheckTrap), "DeathBySpike", MethodType.Setter)]
    public class SpikeDamageFlag_Patch
    {
        public static bool Prefix()
        {
            return Main.SpikeProtection.Protection == SpikeProtection.ProtectionStatus.None;
        }
    }

    [HarmonyPatch(typeof(EventManager), "LaunchEvent")]
    public class EventManager_Patch
    {
        public static bool Prefix(string id)
        {
            return id != "PENITENT_KILLED" || Main.SpikeProtection.Protection == SpikeProtection.ProtectionStatus.None;
        }
    }

    [HarmonyPatch(typeof(Penitent), "OnAwake")]
    public class Penitent_Patch
    {
        public static void Prefix(Penitent __instance)
        {
            CheckTrap trapChecker = __instance.GetComponentInChildren<CheckTrap>();
            GameObject holder = trapChecker.gameObject;
            Object.Destroy(trapChecker);
            holder.AddComponent<CheckTrapDerived>();
        }

        public static void Postfix(Penitent __instance)
        {
            Main.SpikeProtection.LogWarning(__instance.GetComponentInChildren<CheckTrap>().GetType().Name);
        }
    }
}
