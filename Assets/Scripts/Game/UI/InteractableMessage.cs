using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Game {
    /// <summary>
    /// Shows an interactable message on the screen, when it's available.
    /// </summary>
    public class InteractableMessage : MonoBehaviour {
        [SerializeField] private Text text;

        private IInteractionSystem interactionSystem;

        private void OnEnable() {
            if (text != null)
                text.enabled = false;

            interactionSystem = ServiceLocator.Instance.GetSystem<IInteractionSystem>();
            if (interactionSystem != null)
                interactionSystem.onActiveChanged += UpdateMessage;
        }

        private void OnDisable() {
            if (interactionSystem != null)
                interactionSystem.onActiveChanged -= UpdateMessage;
        }

        private void UpdateMessage(Interactable interactable) {
            if (interactable == null || interactionSystem == null) {
                Hide();
                return;
            }

            string message = InteractTypeExtensions.GetMessage(
                interactable.InteractType,
                interactionSystem.MainInteractor.InteractKey,
                interactable.MessageSuffix
            );

            ShowMessage(message);
        }

        public void ShowMessage(string message) {
            text.text = message;
            Show();
        }

        public void Show() {
            text.enabled = true;
        }

        public void Hide() {
            text.enabled = false;
        }
    }
}