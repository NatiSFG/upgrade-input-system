using System;

namespace Game {
    public interface IInteractionSystem {
        public event Action<Interactable> onActiveChanged;

        public Interactor MainInteractor { get; }
        public Interactable Active { get; }

        public void SetMainInteractor(Interactor next);
        public void SetActive(Interactable next);
        public bool TryInteract();
    }
}
