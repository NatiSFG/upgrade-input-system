using System;
using UnityEngine;
using Cinemachine;

namespace Game {
    public class Forklift : MonoBehaviour {
        [SerializeField] private GameObject lift;
        [SerializeField] private GameObject steeringWheel;
        [SerializeField] private GameObject leftTire;
        [SerializeField] private GameObject rightTire;
        [SerializeField] private GameObject backTires;

        [SerializeField] private Vector3 liftLowerLimit;
        [SerializeField] private Vector3 liftUpperLimit;

        [SerializeField] private float speed = 5;
        [SerializeField] private float liftSpeed = 5;

        [SerializeField] private CinemachineVirtualCamera forkliftCamera;
        [SerializeField] private GameObject driverModel;
        [SerializeField] private InteractZone interact;

        //TODO: Convert to use SequenceSystem, as a SequenceBehaviour or MultiSequenceBehaviour, for example.
        //private bool inDriveMode = false;

        //public static event Action onDriveModeEntered;
        //public static event Action onDriveModeExited;

        //private void OnEnable() {
        //    InteractZone.onInteractionComplete += EnterDriveMode;
        //}

        //private void OnDisable() {
        //    InteractZone.onInteractionComplete -= EnterDriveMode;
        //}

        //private void Update() {
        //    if (inDriveMode) {
        //        LiftControls();
        //        CalcutateMovement();
        //        if (Input.GetKeyDown(KeyCode.Escape))
        //            ExitDriveMode();
        //    }
        //}

        //private void EnterDriveMode(InteractZone zone) {
        //    //Enter Forklift
        //    if (!inDriveMode && zone.ZoneID == 5) {
        //        inDriveMode = true;
        //        forkliftCamera.Priority = 11;
        //        onDriveModeEntered?.Invoke();
        //        driverModel.SetActive(true);
        //        interact.CompleteTask(5);
        //    }
        //}

        //private void ExitDriveMode() {
        //    inDriveMode = false;
        //    forkliftCamera.Priority = 9;
        //    driverModel.SetActive(false);
        //    onDriveModeExited?.Invoke();
        //}

        //private void CalcutateMovement() {
        //    float h = Input.GetAxisRaw("Horizontal");
        //    float v = Input.GetAxisRaw("Vertical");
        //    Vector3 direction = new Vector3(0, 0, v);
        //    Vector3 velocity = direction * speed;

        //    transform.Translate(velocity * Time.deltaTime);

        //    if (Mathf.Abs(v) > 0) {
        //        Vector3 tempRot = transform.rotation.eulerAngles;
        //        tempRot.y += h * speed / 2;
        //        transform.rotation = Quaternion.Euler(tempRot);
        //    }
        //}

        //private void LiftControls() {
        //    if (Input.GetKey(KeyCode.R))
        //        LiftUpRoutine();
        //    else if (Input.GetKey(KeyCode.T))
        //        LiftDownRoutine();
        //}

        //private void LiftUpRoutine() {
        //    Vector3 localPos = lift.transform.localPosition;

        //    if (localPos.y < liftUpperLimit.y) {
        //        localPos.y += Time.deltaTime * liftSpeed;
        //        lift.transform.localPosition = localPos;
        //    } else if (localPos.y >= liftUpperLimit.y)
        //        lift.transform.localPosition = liftUpperLimit;
        //}

        //private void LiftDownRoutine() {
        //    Vector3 localPos = lift.transform.localPosition;

        //    if (localPos.y > liftLowerLimit.y) {
        //        localPos.y -= Time.deltaTime * liftSpeed;
        //        lift.transform.localPosition = localPos;
        //    } else if (localPos.y <= liftUpperLimit.y)
        //        lift.transform.localPosition = liftLowerLimit;
        //}
    }
}
