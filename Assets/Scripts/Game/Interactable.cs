using System;
using UnityEngine;

namespace Game {
    public class Interactable : MonoBehaviour {
        [SerializeField] private Sprite inventoryIcon;
        [SerializeField] private InteractType interactType;

        [Tooltip("The items in the world at this location that can be picked up.")]
        [SerializeField, Obsolete("Use a separate " + nameof(ItemActionBehaviour) + " script instead.")] private GameObject[] worldItems = { };

        [Tooltip("This is the ending of the display message, which is given in the format:\n\n" +
            "[Press|Hold] the {key} key to {messageSuffix}.")]
        [SerializeField] private string messageSuffix = "";

        private bool isPlayerInRange = false;
        private int interactCount = 0;

        private IInteractionSystem interactions;
        private ISequenceSystem sequences;

        public Sprite InventoryIcon => inventoryIcon;
        public InteractType InteractType => interactType;
        public string MessageSuffix => messageSuffix;

        public bool IsPlayerInRange {
            get { return isPlayerInRange; }
            set {
                bool changed = isPlayerInRange != value;
                
                isPlayerInRange = value;

                if (changed) {
                    if (interactions != null)
                        interactions.SetActive(isPlayerInRange ? this : null);
                }
            }
        }

        private void OnEnable() {
            interactions = ServiceLocator.Instance.GetSystem<IInteractionSystem>();
            sequences = ServiceLocator.Instance.GetSystem<ISequenceSystem>();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponentInParent<Player>() != null)
                IsPlayerInRange = true;
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.GetComponentInParent<Player>() != null)
                IsPlayerInRange = false;
        }

        public void OnInteract() {
            if (sequences != null) {
                int index = sequences.ActiveIndex;
                if (index < sequences.SequenceCount - 1)
                    sequences.ActivateSequenceAt(sequences.ActiveIndex + 1);
            }
        }
    }
}
