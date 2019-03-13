using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.WOF.Sungsoo
{
    public class Player_Manager : MonoBehaviour
    {

        CharacterController controller;
        Rigidbody rb;
        Animator animator;
        public Transform[] respawns;

        private Vector3 MoveDir;

        public bool isJump = false;
        int jumpCount = 2;
        bool alreadyJump;
        public float jumpSpeed = 7.0f;
        public float gravity = 1.0f;

        [SerializeField] JoyStick joystick;


        // Use this for initialization
        void Start()
        {
            controller = GetComponent<CharacterController>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            MoveDir = Vector3.zero;
            respawns = GameObject.Find("Respawn").GetComponentsInChildren<Transform>();
        }

        public void IsJump()
        {
            if (jumpCount > 0)
            {
                isJump = true;
                JoyStick.jumpComplete = false;
            }

        }

        public void ChangLayer(int num)
        {
            gameObject.layer = num;
        }

        // Update is called once per frame
        void Update()
        {

            if (controller.isGrounded && isJump)
            {
                MoveDir.y = jumpSpeed;
            }
            isJump = false;

            MoveDir.y -= gravity * Time.deltaTime;
            controller.Move(MoveDir * Time.deltaTime);
        }


        public void MoveAnimator(string animName, bool animState)
        {
            animator.SetBool(animName, animState);
        }

    }
}
