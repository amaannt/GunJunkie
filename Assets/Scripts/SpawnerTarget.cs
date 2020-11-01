using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnerTarget : MonoBehaviourPun
{

    public GameObject[] dummies;
    public GameObject dummyModel;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnAll();
        }
    }
    
    void SpawnAll() {
        // dummies = new GameObject[12];
        for (int x = 0; x < 12; x++)
        {
             PhotonNetwork.Instantiate("MaleDummy", new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60)), Quaternion.identity);
            
        }
    }
   
}
