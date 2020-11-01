using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FireRateReward : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Got Reward");
        if (collision.gameObject.tag == "Player")
        {
            if (photonView.IsMine)
            {
                collision.gameObject.GetComponent<GunScript>().fireRate = 15f;

            }

            photonView.RPC("disablethis", RpcTarget.All);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Got Reward with " + other.gameObject.tag);

            other.gameObject.GetComponent<GunScript>().fireRate = 15f;
            
            //every RPC requires a photonview component
            this.photonView.RPC("disablethis", RpcTarget.All);
        }
    }

    [PunRPC]
    public void disablethis()
    {
        gameObject.SetActive(false);
    }
}
