using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Cinemachine;

namespace Game.Scripts.LiveObjects {
    public class Laptop : MonoBehaviour {
        [SerializeField] private Slider progressBar;
        [SerializeField] private int hackTime = 5;
        [SerializeField] private CinemachineVirtualCamera[] cameras;
        [SerializeField] private InteractZone interact;

        private int activeCamera = 0;
        private bool isHacked = false;

        public static event Action onHackComplete;
        public static event Action onHackEnded;

        private void OnEnable() {
            InteractZone.onHoldStarted += InteractableZone_onHoldStarted;
            InteractZone.onHoldEnded += InteractableZone_onHoldEnded;
        }

        private void Update() {
            if (isHacked == true) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    var previous = activeCamera;
                    activeCamera++;

                    if (activeCamera >= cameras.Length)
                        activeCamera = 0;

                    cameras[activeCamera].Priority = 11;
                    cameras[previous].Priority = 9;
                }

                if (Input.GetKeyDown(KeyCode.Escape)) {
                    isHacked = false;
                    onHackEnded?.Invoke();
                    ResetCameras();
                }
            }
        }

        void ResetCameras() {
            foreach (var cam in cameras) {
                cam.Priority = 9;
            }
        }

        private void InteractableZone_onHoldStarted(int zoneID) {
            if (zoneID == 3 && isHacked == false) //Hacking terminal
            {
                progressBar.gameObject.SetActive(true);
                StartCoroutine(HackingRoutine());
                onHackComplete?.Invoke();
            }
        }

        private void InteractableZone_onHoldEnded(int zoneID) {
            if (zoneID == 3) //Hacking terminal
            {
                if (isHacked == true)
                    return;

                StopAllCoroutines();
                progressBar.gameObject.SetActive(false);
                progressBar.value = 0;
                onHackEnded?.Invoke();
            }
        }


        IEnumerator HackingRoutine() {
            while (progressBar.value < 1) {
                progressBar.value += Time.deltaTime / hackTime;
                yield return new WaitForEndOfFrame();
            }

            //successfully hacked
            isHacked = true;
            interact.CompleteTask(3);

            //hide progress bar
            progressBar.gameObject.SetActive(false);

            //enable Vcam1
            cameras[0].Priority = 11;
        }

        private void OnDisable() {
            InteractZone.onHoldStarted -= InteractableZone_onHoldStarted;
            InteractZone.onHoldEnded -= InteractableZone_onHoldEnded;
        }
    }
}
