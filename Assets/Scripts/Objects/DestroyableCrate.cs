using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects {
    public class Crate : MonoBehaviour {
        [SerializeField] private float punchDelay;
        [SerializeField] private GameObject wholeCrate, brokenCrate;
        [SerializeField] private Rigidbody[] pieces;
        [SerializeField] private BoxCollider crateCollider;
        [SerializeField] private InteractZone interact;

        private bool isReadyToBreak = false;
        private List<Rigidbody> brakeOff = new List<Rigidbody>();

        private void OnEnable() {
            InteractZone.onInteractionComplete += BreakCrate;
        }

        private void BreakCrate(InteractZone zone) {
            if (isReadyToBreak == false && brakeOff.Count > 0) {
                wholeCrate.SetActive(false);
                brokenCrate.SetActive(true);
                isReadyToBreak = true;
            }
            //crate zone
            if (isReadyToBreak && zone.GetZoneID() == 6) {
                if (brakeOff.Count > 0) {
                    BreakPart();
                    StartCoroutine(PunchDelay());
                } else if (brakeOff.Count == 0) {
                    isReadyToBreak = false;
                    crateCollider.enabled = false;
                    interact.CompleteTask(6);
                    Debug.Log("Completely Busted");
                }
            }
        }

        private void Start() {
            brakeOff.AddRange(pieces);
        }

        public void BreakPart() {
            int rng = Random.Range(0, brakeOff.Count);
            brakeOff[rng].constraints = RigidbodyConstraints.None;
            brakeOff[rng].AddForce(new Vector3(1f, 1f, 1f), ForceMode.Force);
            brakeOff.Remove(brakeOff[rng]);
        }

        IEnumerator PunchDelay() {
            float delayTimer = 0;
            while (delayTimer < punchDelay) {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }
            interact.ResetAction(6);
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= BreakCrate;
        }
    }
}
