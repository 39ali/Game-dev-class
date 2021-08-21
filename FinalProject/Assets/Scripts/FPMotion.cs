using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMotion : MonoBehaviour
{
    public GameObject playerCamera; // needs to be connected to real object in Unity

    private CharacterController controller;
    private float speed = 0.1f;
    private float rx = 0f, ry;
    private float angularSpeed = 1f;
    private AudioSource footStepSound;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); // gets player controller
        footStepSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float dx, dz;
        
                // mouse to rotate player
                rx -= Input.GetAxis("Mouse Y") * angularSpeed;
        // use Clampf to limit the sight angles

        // runs only on playerCamera
        ry = transform.localEulerAngles.y+ Input.GetAxis("Mouse X") * angularSpeed;
                playerCamera.transform.localEulerAngles = new Vector3(rx, 0, 0);
        // runs on player
                transform.localEulerAngles = new Vector3(0,ry,0); // sets new orientation of player
                // keyboard
                dx = Input.GetAxis("Horizontal") * speed; // Horizontal means 'A' or 'D'
                dz = Input.GetAxis("Vertical") * speed; // Verticalal means 'W' or 'S'

                Vector3 motion = new Vector3(dx, 0, dz); // in local coordinates

                motion = transform.TransformDirection(motion); // in Global coordinates
                controller.Move(motion);


        // example of basic motion to FORWARD direction (z-axis)
        /*
                transform.SetPositionAndRotation(
                    new Vector3(transform.position.x, transform.position.y, transform.position.z + speed),
                    transform.rotation);
        */

        // another example of basic motion to FORWARD direction (z-axis)
        //  transform.Translate(new Vector3(0, 0, speed));

        // add sound of foot steps
        if(!footStepSound.isPlaying && controller.velocity.magnitude>0.1f)
            footStepSound.Play();

    }
}
