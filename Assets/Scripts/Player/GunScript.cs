
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;

public class GunScript : MonoBehaviourPunCallbacks, IPunObservable
{


    string GunType;
    MeshRenderer ActiveGun;
    public List<string> typeOfGuns;
    public List<GameObject> gunObjects;
    public List<GameObject> PlaceHoldergunObjects;
    int CurrentGunCode;

    public float fireRate = 10;


    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    //gun effects
    //muzzle flash
    public GameObject muzzleflash;

    //impact effects
    public GameObject impactEffectNormal;
    public GameObject impactEffectTarget;


    //force to add to certain objects
    public float impactForce = 100f;



    private float nextTimeToFire = 0f;

    public Text accuracyText;
    int bulletsFired = 0;
    int bulletsHit = 0;

    public LayerMask canBeShot;

    //if the setting menu has been opened 
    public bool isPaused = false;

    //if the gun is being fired
    public bool isFiring;



    //username current player
    public string CurrentUser;
    public GameObject killFeed;

    public void Start()
    {

        //adding guns temporarily like this
        typeOfGuns = new List<string>();

        foreach(GameObject g in gunObjects)
        {
            typeOfGuns.Add(g.name);
            Debug.Log(g.name+ " Loaded");
        }
        //gunObjects = new List<GameObject>();

        //start with rifle (AK47)
        GunType = typeOfGuns[0];
        CurrentGunCode = 0;
        gunObjects = (GameObject.FindGameObjectsWithTag("Gun")).ToList();

        accuracyText = GameObject.FindGameObjectWithTag("AccuracyText").GetComponent<Text>();
        killFeed = GameObject.FindGameObjectWithTag("killfeed");
        CurrentUser = killFeed.GetComponent<PlayersManager>().CurrentPlayerName;
        Debug.Log("Current User from Gun Script" + killFeed.GetComponent<PlayersManager>().CurrentPlayerName);
        foreach (GameObject g in gunObjects)
        {
            g.SetActive(false);
        }
        gunObjects[0].SetActive(true); //set active first weapon
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            //set audio listen for everyone else but player off
            fpsCam.GetComponent<AudioListener>().enabled = false;
            return;
        }


        //accept user input if the game has not been paused (client side only)
        if (!isPaused)
        {
            UserInput();
        }

