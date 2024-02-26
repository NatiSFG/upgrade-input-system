using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI {
    public class UIManager : MonoBehaviour {
        #region Singleton
        private static UIManager instance;
        public static UIManager Instance {
            get {
                if (instance == null)
                    Debug.LogError("UI Manager is NULL.");
                return instance;
            }
        }
        #endregion

        [SerializeField] private Text interactZone;
        [SerializeField] private Image inventory;
        [SerializeField] private RawImage droneCameraView;

        private void Awake() {
            instance = this;
        }

        public void DisplayInteractableZoneMessage(bool showMessage, string message = null) {
            interactZone.text = message;
            interactZone.gameObject.SetActive(showMessage);
        }

        public void UpdateInventoryDisplay(Sprite icon) {
            inventory.sprite = icon;
        }

        public void DroneView(bool Active) {
            droneCameraView.enabled = Active;
        }
    }
}
