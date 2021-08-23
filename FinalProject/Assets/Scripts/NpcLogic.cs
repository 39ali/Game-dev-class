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




    //   public GameObject aCamera;
    private LineRenderer lr;
    public float shootingRange = 100;
    public float shotDamage = 30;

    [SerializeField]
    public GameObject MuzzleEnd;
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    public ParticleSystem MuzzleFlash;


    public GameLogicScript gameLogic;


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

        lr = GetComponent<LineRenderer>();
        sound = GunInHand.GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        //   if (gameObject.name.Contains("follower"))
        // Debug.Log("currentState: " + this.state);
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
                        if (state == NpcState.Attacking && prevState != state)
                        {
                            restAnimationState();
                            npcAnim.SetBool("isShooting", true);
                        }

                        StartCoroutine(ShootGun());
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
                    // Debug.Log(gameObject.name + " was killed ");
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

    GameObject EnemeyIsInRange()
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

                    NpcLogic npc = enemies[j].GetComponent<NpcLogic>();
                    if (npc != null && !npc.isDead())
                    {
                        return obj;
                    }

                    GunShooting player = enemies[j].GetComponent<GunShooting>();
                    if (player != null && !player.isDead())
                    {
                        return obj;
                    }

                    //return obj;
                }
            }
        }

        return null;
    }

    public void TakeDamage(float n)
    {
        health -= n;
        gameLogic.AddText(gameObject.name + " took " + n + "damage");
        if (health <= 0)
        {
            state = NpcState.Killed;
            navMeshAgent.enabled = false;

            gameLogic.AddText(gameObject.name + "got killed");

        }
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


    IEnumerator ShootGun()
    {


        yield return new WaitForSeconds(2f);


        GameObject enemy = EnemeyIsInRange();


        if (enemy)
        {
            navMeshAgent.SetDestination(this.transform.position);
            transform.LookAt(enemy.transform);

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, shootingRange))
            {
                 

                NpcLogic npc = hit.transform.gameObject.GetComponent<NpcLogic>();
                if (npc != null)
                {
                    Debug.Log(hit.transform.name + "was shot " + shotDamage);
                    npc.TakeDamage(shotDamage);
                }

                GunShooting player = hit.transform.gameObject.GetComponent<GunShooting>();
                if (player != null)
                {
                    Debug.Log(hit.transform.name + "was shot " + shotDamage);
                    player.TakeDamage(shotDamage);
                }

            }
        }

    }


    public IEnumerator ShowShot()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);

        lr.SetPosition(0, ray.origin);
        lr.SetPosition(1, ray.GetPoint(shootingRange));
        lr.enabled = true;
        MuzzleFlash.Play();
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        lr.enabled = false;
    }


    public bool isDead()
    {
        return health <= 0;
    }
}
