using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnightBehavior : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        /*
        if(Input.GetKey("x"))
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            if (Input.GetKey("z"))
                anim.SetBool("IsDying", true);
            else
                 anim.SetBool("IsWalking", false);
        }
        */
    }
}
