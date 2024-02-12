﻿using Blasphemous.Framework.Items;
using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using UnityEngine;

namespace Blasphemous.BootsOfPleading;

public class SpikeProtection : BlasMod
{
    public SpikeProtection() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private const float PROTECTION_TIME = 2f;

    private bool m_ProtectFromSpikes;
    public bool ProtectFromSpikes
    {
        get => m_ProtectFromSpikes;
        set
        {
            m_ProtectFromSpikes = value;
            LeftSpikes();
        }
    }

    public bool DeadForReal { get; private set; } = false;
    private bool UsingIFrames { get; set; } = false;
    private bool CurrentlyInSpikes { get; set; } = false;

    private bool TientoActive => Core.InventoryManager.IsPrayerEquipped("PR11") && Core.Logic.Penitent.PrayerCast.Casting;

    private float currentProtectionTime;

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterItem(new BootsRelic().AddEffect(new SpikeProtectionEffect()));
    }

    protected override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        DeadForReal = false;
        CurrentlyInSpikes = false;
    }

    protected override void OnUpdate()
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

        // If the boots aren't equipped, or you have no health, or you were already in spikes, instakill
        if (!ProtectFromSpikes || currentHealth <= 1.2f || CurrentlyInSpikes)
        {
            DeadForReal = true;
            return true;
        }

        // This means this is the first time touching spikes
        Log("Preventing spike death!");
        UsingIFrames = true;
        CurrentlyInSpikes = true;
        currentProtectionTime = PROTECTION_TIME;

        Hit spikeHit = new()
        {
            DamageAmount = currentHealth - 1,
            DamageType = DamageArea.DamageType.Normal,
            DamageElement = DamageArea.DamageElement.Contact,
            AttackingEntity = Core.Logic.Penitent.gameObject,
            Unblockable = true,
            Unparriable = true,
            Unnavoidable = true
        };
        if (TientoActive)
        {
            Core.Logic.Penitent.Stats.Life.Current = 1;
        }

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
