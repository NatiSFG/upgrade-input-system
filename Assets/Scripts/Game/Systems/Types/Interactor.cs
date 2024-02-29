using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
    public class Interactor : MonoBehaviour {
        [SerializeField] private Key interactKey = Key.Space;

        private IInteractionSystem interaction;
        private Keyboard keyboard;

        public Key InteractKey => interactKey;

        private void OnEnable() {
            interaction = ServiceLocator.Instance.GetSystem<IInteractionSystem>();
            keyboard = InputSystem.GetDevice<Keyboard>();

            if (interaction != null)
                interaction.SetMainInteractor(this);
        }

        private void Update() {
            if (keyboard != null) {
                if (keyboard[interactKey].wasPressedThisFrame) {
                    interaction.TryInteract();
                }
            }
        }
    }
}