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

        [SerializeField] private Image inventory;
        [SerializeField] private RawImage droneCamera;

        private void Awake() {
            instance = this;
        }

        public void UpdateInventoryDisplay(Sprite icon) {
            if (icon != null) {
                inventory.sprite = icon;
                inventory.color = Color.white;
            } else {
                inventory.sprite = null;
                inventory.color = Color.clear;
            }
        }

        public void DroneView(bool Active) {
            droneCamera.enabled = Active;
        }
    }
}
