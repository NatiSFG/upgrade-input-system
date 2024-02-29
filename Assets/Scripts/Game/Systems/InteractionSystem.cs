using System;
using UnityEngine;

namespace Game {
    public class InteractionSystem : MonoBehaviour, IInteractionSystem {
        private Interactable active;
        private Interactor mainInteractor;

        public event Action<Interactable> onActiveChanged;

        public Interactor MainInteractor => mainInteractor;

        /// <summary>
        /// The <see cref="Interactable"/> object that is nearby, if any.
        /// </summary>
        public Interactable Active => active;

        public void SetMainInteractor(Interactor next) {
            if (mainInteractor != null)
                throw new InvalidOperationException("The main interactor (perhaps your player) has already been set!");
            mainInteractor = next;
        }

        public void SetActive(Interactable next) {
            active = next;
            onActiveChanged?.Invoke(next);
        }

        public bool TryInteract() {
            if (active == null)
                return false;
            active.OnInteract();
            return true;
        }
    }
}