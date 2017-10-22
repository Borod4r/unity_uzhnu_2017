//This script tracks the location of the mouse in 3D space. It does this by taking the location
//of the mouse on the screen (2D space). It then draws a line from the camera, through the mouse's 
//position on the screen into the world. Finally, it determines where this line hits a collider in
//the 3D scene. 

//This line of code is special and its purpose is to disable warning 0414 in this script. That warning
//writes to the console and tells you that this script has a variable that is created but never used. The IDE
//thinks we don't use the variable isTouchAiming because it is wrapped in platform specific code. Therefore
//when we are on PC we don't use that variable, but when we are mobile we do. Instead of having this warning
//constantly in the console window, this line simply turns that warning off (for this script only)
#pragma warning disable 0414
using UnityEngine;

public class MouseLocation : MonoBehaviour
{
	public static MouseLocation Instance;		//A reference to a MouseLacation object. This allows the class to have a public reference to itself to other scripts can 
												//access it without having a reference to it. 

	[HideInInspector]
	public Vector3 MousePosition;	            //Location in 3D space of the mouse cursor	
	[HideInInspector]
	public bool IsValid;			            //Is the mouse location valid?

	[SerializeField]
	private LayerMask _whatIsGround;		    //A LayerMask indicating what is considered to be ground when determining the mouse's location

	private Ray _mouseRay;						//A ray that will be used to find the mouse
	private RaycastHit _hit;					//A RaycastHit which will store information about a raycast
	private Vector2 _screenPosition;			//Where the mouse is on the screen
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake()
	{
		//This is a common approach to handling a class with a reference to itself.
		//If instance variable doesn't exist, assign this object to it
		if (Instance == null) Instance = this;
		//Otherwise, if the instance variable does exist, but it isn't this object, destroy this object.
		//This is useful so that we cannot have more than one MouseLocation object in a scene at a time.
		else if (Instance != this) Destroy(this);
	}

	private void Update()
	{
		//Assume the mouse location isn't valid
		IsValid = false;

		//record the mouse's position to the screenPosition variable
		_screenPosition = Input.mousePosition;

		//Create a ray that extends from the main camera, through the mouse's position on the screen
		//into the scene
		_mouseRay = Camera.main.ScreenPointToRay(_screenPosition);

		//If the ray from our camera hits something that is ground...
		if (Physics.Raycast(_mouseRay, out _hit, 100f, _whatIsGround))
		{
			//...the mouse position is valid...
			IsValid = true;
			//...and record the point in 3D space that the ray hit the ground
			MousePosition = _hit.point;
		}
	}
}
