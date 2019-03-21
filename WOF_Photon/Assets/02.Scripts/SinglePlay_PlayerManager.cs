using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.WOF.Sungsoo
{
    public class SinglePlay_PlayerManager : MonoBehaviour
    {
        CharacterController controller;
        Animator animator;
        public Transform[] respawns;

        private Vector3 MoveDir;
        public bool isJump = false;
        int jumpCount = 2;
        public bool alreadyJump;
        public float jumpSpeed = 7.0f;
        public float gravity = 1.0f;

        [SerializeField] JoyStick joystick;

        void Awake()
        {

        }
        // Use this for initialization
        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            MoveDir = Vector3.zero;
            respawns = GameObject.Find("Respawn").GetComponentsInChildren<Transform>();
        }

        public void IsJump()
        {
            if (jumpCount > 0)
            {
                jumpCount--;
                isJump = true;
                StartCoroutine(AJ());
            }
        }

        IEnumerator AJ()
        {
            yield return null;
            alreadyJump = true;
            yield return new WaitForSeconds(0.5f);
            alreadyJump = false;
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


            if (controller.isGrounded)
                jumpCount = 2;

            MoveDir.y -= gravity * Time.deltaTime;
            controller.Move(MoveDir * Time.deltaTime);
        }


        public void MoveAnimator(string animName, bool animState)
        {
            animator.SetBool(animName, animState);
        }

    }
}
