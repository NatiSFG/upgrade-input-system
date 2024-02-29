using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    /// <summary>
    /// Contains a central point for all the ordered game states (GameSequences),
    /// including activating and deactivating logic that is specific to each sequence.
    /// </summary>
    public class SequenceSystem : MonoBehaviour {
        [SerializeField, ReadOnlyField] private int activeIndex = -1;
        [SerializeField] private GameObject multiSequenceScripts;
        [SerializeField] private List<GameObject> sequences = new();

        private List<SequenceBehaviour> behaviours = new();

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

        public event Action onSequenceStarted;
        public event Action onSequenceEnded;

        //ASSUMPTIONS:
        //  - The game starts right away on sequence 0.

        private void Start() {
            if (sequences.Count <= 0) {
                Debug.LogWarning("No game sequences were add to the list!", this);
                enabled = false;
                return;
            }
            ActivateSequenceAt(0);
        }

        private void OnDestroy() {
            DeactivateCurrentSequence();
        }

        public void ActivateSequenceAt(int index) {
            sequences[index].SetActive(true);
            activeIndex = index;

            sequences[index].GetComponentsInChildren<SequenceBehaviour>(behaviours);
            foreach (SequenceBehaviour current in behaviours)
                current.OnSequenceStarted();

            onSequenceStarted?.Invoke();
        }

        private bool DeactivateCurrentSequence() {
            if (activeIndex < 0)
                return false;
            sequences[activeIndex].SetActive(false);
            activeIndex = -1;
            onSequenceEnded?.Invoke();
            return true;
        }
    }
}
