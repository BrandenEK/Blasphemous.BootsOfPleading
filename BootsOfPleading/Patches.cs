using HarmonyLib;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent;
using Gameplay.UI;
using Framework.Managers;
using Framework.Dialog;
using Framework.Inventory;
using UnityEngine;
using Tools.Playmaker2.Action;
using System.Collections.Generic;

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

    [HarmonyPatch(typeof(CheckTrap), "DeathBySpike", MethodType.Setter)]
    public class SpikeDamageFlag_Patch
    {
        public static bool Prefix()
        {
            return Main.SpikeProtection.DeadForReal;
        }
    }

    [HarmonyPatch(typeof(EventManager), "LaunchEvent")]
    public class EventManager_Patch
    {
        public static bool Prefix(string id)
        {
            return id != "PENITENT_KILLED" || Main.SpikeProtection.DeadForReal;
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

            BoxCollider2D collider = holder.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(collider.size.x, collider.size.y + 0.6f);
            collider.offset = new Vector2(collider.offset.x, collider.offset.y + 0.3f);

            holder.AddComponent<CheckTrapDerived>();
        }
    }

    [HarmonyPatch(typeof(DialogStart), "DialogEnded")]
    public class DialogStart_Patch
    {
        public static void Prefix(string id)
        {
            if (id != "DLG_0312") return;

            Relic boots = Core.InventoryManager.GetRelic("RE401");
            if (boots == null) return;

            Core.InventoryManager.AddRelic(boots);
            UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, boots.caption, boots.picture, InventoryManager.ItemType.Relic, 3f, true);
        }
    }

    //[HarmonyPatch(typeof(DialogManager), "Start")]
    //public class DialogManagerStart_Patch
    //{
    //    public static void Postfix(Dictionary<string, DialogObject> ___allDialogs)
    //    {
    //        DialogObject dialog = new DialogObject();
    //        dialog.dialogType = DialogObject.DialogType.GiveObject;
    //        dialog.itemType = InventoryManager.ItemType.Relic;
    //        dialog.item = "RE401";
    //        dialog.dialogLines = new List<string>()
    //        {
    //            "It's dangerous to go alone Penitent One, take this."
    //        };
    //        ___allDialogs.Add("DLG_MOD_BOOTS", dialog);
    //    }
    //}
}
