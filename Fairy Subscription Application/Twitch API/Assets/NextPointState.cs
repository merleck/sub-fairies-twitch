using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPointState : State
{
    public List<GameObject> waypoints;
    public int curWaypoint = 0;

    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    [Range(0.0f,1.0f)]
    public float wanderAmount = 0.5f;
    public float wanderTime = 60.0f;


    public float offset = 0.1f;
    public float radius = 3.0f;
    public float rate = 180.0f;
    GameObject targetAux;

    float timer = 0.0f;

    void NearestWaypoint()
    {
        float Distance = Mathf.Infinity;
        foreach (GameObject w in waypoints)
        {
            if ((gameObject.transform.position - w.transform.position).magnitude < Distance)
            {
                Distance = (gameObject.transform.position - w.transform.position).magnitude;
                curWaypoint = waypoints.IndexOf(w);
            }
        }
    }

    public override void Awake ()
    {
        targetAux = new GameObject();
        targetAux.transform.position = transform.position;

        timer = Random.Range(0.0f, wanderTime/2);

        base.Awake();

        //go to start when called by user
        ConditionBool cb = new ConditionBool();
        cb.valueCondition = agent;
        cb.valueTest = true;
        State st = GetComponent<ArriveState>();
        Transition transition = new Transition();
        transition.condition = cb;
        transition.target = st;
        transitions.Add(transition);
    }
	
	public override void Update ()
    {
        timer += Time.deltaTime;
        if (timer > wanderTime)
        {
            if (curWaypoint == waypoints.Count - 1)
                curWaypoint = 0;
            else
                curWaypoint++;
            target = waypoints[curWaypoint];
            timer = Random.Range(0.0f, wanderTime / 2);
        }
        agent.SetSteering(GetSteering());
    }

    public override Steering GetSteering()
    {
        //waypoint arrive behaviour
        Steering waypointSteering = new Steering();

        Vector2 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;

        if(!(distance < targetRadius))
        {
            if (distance > slowRadius)
                targetSpeed = agent.maxSpeed;
            else
                targetSpeed = agent.maxSpeed * distance / slowRadius;
            Vector2 desiredVelocity = direction;
            desiredVelocity.Normalize();
            desiredVelocity *= targetSpeed;
            waypointSteering.linear = desiredVelocity - agent.velocity;
            waypointSteering.linear /= timeToTarget;
            
            if(waypointSteering.linear.magnitude > agent.maxAccel)
            {
                waypointSteering.linear.Normalize();
                waypointSteering.linear *= agent.maxAccel;
            }
        }
        
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
        targetAux.transform.position = targetPosition;

        //linear values
        wanderSteering.linear = targetAux.transform.position - transform.position;
        wanderSteering.linear.Normalize();
        wanderSteering.linear = wanderSteering.linear * agent.maxAccel;


        //Blend the behaviours
        Steering steering = new Steering();

        steering.linear += waypointSteering.linear * (1 - wanderAmount);
        steering.linear += wanderSteering.linear * wanderAmount;

        return steering;
    }

    public override void OnEnable()
    {
        NearestWaypoint();
        target = waypoints[curWaypoint];
    }

    public override void OnDisable()
    {

    }
}
