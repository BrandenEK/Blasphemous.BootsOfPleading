using ModdingAPI.Items;
using UnityEngine;

namespace BootsOfPleading
{
    public class BootsRelic : ModRelic
    {
        protected override string Id => "RE401";

        protected override string Name => Main.SpikeProtection.Localize("itmnam");

        protected override string Description => Main.SpikeProtection.Localize("itmdes");

        protected override string Lore => Main.SpikeProtection.Localize("itmlor");

        protected override bool CarryOnStart => true;

        protected override bool PreserveInNGPlus => true;

        protected override bool AddToPercentCompletion => false;

        protected override bool AddInventorySlot => true;

        protected override void LoadImages(out Sprite picture)
        {
            picture = Main.SpikeProtection.FileUtil.loadDataImages("boots.png", 32, 32, 32, 0, true, out Sprite[] images) ? images[0] : null;
        }
    }
}