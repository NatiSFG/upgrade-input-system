using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    [SerializeField] private GameObject section;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Liftable"))
            other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Liftable"))
            other.transform.parent = section.transform;
    }
}
