//using System;
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
        #endregion

        #region Priavte Fields
        private string serverURL = "http://13.209.6.8:3000/";
        bool isConnecting;
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>

        string gameVersion = "1";

        struct PlayerInfo
        {
           public string check;
           public string username;
           public string test;
        };
        #endregion

        PlayerInfo user = new PlayerInfo();
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
        }

        IEnumerator AwakeCor()
        {
            string serialNum = SystemInfo.deviceUniqueIdentifier;
            WWWForm form = new WWWForm();
            form.AddField("code", serialNum);

            WWW www = new WWW(serverURL, form);
            yield return www;
            string wwwReuslt = www.text;
            Debug.Log("Recieve : " + wwwReuslt);
            Debug.Log("SerialNum : " + serialNum);



            if (wwwReuslt != "Unauthorized")
            {
                user.check = JsonUtility.FromJson<PlayerInfo>(wwwReuslt).check;
                user.username = JsonUtility.FromJson<PlayerInfo>(wwwReuslt).username;

                Debug.Log(wwwReuslt);
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

        public void GoToMultiPlay()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void CreateRoom()
        {
            int roomNum = Random.RandomRange(0, 100);
            PhotonNetwork.CreateRoom("" + roomNum, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        //public void Register()
        //{
        //    StartCoroutine(Register_Cor());
        //}

        //IEnumerator Register_Cor()
        //{
        //    Regex nickNameRule = new Regex(@"^[a-zA-Z]+[0-9]{4,14}$"); //영어, 숫자 조합 확인.
        //    Regex nickNameRule2 = new Regex(@"^[가-힣]{2,7}$"); // 한글 확인.
        //    //Regex passwordRule = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"); // ^.*(?=^.{8,15}$)(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%^&+=]).*$/


        //    Match nickNameChecking = nickNameRule.Match(u_InputNick.text);
        //    Match nickNameChecking2 = nickNameRule2.Match(u_InputNick.text);
        //    //Match passwordChecking = passwordRule.Match(u_PW.text);

        //    //isConnecting = true;

        //    if (u_InputNick.text == "")
        //    {
        //        StartCoroutine(Send_Alarm("닉네임을 확인해주세요."));
        //    }
        //    //else if (u_PW.text == "")
        //    //{
        //    //    StartCoroutine(Send_Alarm("비밀번호를 확인해주세요."));
        //    //}
        //    else
        //    {
        //        WWWForm form = new WWWForm();
        //        form.AddField("username", u_InputNick.text);
        //        //form.AddField("password", u_PW.text);
        //        //form.AddField("code", DateTime.Now.ToString("yyyyMMddHHmmss"));

        //        WWW www = new WWW(serverURL+"login", form);
        //        yield return www;
        //        string wwwReuslt = www.text;
        //        Debug.Log(wwwReuslt);

        //        if (wwwReuslt != "Unauthorized")
        //        {
                    //user = JsonUtility.FromJson<PlayerInfo>(wwwReuslt);
                    //user.s_Nickname = wwwReuslt;
                    //StartCoroutine(Send_Alarm("닉네임이 생성되었습니다."));
        //        }
        //        else
        //        {
        //            StartCoroutine(Send_Alarm("닉네임 생성이 실패하였습니다."));
        //        }
        //    }
        //else
        //{
        //    if (PhotonNetwork.IsConnected)
        //    {
        //        PhotonNetwork.JoinRandomRoom();
        //    }
        //    else
        //    {
        //        PhotonNetwork.GameVersion = gameVersion;
        //        PhotonNetwork.ConnectUsingSettings();
        //    }

        //    Progress_Label.SetActive(true);
        //    Control_Panel.SetActive(false);
        //}
        //}

        //IEnumerator Register()
        //{
        //    Regex nickNameRule = new Regex(@"^[a-zA-Z]+[0-9]{4,14}$"); //영어, 숫자 조합 확인.
        //    Regex nickNameRule2 = new Regex(@"^[가-힣]{2,7}$"); // 한글 확인.
        //    Regex passwordRule = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"); // ^.*(?=^.{8,15}$)(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%^&+=]).*$/

        //    Match nickNameChecking = nickNameRule.Match(u_InputNick.text);
        //    Match nickNameChecking2 = nickNameRule2.Match(u_InputNick.text);
        //    Match passwordChecking = passwordRule.Match(u_PW.text);

        //    if(!nickNameChecking.Success && !nickNameChecking2.Success)
        //    {
        //        StartCoroutine("닉네임을 확인해주세요.");
        //    }
        //    else if (!passwordChecking.Success || u_PW.text != u_PW.text)
        //    {
        //        StartCoroutine("비밀번호를 확인해주세요.");
        //    }
        //    else
        //    {
        //        WWWForm form = new WWWForm();
        //        form.AddField("username", u_InputNick.text);
        //        form.AddField("password", u_PW.text);
        //        form.AddField("code", DateTime.Now.ToString("yyyyMMddHHmmss"));

        //        WWW www = new WWW(serverURL, form);
        //        yield return www;
        //        string wwwReuslt = www.text;
        //        Debug.Log(wwwReuslt);

        //        if (wwwReuslt != "Unauthorized")
        //            user = JsonUtility.FromJson<PlayerInfo>(wwwReuslt);

        //        StartCoroutine(Send_Alarm("닉네임이 생성되었습니다."));

        //    }

        //}
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
            randomRoomNumber = Random.RandomRange(1, 4);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            Debug.Log("We load the 'Room for '" + randomRoomNumber);
            PhotonNetwork.LoadLevel("Room for " + randomRoomNumber);
        }

        #endregion

        #region Coroutine

        IEnumerator Send_Alarm(string message)
        {
            u_AlarmBar.transform.GetChild(0).GetComponent<Text>().text = message;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", true);
            yield return null;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", false);
        }

        #endregion
    }
}
