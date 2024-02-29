using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Game {
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour {
        [SerializeField] private Detonator detonator;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private GameObject model;
        [SerializeField] private CharacterController controller;

        [Header("Movement")]
        [SerializeField] private float currentWalkSpeed;
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float rotateSpeed = 1;

        [Header("Input")]
        [SerializeField] private InputActionAsset sharedPlayerMovement;
        [SerializeField] private string actionMapName = "Player Movement";
        [SerializeField] private InputActionReference sharedMove;
        [SerializeField] private InputActionReference sharedRotate;

        private InputActionMap playerMovement;
        private InputAction move;
        private InputAction rotate;
        private Animator anim;
        private int speedId;
        private Vector3 velocity;

        private void Start() {
            anim = GetComponentInChildren<Animator>();
            speedId = Animator.StringToHash("Speed");

            playerMovement = sharedPlayerMovement.FindActionMap(actionMapName).Clone();
            playerMovement.Enable();
            move = playerMovement[sharedMove.action.name];
            rotate = playerMovement[sharedRotate.action.name];
        }

        private void OnDestroy() {
            playerMovement.Dispose();
            playerMovement = null;
            move = null;
            rotate = null;
        }

        private void OnEnable() {
            //TODO: Convert to use SequenceSystem, as a SequenceBehaviour or MultiSequenceBehaviour, for example.
            //InteractZone.onInteractionComplete += ShowDetonatorOrExplode;
            //Laptop.onHackComplete += ReleasePlayerControl;
            //Laptop.onHackEnded += ReturnPlayerControl;
            //Forklift.onDriveModeEntered += ReleasePlayerControl;
            //Forklift.onDriveModeExited += ReturnPlayerControl;
            //Forklift.onDriveModeEntered += HidePlayer;
            //Drone.onEnterFlightMode += ReleasePlayerControl;
            //Drone.onExitFlightmode += ReturnPlayerControl;
        }

        private void OnDisable() {
            //TODO: Convert to use SequenceSystem, as a SequenceBehaviour or MultiSequenceBehaviour, for example.
            //InteractZone.onInteractionComplete -= ShowDetonatorOrExplode;
            //Laptop.onHackComplete -= ReleasePlayerControl;
            //Laptop.onHackEnded -= ReturnPlayerControl;
            //Forklift.onDriveModeEntered -= ReleasePlayerControl;
            //Forklift.onDriveModeExited -= ReturnPlayerControl;
            //Forklift.onDriveModeEntered -= HidePlayer;
            //Drone.onEnterFlightMode -= ReleasePlayerControl;
            //Drone.onExitFlightmode -= ReturnPlayerControl;
        }

        private void Update() {
            CalcutateMovement();
        }

        private void CalcutateMovement() {
            Vector2 moveInput = move.ReadValue<Vector2>();
            currentWalkSpeed = (moveInput.magnitude > 0) ? walkSpeed : 0;

            Vector3 prevVelocity = velocity;
            velocity = currentWalkSpeed * transform.forward * moveInput.y;

            if (controller.isGrounded)
                velocity.y = 0;
            else
                velocity.y = prevVelocity.y + Physics.gravity.y * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (anim != null)
                anim.SetFloat(speedId, currentWalkSpeed);

            float rotationalInput = rotate.ReadValue<float>();
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y += rotateSpeed * rotationalInput;
            transform.eulerAngles = eulerAngles;
        }

        private void ShowDetonatorOrExplode(InteractZone zone) {
            //TODO: Convert to use SequenceSystem, as a SequenceBehaviour or MultiSequenceBehaviour, for example.
            //switch (zone.ZoneID) {
            //    case 1: //place c4
            //        detonator.Show();
            //        break;
            //    case 2: //Trigger Explosion
            //        TriggerExplosive();
            //        break;
            //}
        }

        private void ReleasePlayerControl() {
            followCamera.Priority = 9;
        }

        private void ReturnPlayerControl() {
            model.SetActive(true);
            followCamera.Priority = 10;
        }

        private void HidePlayer() {
            model.SetActive(false);
        }

        private void TriggerExplosive() {
            //detonator.TriggerExplosion();
        }
    }
}
