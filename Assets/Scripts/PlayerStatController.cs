using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerStatController : MonoBehaviourPunCallbacks
{
    public float CurrentHealth = 100f;
    public float Max_Health = 100f;
    float healthSaved = 100f;

    //last attacked by this player
    string LastHitByPlayerName;
    //killfeed to send kill data to
    public GameObject killFeed;


    [SerializeField]
    private GameObject meshChild; //the visible part of the target

    //heath status
    [SerializeField]
    private TextMeshProUGUI healthStat;

    private void Start()
    {
        healthStat = GameObject.FindGameObjectWithTag("healthText").GetComponent<TMPro.TextMeshProUGUI>();
        killFeed = GameObject.FindGameObjectWithTag("killfeed");
    }

    bool isDead = false;
    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            if (photonView.IsMine)
            {
                CurrentHealth = healthSaved;
                RespawnPlayer();

            }
            
        }
    }
    [PunRPC]
    public void TakeDamage(float amount, string attackerName)
    {

        LastHitByPlayerName = attackerName;
        CurrentHealth -= amount;

        //send health text

        healthManage();

        if (CurrentHealth <= 0f)
        {
            //set skin invisible
            // meshChild.SetActive(false);

            //respawn current player immediately 
            /*CAN BE CHANGED LATER*/
            // photonView.RPC("RespawnPlayer", RpcTarget.All);
           
            //adds a kill to last known hitter
            killFeed = GameObject.FindGameObjectWithTag("killfeed");

            //send killfeed kill with player name from the killed player only
            if (photonView.IsMine)
            {
                killFeed.GetComponent<PlayersManager>().photonView.RPC("addKillToPlayer", RpcTarget.All, LastHitByPlayerName);
            }

        }
    }


    //manage health status
    void healthManage()
    {
        if (photonView.IsMine)
        {
            if (CurrentHealth >= 0f)
            {

                //change health sign colors
                if (CurrentHealth>50f)
                {
                    healthStat.color = Color.green ;
                } else if(CurrentHealth > 20f)
                {
                    healthStat.color = Color.yellow;
                }
                else if (CurrentHealth > 0f)
                {
                    healthStat.color = Color.red;
                }


                if (healthStat != null)
                    healthStat.text = CurrentHealth + "/" + Max_Health;

            }
            else
            {
                if (healthStat != null)
                    healthStat.text = 0 + "/" + Max_Health;

            }
        }

    }
    [PunRPC]
    void RespawnPlayer()
    {
        gameObject.transform.position = new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60));
        

            CurrentHealth = healthSaved;
            Max_Health = healthSaved;
            //change health text back to max health
            healthStat.text = healthSaved + "/" + Max_Health;
            //change health color back
            healthStat.color = Color.green;



        // meshChild.SetActive(true);


    }

    
}
