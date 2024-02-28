using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.UI;


namespace Game.Scripts.LiveObjects {
    public class InteractZone : MonoBehaviour {
        private enum ZoneType {
            Collectable,
            Action,
            HoldAction
        }

        private enum KeyState {
            Press,
            PressHold
        }

        [SerializeField] private ZoneType zoneType;
        [SerializeField] private int zoneID, requiredID;
        [SerializeField] [Tooltip("Press the (---) Key to .....")] private string displayMessage;
        [SerializeField] private GameObject[] zoneItems;
        [SerializeField] private Sprite inventoryIcon;
        [SerializeField] private KeyCode zoneKeyInput;
        [SerializeField] private KeyState keyState;
        [SerializeField] private GameObject marker;

        private bool inZone = false;
        private bool itemsCollected = false;
        private bool actionPerformed = false;
        private bool inHoldState = false;
        private static int currentZoneID = 0;

        public static int CurrentZoneID { get; set; }

        public static event Action<InteractZone> onInteractionComplete;
        public static event Action<int> onHoldStarted;
        public static event Action<int> onHoldEnded;

        private void OnEnable() {
            InteractZone.onInteractionComplete += SetMarker;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && currentZoneID > requiredID) {
                switch (zoneType) {
                    case ZoneType.Collectable:
                        if (itemsCollected == false) {
                            inZone = true;
                            if (displayMessage != null) {
                                string message = $"Press the {zoneKeyInput.ToString()} key to {displayMessage}.";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            } else UIManager.Instance.DisplayInteractableZoneMessage(true, $"Press the {zoneKeyInput.ToString()} key to collect");
                        }
                        break;

                    case ZoneType.Action:
                        if (actionPerformed == false) {
                            inZone = true;
                            if (displayMessage != null) {
                                string message = $"Press the {zoneKeyInput.ToString()} key to {displayMessage}.";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            } else UIManager.Instance.DisplayInteractableZoneMessage(true, $"Press the {zoneKeyInput.ToString()} key to perform action");
                        }
                        break;

                    case ZoneType.HoldAction:
                        inZone = true;
                        if (displayMessage != null) {
                            string message = $"Press the {zoneKeyInput.ToString()} key to {displayMessage}.";
                            UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                        } else UIManager.Instance.DisplayInteractableZoneMessage(true, $"Hold the {zoneKeyInput.ToString()} key to perform action");
                        break;
                }
            }
        }

        private void Update() {
            if (inZone && Input.GetKeyDown(zoneKeyInput) && keyState != KeyState.PressHold) {
                switch (zoneType) {
                    case ZoneType.Collectable:
                        if (itemsCollected == false) {
                            CollectItems();
                            itemsCollected = true;
                            UIManager.Instance.DisplayInteractableZoneMessage(false);
                        }
                        break;

                    case ZoneType.Action:
                        if (actionPerformed == false) {
                            PerformAction();
                            actionPerformed = true;
                            UIManager.Instance.DisplayInteractableZoneMessage(false);
                        }
                        break;
                }
            }
            else if (Input.GetKey(zoneKeyInput) && keyState == KeyState.PressHold && inHoldState == false) {
                inHoldState = true;
                switch (zoneType) {
                    case ZoneType.HoldAction:
                        PerformHoldAction();
                        break;
                }
            }
            if (Input.GetKeyUp(zoneKeyInput) && keyState == KeyState.PressHold) {
                inHoldState = false;
                onHoldEnded?.Invoke(zoneID);
            }
        }

        private void CollectItems() {
            foreach (var item in zoneItems) {
                item.SetActive(false);
            }
            UIManager.Instance.UpdateInventoryDisplay(inventoryIcon);
            CompleteTask(zoneID);
            onInteractionComplete?.Invoke(this);
        }

        private void PerformAction() {
            foreach (var item in zoneItems) {
                item.SetActive(true);
            }

            if (inventoryIcon != null)
                UIManager.Instance.UpdateInventoryDisplay(inventoryIcon);
            onInteractionComplete?.Invoke(this);
        }

        private void PerformHoldAction() {
            UIManager.Instance.DisplayInteractableZoneMessage(false);
            onHoldStarted?.Invoke(zoneID);
        }

        public GameObject[] GetItems() {
            return zoneItems;
        }

        public int GetZoneID() {
            return zoneID;
        }

        public void CompleteTask(int zoneID) {
            if (zoneID == this.zoneID) {
                currentZoneID++;
                onInteractionComplete?.Invoke(this);
            }
        }

        public void ResetAction(int zoneID) {
            if (zoneID == this.zoneID)
                actionPerformed = false;
        }

        public void SetMarker(InteractZone zone) {
            if (zoneID == currentZoneID)
                marker.SetActive(true);
            else marker.SetActive(false);
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                inZone = false;
                UIManager.Instance.DisplayInteractableZoneMessage(false);
            }
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= SetMarker;
        }
    }
}
