//This script keeps track of the player's health. It is also used to communicate with the GameManager

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField]
	private int _maxHealth = 100;				//Player's maximum health
	
	private PlayerMovement _playerMovement;		//Reference to the player's movement script
	private Animator _animator;					//Reference to the animator component
	
	private int _currentHealth;									//The current health of the player
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		//Grab the needed component references
		_playerMovement = GetComponent<PlayerMovement>();
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		_currentHealth = _maxHealth;
	}
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	public void TakeDamage(int damageAmount)
	{
		if (!IsAlive()) return;
		
		_currentHealth -= damageAmount;

		if (IsAlive()) return;
		
		//If there is a player movement script, tell it to be defeated
		if (_playerMovement != null) _playerMovement.Defeated();

		//Set the Die parameter in the animator
		_animator.SetTrigger("Die");
	}
	
	public bool IsAlive()
	{
		//If the currentHealth is above 0 return true (the player is alive), otherwise return false
		return _currentHealth > 0;
	}
}

