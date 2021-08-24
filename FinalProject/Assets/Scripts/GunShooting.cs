using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject GunInHand;
    public GameObject GrenadeInHand;
    public GameObject aCamera;
    private LineRenderer lr;
    [SerializeField]
    public GameObject MuzzleEnd;
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    public ParticleSystem MuzzleFlash;
    public float shootingRange = 100;
    public float shotDamage = 30;
    public bool hasGun = false;
    public bool hasGrenade = false;

    public float health = 100;

    public GameLogicScript gameLogic;

    public GameObject youDiedText; 
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        sound = GunInHand.GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TouchBtn") && GunInHand.activeSelf)
        {
            StartCoroutine(ShowShot());

            RaycastHit hit;
            if (Physics.Raycast(MuzzleEnd.transform.position, MuzzleEnd.transform.forward, out hit, shootingRange))
            {
    

                NpcLogic npc = hit.transform.gameObject.GetComponent<NpcLogic>();
                if (npc != null)
                {
                   // Debug.Log(hit.transform.gameObject.name);
                    npc.TakeDamage(shotDamage);
                }
            }
        }
    }
    public IEnumerator ShowShot()
    {
        var ray = new Ray(MuzzleEnd.transform.position, aCamera.transform.forward);

        lr.SetPosition(0, ray.origin);
        lr.SetPosition(1, ray.GetPoint(shootingRange));
        lr.enabled = true;
        MuzzleFlash.Play();
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        lr.enabled = false;
    }


    public bool SetGunActive(bool g)
    {
        if (!hasGun)
        {
            GunInHand.SetActive(g);
            hasGun = true;
            return true;
        }
        return false;
    }
    public bool SetGrenadeActive(bool g)
    {
        if (!hasGrenade)
        {
            GrenadeInHand.SetActive(g);
            hasGrenade = true;
            return true;
        }
        return false;
    }

    public void TakeDamage(float n)
    {
        gameLogic.AddText(gameObject.name + " took " + n + " damage");
        health -= n;

        if (health <= 0)
        {
            gameLogic.AddText(gameObject.name + " got killed");
            youDiedText.SetActive(true);
            GetComponent<CharacterController>().enabled = false;
        }


    }

    public bool isDead()
    {
        return health <= 0;
    }

}
