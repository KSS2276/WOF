using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


namespace Com.WOF.Sungsoo
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField]
        private byte maxPlayersPerRoom = 6;

        [SerializeField]
        private GameObject Control_Panel;

        [SerializeField]
        private GameObject GameMode_Panel;

        [SerializeField]
        private GameObject Progress_Label;

        [SerializeField]
        private GameObject gameMode_Nickname;

        [SerializeField]
        private GameObject SinglePlay_Panel;

        [SerializeField]
        private GameObject MultiPlay_Panel;

        [SerializeField]
        private List<GameObject> SingleMode_Theme = new List<GameObject>();
 
        #endregion

        #region Priavte Fields
        private string serverURL = "http://13.209.6.8:3000/";
        bool isConnecting;
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>

        string gameVersion = "1";


 

        [SerializeField]
        public class PlayerInfo
        {
            public string check;
            public string username;
            public string serialNum;

            public int kill;
            public int death;
            public int assi;


            //public class stage_char_JSON
            //{
            //    public bool[] character;
            //    public int[] stage;
            //    public int[] stage_Max;
            //}

        }

        [Serializable]
        public class stage_char_JSON
        {
            public bool[] character;
            public int[] stage;
            public int[] stage_Max;
        }



        #endregion

        PlayerInfo user = new PlayerInfo();
        stage_char_JSON scJSON = new stage_char_JSON();

        public static int randomRoomNumber;

        #region MonoBehaviour CallBacks
        #endregion

        #region Public Fields
        public GameObject u_AlarmBar;
        public InputField u_InputNick;

        //public InputField u_PW;
        //public InputField u_PW_CaaAZ;
        #endregion


        void Awake()
        {
            StartCoroutine(AwakeCor());
            scJSON.stage_Max = new int[] { 8, 8, 8, 8, 8, 8 };
        }

        IEnumerator AwakeCor()
        {
            user.serialNum = SystemInfo.deviceUniqueIdentifier;
            WWWForm form = new WWWForm();
            form.AddField("code", user.serialNum);

            WWW www = new WWW(serverURL, form);
            yield return www;
            string wwwResult = www.text;
            Debug.Log("Recieve : " + wwwResult);
            Debug.Log("SerialNum : " + user.serialNum);



            if (wwwResult != "Unauthorized")
            {
                Debug.Log(JsonUtility.FromJson<PlayerInfo>(wwwResult));
                Debug.Log(JsonUtility.FromJson<PlayerInfo>(wwwResult).check);
                Debug.Log(JsonUtility.FromJson<PlayerInfo>(wwwResult).username);
                user.check = JsonUtility.FromJson<PlayerInfo>(wwwResult).check;
                user.username = JsonUtility.FromJson<PlayerInfo>(wwwResult).username;

                Debug.Log(wwwResult);
                Debug.Log(user.check);
                Debug.Log(user.username);


                if (user.check != "true")
                {
                    Control_Panel.SetActive(true);
                    GameMode_Panel.SetActive(false);
                }
                else
                {
                    Control_Panel.SetActive(false);
                    GameMode_Panel.SetActive(true);
                    gameMode_Nickname.transform.GetChild(0).GetComponent<Text>().text = "닉네임 : " + user.username;
                    gameMode_Nickname.SetActive(true);
                    StartCoroutine(Send_Alarm("환영합니다."));

                }

            }
            else
            {
                StartCoroutine(Send_Alarm("닉네임 생성이 실패하였습니다."));
            }

            //PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Progress_Label.SetActive(false);
            Control_Panel.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Public Methods


        public void Login()
        {
            StartCoroutine(LoginCor());
        }



        public void GoToMultiPlay()
        {
            GameMode_Panel.SetActive(false);
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void CreateRoom()
        {
            int roomNum = UnityEngine.Random.RandomRange(0, 100);
            PhotonNetwork.CreateRoom("" + roomNum, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public void GoToSinglePlay()
        {
            StartCoroutine(GoToSinglePlay_Cor());
        }

        IEnumerator GoToSinglePlay_Cor()
        {
            WWWForm form = new WWWForm();
            form.AddField("code", user.serialNum);

            WWW www = new WWW(serverURL + "single", form);
            yield return www;

            string wwwResult = www.text;

            if (wwwResult != "Unauthorized")
            {
                //{
                //    "check":"true",
                //    "scJSON":{"character":[false,false,false,false,false,false], "stage":[1,0,0,0,0,0]}
                //}

                Debug.Log("GoToSinglePlay_Result : " + wwwResult); // Json 결과 값 콘솔 창에 표시.
                Debug.Log(JsonUtility.FromJson<PlayerInfo>(wwwResult).check); 


                scJSON.stage = JsonUtility.FromJson<stage_char_JSON>(wwwResult).stage; 
                scJSON.character = JsonUtility.FromJson<stage_char_JSON>(wwwResult).character;

                GameMode_Panel.SetActive(false);
                SinglePlay_Panel.SetActive(true);

                SinglePlay_Panel.transform.GetChild(0).GetChild(1).GetComponent<Scrollbar>().value = 0; // 싱글모드 스크롤바 값 초기화.

                for (int i = 0; i < scJSON.character.Length; i++)
                {
                    if (scJSON.character[i])
                    {
                        SingleMode_Theme[i].GetComponent<Button>().interactable = true;
                        SingleMode_Theme[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    }
                    else if(i < 5 && !scJSON.character[i])
                    {
                        SingleMode_Theme[i + 1].GetComponent<Button>().interactable = false;
                        SingleMode_Theme[i + 1].GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }
                    else if(i == 5 && !scJSON.character[i])
                    {
                        SingleMode_Theme[i].GetComponent<Button>().interactable = false;
                        SingleMode_Theme[i].GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }

                    SingleMode_Theme[i].transform.GetChild(0).GetComponent<Text>().text = scJSON.stage[i] + " / " + scJSON.stage_Max[i];
                    // 스테이지 별 현재 깬 스테이지 / 맥스 스테이지 표시.

                }
            }
            else
            {
                StartCoroutine(Send_Alarm("서버와 연결이 끊어졌습니다."));
            }

        }


        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

            Progress_Label.SetActive(false);
            Control_Panel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            CreateRoom();
        }

        public override void OnJoinedRoom()
        {
            randomRoomNumber = UnityEngine.Random.RandomRange(1, 4);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            Debug.Log("We load the 'Room for '" + randomRoomNumber);
            PhotonNetwork.LoadLevel("CollectChar");

            //PhotonNetwork.LoadLevel("Room for " + randomRoomNumber);
        }

        #endregion

        #region Coroutine

        IEnumerator LoginCor()
        {
            string serialNum = SystemInfo.deviceUniqueIdentifier;

            Regex nickNameRule = new Regex(@"^[a-zA-Z]+[0-9]{4,14}$"); //영어, 숫자 조합 확인.
            Regex nickNameRule2 = new Regex(@"^[가-힣]{2,7}$"); // 한글 확인.


            Match nickNameChecking = nickNameRule.Match(u_InputNick.text);
            Match nickNameChecking2 = nickNameRule2.Match(u_InputNick.text);

            //isConnecting = true;

            if (u_InputNick.text == "")
            {
                StartCoroutine(Send_Alarm("닉네임을 확인해주세요."));
            }

            else
            {
                WWWForm form = new WWWForm();
                form.AddField("username", u_InputNick.text);
                form.AddField("code", serialNum);
                //form.AddField("password", u_PW.text);
                //form.AddField("code", DateTime.Now.ToString("yyyyMMddHHmmss"));

                WWW www = new WWW(serverURL + "login", form);
                yield return www;
                string wwwReuslt = www.text;
                Debug.Log(wwwReuslt);

                if (wwwReuslt != "Unauthorized")
                {
                    Control_Panel.SetActive(false);
                    GameMode_Panel.SetActive(true);
                    StartCoroutine(Send_Alarm("닉네임이 생성되었습니다."));
                }
                else
                {
                    StartCoroutine(Send_Alarm("닉네임 생성이 실패하였습니다."));
                }
            }
        }

        IEnumerator Send_Alarm(string message)
        {
            u_AlarmBar.transform.SetSiblingIndex(u_AlarmBar.transform.parent.childCount - 1);
            u_AlarmBar.transform.GetChild(0).GetComponent<Text>().text = message;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", true);
            yield return null;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", false);
        }

        #endregion
    }
}
