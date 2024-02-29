using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.LiveObjects {
    public class EndZone : MonoBehaviour {
        private void OnEnable() {
            InteractZone.onInteractionComplete += ReachedEndZone;
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= ReachedEndZone;
        }

        private void ReachedEndZone(InteractZone zone) {
            if (zone.GetZoneID() == 7) {
                InteractZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        }
    }
}
