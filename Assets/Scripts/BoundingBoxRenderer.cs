using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxRenderer : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        try {
            if (other.transform.GetChild(0).GetChild(0).TryGetComponent(out BoundingBox box)) {
                box.gameObject.SetActive(true);
            }
        } catch {}
    }

    private void OnTriggerExit(Collider other) {
        try {
            if (other.transform.GetChild(0).GetChild(0).TryGetComponent(out BoundingBox box)) {
                box.gameObject.SetActive(false);
            }
        } catch {}
    }
}
