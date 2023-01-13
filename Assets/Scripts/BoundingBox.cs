using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {

    void Update() {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 110, 90);
    }
}
