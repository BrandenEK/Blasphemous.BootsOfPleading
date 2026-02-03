using BepInEx;

namespace Blasphemous.BootsOfPleading;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "3.0.0")]
[BepInDependency("Blasphemous.Framework.Items", "0.2.0")]
internal class Main : BaseUnityPlugin
{
    public static SpikeProtection SpikeProtection { get; private set; }

    private void Start()
    {
        SpikeProtection = new SpikeProtection();
    }
}
