using ModdingAPI;
using UnityEngine;
using Framework.Managers;
using Gameplay.GameControllers.Entities;

namespace BootsOfPleading
{
    public class SpikeProtection : Mod
    {
        public SpikeProtection(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

        public enum ProtectionStatus { None, IFrames, Protected }
        private const float PROTECTION_TIME = 2f;
        private const float REGEN_TIME = 0.5f;

        private bool m_ProtectFromSpikes;
        public bool ProtectFromSpikes
        {
            get { return m_ProtectFromSpikes; }
            set
            {
                m_ProtectFromSpikes = value;
                LeftSpikes();
            }
        }

        public ProtectionStatus Protection { get; private set; }
        private float currentProtectionTime;
        private float currentRegenTime;

        protected override void Initialize()
        {
            RegisterItem(new BootsRelic().AddEffect<SpikeProtectionEffect>());
            ProtectFromSpikes = false;
        }

        protected override void Update()
        {
            if (Protection == ProtectionStatus.IFrames)
            {
                currentProtectionTime -= Time.deltaTime;
                if (currentProtectionTime <= 0)
                {
                    Protection = ProtectionStatus.None;
                }
            }
            else if (Protection == ProtectionStatus.None && ProtectFromSpikes)
            {
                currentRegenTime -= Time.deltaTime;
                if (currentRegenTime <= 0)
                {
                    Protection = ProtectionStatus.Protected;
                }
            }
        }

        public bool InSpikes()
        {
            if (Protection == ProtectionStatus.Protected)
            {
                float currentHealth = Core.Logic.Penitent.Stats.Life.Current;
                if (currentHealth <= 1)
                {
                    Protection = ProtectionStatus.None;
                    return true;
                }

                Log("Preventing spike death!");
                Protection = ProtectionStatus.IFrames;
                currentProtectionTime = PROTECTION_TIME;

                Hit spikeHit = new Hit()
                {
                    DamageAmount = currentHealth - 1,
                    DamageType = DamageArea.DamageType.Normal,
                    DamageElement = DamageArea.DamageElement.Contact,
                    AttackingEntity = Core.Logic.Penitent.gameObject,
                    Unblockable = true,
                    Unparriable = true,
                    Unnavoidable = true
                };
                Core.Logic.Penitent.Damage(spikeHit);
                Core.Audio.PlaySfxOnCatalog("PenitentDeathBySpike");
            }
            return false;
        }

        public void LeftSpikes()
        {
            Protection = ProtectionStatus.None;
            currentRegenTime = REGEN_TIME;
        }
    }
}