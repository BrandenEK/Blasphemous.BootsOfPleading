using ModdingAPI.Items;
using UnityEngine;

namespace BootsOfPleading
{
    public class BootsRelic : ModRelic
    {
        protected override string Id => "RE401";

        protected override string Name => "Boots of Pleading";

        protected override string Description => "Old steel boots, covered in rust and dark splotches. This Miracle of lesser-known origin protects the Penitent one from inevitable death by spikes.";

        protected override string Lore => "High Judge Marcus was known for his temper. When pilgrim Cristobal dared to enter his chambers with foot bare and dirty, the punishment was swift. Every day poor Cristobal would have to fetch water for the monks of nearby church. Every day he would have to wear steel boots, lined with sharp spikes inside. Every noon he would empty the boots of his blood. At the end of each week he would come to the Judge's attendant, pleading for meeting of mercy. But little did pilgrim Cristobal know, the Judge forgot about his existence on the next day. And thus, for seven years, he would fetch the water, dry the boots and plead in tears. Until one day, the boots, removed, were dry. That day, the pleadings of St. Cristobal were heard.";

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