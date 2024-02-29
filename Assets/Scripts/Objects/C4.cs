using UnityEngine;
using Game.Scripts.Interfaces;

namespace Game.Scripts.LiveObjects {
    public class C4 : MonoBehaviour {
        [SerializeField] private GameObject explosionPrefab;

        private Collider[] hits = new Collider[5];

        public void Explode() {
            Vector3 pos = transform.position;
            GameObject.Instantiate(explosionPrefab, pos, Quaternion.identity);
            int count = Physics.OverlapSphereNonAlloc(pos, 1, hits);

            if (count > 0) {
                foreach (Collider obj in hits) {
                    if (obj != null && obj.TryGetComponent(out IDestroyable destroyable))
                        destroyable.DestroyAction();
                }
            }
            Destroy(gameObject);
        }

        public void Place(Transform target) {
            transform.SetPositionAndRotation(target.position, target.rotation);
            transform.parent = null;
        }
    }
}
