using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Interfaces;

namespace Game.Scripts.LiveObjects {
    public class Taxi : MonoBehaviour, IDestroyable {
        [SerializeField] private GameObject wreckedTaxi;

        public void DestroyAction() {
            wreckedTaxi.transform.SetPositionAndRotation(transform.position, transform.rotation);
            wreckedTaxi.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
