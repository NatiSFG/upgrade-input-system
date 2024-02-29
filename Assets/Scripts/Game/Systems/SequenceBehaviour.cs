using UnityEngine;

namespace Game {
    /// <summary>
    /// Represents a <see cref="MonoBehaviour"/> that reacts to the <see cref="SequenceSystem"/> starting a given sequence.
    /// </summary>
    public abstract class SequenceBehaviour : MonoBehaviour {
        public abstract void OnSequenceStarted();
        public abstract void OnSequenceEnded();
    }
}
