using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererFollow : MonoBehaviour {
    Transform Player;
    Vector3 originalPos;

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindWithTag("Player").transform;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        //transform.LookAt(Player);
        //transform.position = originalPos + new Vector3(transform.forward.x, 0, transform.forward.z);
    }
}
