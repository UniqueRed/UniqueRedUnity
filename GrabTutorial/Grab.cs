using UnityEngine;

/// <summary>
/// Drag a rigidbody with the mouse using a spring joint.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Grab : MonoBehaviour
{
  public float force = 600;
	public float damping = 6;
	
	Transform jointTrans;
	float dragDepth;


         // array for storing if the 3 mouse buttons are dragging
     bool[] isDragActive;
 
     // for remembering if a button was down in previous frame
     bool[] downInPreviousFrame;
 
     void Start () {
         isDragActive = new bool[] {false, false, false};
         downInPreviousFrame = new bool[] {false, false, false};
     }
     
     void Update () {
         for (int i=0; i<isDragActive.Length; i++)
         {
             if (Input.GetMouseButton(1))
             {
                 if (downInPreviousFrame[1])
                 {
                     if (isDragActive[1])
                     {
                         OnDragging(1);
                     }
                     else
                     {
                         isDragActive[1] = true;
                         OnDraggingStart(1);
                     }
                 }
                 downInPreviousFrame[1] = true;
             }
             else
             {
                 if (isDragActive[1])
                 {
                     isDragActive[1] = false;
                     OnDraggingEnd(1);
                 }
                 downInPreviousFrame[1] = false;
             }
         }
     }
 
     public virtual void OnDraggingStart(int mouseButton)
     {
         // implement this for start of dragging
         HandleInputBegin (Input.mousePosition);
     }
 
     public virtual void OnDragging(int mouseButton)
     {
         // implement this for dragging
         HandleInput (Input.mousePosition);
     }
 
     public virtual void OnDraggingEnd(int mouseButton)
     {
         // implement this for end of dragging
         HandleInputEnd (Input.mousePosition);
     }

	public void HandleInputBegin (Vector3 screenPosition)
	{
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Interactive")) {
				dragDepth = CameraPlane.CameraToPointDepth (Camera.main, hit.point);
				jointTrans = AttachJoint (hit.rigidbody, hit.point);
			}
		}
	}
	
	public void HandleInput (Vector3 screenPosition)
	{
		if (jointTrans == null)
			return;
		var worldPos = Camera.main.ScreenToWorldPoint (screenPosition);
		jointTrans.position = CameraPlane.ScreenToWorldPlanePoint (Camera.main, dragDepth, screenPosition);
	}
	
	public void HandleInputEnd (Vector3 screenPosition)
	{
		Destroy (jointTrans.gameObject);
	}
	
	Transform AttachJoint (Rigidbody rb, Vector3 attachmentPosition)
	{
		GameObject go = new GameObject ("Attachment Point");
		go.hideFlags = HideFlags.HideInHierarchy; 
		go.transform.position = attachmentPosition;
		
		var newRb = go.AddComponent<Rigidbody> ();
		newRb.isKinematic = true;
		
		var joint = go.AddComponent<ConfigurableJoint> ();
		joint.connectedBody = rb;
		joint.configuredInWorldSpace = true;
		joint.xDrive = NewJointDrive (force, damping);
		joint.yDrive = NewJointDrive (force, damping);
		joint.zDrive = NewJointDrive (force, damping);
		joint.slerpDrive = NewJointDrive (force, damping);
		joint.rotationDriveMode = RotationDriveMode.Slerp;
		
		return go.transform;
	}
	
	private JointDrive NewJointDrive (float force, float damping)
	{
		JointDrive drive = new JointDrive ();
		drive.mode = JointDriveMode.Position;
		drive.positionSpring = force;
		drive.positionDamper = damping;
		drive.maximumForce = Mathf.Infinity;
		return drive;
	}
}
