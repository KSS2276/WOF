using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        private GameObject Progress_Label;

        #endregion

        #region Priavte Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>

        string gameVersion = "1";
        #endregion

        #region MonoBehaviour CallBacks
        #endregion


        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
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

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }

            Progress_Label.SetActive(true);
            Control_Panel.SetActive(false);
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
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
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }

        #endregion
    }
}
