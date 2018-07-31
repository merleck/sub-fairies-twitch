using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : AgentBehaviour
{
    public float targetRadius;
    public float slowRadius;
    public bool facing;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        facing = false;
        if (target == null)
            return steering;
        float targetOrientation = target.GetComponent<Agent>().orientation;
        float rotation = targetOrientation + agent.orientation;
        
        //map rotation between -180 and 180 degrees
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);
        
        //if aligned to target already
        if (rotationSize < targetRadius)
        {
            facing = true;
            return steering;
        }

        float targetRotation;

        //if not close to facing target
        if (rotationSize > slowRadius)
            targetRotation = agent.maxRotation;
        else
            targetRotation = agent.maxRotation * rotationSize / slowRadius;

        //turn left or right
        targetRotation *= rotation / rotationSize;

        //set steering
        steering.angular = targetRotation = agent.rotation;
        steering.angular /= timeToTarget;
        float angularAccel = Mathf.Abs(steering.angular);
        if(angularAccel > agent.maxAngularAccel)
        {
            steering.angular /= angularAccel;
            steering.angular *= agent.maxAngularAccel;
        }
        return steering;   
    }
}
