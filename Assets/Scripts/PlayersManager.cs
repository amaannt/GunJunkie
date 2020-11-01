using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Com.Amaan.GunJunkie;

public class PlayersManager : MonoBehaviourPunCallbacks
{
   // public static PlayersManager instance;

    private string[] playerUsernames;

    private int[] playerKills;

    public string CurrentPlayerName;
    public InputField userName;
    public GameObject warning;
    public GameObject launcher;
    public GameObject panelLoading;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        playerUsernames = new string[10];
        playerKills = new int[10];
        for (int x = 0; x < playerUsernames.Length; x++)
        {
            playerUsernames[x] = "";
            playerKills[x] = 0;
        }  //getComp();
    }
    bool sentUserName = false;
    private void Update()
    {
        if (PhotonNetwork.InRoom && !sentUserName)
        {
            if (!PhotonNetwork.IsMasterClient)
            {

                userName = GameObject.FindGameObjectWithTag("userName").GetComponent<InputField>();
                photonView.RPC("AddCurrentPlayerToMaster", RpcTarget.All, userName.text);
                sentUserName = true;
            }
        }
    }
    //getter for kills
    public int[] getKills()
    {
        return playerKills;
    }
    public string[] getUsernames()
    {
        return playerUsernames;
    }

    public void SetPlayerUserName()
    {
        getComp();
        CurrentPlayerName = userName.text;
    }
    void getComp()
    {
        userName = GameObject.FindGameObjectWithTag("userName").GetComponent<InputField>();
        warning = GameObject.FindGameObjectWithTag("warningSignin");
        launcher = GameObject.FindGameObjectWithTag("StartGameLauncher");
    }
    bool taken = false;

    [PunRPC]
    public void recieveKillFeed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //keeps sending kill details to everyother client
            this.photonView.RPC("setNamesKills",RpcTarget.All,playerUsernames,playerKills);
            Debug.Log("Players in the Room " );
            for (int x = 0; x < playerUsernames.Length; x++)
            {
                if (playerUsernames[x] != "")
                    Debug.Log(x + ": " + playerUsernames[x]);
            }
        }

    }
   [PunRPC]
    public void setNamesKills(string[] names,int[] kills)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            playerUsernames = names;
            playerKills = kills;
        }
    }

   

    [PunRPC]
    public void AddCurrentPlayerToMaster(string playerNameSent)
    {
        Debug.Log("Current Player Master Status :" + PhotonNetwork.IsMasterClient   );

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Found Master");
            Debug.Log("Adding " + playerNameSent + " to lobby...");
            for (int x = 0; x < playerUsernames.Length; x++)
            {
                if (playerUsernames[x] == "")
                {
                    playerUsernames[x] = playerNameSent;
                    break;
                }

            }
            for (int x = 0; x < playerUsernames.Length; x++)
            {
                if(playerUsernames[x] != "")
                    Debug.Log(x + ": " + playerUsernames[x]);
            }
        }
       
    }
    [PunRPC]
    public void AnnounceKill(string NameOfPlayerAttacking)
    {
        Debug.Log(NameOfPlayerAttacking + " Got A Kill!");
    }
    [PunRPC]
    public void addKillToPlayer(string NameOfPlayerAttacking) {
        photonView.RPC("AnnounceKill",RpcTarget.All,NameOfPlayerAttacking);
        if (PhotonNetwork.IsMasterClient)
        {
            for (int index = 0; index < playerUsernames.Length; index++)
            {

                if (playerUsernames[index].Equals(NameOfPlayerAttacking))
                {
                    playerKills[index] += 1;
                }
            }
        }

       
    }

    
}
