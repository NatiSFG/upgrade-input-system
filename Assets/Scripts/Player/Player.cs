using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;
using Cinemachine;

namespace Game.Scripts.Player {
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour {
        [SerializeField] private Detonator detonator;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private GameObject model;
        [SerializeField] private float speed = 5.0f;

        private CharacterController controller;
        private Animator anim;
        private bool isPlayerGrounded;
        private bool canMove = true;

        private void OnEnable() {
            InteractableZone.onZoneInteractionComplete += ShowDetonatorOrExplode;
            Laptop.onHackComplete += ReleasePlayerControl;
            Laptop.onHackEnded += ReturnPlayerControl;
            Forklift.onDriveModeEntered += ReleasePlayerControl;
            Forklift.onDriveModeExited += ReturnPlayerControl;
            Forklift.onDriveModeEntered += HidePlayer;
            Drone.OnEnterFlightMode += ReleasePlayerControl;
            Drone.onExitFlightmode += ReturnPlayerControl;
        }

        private void Start() {
            controller = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if (canMove == true)
                CalcutateMovement();
        }

        private void CalcutateMovement() {
            isPlayerGrounded = controller.isGrounded;
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            transform.Rotate(transform.up, h);

            var direction = transform.forward * v;
            var velocity = direction * speed;
            anim.SetFloat("Speed", Mathf.Abs(velocity.magnitude));

            if (isPlayerGrounded)
                velocity.y = 0f;
            if (!isPlayerGrounded)
                velocity.y += -20f * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void ShowDetonatorOrExplode(InteractableZone zone) {
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
            InteractableZone.onZoneInteractionComplete -= ShowDetonatorOrExplode;
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
