using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class InventorySystem : MonoBehaviour, IItemInventory {
        [SerializeField] private Image image;

        private Sprite currentItemIcon;

        public Sprite CurrentItemIcon {
            get { return currentItemIcon; }
            set {
                currentItemIcon = value;
                UpdateInventoryUI(currentItemIcon);
            }
        }

        private void UpdateInventoryUI(Sprite icon) {
            if (icon != null) {
                image.sprite = icon;
                image.color = Color.white;
            } else {
                image.sprite = null;
                image.color = Color.clear;
            }
        }
    }
}