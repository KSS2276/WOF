using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.WOF.Sungsoo
{
    public class Collect_Character : MonoBehaviour
    {

        int charCount = 6; // 캐릭터 갯수.

        [SerializeField]
        private GameObject roomImagePanels; // 방 이미지들.

        [SerializeField]
        private GameObject collectedSqare; // 선택 사각형.

        [SerializeField]
        private Text collectedSquare_Nick;

        [SerializeField]
        private List<GameObject> charactersPanel;

        List<int> charInt_List = new List<int>();
        List<GameObject> char_List = new List<GameObject>();

        int charSeq = 0;

        [SerializeField]
        private bool[] char_Use; // 캐릭터 선택 가능 여부. 혹시 다른 사람이 먼저 캐릭터를 골랐을 경우.


        void Awake()
        {
            char_Use = new bool[charCount];
            //collectedSquare_Nick.text = Account_Manager.PlayerInfo.s_Nickname;

            for (int i = 0; i < roomImagePanels.transform.childCount; i++)
            {
                roomImagePanels.transform.GetChild(i).gameObject.SetActive(false);
            }
            roomImagePanels.transform.GetChild(Launcher.randomRoomNumber).gameObject.SetActive(true);
            // Launcher 스크립트에서 정해놓은 방을 캐릭터 고르는 씬에서 보여주기 위해서.

            for (int i = 0; i < charactersPanel.Count; i++)
            {
                for (int j = 0; j < charactersPanel[i].transform.childCount; j++)
                {
                    charInt_List.Add(charSeq++);
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

            int randCollectChar = Random.RandomRange(0, 6);


        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}
