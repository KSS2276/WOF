using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Mobile : MonoBehaviour
{

    public float jumpPower = 6f;
    Rigidbody rigidbody; // 리지드바디
    Vector3 movement; // Vector3 값 변수형
    bool isJumping;
    bool isGrounded;
    bool clickJump = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    public void ClickJump()
    {
        clickJump = true;
    }

    void Update()
    {
        if (clickJump && isGrounded)
        {
            clickJump = false;
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        Jump();
    }


    void OnCollisionStay()
    {
        isGrounded = true;
    }


    public void Jump() // 점프 함수
    {
        if (!isJumping)
            return;

        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;
        isJumping = false;
        clickJump = false;
    }
}