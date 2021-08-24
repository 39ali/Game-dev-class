using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameLogicScript : MonoBehaviour
{


    public GameObject[] team1;
    public GameObject[] team2;

    public GameObject canvas;
    public Text textBox;


    public GameObject gameOverImage;
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject newGO = new GameObject("myTextGO");
        //newGO.transform.SetParent(canvas.transform);

        //Text myText = newGO.AddComponent<Text>();
        //myText.text = "Ta-dah!";
    }

    // Update is called once per frame
    void Update()
    {

        int deadCount1 = 0, deadCount2 = 0;
        for (int i = 0; i < team1.Length; i++)
        {

            NpcLogic npc = team1[i].GetComponent<NpcLogic>();
            if (npc != null)
            {
                if (npc.isDead())
                {
                    deadCount1++;
                }
            }

            GunShooting player = team1[i].GetComponent<GunShooting>();
            if (player != null)
            {
                if (player.isDead())
                {
                    deadCount1++;
                }
            }
        }

        for (int i = 0; i < team2.Length; i++)
        {

            NpcLogic npc = team2[i].GetComponent<NpcLogic>();
            if (npc != null)
            {
                if (npc.isDead())
                {
                    deadCount2++;
                }
            }

            GunShooting player = team2[i].GetComponent<GunShooting>();
            if (player != null)
            {
                if (player.isDead())
                {
                    deadCount2++;
                }
            }
        }

        if (deadCount1 == team1.Length && deadCount2 == team2.Length)
        {
            gameOverImage.SetActive(true);
            gameOverText.text = "GameOver \n\n it's a tie!";
        }

        if (deadCount1 == team1.Length)
        {
            //team 2 won 
            gameOverImage.SetActive(true);
            gameOverText.text = "GameOver \n\n Your team won!";
            Cursor.lockState = CursorLockMode.None;
        }

        if (deadCount2 == team2.Length)
        {
            //team 1 won 
            gameOverImage.SetActive(true);
            gameOverText.text = "GameOver \n\n NPC team won!";
            Cursor.lockState = CursorLockMode.None;
        }

    }


    public void AddText(string text)
    {

        StartCoroutine(yeildText(text));
    }

    IEnumerator yeildText(string text)
    {

        yield return new WaitForSeconds(1.0f);
        textBox.text = text;
    }

}
