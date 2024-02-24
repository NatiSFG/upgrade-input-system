using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Interfaces;

namespace Game.Scripts.LiveObjects {
    public class Taxi : MonoBehaviour, IDestroyable {
        [SerializeField] private GameObject onDestroyed;

        public void DestroyAction() {
            onDestroyed.transform.SetPositionAndRotation(transform.position, transform.rotation);
            onDestroyed.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
