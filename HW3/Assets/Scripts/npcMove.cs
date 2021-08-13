using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class npcMove : MonoBehaviour
{

	[SerializeField]
	Transform[] distenations; 

	public NavMeshAgent navMeshAgent; 

	private int currentIndex ; 

	//private bool running= false;
	public bool moving = false ; 
	public Animator npcAnim;

	
    // Start is called before the first frame update
    void Start()
    {
     navMeshAgent = this.GetComponent<NavMeshAgent>();
     npcAnim =this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
  	 npcAnim.SetBool("isWalking", false);	
  	 
    }

    // Update is called once per frame
    void Update()
    {

		
        
    }

    public bool tryStop(){
    	if (moving&& navMeshAgent.remainingDistance<1f){		
				moving = false ;
				npcAnim.SetBool("isWalking", false);	
				return true ;
	    	}	

	    return false ;
    }

 
    public void Move (int index){
    	moving=true ; 
    	currentIndex= index; 
    	Vector3 target = distenations[currentIndex].transform.position; 
    	navMeshAgent.SetDestination(target);
       	npcAnim.SetBool("isWalking", true);
    }

    public int GetCurrentDistenationIndex(){
    	return currentIndex;
    }

    public int GetDistenationLength(){
    	return distenations.Length;
    }
}
