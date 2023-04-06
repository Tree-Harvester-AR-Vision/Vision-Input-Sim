using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookDelta = Vector2.zero;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
        transform.Rotate(0, moveDelta.x / rotateDiv, 0);
        CameraLook();

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    
    Rigidbody rb;
    Vector2 moveDelta;
    Vector2 lookDelta;
    float xRot;
    float yRot;

    public float moveSpeed;
    public float rotateDiv;
    public float mouseSens;
    public GameObject playerCam;

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
