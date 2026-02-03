using Blasphemous.ModdingAPI.Helpers;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using HarmonyLib;
using Tools.Playmaker2.Action;
using UnityEngine;

namespace Blasphemous.BootsOfPleading;

/// <summary>
/// If in Iframes or first hit with enough health, skip the rest of spike damage
/// </summary>
[HarmonyPatch(typeof(CheckTrap), nameof(CheckTrap.SpikeTrapDamage))]
class CheckTrap_SpikeTrapDamage_Patch
{
    public static bool Prefix()
    {
        return Main.SpikeProtection.InSpikes();
    }
}

/// <summary>
/// Only set this if actually dead
/// </summary>
[HarmonyPatch(typeof(CheckTrap), nameof(CheckTrap.DeathBySpike), MethodType.Setter)]
class CheckTrap_DeathBySpike_Patch
{
    public static bool Prefix()
    {
        return Main.SpikeProtection.DeadForReal;
    }
}

/// <summary>
/// Only launch event if actually dead
/// </summary>
[HarmonyPatch(typeof(EventManager), nameof(EventManager.LaunchEvent))]
class EventManager_LaunchEvent_Patch
{
    public static bool Prefix(string id)
    {
        return id != "PENITENT_KILLED" || Main.SpikeProtection.DeadForReal;
    }
}

/// <summary>
/// Replace default trap checker with a longer one that extends into the spikes
/// </summary>
[HarmonyPatch(typeof(Penitent), nameof(Penitent.OnAwake))]
class Penitent_OnAwake_Patch
{
    public static void Prefix(Penitent __instance)
    {
        CheckTrap trapChecker = __instance.GetComponentInChildren<CheckTrap>();
        GameObject holder = trapChecker.gameObject;
        Object.Destroy(trapChecker);

        BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x, collider.size.y + 0.4f);
        collider.offset = new Vector2(collider.offset.x, collider.offset.y + 0.2f);

        holder.AddComponent<CheckTrapDerived>();
    }
}

/// <summary>
/// Give the boots when finishing a certain dialog
/// </summary>
[HarmonyPatch(typeof(DialogStart), nameof(DialogStart.DialogEnded))]
class DialogStart_DialogEnded_Patch
{
    public static void Prefix(string id)
    {
        if (id == "DLG_0207")
        {
            ItemHelper.AddAndDisplayItem("RE401");
        }
    }
}