        //currently firing the weapon
        if (isFiring && Time.time >= nextTimeToFire && !isPaused)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            photonView.RPC("Shoot", RpcTarget.All, CurrentGunCode);
        }


        
    }

    public bool weaponActive = true;
    
    void UserInput() {

        if (Input.GetMouseButton(0))
        {
            
            isFiring = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;

        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (photonView.IsMine)
            {
                TriggerWeaponChange(0, 10); //guncode 0 (ak47); fireRate 10
                photonView.RPC("WeaponActivate", RpcTarget.All, 0);

                photonView.RPC("WeaponDeactivate", RpcTarget.All, 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (photonView.IsMine)
            {
                TriggerWeaponChange(1, 1); //guncode 1(rocketlauncher); fireRate 1

                photonView.RPC("WeaponActivate", RpcTarget.All, 1);
                photonView.RPC("WeaponDeactivate", RpcTarget.All, 0);
            }
        }
        //omitted for testing
        /* if (Input.GetKeyDown(KeyCode.Alpha2))
         {
             TriggerWeaponChange(1, 1); //guncode 1(rocketlauncher); fireRate 1
         }
         if (Input.GetKeyDown(KeyCode.Alpha1))
         {
             TriggerWeaponChange(0, 10); //guncode 0 (ak47); fireRate 10
         }*/

    }

    [PunRPC]
    void WeaponActivate(int code)
    {
        PlaceHoldergunObjects[code].SetActive(true);

    }

    [PunRPC]
    void WeaponDeactivate(int code)
    {
        PlaceHoldergunObjects[code].SetActive(false);
    }



    // USE THIS CODE TO CHANGE WEAPONS AND RATE OF FIRE
    void TriggerWeaponChange( int code, int rateOfFire)
    {
        //set firerate
        fireRate = rateOfFire;
        nextTimeToFire = 0f;

        CurrentGunCode = code;
        GunChangeLocal(CurrentGunCode);
        //photonView.RPC("GunChange", RpcTarget.All,code);

        Debug.Log("Gun Changed to " + typeOfGuns[CurrentGunCode]);
        // GunChange();
    }


    void GunChangeLocal(int gunCode) {


        foreach (GameObject g in gunObjects)
        {

            if (g.name.Equals(typeOfGuns[gunCode]))
            {
                //access script inside the gun

                g.SetActive(true);

                
            }
            else
            {

                g.SetActive(false);

            }
        }
    }

    [PunRPC]
    //changing weapons
    void GunChange(int gunCode)
    {

        if (photonView.IsMine)
        {


            foreach (GameObject g in gunObjects)
            {

                if (g.name.Equals(typeOfGuns[gunCode]))
                {
                    g.SetActive(true);
                    
                }
                else
                {
                    g.SetActive(false);
                    
                }
            }
        }
    }
    #region Shooting Code
    //point of fire
    public GameObject gunPoint, gunPointR;

    //rocket launcher bullets
    public Rigidbody rocket;
    public float rocketSpeed = 40f;
    [PunRPC]
    void Shoot(int gunCode)
    {
        gunPoint = GameObject.FindGameObjectWithTag("GunPoint");
        gunPointR = GameObject.FindGameObjectWithTag("GunPointRocket");
        gunObjects[gunCode].GetComponent<AudioSource>().Play();

        Debug.Log("Shot from " + gunObjects[gunCode].name);

        /**
         * ADD APPROPRIATE GUNCODES 
         * 0 - RIFLES/PISTOLS
         * 1 - ROCKET LAUNCHER
         * ~MORE INCOMING~
         * **/
        //fires the AK
        if (CurrentGunCode == 0)
        {   
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, canBeShot))
            {
                if (photonView.IsMine)
                {
                    GameObject effectDefGo = PhotonNetwork.Instantiate(muzzleflash.GetComponent<ParticleSystem>().name, gunPoint.transform.position, Quaternion.LookRotation(fpsCam.transform.forward), 0);
                    effectDefGo.tag = "myMuzzle";
                    effectDefGo.GetComponent<ParticleSystem>().Play();
                }
                else if (!photonView.IsMine)
                {
                    muzzleflash.tag = "muzzleflash";
                }

                //if target is hit (dummies)
                target Target = hit.transform.GetComponent<target>();
                if (Target != null)
                {
                    Target.photonView.RPC("TakeDamage", RpcTarget.All, 10f);
                    Instantiate(impactEffectTarget, hit.point, Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    Instantiate(impactEffectNormal, hit.point, Quaternion.LookRotation(hit.normal));
                }
                //if player is hit

                if (hit.collider.gameObject.layer == 9)
                {

                    hit.collider.gameObject.GetComponent<PlayerStatController>().photonView.RPC("TakeDamage", RpcTarget.All, 15f, CurrentUser);

                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                //if player himself hits the target
                if (photonView.IsMine)
                {
                    if (hit.transform.tag.Equals("targets"))
                    {
                        bulletsHit += 1;
                    }
                    bulletsFired += 1;
                    accuracyText.text = "Accuracy: " + (float)bulletsHit / bulletsFired * 100;
                }
                //Debug.Log((float)bulletsHit / bulletsFired * 100);

            }

        }
        //fires the rocket launcher
        else if(CurrentGunCode == 1){
            Rigidbody clonedRocket;
            clonedRocket = Instantiate(rocket, gunPointR.transform.position, gunObjects[gunCode].transform.rotation);
            clonedRocket.velocity = fpsCam.transform.TransformDirection(Vector3.forward * rocketSpeed);

        }
    }
    #endregion

    #region IPunObservable implementation



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isFiring);

            //.SendNext(weaponActive);
            /*foreach (GameObject obj in gunObjects)
            {
                stream.SendNext(obj.activeSelf);
            }*/
        }
        else if(stream.IsReading)
        {
            isFiring = (bool)stream.ReceiveNext();
            //weaponActive = (bool)stream.ReceiveNext();

           /* foreach (GameObject obj in gunObjects)
            {
                obj.SetActive((bool)stream.ReceiveNext());
            }*/
            // Network player, receive data
            
        }
    }


    #endregion
}
