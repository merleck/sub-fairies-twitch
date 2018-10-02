using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveState : State
{
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;
    bool arrived = false;
    public float calledTime = 5.0f;
    float timer = 0.0f;

    public override void Awake()
    { 
        base.Awake();

        //go to start when called by user
        ConditionBool cb = new ConditionBool();
        cb.valueCondition = agent;
        cb.valueTest = false;
        State st = GetComponent<NextPointState>();
        Transition transition = new Transition();
        transition.condition = cb;
        transition.target = st;
        transitions.Add(transition);
    }

    public override void Update()
    {
        agent.SetSteering(GetSteering());
        if (arrived)
        {
            timer += Time.deltaTime;
        }
        if(timer > calledTime)
        {
            timer = 0.0f;
            agent.called = false;
        }
    }

    public override Steering GetSteering()
    {
        //waypoint arrive behaviour
        Steering steering = new Steering();

        Vector2 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;
        arrived = true;

        if (!(distance < targetRadius))
        {
            arrived = false;
            if (distance > slowRadius)
                targetSpeed = agent.maxSpeed;
            else
                targetSpeed = agent.maxSpeed * distance / slowRadius;
            Vector2 desiredVelocity = direction;
            desiredVelocity.Normalize();
            desiredVelocity *= targetSpeed;
            steering.linear = desiredVelocity - agent.velocity;
            steering.linear /= timeToTarget;

            if (steering.linear.magnitude > agent.maxAccel)
            {
                steering.linear.Normalize();
                steering.linear *= agent.maxAccel;
            }
        }
        
        return steering;
    }

    public override void OnEnable()
    {

    }

    public override void OnDisable()
    {
    }
}
