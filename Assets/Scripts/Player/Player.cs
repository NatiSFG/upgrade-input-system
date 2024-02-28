using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Player {
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour {
        [SerializeField] private Detonator detonator;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private GameObject model;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float walkSpeed = 2.5f;

        private PlayerInputActions input;
        private Animator anim;
        private bool canMove = true;

        private void OnEnable() {
            InteractZone.onInteractionComplete += ShowDetonatorOrExplode;
            Laptop.onHackComplete += ReleasePlayerControl;
            Laptop.onHackEnded += ReturnPlayerControl;
            Forklift.onDriveModeEntered += ReleasePlayerControl;
            Forklift.onDriveModeExited += ReturnPlayerControl;
            Forklift.onDriveModeEntered += HidePlayer;
            Drone.OnEnterFlightMode += ReleasePlayerControl;
            Drone.onExitFlightmode += ReturnPlayerControl;
        }

        private void Start() {
            input = new PlayerInputActions();
            input.PlayerMovement.Enable();

            anim = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if (canMove)
                CalcutateMovement();
        }

        private void CalcutateMovement() {
            if (anim != null) {
                Vector2 move = input.PlayerMovement.Move.ReadValue<Vector2>();
                currentSpeed = move.magnitude > 0 ? walkSpeed : 0f;
                anim.SetFloat("Speed", currentSpeed);
                transform.Translate(new Vector3(move.x, 0, move.y) * Time.deltaTime * currentSpeed);
            }
        }

        private void ShowDetonatorOrExplode(InteractZone zone) {
            switch (zone.GetZoneID()) {
                case 1: //place c4
                    detonator.Show();
                    break;
                case 2: //Trigger Explosion
                    TriggerExplosive();
                    break;
            }
        }

        private void ReleasePlayerControl() {
            canMove = false;
            followCamera.Priority = 9;
        }

        private void ReturnPlayerControl() {
            model.SetActive(true);
            canMove = true;
            followCamera.Priority = 10;
        }

        private void HidePlayer() {
            model.SetActive(false);
        }

        private void TriggerExplosive() {
            detonator.TriggerExplosion();
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= ShowDetonatorOrExplode;
            Laptop.onHackComplete -= ReleasePlayerControl;
            Laptop.onHackEnded -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= ReleasePlayerControl;
            Forklift.onDriveModeExited -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= HidePlayer;
            Drone.OnEnterFlightMode -= ReleasePlayerControl;
            Drone.onExitFlightmode -= ReturnPlayerControl;
        }
    }
}
