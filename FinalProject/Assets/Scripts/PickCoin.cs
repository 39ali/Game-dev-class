using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickCoin : MonoBehaviour
{
    public AudioSource pickSnd;
    public static int goldCount;
    public Text textGold;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // only Player can pick coins
        {
            goldCount++;
          pickSnd.Play();
          this.gameObject.SetActive(false); // turn THIS off
            textGold.GetComponent<Text>().text = "Gold: " + goldCount;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        goldCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
