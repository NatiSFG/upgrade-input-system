using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class EndZone : MonoBehaviour {
        private void OnEnable() {
            InteractZone.onInteractionComplete += ReachedEndZone;
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= ReachedEndZone;
        }

        private void ReachedEndZone(InteractZone zone) {
            if (zone.ZoneID == 7) {
                InteractZone.CurrentZoneID = 0;
                SceneManager.LoadScene(0);
            }
        }
    }
}
