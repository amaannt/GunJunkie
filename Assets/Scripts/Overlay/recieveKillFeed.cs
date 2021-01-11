using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recieveKillFeed : MonoBehaviour
{
    public GameObject killData;
    public GameObject LoadFeedData;
    void Awake()
    {

        killData = GameObject.FindGameObjectWithTag("killfeed");

        LoadData();

    }
    void OnEnable()
    {
        LoadData();
    }

    void LoadData()
    {
        killData.GetComponent<PlayersManager>().photonView.RPC("recieveKillFeed", RpcTarget.All);
        LoadFeedData.GetComponent<GameController>().LookAtKillFeed();
    }
}
