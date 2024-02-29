using System;
using UnityEngine.InputSystem;

namespace Game {
    /// <summary>
    /// Represents the type of action the player may perform on an <see cref="Interactable"/>.
    /// </summary>
    [Serializable]
    public enum InteractType {
        Collectable,
        Action,
        HoldAction
    }

    public static class InteractTypeExtensions {
        public static bool IsHoldAction(this InteractType type) => type == InteractType.HoldAction;
        public static string GetVerb(this InteractType type) => (type == InteractType.HoldAction) ? "Hold" : "Press";

        //NOTE: Is it a "suffix", or more accurate to call it a "predicate"? (in English)
        public static string GetMessage(this InteractType type, Key key, string suffix) {
            string verb = type.GetVerb();
            if (string.IsNullOrWhiteSpace(suffix))
                suffix = "interact";
            return verb + " the " + key + " key to " + suffix + ".";
        }
    }
}