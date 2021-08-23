using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject GunInHand;
    public GameObject GrenadeInHand;
    public GameObject aCamera;
    [SerializeField]
    public GameObject target;
    private LineRenderer lr;
    [SerializeField]
    public GameObject MuzzleEnd;
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    public ParticleSystem MuzzleFlash;
    [SerializeField]
    public GameObject Enemy;
    private float range = 500;

    private bool hasGun = false;
    private bool hasGrenade = false;
    //  private Vector3 dir = new Vector3(1);
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        sound = GunInHand.GetComponent<AudioSource>();
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TouchBtn") && GunInHand.activeSelf)
        {
            Debug.Log("shoot");
            // RaycastHit hit;
            //   if(Physics.Raycast(aCamera.transform.position, aCamera.transform.forward, out hit,range))
            // {
            //   Debug.Log(hit.transform.name);
            //   target.transform.position = hit.point;
            StartCoroutine(ShowShot());
            // if(hit.transform.gameObject.name==Enemy.gameObject.name)
            //  {
            //      Animator a = Enemy.GetComponent<Animator>();
            //      a.SetBool("IsDying", true);
            //   }
            //}
        }
    }
    public IEnumerator ShowShot()
    {
        //  Debug.Log(aCamera.transform.forward);

        var ray = new Ray(aCamera.transform.position, aCamera.transform.forward);
        Debug.Log("print this:");
        Debug.Log(aCamera.transform.position);
        Debug.Log(aCamera.transform.forward);
        Debug.Log(MuzzleEnd.transform.forward);
        lr.SetPosition(0, ray.origin);
        lr.SetPosition(1, ray.GetPoint(100));
        //   lr.SetPosition(1, MuzzleEnd.transform.position+ aCamera.transform.forward*range);
        lr.enabled = true;
        // target.SetActive(true);
        MuzzleFlash.Play();
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        lr.enabled = false;
        // target.SetActive(false);
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
}
