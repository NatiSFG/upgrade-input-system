using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects {
    public class Detonator : MonoBehaviour {
        [SerializeField] private C4 c4;
        [SerializeField] private InteractableZone[] interactableZone;

        private bool c4Placed;
        private new MeshRenderer renderer;

        private void OnEnable() {
            InteractableZone.onZoneInteractionComplete += PlaceC4;
        }

        private void Start() {
            renderer = GetComponent<MeshRenderer>();
        }

        private void PlaceC4(InteractableZone zone) {
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
            interactableZone[1].CompleteTask(2);
            Destroy(this.gameObject);
        }

        void PlaceC4(Transform target) {
            c4.Place(target);
            c4.gameObject.SetActive(true);
            c4Placed = true;
            interactableZone[0].CompleteTask(1);
        }

        public void Show() {
            renderer.enabled = true;
        }

        private void Ondisable() {
            InteractableZone.onZoneInteractionComplete -= PlaceC4;
        }
    }
}
