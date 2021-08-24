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
    public float shotDamage = 15;

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


    private float shotTime = 0f;
    private float grenadeTime = 0f;
    private int chosenWeapon = 0;
    private bool isAttacking = false;


    public GameObject grenadePrefab;
    public Transform hand;
    public float throwFroce = 10f;

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

                    if (isAttacking)
                    {
                        weaponChosen = chosenWeapon;
                    }
                    else
                    {
                        isAttacking = true;
                        chosenWeapon = weaponChosen;
                    }


                    //gun
                    if (weaponChosen == 0)
                    {
                        shotTime += Time.deltaTime;
                        if (shotTime >= 1f || shotTime == 0)
                        {
                            restAnimationState();
                            npcAnim.SetBool("isShooting", true);

                            StartCoroutine(ShootGun());
                            StartCoroutine(ShowShot());
                            shotTime = 0f;
                            isAttacking = false;
                        }
                        //if (state == NpcState.Attacking && prevState != state)

                    }
                    else // grenade 
                    {
                        grenadeTime += Time.deltaTime;
                        if (grenadeTime >= 2.15f || grenadeTime == 0)
                        {
                            restAnimationState();
                            npcAnim.SetBool("isThrowing", true);


                            ThrowGrenade();


                            grenadeTime = 0f;
                            isAttacking = false;
                        }

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

        Debug.Log(gameObject.name + " took " + n + " damage ,health is :" + health);

        gameLogic.AddText(gameObject.name + " took " + n + " damage");
        if (health <= 0)
        {
            state = NpcState.Killed;
            navMeshAgent.SetDestination(this.transform.position);
            gameLogic.AddText(gameObject.name + " got killed");
            GetComponent<Collider>().enabled = false;

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



    void ThrowGrenade()
    {

        GameObject enemy = EnemeyIsInRange();
        if (enemy)
        {
            navMeshAgent.SetDestination(this.transform.position);
            transform.LookAt(enemy.transform);
            GameObject gren = Instantiate(grenadePrefab, hand.position, transform.rotation) as GameObject;
            gren.GetComponent<Rigidbody>().AddForce(transform.forward * throwFroce, ForceMode.Impulse);
        }
    }


    IEnumerator ShootGun()
    {

        GameObject enemy = EnemeyIsInRange();


        if (enemy)
        {
            navMeshAgent.SetDestination(this.transform.position);
            transform.LookAt(enemy.transform);

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, shootingRange))
            {

                if (hit.transform.gameObject == enemy)
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
        yield return new WaitForSeconds(0.1f);
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
