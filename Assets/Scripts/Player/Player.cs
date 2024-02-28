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
        [Header("Movement")]
        [SerializeField] private float currentWalkSpeed;
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float rotateSpeed = 1;


        private PlayerInputActions input;
        private Animator anim;
        private bool canWalk = true;

        private void Start() {
            input = new PlayerInputActions();
            input.PlayerMovement.Enable();

            anim = GetComponentInChildren<Animator>();
        }

        private void OnEnable() {
            InteractZone.onInteractionComplete += ShowDetonatorOrExplode;
            Laptop.onHackComplete += ReleasePlayerControl;
            Laptop.onHackEnded += ReturnPlayerControl;
            Forklift.onDriveModeEntered += ReleasePlayerControl;
            Forklift.onDriveModeExited += ReturnPlayerControl;
            Forklift.onDriveModeEntered += HidePlayer;
            Drone.onEnterFlightMode += ReleasePlayerControl;
            Drone.onExitFlightmode += ReturnPlayerControl;
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= ShowDetonatorOrExplode;
            Laptop.onHackComplete -= ReleasePlayerControl;
            Laptop.onHackEnded -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= ReleasePlayerControl;
            Forklift.onDriveModeExited -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= HidePlayer;
            Drone.onEnterFlightMode -= ReleasePlayerControl;
            Drone.onExitFlightmode -= ReturnPlayerControl;
        }

        private void Update(){
            if (canWalk)
                CalcutateMovement();
        }

        private void CalcutateMovement() {
            #region old code
            /*
            _playerGrounded = _controller.isGrounded;
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            transform.Rotate(transform.up, h);

            var direction = transform.forward * v;
            var velocity = direction * _speed;


            _anim.SetFloat("Speed", Mathf.Abs(velocity.magnitude));


            if (_playerGrounded)
                velocity.y = 0f;
            if (!_playerGrounded)
            {
                velocity.y += -20f * Time.deltaTime;
            }
            
            _controller.Move(velocity * Time.deltaTime);
            */
            #endregion
            Vector2 walk = input.PlayerMovement.Move.ReadValue<Vector2>();
            currentWalkSpeed = walk.magnitude > 0 ? walkSpeed : 0f;
            transform.Translate(new Vector3(walk.x, 0, walk.y) * Time.deltaTime * currentWalkSpeed);
            if (anim != null)
                anim.SetFloat("Speed", currentWalkSpeed);

            //float rotate = input.PlayerMovement.Rotate.ReadValue<Vector2>();
            //transform.Rotate(transform.up, rotate.normalized);
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
            canWalk = false;
            followCamera.Priority = 9;
        }

        private void ReturnPlayerControl() {
            model.SetActive(true);
            canWalk = true;
            followCamera.Priority = 10;
        }

        private void HidePlayer() {
            model.SetActive(false);
        }

        private void TriggerExplosive() {
            detonator.TriggerExplosion();
        }
    }
}
