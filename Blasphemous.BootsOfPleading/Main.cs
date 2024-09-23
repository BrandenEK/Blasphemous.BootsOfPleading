using BepInEx;

namespace Blasphemous.BootsOfPleading;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.4.1")]
[BepInDependency("Blasphemous.Framework.Items", "0.1.1")]
internal class Main : BaseUnityPlugin
{
    public static SpikeProtection SpikeProtection { get; private set; }

    private void Start()
    {
        SpikeProtection = new SpikeProtection();
    }
}
