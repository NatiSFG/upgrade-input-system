using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class PlayerMovementEnabler : MultiSequenceBehaviour {
        [SerializeField] private SequenceSystem system;

        //WARNING: Coupling to scene object!
        [SerializeField] private Behaviour target;

        [Tooltip("Determines whether or not player input will control the character during each game sequence.\n\n" +
            "The indices directly correspond to the " + nameof(SequenceSystem) + "'s list of sequences.")]
        [SerializeField] private List<bool> playerInputStatus = new();

        private void OnValidate() {
            if (system == null) {
                playerInputStatus.Clear();
            } else {
                int desiredCount = system.SequenceCount;
                while (playerInputStatus.Count < desiredCount)
                    playerInputStatus.Add(true);
                if (playerInputStatus.Count > desiredCount)
                    playerInputStatus.RemoveRange(desiredCount, playerInputStatus.Count - desiredCount);
            }
        }

        public override void OnSequenceStarted(int sequenceIndex, GameObject sequence) {
            target.enabled = playerInputStatus[sequenceIndex];
        }
        public override void OnSequenceEnded(int sequenceIndex, GameObject sequence) { }
    }
}
