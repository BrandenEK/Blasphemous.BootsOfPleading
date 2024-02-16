using Blasphemous.Framework.Items;

namespace Blasphemous.BootsOfPleading;

internal class SpikeProtectionEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect()
    {
        Main.SpikeProtection.ProtectFromSpikes = true;
    }

    protected override void RemoveEffect()
    {
        Main.SpikeProtection.ProtectFromSpikes = false;
    }
}
