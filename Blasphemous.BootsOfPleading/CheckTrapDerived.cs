using Gameplay.GameControllers.Entities;
using UnityEngine;

namespace Blasphemous.BootsOfPleading;

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
