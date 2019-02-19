using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.WOF.Sungsoo
{

    public class JoyStick : MonoBehaviour
    {
        [SerializeField] Player_Manager Player;
        // 공개
        [SerializeField]
        private Transform PlayerTr;        // 플레이어.

        [SerializeField]
        private Transform Stick;         // 조이스틱.

        [SerializeField]
        private Vector3 JoyVec;         // 조이스틱의 벡터(방향)

        // 비공개
        private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
        private float Radius;           // 조이스틱 배경의 반 지름.
        private bool MoveFlag;          // 플레이어 움직임 스위치.



        IEnumerator Start()
        {
            yield return PlayerTr.gameObject.activeSelf;
            PlayerTr = GameObject.FindWithTag("Player").transform;
            Player = PlayerTr.GetComponent<Player_Manager>();
            Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
            StickFirstPos = Stick.transform.position;

            // 캔버스 크기에대한 반지름 조절.
            float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
            Radius *= Can;

            MoveFlag = false;
        }


        void Update()
        {
            if (MoveFlag)
                PlayerTr.transform.Translate(Vector3.forward * Time.deltaTime * 7f);
        }

        // 드래그
        public void Drag(BaseEventData _Data)
        {
            MoveFlag = true;
            PointerEventData Data = _Data as PointerEventData;
            Vector3 Pos = Data.position;
            // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
            JoyVec = (Pos - StickFirstPos).normalized;
            // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
            float Dis = Vector3.Distance(Pos, StickFirstPos);
            // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
            if (Dis < Radius)
                Stick.position = StickFirstPos + JoyVec * Dis;
            // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
            else
                Stick.position = StickFirstPos + JoyVec * Radius;

            if (JoyVec.x > 0.3 && JoyVec.x < 1) // 앞으로
            {
                PlayerTr.eulerAngles = new Vector3(0, 90, 0);

            }

            if (JoyVec.x < -0.3 && JoyVec.x > -1) // 뒤로
            {
                PlayerTr.eulerAngles = new Vector3(0, -90, 0);

            }

            if (JoyVec.y > 0.6) // 점프
            {
                Player.IsJump();
            }
            //else if (JoyVec.x < 0)
            //{
            //    Player.eulerAngles = new Vector3(0, -90, 0);
            //}
            //Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0); // 플레이어 3d 이동.
            //Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);    
        }

        // 드래그 끝.
        public void DragEnd()
        {
            Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
            JoyVec = Vector3.zero;          // 방향을 0으로.
            MoveFlag = false;
        }
    }
}