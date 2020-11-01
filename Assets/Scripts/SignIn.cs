using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Com.Amaan.GunJunkie;

public class SignIn : MonoBehaviourPun
{
   
    public InputField userName;
    public string user;
    public GameObject killfeed;
    public GameObject launcher;

    public GameObject warningUsername;


    public GameObject LoadingScreen;
    private void Awake()
    {
        //launcher.GetComponent<LauncherMainMenu>().Connect();
        userName = GameObject.FindGameObjectWithTag("userName").GetComponent<InputField>();
        userName.characterLimit = 10;
        killfeed = GameObject.FindGameObjectWithTag("killfeed");

        user=userName.text;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {

            addPlayer();
        }
    }
    public void addPlayer()
    {
        if (userName.text.Length <= 2)
        {
            warningUsername.SetActive(true);
        }
        else
        {
            LoadingScreen.SetActive(true);
            launcher.SetActive(true);
        }
        //Debug.Log(killfeed);
       //killfeed.GetComponent<PlayersManager>().photonView.RPC("AddCurrentPlayer", RpcTarget.All, userName.text);
    }
}
