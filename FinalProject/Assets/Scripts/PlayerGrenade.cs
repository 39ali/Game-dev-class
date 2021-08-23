using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform hand;
    public float throwFroce = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GunShooting p = this.GetComponent<GunShooting>();
        if (Input.GetKeyDown(KeyCode.Q) && p.hasGrenade)
        {
            GameObject gren = Instantiate(grenadePrefab, hand.position, hand.rotation) as GameObject;
            gren.GetComponent<Rigidbody>().AddForce(hand.forward * throwFroce, ForceMode.Impulse);
        }


    }
}
