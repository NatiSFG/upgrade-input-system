using System;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects {
    public class Drone : MonoBehaviour {
        private enum Tilt {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Animator propAnim;
        [SerializeField] private CinemachineVirtualCamera droneCamera;
        [SerializeField] private InteractZone interact;

        private bool inFlightMode = false;

        public static event Action onEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable() {
            InteractZone.onInteractionComplete += EnterFlightMode;
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= EnterFlightMode;
        }

        private void EnterFlightMode(InteractZone zone) {
            //drone cutscene
            if (inFlightMode != true && zone.GetZoneID() == 4) {
                propAnim.SetTrigger("Start Flying");
                droneCamera.Priority = 11;
                inFlightMode = true;
                onEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                interact.CompleteTask(4);
            }
        }

        private void ExitFlightMode() {
            droneCamera.Priority = 9;
            inFlightMode = false;
            UIManager.Instance.DroneView(false);
        }

        private void Update() {
            if (inFlightMode) {
                CalculateTilt();
                Rotate();

                if (Input.GetKeyDown(KeyCode.Escape)) {
                    inFlightMode = false;
                    onExitFlightmode?.Invoke();
                    ExitFlightMode();
                }
            }
        }

        private void FixedUpdate() {
            rb.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
            if (inFlightMode)
                UpAndDownMovement();
        }

        private void Rotate() {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y -= speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y += speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }
        }

        private void UpAndDownMovement() {
            if (Input.GetKey(KeyCode.F))
                rb.AddForce(transform.up * speed, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.V))
                rb.AddForce(-transform.up * speed, ForceMode.Acceleration);
        }

        private void CalculateTilt() {
            if (Input.GetKey(KeyCode.A)) transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            else if (Input.GetKey(KeyCode.D)) transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            else if (Input.GetKey(KeyCode.W)) transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            else if (Input.GetKey(KeyCode.S)) transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            else transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }
    }
}
