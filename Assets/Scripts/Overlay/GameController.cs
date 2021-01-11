
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject settingsPanel;
    bool isSettingsActive = false;
    GameObject[] gun;
    GameObject player;

    //volume section
    public Slider volumeSlider;
    public Text volumeText;

    //mouse sensitivity Option
    public Slider sensSlider;
    public Text sensText;


    //check kill feed
    public GameObject killFeedDisplay;
    GameObject killFeedData;

    //AI bot settings 
    [SerializeField]
    private GameObject AISettings;
    [SerializeField]
    private GameObject botSettings;
    public GameObject headSettings;
    //player names in the tab menu
    public Text[] playerNames;

    private void Awake()
    {
        for (int x = 0; x < playerNames.Length; x++)
        {
            playerNames[x].text = " ";
        }
    }
    void Start()
    {
        //change initial settings here
        volumeSlider.value = 0.05f;
        
        sensSlider.minValue = 0.01f;
        sensSlider.maxValue = 10f;
        sensSlider.value = 1.2f;

        //get all the gameobjects initialized for access later here
        gun = GameObject.FindGameObjectsWithTag("Gun");
        player = GameObject.FindWithTag("Player");
        //settingsPanel = GameObject.FindGameObjectWithTag("settingsPanel");

       // killFeedDisplay = GameObject.FindGameObjectWithTag("feedPanel");
        //kill feed initialization
        killFeedData = GameObject.FindGameObjectWithTag("killfeed");

        //if the player is the owner of the server show these settings
        AISettings.SetActive(PhotonNetwork.IsMasterClient);
        botSettings.SetActive(PhotonNetwork.IsMasterClient);
        headSettings.SetActive(PhotonNetwork.IsMasterClient); 
       
    }
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsActive)
        {
            SetPauseActivity(true);
        } else if(Input.GetKeyDown(KeyCode.Escape) && isSettingsActive)
        {
            SetPauseActivity(false);
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        LookAtKillFeed();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            killFeedDisplay.SetActive(true);
            //LookAtKillFeed();

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            killFeedDisplay.SetActive(false);
        }

        if (isSettingsActive)
        {
            //settings
            volumeText.text = (int)(volumeSlider.value*100)+"%";
            sensText.text = (float)Math.Round(sensSlider.value * 100f) / 100f + ""; //set to two decimal places
        }
    }


    public void LookAtKillFeed() {

        int Playercounter = 0;

        killFeedData = GameObject.FindGameObjectWithTag("killfeed");
       
        string[] namesOfPlayers = killFeedData.GetComponent<PlayersManager>().getUsernames();
        int[] killsOfPlayers = killFeedData.GetComponent<PlayersManager>().getKills();
        
        for (int x = 0; x < namesOfPlayers.Length; x++)
        {
           
            if (namesOfPlayers[x] != "" && Playercounter < playerNames.Length)
            {
                playerNames[Playercounter].text = namesOfPlayers[x] + "   " + killsOfPlayers[x];
                Playercounter++;
                
            }
        }
    }
    public void SetPauseActivity(bool isActive)
    {
        player = GameObject.FindWithTag("Player");
        settingsPanel.SetActive(isActive);
        isSettingsActive = isActive;
        player.GetComponent<GunScript>().isPaused = isActive;
        player.GetComponent<SC_FPSController>().isPaused = isActive;
        Debug.Log("Set pause activity Reaches here");
    }

    public void VolumeAccess(float value)
    {
        if (GameObject.FindGameObjectsWithTag("Gun") != null)
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gun")) { 
                if(g.GetComponent<AudioSource>() != null)   
                {
                    g.GetComponent<AudioSource>().volume = value;
                }
            }

    }
    public void SensAccess(float value)
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<SC_FPSController>().lookSpeed = value;

    }
}
