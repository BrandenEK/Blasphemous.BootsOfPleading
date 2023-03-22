using HarmonyLib;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.UI;
using Framework.Managers;
using Framework.Inventory;
using Tools.Playmaker2.Action;
using UnityEngine;

namespace BootsOfPleading
{
    // If in Iframes or first hit with enough health, skip the rest of spike damage
    [HarmonyPatch(typeof(CheckTrap), "SpikeTrapDamage")]
    public class SpikeDamage_Patch
    {
        public static bool Prefix()
        {
            return Main.SpikeProtection.InSpikes();
        }
    }

    // Only set this if actually dead
    [HarmonyPatch(typeof(CheckTrap), "DeathBySpike", MethodType.Setter)]
    public class SpikeDamageFlag_Patch
    {
        public static bool Prefix()
        {
            return Main.SpikeProtection.DeadForReal;
        }
    }

    // Only launch event if actually dead
    [HarmonyPatch(typeof(EventManager), "LaunchEvent")]
    public class EventManager_Patch
    {
        public static bool Prefix(string id)
        {
            return id != "PENITENT_KILLED" || Main.SpikeProtection.DeadForReal;
        }
    }

    // Replace default trap checker with a longer one that extends into the spikes
    [HarmonyPatch(typeof(Penitent), "OnAwake")]
    public class Penitent_Patch
    {
        public static void Prefix(Penitent __instance)
        {
            CheckTrap trapChecker = __instance.GetComponentInChildren<CheckTrap>();
            GameObject holder = trapChecker.gameObject;
            Object.Destroy(trapChecker);

            BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(collider.size.x, collider.size.y + 0.6f);
            collider.offset = new Vector2(collider.offset.x, collider.offset.y + 0.3f);

            holder.AddComponent<CheckTrapDerived>();
        }
    }

    // Give the boots when finishing a certain dialog
    [HarmonyPatch(typeof(DialogStart), "DialogEnded")]
    public class DialogStart_Patch
    {
        public static void Prefix(string id)
        {
            if (id != "DLG_0207") return;

            BaseInventoryObject obj = Core.InventoryManager.GetBaseObject("RE401", InventoryManager.ItemType.Relic);
            if (obj == null) return;

            obj = Core.InventoryManager.AddBaseObjectOrTears(obj);
            UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, obj.caption, obj.picture, InventoryManager.ItemType.Relic, 3f, true);
        }
    }
}
