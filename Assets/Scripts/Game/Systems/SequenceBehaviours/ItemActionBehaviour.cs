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

            public void Perform() {
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

        [Tooltip("The items in the world at this location that can be picked up.")]
        [SerializeField] private ItemInstruction[] worldItems = { };

        public override void OnSequenceStarted() { }
        public override void OnSequenceEnded() {
            foreach (ItemInstruction i in worldItems)
                i.Perform();
        }
    }
}
