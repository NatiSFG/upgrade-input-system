using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class InteractableSequenceHandler : MultiSequenceBehaviour {
        [Serializable]
        public struct TargetMapping {
            [ReadOnlyField] public GameObject sequence; //NOTE: It would be better for this to be editor-only, not saved in our actual runtime data
            public Interactable interactable;
        }

        [SerializeField] private SequenceSystem system;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private TargetMapping[] sequenceInteractables = { };

        [Space(20)]
        [SerializeField, ReadOnlyField] private Interactable currentInteractable;

        private GameObject vfx;

        private GameObject VFX {
            get {
                if (vfx == null) {
                    vfx = GameObject.Instantiate(vfxPrefab);
                }

                return vfx;
            }
        }

        //WARNING: COPIED AND MODIFIED from PlayerMovementEnabler.cs
        //Can be genericized in the base class, but would be more advanced generics-based code.
        private void OnValidate() {
            if (system == null) {
                Array.Resize(ref sequenceInteractables, 0);
            } else {
                int desiredCount = system.SequenceCount;
                Array.Resize(ref sequenceInteractables, desiredCount);
                for (int i = 0; i < desiredCount; i++)
                    sequenceInteractables[i].sequence = system.GetSequenceAt(i);
            }
        }

        private void OnDestroy() {
            if (vfx != null)
                GameObject.Destroy(vfx);
        }

        public override void OnSequenceStarted(int sequenceIndex, GameObject sequence) {
            if (TryGetInteractableFor(sequence, out currentInteractable)) {
                currentInteractable.gameObject.SetActive(true);
                GameObject vfx = VFX;
                if (vfx != null) {
                    vfx.SetActive(true);
                    vfx.transform.position = currentInteractable.transform.position;
                }
            }
        }

        public override void OnSequenceEnded(int sequenceIndex, GameObject sequence) {
            if (currentInteractable != null) {
                currentInteractable.gameObject.SetActive(false);
                currentInteractable = null;
            }
            if (vfx != null) {
                vfx.SetActive(false);
            }
        }

        private bool TryGetInteractableFor(GameObject sequence, out Interactable result) {
            foreach (TargetMapping mapping in sequenceInteractables) {
                if (mapping.sequence == sequence) {
                    result = mapping.interactable;
                    return true;
                }
            }
            result = null;
            return false;
        }
    }
}