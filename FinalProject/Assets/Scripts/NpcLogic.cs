using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class NpcLogic : MonoBehaviour
{

    [SerializeField]
    public Transform[] PatrolPositions;
    public PickGun[] Guns;
    public PickGun[] Grenades;

    public GameObject GunInHand;
    public GameObject GrenadeInHand;

    public GameObject leader;
    public GameObject[] enemies;
    public SphereCollider enemyRange;


    private NavMeshAgent navMeshAgent;
    private int currentIndex;

    private Animator npcAnim;

    private float health;
    private bool hasGun = false;
    private bool hasGrenade = false;

    private GameObject currentLookableGun;
    private GameObject currentLookableGrenade;

    enum NpcState
    {
        Idle,
        Patrolling,
        LookingForGun,
        LookingForBomb,
        Attacking,
        Killed,
        FollowLeader
    }


    private NpcState state;
    private NpcState prevState;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        npcAnim = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        state = NpcState.Patrolling;
        health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //   if (gameObject.name.Contains("follower"))
        //  Debug.Log("currentState: " + this.state);
        if (state == NpcState.Killed)
        {

        }
        else if (EnemeyIsInRange() && (hasGun || hasGrenade))
        {
            state = NpcState.Attacking;
        }
        else if (!hasGun)
        {
            state = NpcState.LookingForGun;
        }
        else if (!hasGrenade)
        {
            state = NpcState.LookingForBomb;
        }
        else if (EnemeyIsInRange())
        {
            state = NpcState.Attacking;
        }
        else if (!leader)
        {
            state = NpcState.Patrolling;
        }
        else
        {
            state = NpcState.FollowLeader;
        }

        switch (this.state)
        {
            case NpcState.Patrolling:
                {


                    if (state == NpcState.Patrolling && navMeshAgent.remainingDistance < 1f)
                    {
                        restAnimationState();
                        npcAnim.SetBool("isIdle", true);
                        state = NpcState.Idle;
                    }
                    if (state == NpcState.Idle)
                    {
                        StartCoroutine(PatrolMove((currentIndex + 1) % PatrolPositions.Length));
                    }


                    break;
                }

            case NpcState.FollowLeader:
                {
                    if (state == NpcState.FollowLeader && navMeshAgent.remainingDistance < 0.9f)
                    {
                        restAnimationState();
                        npcAnim.SetBool("isIdle", true);
                        state = NpcState.Idle;
                    }
                    if (state == NpcState.Idle)
                    {
                        StartCoroutine(FollowerMove());
                    }
                    break;
                }

            case NpcState.Attacking:
                {
                    // gun:0 , grenade:1
                    int weaponChosen = 0;
                    if (hasGrenade && hasGun)
                    {
                        float g = Random.Range(-10.0f, 10.0f);
                        if (g < 0)
                        {
                            weaponChosen = 0;
                        }
                        else
                        {
                            weaponChosen = 1;
                        }
                    }
                    else if (hasGrenade)
                    {
                        weaponChosen = 1;
                    }
                    else if (hasGun)
                    {
                        weaponChosen = 0;
                    }

                    //gun
                    if (weaponChosen == 0)
                    {
                        restAnimationState();
                        npcAnim.SetBool("isShooting", true);
                        ShootGun();
                    }
                    else
                    {

                    }

                    break;

                }
            case NpcState.LookingForGun:
                {
                    if (prevState != NpcState.LookingForGun || (currentLookableGun != null && !currentLookableGun.activeSelf))
                    {
                        for (int i = 0; i < Guns.Length; i++)
                        {
                            if (Guns[i].gameObject.activeSelf)
                            {
                                currentLookableGun = Guns[i].gameObject;
                                Vector3 gunPos = Guns[i].gameObject.transform.position;
                                navMeshAgent.SetDestination(gunPos);
                                restAnimationState();
                                npcAnim.SetBool("isWalking", true);
                                break;
                            }
                        }
                    }
                    break;
                }
            case NpcState.LookingForBomb:
                {
                    if (prevState != NpcState.LookingForBomb || (currentLookableGrenade != null && !currentLookableGrenade.activeSelf))
                    {
                        for (int i = 0; i < Grenades.Length; i++)
                        {
                            if (Grenades[i].gameObject.activeSelf)
                            {
                                currentLookableGrenade = Grenades[i].gameObject;
                                Vector3 grenadePos = Grenades[i].gameObject.transform.position;
                                navMeshAgent.SetDestination(grenadePos);
                                restAnimationState();
                                npcAnim.SetBool("isWalking", true);
                                break;
                            }
                        }
                    }
                    break;
                }

            case NpcState.Killed:
                {
                    Debug.Log(gameObject.name + " was killed ");
                    // play Animation isDead 
                    restAnimationState();
                    npcAnim.SetBool("isDead", true);
                    // Destroy(gameObject);
                    break;
                }
        }
        prevState = state;
    }







    public IEnumerator PatrolMove(int index)
    {
        yield return new WaitForSeconds(2.1f);
        currentIndex = index;
        Vector3 target = PatrolPositions[currentIndex].transform.position;
        navMeshAgent.SetDestination(target);
        restAnimationState();
        npcAnim.SetBool("isWalking", true);
    }

    public IEnumerator FollowerMove()
    {
        yield return new WaitForSeconds(1.5f);

        Vector3 target = leader.transform.position;
        navMeshAgent.SetDestination(target);
        restAnimationState();
        npcAnim.SetBool("isWalking", true);
    }

    bool EnemeyIsInRange()
    {

        float radius = enemyRange.radius;

        Collider[] coll = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < coll.Length; i++)
        {
            GameObject obj = coll[i].gameObject;

            for (int j = 0; j < enemies.Length; j++)
            {
                if (obj != null && obj == enemies[j])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TakeDamage(float n)
    {
        Debug.Log(gameObject.name + "took damage : " + n);
        health -= n;
        if (health <= 0)
        {
            state = NpcState.Killed;
        }
    }

    void AttackPlayer()
    {

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


    void restAnimationState()
    {
        npcAnim.SetBool("isWalking", false);
        npcAnim.SetBool("isShooting", false);
        npcAnim.SetBool("isThrowing", false);
        npcAnim.SetBool("isIdle", false);
        npcAnim.SetBool("isDead", false);
    }


    void ShootGun()
    {

    }
}
