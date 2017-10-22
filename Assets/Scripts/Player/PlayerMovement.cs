//This script handles moving the player. As the player doesn't move using a navmesh agent, some calculations have to be done to
//get the appropriate level of control.

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float _speed = 6f;									//The speed that the player moves
	
	private Vector3 _moveDirection = Vector3.zero;				//The direction the player should move
	private Vector3 _lookDirection = Vector3.forward;			//The direction the player should face
	private Rigidbody _rigidBody;								//Reference to the rigidbody component	

	//---------------------------------------------------------------------
	// Properties
	//---------------------------------------------------------------------

	public Vector3 MoveDirection
	{
		get { return _moveDirection; }
		set { _moveDirection = value; }
	}

	public Vector3 LookDirection
	{
		get { return _lookDirection; }
		set { _lookDirection = value; }
	}

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Awake()
	{
		//Grab the needed component references
		_rigidBody = GetComponent<Rigidbody>();
	}

	//Move with physics so the movement code goes in FixedUpdate()
	private void FixedUpdate ()
	{
		//Remove any Y value from the desired move direction
		_moveDirection.Set (_moveDirection.x, 0, _moveDirection.z);
		//Move the player using the MovePosition() method of its rigidbody component. This moves the player is a more
		//physically accurate way than transform.Translate() does
		_rigidBody.MovePosition (transform.position + _moveDirection.normalized * _speed * Time.deltaTime);

		//Remove any Y value from the desired look direction
		_lookDirection.Set (_lookDirection.x, 0, _lookDirection.z);
		//Rotate the player using the MoveRotation() method of its rigidbody component. This rotates the player is a more
		//physically accurate way than transform.Rotate() does. We also use the LookRotation() method of the Quaternion
		//class to help use convert our euler angles into a quaternion
		_rigidBody.MoveRotation (Quaternion.LookRotation (_lookDirection));
    }
}

