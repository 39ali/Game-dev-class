// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NpcController : MonoBehaviour
// {


// 	private npcMove[] npcs; 
// 	private int currentNpc=0;
// 	//move only once npc at time 
// 	private bool oneIsWalking=false ;
//     // Start is called before the first frame update
//     void Start()
//     {
//        npcs = FindObjectsOfType<npcMove>();      
//     }

//    void Update()
//     {
//     	for (int i =0;i< npcs.Length;i++){
//     		if (npcs[i].tryStop()){
//     			oneIsWalking=false ;
//     			Debug.Log("making npc {" +i +"} stop");
//     		}		
//     	}

// 		if (!oneIsWalking){
// 			MoveNpc();
// 			oneIsWalking=true ; 
// 		}

//     }

//     void MoveNpc(){

//     	//get the distenation index 
//     	int distenationsLnegth = npcs[0].GetDistenationLength();
//     	bool wasPicked = false  ; 
//     	int disIndex = 0; 
//     	while (!wasPicked){
// 			disIndex = Random.Range(0,distenationsLnegth);

// 			for (int i=0 ;i< npcs.Length;i++){
// 				if (npcs[i].GetCurrentDistenationIndex()==disIndex){
// 					wasPicked=false; 
// 					break;
// 				}
// 				wasPicked=true ;
// 			}
//     	}

// 		int index = ++currentNpc%npcs.Length;
// 		Debug.Log("making npc {" +index +"} walk");
// 		npcs[index].Move(disIndex);

//     	}
//     }

