using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    public float wanderTime = 60.0f;

    public float offset = 0.1f;
    public float radius = 3.0f;
    public float rate = 180.0f;

    public float timer = 0.0f;

    public override void Awake()
    {
        target = new GameObject();
        target.transform.position = transform.position;
        timer += Random.Range(0.0f,30.0f);
        base.Awake();
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > wanderTime)
        {
            timer = 0.0f;
        }
        agent.SetSteering(GetSteering());
    }

    public override Steering GetSteering()
    {
        //wander behaviour
        Steering wanderSteering = new Steering();

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

        //linear values
        wanderSteering.linear = target.transform.position - transform.position;
        wanderSteering.linear.Normalize();
        wanderSteering.linear = wanderSteering.linear * agent.maxAccel;

        return wanderSteering;
    }

    public override void OnEnable()
    {

    }

    public override void OnDisable()
    {

    }
}
