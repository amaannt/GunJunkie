using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponState : MonoBehaviourPun
{


    public List<GameObject> gunObjects;
    Vector3 originalScale;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = gameObject.transform.position;     
    }

    void Update() {
       
        UserInput();
    }


    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (photonView.IsMine){

                photonView.RPC("WeaponActivate", RpcTarget.All, 0);

                photonView.RPC("WeaponDeactivate", RpcTarget.All, 1);
            }
           
        }
         if(Input.GetKeyDown(KeyCode.Alpha2) )
        {
            if (photonView.IsMine)
            {
                photonView.RPC("WeaponActivate", RpcTarget.All, 1);
                photonView.RPC("WeaponDeactivate", RpcTarget.All, 0);
            }
        }
       


    }

    [PunRPC]
    void WeaponActivate(int code) {
         gunObjects[code].SetActive(true);
       
        Debug.Log(gameObject.name + " Activated");
    }

    [PunRPC]
    void WeaponDeactivate(int code)
    {
        gunObjects[code].SetActive(false);
        Debug.Log(gameObject.name + " Deactivated");
    }
    public void setStateWeapon(bool isActiveRec,string ID)
    {
        isActive = isActiveRec;
       /* if (photonView.IsMine)
            this.photonView.RPC("gunState", RpcTarget.All, isActive,ID);*/
    }
    [PunRPC]
    public void gunState(bool isActiveRec,string ID)
    {

        if (isActive)
        {

            Debug.Log(ID + " changed his weapon to " + gameObject.name);
        }else
        {
            Debug.Log(ID + " changed his weapon from " + gameObject.name);
        }

    }

}
