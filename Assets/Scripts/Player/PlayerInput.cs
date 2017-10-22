using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	private PlayerMovement _playerMovement = null;	//Reference to the player's movement script
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		//Grab the needed component references
		_playerMovement = GetComponent<PlayerMovement> ();
	}

	private void Update ()
	{
		//If there is no movement script, leave
		if (_playerMovement == null) return;
		//Get the raw Horizontal and Vertical inputs (raw inputs have no smoothing applied)
		var horizontal = Input.GetAxisRaw("Horizontal");
		var vertical = Input.GetAxisRaw("Vertical");
		//Tell the movement script to move on the X and Z axes with no Y axis movement
		_playerMovement.MoveDirection = new Vector3(horizontal, 0, vertical);
		//If there is a MouseLocation script and the mouse's position is valid...
		if (MouseLocation.Instance != null && MouseLocation.Instance.IsValid) {
			//Find the point the player should look at by subtracting the player's position from the mouse's position
			Vector3 lookPoint = MouseLocation.Instance.MousePosition - _playerMovement.transform.position;
			//Tell the player what direction to look
			_playerMovement.LookDirection = lookPoint;
		}
	}
}

