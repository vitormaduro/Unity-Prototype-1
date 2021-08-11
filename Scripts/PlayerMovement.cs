using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Transform camera;
    Animator animator;
    Vector3 playerVelocity;
    float vSpeed;

    public float playerSpeed = 20.0f;
    public float jumpSpeed = 0.2f;
    public float gravitySpeed = 1f;
    public bool isInventoryOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        camera = gameObject.GetComponentInChildren<Camera>().transform;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInventoryOpen) return;

        float horizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;

        Vector3 movement = camera.transform.right * horizontal + camera.transform.forward * vertical;
        if(controller.isGrounded)
        {
            vSpeed = 0f;
            if(Input.GetButtonDown("Jump"))
            {
                vSpeed = jumpSpeed;
            }
        }

        vSpeed -= gravitySpeed * Time.deltaTime;

        movement.y = vSpeed;
        controller.Move(movement);

        if(movement.x != 0f || movement.z != 0f) {
            animator.SetBool("isMoving", true);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 250 * Time.deltaTime);

        Quaternion camRotation = camera.rotation;
        camRotation.x = 0f;
        camRotation.z = 0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, camRotation, 0.1f);
    }
}