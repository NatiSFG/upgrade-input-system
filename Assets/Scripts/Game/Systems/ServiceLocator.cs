using UnityEngine;

namespace Game {
    /// <summary>
    /// Represents an object that has a list of all major game systems in the running app (in playmode only).
    /// </summary>
    /// <remarks>
    /// For simplicitly, this implementation only works in 1 scene.<br />
    /// This is based of the Gang of Four's Service Locator pattern for decoupling.
    /// </remarks>

    //NOTE: This makes this script guaranteed to have its Unity messages (Awake, Start, Update, etc.) get called FIRST before other scripts, by default,
    //unless you set THEIR scripting execution order to -10001 or lower (before ours).
    //You can also change this in Edit → ProjectSettings → Script Execution Order
    [DefaultExecutionOrder(-10000)]
    public class ServiceLocator : MonoBehaviour {
        private static ServiceLocator instance;
        public static ServiceLocator Instance {
            get {
                //NOTE: I would use this, but it requires a prefab in a Resources folder,
                //  and the SequenceSystem (as-is) requires a lot of scene-specific setup.
                //  This is fixable, but takes significant effort.
                //if (instance == null)
                //    instance = Resources.Load<ServiceLocator>("Service Locator");

                return instance;
            }
        }

        private void Awake() {
            if (instance != null && instance != this) {
                GameObject.DestroyImmediate(gameObject);
                Debug.LogError("Duplicate " + nameof(ServiceLocator) + " was found, and automatically deleted!");
                return;
            }
            instance = this;
        }

        //NOTE: Just a nice abstraction for now, but it's easy to add features to this ServiceLocator implementation in the future.
        //  (WHICH IS VERY IMPORTANT!)
        public T GetSystem<T>() {
            return GetComponentInChildren<T>();
        }
    }
}