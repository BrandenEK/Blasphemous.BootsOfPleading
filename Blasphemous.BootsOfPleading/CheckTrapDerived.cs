using Gameplay.GameControllers.Entities;
using UnityEngine;

namespace Blasphemous.BootsOfPleading;

/// <summary>
/// Component added to the player instead of the original that calls special method
/// </summary>
public class CheckTrapDerived : CheckTrap
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpikeTrap"))
        {
            Main.SpikeProtection.LeftSpikes();
        }
    }
}
