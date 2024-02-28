using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
            InteractZone.onHoldStarted += HackingInProgress;
            InteractZone.onHoldEnded += FinishedHacking;
        }

        private void Update() {
            if (isHacked) {
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

        private void HackingInProgress(int zoneID) {
            //Hacking cameras
            if (zoneID == 3 && isHacked == false) {
                progressBar.gameObject.SetActive(true);
                StartCoroutine(HackingRoutine());
                onHackComplete?.Invoke();
            }
        }

        private void FinishedHacking(int zoneID) {
            //hacking cameras
            if (zoneID == 3) {
                if (isHacked)
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

            isHacked = true;
            interact.CompleteTask(3);

            progressBar.gameObject.SetActive(false);
            cameras[0].Priority = 11;
        }

        private void OnDisable() {
            InteractZone.onHoldStarted -= HackingInProgress;
            InteractZone.onHoldEnded -= FinishedHacking;
        }
    }
}
