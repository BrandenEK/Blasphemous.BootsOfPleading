using BepInEx;

namespace BootsOfPleading
{
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
    [BepInDependency("com.damocles.blasphemous.modding-api", "1.2.0")]
    [BepInProcess("Blasphemous.exe")]
    public class Main : BaseUnityPlugin
    {
        public const string MOD_ID = "com.author.blasphemous.boots-of-pleading";
        public const string MOD_NAME = "Boots of Pleading";
        public const string MOD_VERSION = "1.0.0";

        public static SpikeProtection SpikeProtection { get; private set; }

        private void Start()
        {
            SpikeProtection = new SpikeProtection(MOD_ID, MOD_NAME, MOD_VERSION);
        }
    }
}
