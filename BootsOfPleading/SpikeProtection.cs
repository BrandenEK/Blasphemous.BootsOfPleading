using ModdingAPI;
using UnityEngine;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using Gameplay.UI.Others.UIGameLogic;

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
                    Log(("Iframes are over - No more protection"));
                }
            }
            else if (Protection == ProtectionStatus.None && ProtectFromSpikes)
            {
                currentRegenTime -= Time.deltaTime;
                if (currentRegenTime <= 0)
                {
                    Protection = ProtectionStatus.Protected;
                    Log("Regen is over - Now protected");
                }
            }
            Object.FindObjectOfType<PlayerPurgePoints>().text.text = Protection.ToString();
        }

        public bool InSpikes()
        {
            if (Protection == ProtectionStatus.Protected)
            {
                Log("Preventing spike death!");
                Protection = ProtectionStatus.IFrames;
                currentProtectionTime = PROTECTION_TIME;

                float damage = Core.Logic.Penitent.Stats.Life.Current - 1;
                if (damage < 1)
                    return true;

                Hit spikeHit = new Hit()
                {
                    DamageAmount = damage,
                    DamageType = DamageArea.DamageType.Normal,
                    DamageElement = DamageArea.DamageElement.Contact,
                    AttackingEntity = Core.Logic.Penitent.gameObject
                };
                Core.Logic.Penitent.Damage(spikeHit);
            }
            return false;
        }

        public void LeftSpikes()
        {
            Log("Left spikes");
            Protection = ProtectionStatus.None;
            currentRegenTime = REGEN_TIME;
        }
    }
}