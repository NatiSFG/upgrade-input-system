using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private InteractableZone interactZone;

        private bool inFlightMode = false;

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable() {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
        }

        private void EnterFlightMode(InteractableZone zone) {
            //drone cutscene
            if (inFlightMode != true && zone.GetZoneID() == 4) {
                propAnim.SetTrigger("StartProps");
                droneCamera.Priority = 11;
                inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                interactZone.CompleteTask(4);
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
                CalculateMovementUpdate();

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
                CalculateMovementFixedUpdate();
        }

        private void CalculateMovementUpdate() {
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

        private void CalculateMovementFixedUpdate() {
            if (Input.GetKey(KeyCode.Space))
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

        private void OnDisable() {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
