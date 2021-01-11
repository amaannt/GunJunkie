using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionScript : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;
    // Start is called before the first frame update
    void Start()
    {
        // PLAY THE EXPLOSION
        gameObject.GetComponent<ParticleSystem>().Play();

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null&&rb.tag != "Player")
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
    private void Update()
    {
        if (gameObject.GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(gameObject);
        }
    }

}
