using UnityEngine;

public class LeftSwing : MonoBehaviour {

    private Vector3 swingPoint;
    public LayerMask whatIsSwingable;
    public Transform origin, camera, player;
    private float maxDistance = 75f;
    private SpringJoint joint;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSwing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSwing();
        }
    }


    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsSwingable))
        {
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.01f;
            joint.minDistance = distanceFromPoint * 0.001f;

            //Adjust these values to fit your game.
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopSwing() {
        Destroy(joint);
    }

    public bool IsSwinging() {
        return joint != null;
    }

    public Vector3 GetSwingPoint() {
        return swingPoint;
    }
}
