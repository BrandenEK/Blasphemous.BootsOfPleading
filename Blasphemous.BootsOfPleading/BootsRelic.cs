using Blasphemous.Framework.Items;
using UnityEngine;

namespace Blasphemous.BootsOfPleading;

public class BootsRelic : ModRelic
{
    protected override string Id => "RE401";

    protected override string Name => Main.SpikeProtection.LocalizationHandler.Localize("itmnam");

    protected override string Description => Main.SpikeProtection.LocalizationHandler.Localize("itmdes");

    protected override string Lore => Main.SpikeProtection.LocalizationHandler.Localize("itmlor");

    protected override Sprite Picture => Main.SpikeProtection.FileHandler.LoadDataAsSprite("boots.png", out Sprite image) ? image : null;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => false;

    protected override bool AddInventorySlot => true;

    protected override bool CarryOnStart => false;
}