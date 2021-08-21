using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject muzzle;
    public GameObject target;
    public GameObject aCamera;
    public GameObject aGun;
    public GameObject aGrenade;
    private AudioSource sound;
    public ParticleSystem MuzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        sound = aGun.GetComponent<AudioSource>();
    }

    IEnumerator Shoot()
    {
        lr.SetPosition(0, muzzle.transform.position);
        lr.SetPosition(1, target.transform.position);
        sound.Play();
        lr.enabled = true;
        MuzzleFlash.Play();
        yield return new WaitForSeconds(0.1f);
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TouchBtn"))
        {
            RaycastHit hit;
            if (Physics.Raycast(aCamera.transform.position, aCamera.transform.forward, out hit))
            {
                target.transform.position = hit.point;
                StartCoroutine(Shoot());
                //               lr.SetPosition(0, muzzle.transform.position);
                //             lr.SetPosition(1, target.transform.position);
            }
        }
    }

    public void SetGunActive(bool g)
    {
        aGun.SetActive(g);
    }
    public void SetGrenadeActive(bool g)
    {
        aGun.SetActive(g);
    }
}
