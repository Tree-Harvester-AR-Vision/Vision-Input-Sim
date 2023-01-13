using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    Rigidbody rb;
    Vector2 moveDelta;
    Vector2 lookDelta;
    float xRot;
    float yRot;

    public float moveSpeed;
    public float rotateDiv;
    public float mouseSens;
    public GameObject playerCam;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookDelta = Vector2.zero;
    }

    // Update is called once per frame
    void Update() {
        rb.velocity = -transform.forward * moveDelta.y * moveSpeed;
        transform.Rotate(0, moveDelta.x / rotateDiv, 0);
        CameraLook();
    }

    private void OnMove(InputValue value) {
        moveDelta = value.Get<Vector2>();
    }

    private void OnLook(InputValue value) {
        lookDelta = value.Get<Vector2>();
    }

    private void CameraLook() {
        yRot += lookDelta.x * mouseSens;
        xRot -= lookDelta.y * mouseSens;

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        playerCam.transform.GetChild(0).rotation = Quaternion.Euler(xRot, yRot, 0);
        playerCam.transform.GetChild(1).rotation = Quaternion.Euler(xRot, yRot, 0);
    }
}
