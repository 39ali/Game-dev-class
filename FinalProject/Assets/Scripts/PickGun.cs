using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickGun : MonoBehaviour
{
    public GameObject currentPlayer;
    private int _rotationSpeed = 30;
    private void OnMouseDown()
    {
        if (currentPlayer.tag == "Player")
        {
            if (gameObject.name.Contains("Gun"))
            {
                bool wasPicked = currentPlayer.GetComponent<GunShooting>().SetGunActive(true);
                if (!wasPicked)
                {
                    return;
                }
            }
            else if (gameObject.name.Contains("m26"))
            {

                bool wasPicked = currentPlayer.GetComponent<GunShooting>().SetGrenadeActive(true);
                if (!wasPicked)
                {
                    return;
                }
            }
            Debug.Log(gameObject.name + " was picked by Player");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //        Debug.Log("collider:" + other.tag);

        if (other.tag == "Npc") // only Player can pick coins
        {

            NpcLogic npc = other.transform.gameObject.GetComponent<NpcLogic>();
            if (gameObject.name.Contains("Gun"))
            {
                bool wasPicked = npc.SetGunActive(true);
                if (!wasPicked)
                {
                    return;
                }
            }
            else if (gameObject.name.Contains("m26"))
            {
                bool wasPicked = npc.SetGrenadeActive(true);
                if (!wasPicked)
                {
                    return;
                }
            }

            this.gameObject.SetActive(false); // turn THIS off
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}

