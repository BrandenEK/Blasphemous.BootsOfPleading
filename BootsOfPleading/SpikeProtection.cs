using ModdingAPI;
using UnityEngine;
using Framework.Managers;
using Gameplay.GameControllers.Entities;

namespace BootsOfPleading
{
    public class SpikeProtection : Mod
    {
        public SpikeProtection(string modId, string modName, string modVersion) : base(modId, modName, modVersion) { }

        private const float PROTECTION_TIME = 2f;

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

        public bool DeadForReal { get; private set; }
        private bool UsingIFrames { get; set; }
        private bool CurrentlyInSpikes { get; set; }

        private bool TientoActive
        {
            get { return Core.InventoryManager.IsPrayerEquipped("PR11") && Core.Logic.Penitent.PrayerCast.Casting; }
        }

        private float currentProtectionTime;

        protected override void Initialize()
        {
            RegisterItem(new BootsRelic().AddEffect<SpikeProtectionEffect>());
            ProtectFromSpikes = false;
        }

        protected override void LevelLoaded(string oldLevel, string newLevel)
        {
            DeadForReal = false;
            CurrentlyInSpikes = false;
        }

        protected override void Update()
        {
            if (UsingIFrames)
            {
                currentProtectionTime -= Time.deltaTime;
                if (currentProtectionTime <= 0)
                {
                    UsingIFrames = false;
                }
            }
        }

        public bool InSpikes()
        {
            float currentHealth = Core.Logic.Penitent.Stats.Life.Current;

            // If using the iframes, never get killed
            if (UsingIFrames)
            {
                return false;
            }

            // If the boots aren't equipped, or you are using tiento, or you have no health, or you were already in spikes, instakill
            if (!ProtectFromSpikes || TientoActive || currentHealth <= 1.2f || CurrentlyInSpikes)
            {
                DeadForReal = true;
                return true;
            }

            // This means this is the first time touching spikes
            Log("Preventing spike death!");
            UsingIFrames = true;
            CurrentlyInSpikes = true;
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
            return false;
        }

        public void LeftSpikes()
        {
            CurrentlyInSpikes = false;
            UsingIFrames = false;
        }
    }
}