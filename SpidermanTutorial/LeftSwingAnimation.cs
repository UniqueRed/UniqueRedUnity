using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSwingAnimation : MonoBehaviour
{
    public Spring spring;
    private LineRenderer lr;
    public LeftSwing leftSwing;
    private Vector3 currentSwingPosition;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;

    void Awake() {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }
    
    void DrawRope() {
        if (!leftSwing.IsSwinging())
        {
            currentSwingPosition = leftSwing.origin.position;
            spring.Reset();
            if(lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if(lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var swingPoint = leftSwing.GetSwingPoint();
        var originPosition = leftSwing.origin.position;
        var up = Quaternion.LookRotation((swingPoint - originPosition).normalized) * Vector3.up;

        currentSwingPosition = Vector3.Lerp(currentSwingPosition, swingPoint, Time.deltaTime * 8f);

        for(var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(originPosition, currentSwingPosition, delta) + offset);
        }
    }
}
