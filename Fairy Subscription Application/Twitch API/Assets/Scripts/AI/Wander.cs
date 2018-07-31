using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : AgentBehaviour
{
    public float offset;
    public float radius;
    public float rate;

    public override void Awake()
    {
        target = new GameObject();
        target.transform.position = transform.position;
        base.Awake();
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        //wander is a random angle
        //target makes the random angle relative to the current orientation
        float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
        float targetOrientation = wanderOrientation + agent.orientation;

        //convert current orientation and position of target by the offset
        Vector2 orientationVec = OriToVec(agent.orientation);
        Vector2 targetPosition = (offset * (Vector3)orientationVec) + transform.position;

        //adjust position of target object based on new random direction
        targetPosition = targetPosition + (OriToVec(targetOrientation) * radius);
        target.transform.position = targetPosition;

        //use parent to get orientation then adjust position value

        //linear values
        steering.linear = target.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;

        return steering;
    }
}
