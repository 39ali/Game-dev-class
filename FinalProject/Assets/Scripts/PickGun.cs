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
            Debug.Log(gameObject.name + " was picked by Player");
            gameObject.SetActive(false);
            if (gameObject.name.Contains("Gun"))
            {
                currentPlayer.GetComponent<GunShooting>().SetGunActive(true);
            }
            else if (gameObject.name.Contains("m26"))
            {
                currentPlayer.GetComponent<GunShooting>().SetGrenadeActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Npc") // only Player can pick coins
        {
            this.gameObject.SetActive(false); // turn THIS off
            npcMove npc = other.transform.gameObject.GetComponent<npcMove>();
            if (gameObject.name.Contains("Gun"))
            {
                npc.SetGunActive(true);
            }
            else if (gameObject.name.Contains("m26"))
            {
                npc.SetGrenadeActive(true);
            }
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
