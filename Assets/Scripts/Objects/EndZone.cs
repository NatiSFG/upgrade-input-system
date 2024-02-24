using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.LiveObjects {
    public class EndZone : MonoBehaviour {
        private void OnEnable() {
            InteractableZone.onZoneInteractionComplete += ReachedEndZone;
        }

        //rename ReachedEndZone
        private void ReachedEndZone(InteractableZone zone) {
            if (zone.GetZoneID() == 7) {
                InteractableZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        }

        private void OnDisable() {
            InteractableZone.onZoneInteractionComplete -= ReachedEndZone;
        }
    }
}
