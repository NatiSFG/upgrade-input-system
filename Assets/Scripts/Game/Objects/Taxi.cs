using UnityEngine;

namespace Game {
    public class Taxi : MonoBehaviour, IDestroyable {
        [SerializeField] private GameObject wreckedTaxi;

        public void DestroyAction() {
            wreckedTaxi.transform.SetPositionAndRotation(transform.position, transform.rotation);
            wreckedTaxi.SetActive(true);
            Destroy(gameObject);
        }
    }
}
