using UnityEngine;

namespace Game {
    public interface ISequenceSystem {
        public int ActiveIndex { get; }
        public GameObject ActiveSequence { get; }
        public int SequenceCount { get; }

        public void ActivateSequenceAt(int index);
    }
}