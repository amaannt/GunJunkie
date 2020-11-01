using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponState : MonoBehaviourPun, IPunObservable
{
    Vector3 originalScale;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = gameObject.transform.localScale;
        if (gameObject.name.Equals("Ak-47"))
        {
            isActive = true;
        }
    }
    
    void Update() {
        if (photonView.IsMine)
            gameObject.SetActive(isActive);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isActive);
        }
        else if(stream.IsReading)
        {
            isActive = (bool)stream.ReceiveNext();
        }
    }
}
