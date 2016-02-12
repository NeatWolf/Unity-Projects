using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

    public GameObject destroyExplosion;
    public GameObject impactExplosion;
    public float health;

    void Start() { }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Weapon"))
    //    {
    //        health--;
    //        if (health == 0)
    //        {
    //            Instantiate(destroyExplosion, transform.position, transform.rotation);
    //            Destroy(other.gameObject);
    //            Destroy(gameObject);
    //        }
    //        else
    //        {
    //            Instantiate(impactExplosion, other.transform.position, other.transform.rotation);
    //            Destroy(other.gameObject);
    //        }
    //    }
    //}

    void Update()
    {
        if(health <= 0)
        {
            Instantiate(destroyExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Damage(float damage)
    {
        health -= damage;
    }

    void ImpactExplosion(Vector3 position)
    {
        Instantiate(impactExplosion, position, transform.rotation);
    }
}
