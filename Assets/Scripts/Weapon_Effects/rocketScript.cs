using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketScript : MonoBehaviour
{
    public GameObject explosion;
    private void OnCollisionEnter(Collision collision)
    {
        // if(collision.collider.tag == "")

        Vector3 globalPositionOfContact = collision.contacts[0].point;
        Debug.Log("We hit " + collision.collider.name + " at " + globalPositionOfContact);
        try
        {

            Instantiate(explosion, transform.position, Quaternion.identity);
            explosion.SetActive(true);
        }
        catch{
            Debug.Log("Explosion not found");
        
        }
        
      
        Destroy(gameObject);
    }

}
