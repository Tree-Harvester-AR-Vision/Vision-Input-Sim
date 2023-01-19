using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {
    public float Width;
    public float Height;
    public Vector3 Center;

    public override string ToString() {
        return $"{{Width: {Width}; Height: {Height}; Center: {Center.ToString()};}}";
    }

    void Start() {
        // calculate these values
    }

    void Update() {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 110, 90);
    }
}
