using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class BoundingBox : MonoBehaviour {
    [JsonProperty] public float Width; // Width in terms of player
    [JsonProperty] public float Height; // Height in terms of player
    [JsonProperty] public Vector3 Center; // Center in terms of player

    private Vector3 TLCorner;
    private Vector3 BLCorner;
    private Vector3 TRCorner;
    private Vector3 centerGlobal;
    private Transform MainCamera;

    public static bool isEqual(BoundingBox box, Vector3 center, float width, float height) {
        if (
            box.Width == width &&
            box.Height == height &&
            box.Center == center
        ) return true;
        else return false;
    }

    void Start() {
        // calculate these values
        centerGlobal = transform.TransformPoint(GetComponent<MeshFilter>().sharedMesh.vertices[60]);
        TLCorner = transform.TransformPoint(GetComponent<MeshFilter>().sharedMesh.vertices[0]);
        TRCorner = transform.TransformPoint(GetComponent<MeshFilter>().sharedMesh.vertices[110]);
        BLCorner = transform.TransformPoint(GetComponent<MeshFilter>().sharedMesh.vertices[10]);

        MainCamera = Camera.main.transform;

        gameObject.SetActive(false);
    }

    void Update() {
        transform.LookAt(MainCamera.position);
        transform.Rotate(0, 110, 90);

        // Calculate values in terms of player's vision
        Center = MainCamera.InverseTransformPoint(centerGlobal);
        Width = Vector3.Distance(MainCamera.InverseTransformPoint(TLCorner), MainCamera.InverseTransformPoint(TRCorner));
        Height = Vector3.Distance(MainCamera.InverseTransformPoint(TLCorner), MainCamera.InverseTransformPoint(BLCorner));
    }
}
