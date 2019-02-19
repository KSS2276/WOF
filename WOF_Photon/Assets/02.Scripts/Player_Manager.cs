using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.WOF.Sungsoo
{
    public class Player_Manager : MonoBehaviour
    {

        CharacterController controller;
        public Transform[] respawns;

        private Vector3 MoveDir;

        public bool isJump = false;
        public float jumpSpeed = 5.0f;
        public float gravity = 20.0f;

        [SerializeField] JoyStick joystick;


        // Use this for initialization
        IEnumerator Start()
        {
            yield return null;
            controller = GetComponent<CharacterController>();
            MoveDir = Vector3.zero;
            respawns = GameObject.Find("Respawn").GetComponentsInChildren<Transform>();
        }

        public void IsJump()
        {
            isJump = true;
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

        //void OnTriggerEnter(Collider col)
        //{
        //    if (col.gameObject.name == "DropDie")
        //    {
        //        int a = Random.RandomRange(0, respawnPlace.Length);
        //    }
        //}

    }
}
