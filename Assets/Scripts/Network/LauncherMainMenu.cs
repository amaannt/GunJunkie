using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

namespace Com.Amaan.GunJunkie
{

    public class LauncherMainMenu : MonoBehaviourPunCallbacks
    {


        public GameObject killfeed;
        public InputField userName;

        public void Awake()
        {
            killfeed = GameObject.FindGameObjectWithTag("killfeed");
            userName = GameObject.FindGameObjectWithTag("userName").GetComponent<InputField>();
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }
        public void Start()
        {
            killfeed.GetComponent<PlayersManager>().SetPlayerUserName();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected!");
            Join();
            base.OnConnectedToMaster();
        }


        // Start is called before the first frame update
        public void Connect()
        {
            Debug.Log("Connecting");
            //PhotonNetwork.GameVersion = "0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        // Update is called once per frame
        public void Join()
        {
            Debug.Log("Joining Room");
           
            PhotonNetwork.JoinRandomRoom();
           
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room Successfully!");
           

            killfeed.GetComponent<PlayersManager>().photonView.RPC("AddCurrentPlayerToMaster", RpcTarget.All, userName.text);
            
            StartGame();
            base.OnJoinedRoom();
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Joining Room Failed!\nCreating new Room");
            Create();
            base.OnJoinRandomFailed(returnCode, message);
        }

        public void Create() {
           
            PhotonNetwork.CreateRoom(Random.Range(0,10)+""+ Random.Range(0, 10)+""+ Random.Range(0, 10) + "" + Random.Range(0, 10));
            
        }
        public void StartGame()
        {

            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}   