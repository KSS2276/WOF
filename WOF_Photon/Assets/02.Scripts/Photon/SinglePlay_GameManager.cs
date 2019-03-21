using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.WOF.Sungsoo
{
    public class SinglePlay_GameManager : MonoBehaviourPunCallbacks
    {
        GameObject LauncherScript;
        private string serverURL = "http://13.209.6.8:3000/";

        public static SinglePlay_GameManager Instance;
        public GameObject playerPrefab;
        public GameObject Player;
        public Transform PlayerRoof;

        [SerializeField]
        private GameObject joyStick;

        [SerializeField]
        private Transform Floor_Comp;

        public Transform Lstair_Comp;

        public Transform Rstair_Comp;

        public Transform Lstair_Diff;

        public Transform Rstair_Diff;

        public Transform Floor_Diff;

        public GameObject Option_Panel;

        float HoldZ = -1.43f;
        #region Photon Callbacks

        //public override void OnLeftRoom()
        //{
        //    SceneManager.LoadScene(0);
        //}

        #endregion

        void Awake()
        {
            LauncherScript = GameObject.Find("Launcher");
        }


        // Start is called before the first frame update
        void Start()
        {
            Instance = this;

            //if (playerPrefab == null)
            //{
            //    Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            //}
            //else
            //{
            //    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

            //    if (PlayerManager.LocalPlayerInstance == null)
            //    {
            //        Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            //        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //        Player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0) as GameObject;
            //    }
            //    else
            //    {
            //        Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            //    }
            //}

            //PlayerRoof = Player.transform.GetChild(Player.transform.childCount - 1);
            PlayerRoof = Player.transform.GetChild(Player.transform.childCount - 1);
        }

        // Update is called once per frame
        void Update()
        {
            Char_Floor_Relation();
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, HoldZ);
            Char_Lstair_Relation();
            Char_Rstair_Relation();
        }

        #region Public Methods

        //public void LeaveRoome()
        //{
        //    PhotonNetwork.LeaveRoom();
        //}

        #endregion

        #region Private Methods


        void Char_Floor_Relation()
        {
            for (int i = 0; i < Floor_Comp.childCount; i++)
            {
                if (PlayerRoof.position.y > Floor_Comp.GetChild(i).position.y)
                    Floor_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                else
                    Floor_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
            }

            for (int i = 0; i < Floor_Diff.childCount; i++)
            {
                if (Vector3.Distance(PlayerRoof.position, Floor_Diff.GetChild(i).GetChild(0).position) < 0.6f && (joyStick.GetComponent<JoyStick>().JoyVec.y <= -0.8f))
                {
                    Floor_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                }
                else if (PlayerRoof.position.y < Floor_Diff.GetChild(i).position.y)
                {
                    Floor_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                }
                else if (PlayerRoof.position.y > Floor_Diff.GetChild(i).position.y && (joyStick.GetComponent<JoyStick>().JoyVec.y > -0.8f))
                {
                    Floor_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                }
                else if (PlayerRoof.position.y > Floor_Diff.GetChild(i).position.y && (joyStick.GetComponent<JoyStick>().JoyVec.y < -0.8f))
                {
                    Floor_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }

        void Char_Lstair_Relation()
        {
            for (int i = 0; i < Lstair_Comp.childCount; i++)
            {
                if (PlayerRoof.position.y > Lstair_Comp.GetChild(i).GetChild(0).position.y)
                {
                    Lstair_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    Lstair_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                }
            }

            for (int i = 0; i < Lstair_Diff.childCount; i++)
            {
                if (PlayerRoof.position.y > Lstair_Diff.GetChild(i).GetChild(0).position.y)
                {
                    Lstair_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    Lstair_Diff.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                }
            }
        }

        void Char_Rstair_Relation()
        {
            for (int i = 0; i < Rstair_Comp.childCount; i++)
            {
                if (PlayerRoof.position.y > Rstair_Comp.GetChild(i).GetChild(0).position.y)
                {
                    Rstair_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    Rstair_Comp.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                }
            }

            if (PlayerRoof.position.y > Rstair_Diff.GetChild(0).position.y)
            {
                Rstair_Diff.GetComponent<BoxCollider>().isTrigger = false;
            }
            else
            {
                Rstair_Diff.GetComponent<BoxCollider>().isTrigger = true;
            }
        }


        //void LoadArena()
        //{

        //    if (!PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        //    }

        //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", Launcher.randomRoomNumber);
        //    PhotonNetwork.LoadLevel("Room for " + Launcher.randomRoomNumber);
        //}
        //#endregion

        //#region Photon Callbacks
        //public override void OnPlayerEnteredRoom(Player newPlayer)
        //{
        //    Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

        //        LoadArena();
        //    }
        //}

        //public override void OnPlayerLeftRoom(Player other)
        //{
        //    Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

        //        LoadArena();
        //    }
        //}

        public void Option_Open()
        {
            Time.timeScale = 0f;
            Option_Panel.SetActive(true);
        }

        public void Option_Close()
        {
            Time.timeScale = 1f;
            Option_Panel.SetActive(false);
        }

        public void LeaveSingleGame()
        {
            Launcher.isSingleFinished = true;
            Destroy(LauncherScript);
            SceneManager.LoadScene("Launcher");
        }

        public void TestVersion_GameClear()
        {
            StartCoroutine(TestVersion_GameClear_Cor());
        }

        IEnumerator TestVersion_GameClear_Cor()
        {
            if (Launcher.stageSeq < 5)
                Launcher.stageSeq += 1;
            
                
            WWWForm form = new WWWForm();

            form.AddField("code", Launcher.user.serialNum);
            form.AddField("stage", "" + Launcher.scJSON.stage);
            form.AddField("character", "" + Launcher.scJSON.character);

            WWW www = new WWW(serverURL + "single", form);
            yield return www;
            string wwwResult = www.text;
            Debug.Log("Recieve : " + wwwResult);
            Debug.Log("SerialNum : " + Launcher.user.serialNum);



            if (wwwResult != "Unauthorized")
            {
                Debug.Log(JsonUtility.FromJson<Launcher.PlayerInfo>(wwwResult));
                Debug.Log(JsonUtility.FromJson<Launcher.PlayerInfo>(wwwResult).check);
                Debug.Log(JsonUtility.FromJson<Launcher.PlayerInfo>(wwwResult).username);
                Launcher.user.check = JsonUtility.FromJson<Launcher.PlayerInfo>(wwwResult).check;
                Launcher.user.username = JsonUtility.FromJson<Launcher.PlayerInfo>(wwwResult).username;

                Debug.Log(wwwResult);

            }
        }
        #endregion

    }
}