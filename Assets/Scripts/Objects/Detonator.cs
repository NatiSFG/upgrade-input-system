using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects {
    public class Detonator : MonoBehaviour {
        [SerializeField] private C4 c4;
        [SerializeField] private InteractZone[] interacts;

        private bool c4Placed;
        private new MeshRenderer renderer;

        private void OnEnable() {
            InteractZone.onInteractionComplete += PlaceC4;
        }

        private void Start() {
            renderer = GetComponent<MeshRenderer>();
        }

        private void PlaceC4(InteractZone zone) {
            if (c4Placed != true && zone.GetZoneID() == 1) {
                PlaceC4(zone.GetItems()[0].transform);
                c4Placed = true;
            }
        }

        public void TriggerExplosion() {
            if (c4Placed == false)
                return;

            c4.Explode();
            c4Placed = false;
            interacts[1].CompleteTask(2);
            Destroy(this.gameObject);
        }

        void PlaceC4(Transform target) {
            c4.Place(target);
            c4.gameObject.SetActive(true);
            c4Placed = true;
            interacts[0].CompleteTask(1);
        }

        public void Show() {
            if (renderer != null)
                renderer.enabled = true;
        }

        private void Ondisable() {
            InteractZone.onInteractionComplete -= PlaceC4;
        }
    }
}
