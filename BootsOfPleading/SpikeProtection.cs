using ModdingAPI;

namespace BootsOfPleading
{
    public class SpikeProtection : Mod
    {
        public SpikeProtection(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

        public bool ProtectFromSpikes { get; set; }

        protected override void Initialize()
        {
            RegisterItem(new BootsRelic().AddEffect<SpikeProtectionEffect>());
        }
    }
}