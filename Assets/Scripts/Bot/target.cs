using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class target : MonoBehaviourPun
{
    public float health = 50f;

    float healthSaved;
    [SerializeField]
    private GameObject meshChild; //the visible part of the target
    private void Start()
    {
        healthSaved = health;
    }

    public void takeDMG()
    {
        photonView.RPC("TakeDamage",RpcTarget.All,10f);
    }
    [PunRPC]
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            meshChild.SetActive(false);
            photonView.RPC("Respawn",RpcTarget.All);
        }
    }

    
    [PunRPC]
    void Respawn()
    {
        gameObject.transform.position = new Vector3(Random.Range(-65,44),45,Random.Range(-58,60));
        health = healthSaved;
        meshChild.SetActive(true);
    }

}
