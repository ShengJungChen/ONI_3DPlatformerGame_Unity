using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public GameObject hitEffect;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other) 
    {
        // destroyed when collide with environment
        Destroy(gameObject);

        // instanciate effect and destroy later
        Vector3 effectPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameObject newHitEffect = Instantiate(hitEffect, effectPosition, transform.rotation);
        Destroy(newHitEffect.gameObject, 3f);

    }

    private void OnTriggerEnter(Collider other) 
    {
        
        if (other.gameObject.CompareTag("Chicken"))
        {
            // instanciate effect and destroy later
            Vector3 effectPosition = new Vector3(other.transform.position.x, other.transform.position.y + 0.5f, other.transform.position.z);
            GameObject newHitEffect = Instantiate(hitEffect, effectPosition, transform.rotation);
            Destroy(newHitEffect.gameObject, 3f);
        }

    }
}
