using Photon.Pun;
using UnityEngine;

using UnityEngine.UI;
public class SpawnerTarget : MonoBehaviourPun
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
        foreach(GameObject bot in dummies)
        {

            bot.GetComponent<MoveToGoalAgent>().BotAIActivityOff();
        }
    }
    public void DummyAIActivityOn()
    {
        foreach (GameObject bot in dummies)
        {
            bot.GetComponent<MoveToGoalAgent>().BotAIActivityOn();
        }
    }

    //disable or enable dummies
    public void DisableBot() {
        foreach (GameObject bot in dummies)
        {
            bot.SetActive(false);
        }
    }
    public void EnableBot() {
        foreach (GameObject bot in dummies)
        {
            bot.SetActive(true);
        }
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
