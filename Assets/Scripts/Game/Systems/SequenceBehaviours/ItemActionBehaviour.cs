using System;
using UnityEngine;

namespace Game {
    public class ItemActionBehaviour : SequenceBehaviour {
        [Serializable]
        private enum ItemActionType {
            Nothing = 0,
            PickUp = 1,
            Drop = 2
        }

        [Serializable]
        private struct ItemInstruction {
            public GameObject gameObject;
            public ItemActionType action;

            public void Perform(IItemInventory inventory) {
                if (gameObject == null)
                    return;

                switch (action) {
                    case ItemActionType.PickUp:
                        gameObject.SetActive(false);
                        break;
                    case ItemActionType.Drop:
                        gameObject.SetActive(true);
                        break;
                }
            }
        }

        [SerializeField] private Sprite inventoryIcon;

        [Tooltip("The items in the world at this location that can be picked up.")]
        [SerializeField] private ItemInstruction[] worldItems = { };

        private IItemInventory inventory;

        private void OnEnable() {
            inventory = ServiceLocator.Instance.GetSystem<IItemInventory>();
        }

        public override void OnSequenceStarted() { }
        public override void OnSequenceEnded() {
            if (inventory != null)
                inventory.CurrentItemIcon = inventoryIcon;

            foreach (ItemInstruction i in worldItems)
                i.Perform(inventory);
        }
    }
}
