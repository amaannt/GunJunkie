using Photon.Pun;
using UnityEngine;

using UnityEngine.UI;
public class SpawnerTarget : MonoBehaviourPunCallbacks
{
    public int NoOfBots = 12;
   
    public GameObject[] dummies;
    public GameObject dummyModel;
    // Start is called before the first frame update
    void Start()
    {
        //initialize gameobject for dummies
        dummies = new GameObject[12];
        //initialize each dummy pffft.
        for (int x = 0; x < NoOfBots; x++)
        {
            dummies[x] = new GameObject();
        }

        //spawn if master client if not then empty map lol
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnAll();
        }

    }


    //toggle dummies AI On and Off
    public void DummyAIActivityOff()
    {

        photonView.RPC("BotIntelligenceActivity", RpcTarget.All, false);
    }
    public void DummyAIActivityOn()
    {

        photonView.RPC("BotIntelligenceActivity", RpcTarget.All, true);
    }

    [PunRPC]
    void BotIntelligenceActivity(bool isSmart)
    {
        foreach (GameObject bot in dummies)
        {
            if (isSmart) {

                bot.GetComponent<MoveToGoalAgent>().BotAIActivityOn();
            }
            else
            {
                bot.GetComponent<MoveToGoalAgent>().BotAIActivityOff();

            }
        }


    }
    //disable or enable dummies
    [PunRPC]
    void BotActivity(bool isExist)
    {
        GameObject[] bots = GameObject.FindGameObjectsWithTag("targets");
        foreach (GameObject bot in bots)
        {
            if (isExist)
            {

                //since disabling them doesnt work well with the program 
                //(once target disabled it does not enable cuz it can't be seen by game)
                
                bot.transform.position = new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60));
                bot.GetComponent<MoveToGoalAgent>().BotAIActivityOn();
                bot.GetComponent<Rigidbody>().useGravity = (true);

            }
            else
            {
                //so change position to something far away
                //turn off AI
                //turn off gravity so they stay there

                bot.gameObject.transform.position = new Vector3(-100000, -10000, -1000);
                bot.GetComponent<MoveToGoalAgent>().BotAIActivityOff();
                bot.GetComponent<Rigidbody>().useGravity = (false);
            }
        }
        Debug.Log("Bots Deactivated");
    }
    public void DisableBot() {
        photonView.RPC("BotActivity", RpcTarget.All, false);
    }
    public void EnableBot() {
        photonView.RPC("BotActivity", RpcTarget.All, true);
    }
    

    //disable or enable custom heads
    //change button appearance
    public GameObject headButton;

    public void DummyHeadOff()
    {
        //toggle all heads off
        foreach (GameObject bot in dummies)
        {

            bot.GetComponent<DummySettings>().NikkiHeadToggleOff();
        }
        //change color          
      
        headButton.GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f);

    }
    public void DummyHeadOn()
    {
        //toggle all heads off
        foreach (GameObject bot in dummies)
        {

            bot.GetComponent<DummySettings>().NikkiHeadToggleOn();
        }
        //change color          
        headButton.GetComponent<Image>().color = new Color(0.2f, 0.3f, 0.7f);
    }

    //spawn all dummies
    void SpawnAll() {
        // dummies = new GameObject[12];
        for (int x = 0; x < NoOfBots; x++)
        {
             dummies[x] = PhotonNetwork.Instantiate("MaleDummy", new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60)), Quaternion.identity);
            
        }
    }
   
}
