//This script controls the movement of the enemies. The enemy uses a navmesh agent to navigate the scene. 
//The target of the enemy's navigation is received from the GameManager and that allows the enemy to chase 
//whatever the GameManager wants (which happens when allies are spawned). Additionally, the enemy movement
//can be affected by a frost debuff which freezes it in place

using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	private static readonly WaitForSeconds UPDATE_DELAY = new WaitForSeconds(.5f);	//The delay between updating the navmesh agent (for efficiency). Since
																					//all enemies have the same delay, this is declared as 'static' so all
																					//enemies share the same one (saves on memory)
	
	private UnityEngine.AI.NavMeshAgent _navMeshAgent; 		//Reference to the navmesh agent component

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private  void Awake()
	{
		//Grab references to the needed components
		_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}

	//When this game object is enabled...
	private void OnEnable()
	{
		//Enabled the nav mesh agent
		_navMeshAgent.enabled = true;
		//Start the ChasePlayer coroutine
		StartCoroutine("ChasePlayer");
	}
	
	//---------------------------------------------------------------------
	// Coroutines
	//---------------------------------------------------------------------

	//This coroutine updates the navmesh agent to chase the player
	private IEnumerator ChasePlayer ()
	{
		//Start by waiting a single frame to give the game a chance to initialize.
		//This is usefull if you start with an enemy in the scene (instead of spawning it)
		yield return null;

		//If there is no GameManager, leave this coroutine permanently (that's 
		//what 'yield break' does
		if (GameManager.Instance == null) yield break;

		//While the navmesh agent is enabled...
		while (_navMeshAgent.enabled)
		{
			//get the target from the GameManager
			var target = GameManager.Instance.Player;
			
			//if the target exists, head towards it
			if (target != null) _navMeshAgent.SetDestination(target.transform.position);
			// finally, wait a time interval before looping
			yield return UPDATE_DELAY;
		}
	}
}

