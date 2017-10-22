//This script handles the game management. Game managers are often completely different and generally provide whatever
//specific and varied services an individual game may need. In this project, in an effort to make the code simple to understand
//and modular, the game manager is tied into several core functions of the player, enemies, and allies. Namely, this manager
//keeps track of the player and the players state, handles all scoring, interfaces with the UI, summons the allies, and 
//reloads the scene when the player is defeated

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;				//This script, like MouseLocation, has a public static reference to itself to that other scripts
													//can access it from anywhere without needing to find a reference to it

	[Header("Player and Enemy Properties")]
	public Transform Player;						//A reference to the player's health script which will be considered "the player"

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Awake()
	{
		//This is a common approach to handling a class with a reference to itself.
		//If instance variable doesn't exist, assign this object to it
		if (Instance == null)
			Instance = this;
		//Otherwise, if the instance variable does exist, but it isn't this object, destroy this object.
		//This is useful so that we cannot have more than one GameManager object in a scene at a time.
		else if (Instance != this)
			Destroy(this);
	}
}
