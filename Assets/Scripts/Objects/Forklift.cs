using System;
using UnityEngine;
using Cinemachine;

namespace Game.Scripts.LiveObjects {
    public class Forklift : MonoBehaviour {
        [SerializeField] private GameObject lift, steeringWheel, leftWheel, rightWheel, rearWheels;
        [SerializeField] private Vector3 liftLowerLimit, liftUpperLimit;
        [SerializeField] private float speed = 5f, liftSpeed = 1f;
        [SerializeField] private CinemachineVirtualCamera forkliftCamera;
        [SerializeField] private GameObject driverModel;
        [SerializeField] private InteractableZone interactableZone;

        private bool inDriveMode = false;

        public static event Action onDriveModeEntered;
        public static event Action onDriveModeExited;

        private void OnEnable() {
            InteractableZone.onZoneInteractionComplete += EnterDriveMode;
        }

        private void EnterDriveMode(InteractableZone zone) {
            //Enter Forklift
            if (inDriveMode != true && zone.GetZoneID() == 5) {
                inDriveMode = true;
                forkliftCamera.Priority = 11;
                onDriveModeEntered?.Invoke();
                driverModel.SetActive(true);
                interactableZone.CompleteTask(5);
            }
        }

        private void ExitDriveMode() {
            inDriveMode = false;
            forkliftCamera.Priority = 9;
            driverModel.SetActive(false);
            onDriveModeExited?.Invoke();

        }

        private void Update() {
            if (inDriveMode == true) {
                LiftControls();
                CalcutateMovement();
                if (Input.GetKeyDown(KeyCode.Escape))
                    ExitDriveMode();
            }
        }

        private void CalcutateMovement() {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            var direction = new Vector3(0, 0, v);
            var velocity = direction * speed;

            transform.Translate(velocity * Time.deltaTime);

            if (Mathf.Abs(v) > 0) {
                var tempRot = transform.rotation.eulerAngles;
                tempRot.y += h * speed / 2;
                transform.rotation = Quaternion.Euler(tempRot);
            }
        }

        private void LiftControls() {
            if (Input.GetKey(KeyCode.R))
                LiftUpRoutine();
            else if (Input.GetKey(KeyCode.T))
                LiftDownRoutine();
        }

        private void LiftUpRoutine() {
            if (lift.transform.localPosition.y < liftUpperLimit.y) {
                Vector3 tempPos = lift.transform.localPosition;
                tempPos.y += Time.deltaTime * liftSpeed;
                lift.transform.localPosition = new Vector3(tempPos.x, tempPos.y, tempPos.z);
            } else if (lift.transform.localPosition.y >= liftUpperLimit.y)
                lift.transform.localPosition = liftUpperLimit;
        }

        private void LiftDownRoutine() {
            if (lift.transform.localPosition.y > liftLowerLimit.y) {
                Vector3 tempPos = lift.transform.localPosition;
                tempPos.y -= Time.deltaTime * liftSpeed;
                lift.transform.localPosition = new Vector3(tempPos.x, tempPos.y, tempPos.z);
            } else if (lift.transform.localPosition.y <= liftUpperLimit.y)
                lift.transform.localPosition = liftLowerLimit;
        }

        private void OnDisable() {
            InteractableZone.onZoneInteractionComplete -= EnterDriveMode;
        }
    }
}
