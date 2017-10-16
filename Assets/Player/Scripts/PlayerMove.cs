using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool isDead = false;

    private Rigidbody rigidbody;
    private Camera camera;
    private float cameraRotationY, timer = -1;
    private Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        animator = GameObject.Find("Hand").GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        rigidbody.MovePosition(rigidbody.position + GetMoveVector() * GetMoveSpeed());
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(GetPlayerRotation()));
        camera.transform.rotation = transform.rotation * Quaternion.Euler(GetCameraRotation());

        if (timer != -1)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f) timer = -1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("isPalkaHit", true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isPalkaHit", false);
        }
    }

    private Vector3 GetMoveVector()
    {
        Vector3 vector2D = Input.GetAxisRaw("Horizontal") * transform.right * 3 + Input.GetAxisRaw("Vertical") * transform.forward * 3;

        return vector2D + GetJump();
    }

    private Vector3 GetJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && timer == -1) timer = 0f;

        if (Input.GetKey(KeyCode.Space) && timer != -1)
        {
            return (Input.GetKey(KeyCode.Space) ? 1 : 0) * 4 * transform.up;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private float GetMoveSpeed()
    {
        return 0.04f;
    }

    private Vector3 GetPlayerRotation()
    {
        return new Vector3(Vector3.zero.x, GetRotation().y, Vector3.zero.z);
    }

    private Vector3 GetCameraRotation()
    {
        return new Vector3(GetRotation().x, Vector3.zero.y, Vector3.zero.z);
    }

    private Vector3 GetRotation()
    {
        cameraRotationY -= Input.GetAxis("Mouse Y");
        cameraRotationY = Mathf.Clamp(cameraRotationY, -70, 90);
        return new Vector3(cameraRotationY, Input.GetAxis("Mouse X"), Vector3.zero.z);
    }
}
