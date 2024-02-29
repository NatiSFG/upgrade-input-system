using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class UIManager : MonoBehaviour {
        private static UIManager instance;
        public static UIManager Instance {
            get {
                if (instance == null)
                    Debug.LogError("UI Manager is NULL.");
                return instance;
            }
        }

        [SerializeField] private RawImage droneCamera;

        private void Awake() {
            instance = this;
        }

        public void DroneView(bool Active) {
            droneCamera.enabled = Active;
        }
    }
}
