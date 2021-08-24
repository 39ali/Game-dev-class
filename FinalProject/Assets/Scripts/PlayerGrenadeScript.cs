using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenadeScript : MonoBehaviour
{
    public float timeDelay = 2f;
    float startTimer;
    bool exploded = false;
    public int damage = 50;
    public float explosiveForce = 20f;
    public float explosiveRadius = 15f;

    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= timeDelay && !exploded)
        {
            Explode();
            exploded = true;
        }

    }

    void Explode()
    {

        //explode effect 
        GameObject particle = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] coll = Physics.OverlapSphere(transform.position, explosiveRadius);

        for (int i = 0; i < coll.Length; i++)
        {
            NpcLogic npc = coll[i].gameObject.GetComponent<NpcLogic>();
            if (npc != null)
            {
                npc.TakeDamage(damage);
                coll[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveRadius);
            }

            GunShooting player = coll[i].gameObject.GetComponent<GunShooting>();
            if (player != null)
            {
                player.TakeDamage(damage);
                coll[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveRadius);
            }
        }

        Destroy(gameObject);
        // Destroy(particle);
    }
}
