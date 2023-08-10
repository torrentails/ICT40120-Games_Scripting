using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]

    public float moveSpeed = 8f;
    public float gravity = -20f;

    CharacterController controller;
    Vector3 inputVector;

    [Header("Aiming")]
    public Transform lookCamera;
    public float sensitivity_x = 15f;
    public float sensitivity_y = 15f;

    public float min_y = -90;
    public float max_y = 90;

    float currentYRotation;
    Vector2 aimVector;

    [Header("Shooting")]
    public float shootRange = 500f;
    public LayerMask shootMask;
    public float fireRate = 0.1f;
    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Look();
    }

    void GetInput()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");

        aimVector.x = Input.GetAxis("Mouse X");
        aimVector.y = Input.GetAxis("Mouse Y");

        if(Input.GetButtonDown("Fire1") && !isFiring) {
            Shoot();
        }
    }

    void Move()
    {
        Vector3 moveVector = transform.TransformDirection(inputVector.normalized);
        moveVector *= moveSpeed;
        moveVector.y = -gravity;
        moveVector *= Time.deltaTime;
        controller.Move(moveVector);
    }

    void Look()
    {
        transform.Rotate(transform.up, aimVector.x * sensitivity_x);

        currentYRotation += aimVector.y * sensitivity_y;

        currentYRotation = Mathf.Clamp(currentYRotation, min_y, max_y);

        lookCamera.eulerAngles = new Vector3(-currentYRotation, lookCamera.eulerAngles.y, lookCamera.eulerAngles.z);
    }

    void Shoot() {
        RaycastHit hit;
        if(Physics.Raycast(lookCamera.position, lookCamera.forward, out hit, shootRange, shootMask)) {
            // isFiring = true;
            print(hit.point.ToString());
            StartCoroutine(FireRoutine());
        }
    }

    IEnumerator FireRoutine() {
        isFiring = true;
        yield return new WaitForSeconds(fireRate);
        isFiring = false;
    }
}
