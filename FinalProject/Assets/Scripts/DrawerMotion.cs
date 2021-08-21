using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerMotion : MonoBehaviour
{
    private Animator anim;
    private bool isDrawerClosed;
    public GameObject CrossHair; // regular
    public GameObject CrossHairTouch; // the trigger (this) is touched
    public GameObject aCamera;
    bool isTriggerHit;

    // Start is called before the first frame update
    void Start()
    {
        // connect to the component of parent object
        anim = this.gameObject.transform.parent.GetComponent<Animator>();
        isDrawerClosed = true;
        isTriggerHit = false;
    }
    /*
    private void OnMouseDown()
    {
        anim.SetBool("IsOpen", isDrawerClosed);
 
        isDrawerClosed = !isDrawerClosed;
    }
    */

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // casts ray from camera towards forward direction and writes down to hit (OUT)
        // the info of GameObject that was hit by this ray
        if(Physics.Raycast(aCamera.transform.position,aCamera.transform.forward,out hit))
        {
            // check if THIS is the GameObject that was hit
            if (hit.transform.gameObject.name == this.gameObject.name && hit.distance < 8) //we are close enough exchange crosshairs
            {
                if (!isTriggerHit)
                {
                    isTriggerHit = true;
                    CrossHair.SetActive(false); // turns crosshair off
                    CrossHairTouch.SetActive(true); //turns crosshair on
                }

                // if the crosshair is on trigger and we press SPACE then activate the animation
                if(Input.GetButtonDown("TouchBtn")) // we have defined SPACE as TouchBtn  
                {
                    anim.SetBool("IsOpen", isDrawerClosed);

                    isDrawerClosed = !isDrawerClosed;
                }

            }
            else // we didn't hit the trigger
            {
                if (isTriggerHit)
                {
                    isTriggerHit = false;
                    CrossHair.SetActive(true); // turns crosshair on
                    CrossHairTouch.SetActive(false); //turns crosshair off
                }
            }//else
            
        }
    }
}
