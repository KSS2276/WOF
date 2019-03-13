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

        public Vector3 JoyVec;         // 조이스틱의 벡터(방향)
        public static bool jumpComplete;

        // 비공개
        private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
        private float Radius;           // 조이스틱 배경의 반 지름.
        private bool MoveFlag;          // 플레이어 움직임 스위치.
        //public int JumpCount = 0;


        private float[] GoDown_RstairY = new float[4];
        private float[] GoDown_LstairY = new float[4];
        private float GoDown_Rstair_DiffY;
        private float[] GoUp_RstairY = new float[4];
        private float[] GoUp_LstairY = new float[4];
        private float GoUp_Rstair_DiffY;



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

            for (int i = 0; i < GameManager.Instance.Rstair_Comp.childCount; i++)
            {
                GoUp_RstairY[i] = GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.y;
                GoDown_RstairY[i] = GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.y + 0.5f;
            }

            for (int i = 0; i < GameManager.Instance.Lstair_Comp.childCount; i++)
            {
                GoUp_LstairY[i] = GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.y;
                GoDown_LstairY[i] = GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.y + 0.5f;
            }

            GoDown_Rstair_DiffY = GameManager.Instance.Rstair_Diff.GetChild(0).position.y;
            GoUp_Rstair_DiffY = GameManager.Instance.Rstair_Diff.GetChild(0).position.y - 0.5f;

        }


        void Update()
        {
            if (MoveFlag)
                PlayerTr.transform.Translate(Vector3.forward * Time.deltaTime * 10f);
        }

        // 드래그
        public void Drag(BaseEventData _Data)
        {
            MoveFlag = true;
            PointerEventData Data = _Data as PointerEventData;
            Vector3 Pos = Data.position;
            // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
            JoyVec = (Pos - StickFirstPos).normalized;
            //Debug.Log("조이스틱 높이 : " + JoyVec.y);
            // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
            float Dis = Vector3.Distance(Pos, StickFirstPos);
            // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
            if (Dis < Radius)
                Stick.position = StickFirstPos + JoyVec * Dis;
            // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
            else
                Stick.position = StickFirstPos + JoyVec * Radius;

            if ((JoyVec.x > 0 && JoyVec.x < 1)) // 앞으로
            {
                PlayerTr.eulerAngles = new Vector3(0, 90, 0);
                Player.MoveAnimator("run", true);
            }

            if ((JoyVec.x < 0 && JoyVec.x > -1)) // 뒤로
            {
                PlayerTr.eulerAngles = new Vector3(0, -90, 0);
                Player.MoveAnimator("run", true);
            }

            if (JoyVec.y > 0.7 && !jumpComplete) // 점프            
                Player.IsJump();
            

            if (JoyVec.y < -0.7)            
                Char_GoDown();          
            else            
                Char_GoUp();
            

            if (JoyVec.y < 0.55)
                Char_RstairDiff_Down();
            else
                Char_RstairDiff_Up();

            
            //else if (JoyVec.x < 0)
            //{
            //    Player.eulerAngles = new Vector3(0, -90, 0);
            //}
            //Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0); // 플레이어 3d 이동.
            //Player.eulerAngles = new Vector3(0, Mathf.`Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);    
        }

        void Char_GoDown() // 캐릭터가 계단을 내려가고 싶을 때.
        {
            for (int i = 0; i < GameManager.Instance.Lstair_Comp.childCount; i++)
            {
                GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position =
                    new Vector3(GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.x,
                    GoDown_LstairY[i], GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.z);
            }

            for (int i = 0; i < GameManager.Instance.Rstair_Comp.childCount; i++)
            {
                GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position =
                    new Vector3(GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.x,
                    GoDown_RstairY[i], GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.z);
            }
        }

        void Char_RstairDiff_Down() // 캐릭터와 별개의 오른쪽 계단과의 관계.
        {
            GameManager.Instance.Rstair_Diff.GetChild(0).position = new Vector3(GameManager.Instance.Rstair_Diff.GetChild(0).position.x,
                GoDown_Rstair_DiffY, GameManager.Instance.Rstair_Diff.GetChild(0).position.z);
        }

        void Char_GoUp() // 캐릭터가 계단을 올라가고 싶을 때.
        {
            for (int i = 0; i < GameManager.Instance.Lstair_Comp.childCount; i++)
            {
                GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position =
                    new Vector3(GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.x,
                    GoUp_LstairY[i], GameManager.Instance.Lstair_Comp.GetChild(i).GetChild(0).position.z);
            }

            for (int i = 0; i < GameManager.Instance.Rstair_Comp.childCount; i++)
            {
                GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position =
                    new Vector3(GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.x,
                    GoUp_RstairY[i], GameManager.Instance.Rstair_Comp.GetChild(i).GetChild(0).position.z);
            }

        }

        void Char_RstairDiff_Up() // 캐릭터와 별개의 오른쪽 계단과의 관계.
        {
            GameManager.Instance.Rstair_Diff.GetChild(0).position = new Vector3(GameManager.Instance.Rstair_Diff.GetChild(0).position.x,
                GoUp_Rstair_DiffY, GameManager.Instance.Rstair_Diff.GetChild(0).position.z);
        }
        // 드래그 끝.
        public void DragEnd()
        {
            Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
            JoyVec = Vector3.zero;          // 방향을 0으로.
            MoveFlag = false;
            Player.MoveAnimator("run", false);
        }
    }
}