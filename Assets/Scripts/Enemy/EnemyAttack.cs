//This script controls the enemy's ability to attack. Attacking occurs when the player is within a trigger collider on the enemy (in range). Furthermore, the enemy
//doesn't attack whenever the player is in range, instead there is a time interval that the enemy tries to attack. Therefore, the player is only attacked
//if they are in range and the time interval occurs. This allows players to dodge in and out of range of enemies with slower attacks.

using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] 
	private float _timeBetweenAttacks = 0.5f;		//How much time goes by between each attack (the attack interval)
	[SerializeField]
	private int _attackDamage = 10;					//How much damage the enemy attacks for

	private Animator _animator;						//Reference to the animator component
	
	private bool _canAttack;						//Can this enemy attack?
	private bool _playerInRange;					//Is the player in range?
	private WaitForSeconds _attackDelay;			//Variable to hold the attack delay

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private  void Awake()
	{
		//Initialize our attackDelay variable here (it can be more efficient than doing it in the coroutine)
		_attackDelay = new WaitForSeconds(_timeBetweenAttacks);
		//Get a reference to the animator component
		_animator = GetComponent<Animator>();
	}

	//When this game object is enabled...
	private void OnEnable()
	{
		//Start the AttackPlayer coroutine
		_canAttack = true;
		StartCoroutine("AttackPlayer");
	}
	
	//When the player enters the trigger collider this is called. In reality, it could be
	//any game object with a collider that enters the trigger. We can be confident that it
	//will be the player, however, since a rigidbody is needed for collision. The player is
	//the only object with a rigidbody, so it is likely the player that has entered the collider
	private void OnTriggerEnter (Collider other)
	{
		//In case other rigidbodies are added to the project, we still want to make sure that the player
		//is the object that entered this collider. If the game object entering this collider
		//is the Player value of the GameManager (it's the player)...
		if(other.transform == GameManager.Instance.Player.transform)
		{
			//Record that the player is in range
			_playerInRange = true;
		}
	}
	
	//When the player leaves the trigger collider this is called
	private void OnTriggerExit (Collider other)
	{
		//If the game object leaving this collider
		//is the Player value of the GameManager (it's the player)...
		if(other.transform == GameManager.Instance.Player.transform)
		{
			//Record that the player is not in range
			_playerInRange = false;
		}
	}
	
	//---------------------------------------------------------------------
	// Coroutines
	//---------------------------------------------------------------------

	//This coroutine checks to see if the enemy can attack the player at a set time interval
	private	IEnumerator AttackPlayer()
	{
		//Start by waiting a single frame to give the game a chance to initialize.
		//This is usefull if you start with an enemy in the scene (instead of spawning it)
		yield return null;

		//If there is no GameManager, leave this coroutine permanently
		//that's what 'yield break' does
		if (GameManager.Instance == null) yield break;

		//While the enemy can attack and the player isn't defeated...
		while (_canAttack && CheckPlayerStatus())
		{
			//...and if the player is in range
			if (_playerInRange) 
			{
				//...Tell the player to take damage. Note that we route enemy damage
				//through to GameManager in case we want to do any modification or validation
				GameManager.Instance.Player.TakeDamage(_attackDamage);
			}
			//Finally, wait an attack delay interval before looping again
			yield return _attackDelay;
		}
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------
	
	//This method determines if the player can be attacked
	private bool CheckPlayerStatus()
	{
		//If the player is alive and well, return true
		if (GameManager.Instance.Player.IsAlive()) return true;

		//If the player isn't alive, the enemies have won, trigger the PlayerDead parameter
		_animator.SetTrigger("PlayerDead");
		//Call the Defeated() method, even though the enemies won, we don't want them
		//running more attack code as it is unnecessary
		Defeated();
		//Return false because the player isn't attackable
		return false;
	}
	
	//Called when the enemy needs to stop attacked (either it was defeated or the player was)
	private void Defeated()
	{
		//Enemy can no longer attack
		_canAttack = false;
	}
}

