using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class npcMove : MonoBehaviour
{

    [SerializeField]
    Transform[] distenations;
    private NavMeshAgent navMeshAgent;
    private int currentIndex;
    private bool moving = false;
    private Animator npcAnim;

    private float health;



    enum NpcState
    {
        Patrolling,
        LookingForGun,
        LookingForBomb,
        Attacking,
        Killed
    }


    private NpcState state;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        npcAnim = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        npcAnim.SetBool("isWalking", false);
        state = NpcState.Patrolling;
        health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case NpcState.Patrolling:
                {

                    if (PlayerIsInRange())
                    {
                        this.state = NpcState.Attacking;
                        break;
                    }

                    tryStop();
                    if (!moving)
                    {
                        StartCoroutine(Move((currentIndex + 1) % distenations.Length));
                    }
                    break;
                }

            case NpcState.Attacking:
                {
                    break;

                }
            case NpcState.LookingForGun:
                {
                    break;
                }
            case NpcState.LookingForBomb:
                {
                    break;
                }

            case NpcState.Killed:
                {
                    break;
                }
        }
    }


    public bool tryStop()
    {
        if (moving && navMeshAgent.remainingDistance < 1f)
        {
            moving = false;
            npcAnim.SetBool("isWalking", false);
            return true;
        }

        return false;
    }


    public IEnumerator Move(int index)
    {
        yield return new WaitForSeconds(2.1f);
        moving = true;
        currentIndex = index;
        Vector3 target = distenations[currentIndex].transform.position;
        navMeshAgent.SetDestination(target);
        npcAnim.SetBool("isWalking", true);


    }

    bool PlayerIsInRange()
    {
        return false;
    }

    void TakeDamage(float n)
    {
        health -= n;
        if (health <= 0)
        {
            state = NpcState.Killed;
        }
    }

    void AttackPlayer()
    {

    }



    public void SetGunActive(bool g)
    {

        //    GunInHand.SetActive(g);
    }
    public void SetGrenadeActive(bool g)
    {
        //   GrenadeInHand.SetActive(g);
    }
}
