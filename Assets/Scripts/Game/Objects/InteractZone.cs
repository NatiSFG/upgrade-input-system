using System;
using UnityEngine;
using Game.UI; //TODO: UI should listen and reach into the game state so it knows what to show. NOT the other way around.

namespace Game {
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

        #region Static Section
        private static int currentZoneID = 0;
        public static int CurrentZoneID {
            get { return currentZoneID; }
            set { currentZoneID = value; }
        }

        public static event Action<InteractZone> onInteractionComplete;
        public static event Action<int> onHoldStarted;
        public static event Action<int> onHoldEnded;
        #endregion

        [SerializeField] private ZoneType zoneType;
        [SerializeField] private KeyCode zoneInputKey;
        [SerializeField] private KeyState keyState;

        [Tooltip("The zone ID that this object responds to.")]
        [SerializeField] private int zoneID;


        [Tooltip("Press the (---) Key to .....")]
        [SerializeField] private string displayMessage;

        [SerializeField] private GameObject[] zoneItems;
        [SerializeField] private Sprite inventoryIcon;
        [SerializeField] private GameObject marker;

        private bool inZone = false;
        private bool itemsCollected = false;
        private bool actionPerformed = false;
        private bool inHoldState = false;

        public int ZoneID => zoneID;

        private void OnEnable() {
            InteractZone.onInteractionComplete += SetMarker;
        }

        private void OnDisable() {
            InteractZone.onInteractionComplete -= SetMarker;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && currentZoneID > zoneID - 1) {
                Debug.Log("zone ID: " + currentZoneID);
                switch (zoneType) {
                    case ZoneType.Collectable:
                        if (itemsCollected == false) {
                            inZone = true;
                            if (displayMessage != null) {
                                string message = "Press the " + zoneInputKey + " key to " + displayMessage + ".";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            } else UIManager.Instance.DisplayInteractableZoneMessage(true, "Press the " + zoneInputKey + " key to collect");
                        }
                        break;

                    case ZoneType.Action:
                        if (actionPerformed == false) {
                            inZone = true;
                            if (displayMessage != null) {
                                string message = $"Press the " + zoneInputKey + " key to " + displayMessage + ".";
                                UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                            } else UIManager.Instance.DisplayInteractableZoneMessage(true, "Press the " + zoneInputKey + " key to perform action");
                        }
                        break;

                    case ZoneType.HoldAction:
                        inZone = true;
                        if (displayMessage != null) {
                            string message = "Hold the " + zoneInputKey + " key to " + displayMessage + ".";
                            UIManager.Instance.DisplayInteractableZoneMessage(true, message);
                        } else UIManager.Instance.DisplayInteractableZoneMessage(true, "Hold the " + zoneInputKey + " key to perform action");
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                inZone = false;
                UIManager.Instance.DisplayInteractableZoneMessage(false);
            }
        }

        private void Update() {
            if (inZone && Input.GetKeyDown(zoneInputKey) && keyState != KeyState.PressHold) {
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
            else if (Input.GetKey(zoneInputKey) && keyState == KeyState.PressHold && inHoldState == false) {
                inHoldState = true;
                switch (zoneType) {
                    case ZoneType.HoldAction:
                        PerformHoldAction();
                        break;
                }
            }
            if (Input.GetKeyUp(zoneInputKey) && keyState == KeyState.PressHold) {
                inHoldState = false;
                Debug.LogWarning("--- onHoldEnded");
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
            Debug.LogWarning("+++ onHoldStarted");
            UIManager.Instance.DisplayInteractableZoneMessage(false);
            onHoldStarted?.Invoke(currentZoneID);
        }

        public GameObject[] GetItems() {
            return zoneItems;
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
    }
}
