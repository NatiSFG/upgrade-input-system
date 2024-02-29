using UnityEngine;

namespace Game {
    /// <summary>
    /// Represents a <see cref="MonoBehaviour"/> that reacts to the <see cref="SequenceSystem"/> starting any sequence.
    /// </summary>
    public abstract class MultiSequenceBehaviour : MonoBehaviour {
        public abstract void OnSequenceStarted(int sequenceIndex, GameObject sequence);
        public abstract void OnSequenceEnded(int sequenceIndex, GameObject sequence);
    }
}
