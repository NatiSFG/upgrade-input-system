using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    /// <summary>
    /// Contains a central point for all the ordered game states (GameSequences),
    /// including activating and deactivating logic that is specific to each sequence.
    /// </summary>
    public class SequenceSystem : MonoBehaviour, ISequenceSystem {
        [SerializeField, ReadOnlyField] private int activeIndex = -1;
        [SerializeField] private GameObject multiSequenceParent;
        [SerializeField] private List<GameObject> sequences = new();

        private List<MultiSequenceBehaviour> multiBehaviours = new();
        private List<SequenceBehaviour> behaviours = new();

        public event Action onSequenceStarted;
        public event Action onSequenceEnded;

        public int ActiveIndex {
            get { return activeIndex; }
        }

        public GameObject ActiveSequence {
            get {
                if (activeIndex < 0)
                    return null;
                return sequences[activeIndex];
            }
        }

        public int SequenceCount => sequences.Count;
        public GameObject GetSequenceAt(int index) => sequences[index];

        //ASSUMPTIONS:
        //  - The game starts right away on sequence 0.

        private void Awake() {
            if (sequences.Count <= 0) {
                Debug.LogWarning("No game sequences were add to the list!", this);
                enabled = false;
                return;
            }

            foreach (GameObject sequence in sequences)
                sequence.SetActive(false);
        }

        private void Start() {
            ActivateSequenceAt(0);
        }

        private void OnDestroy() {
            DeactivateCurrentSequence();
        }

        public void ActivateSequenceAt(int index) {
            DeactivateCurrentSequence();

            activeIndex = index;
            GameObject sequence = ActiveSequence;
            sequence.SetActive(true);

            multiSequenceParent.GetComponentsInChildren<MultiSequenceBehaviour>(multiBehaviours);
            sequences[index].GetComponentsInChildren<SequenceBehaviour>(behaviours);
            foreach (MultiSequenceBehaviour current in multiBehaviours)
                current.OnSequenceStarted(activeIndex, sequence);
            foreach (SequenceBehaviour current in behaviours)
                current.OnSequenceStarted();

            onSequenceStarted?.Invoke();
        }

        private bool DeactivateCurrentSequence() {
            if (activeIndex < 0)
                return false;

            GameObject previous = ActiveSequence;
            sequences[activeIndex].SetActive(false);
            activeIndex = -1;

            foreach (MultiSequenceBehaviour current in multiBehaviours)
                current.OnSequenceEnded(activeIndex, previous);
            foreach (SequenceBehaviour current in behaviours)
                current.OnSequenceEnded();
            onSequenceEnded?.Invoke();
            return true;
        }
    }
}
