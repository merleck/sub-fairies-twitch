using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public GameObject targetAux;
    public Vector3 direction;

    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    private void OnDestroy()
    {
        Destroy(target);
    }

    public override Steering GetSteering()
    {
        if(targetAux != null)
        {
            Agent targetAgent = targetAux.GetComponent<Agent>();
            direction = targetAux.transform.position + (Vector3)targetAgent.velocity - transform.position;

            if(direction.magnitude > 0.0f)
            {
                float targetOrientation = Mathf.Atan2(-direction.x, direction.y);

                targetOrientation *= Mathf.Rad2Deg;
                target.GetComponent<Agent>().orientation = targetOrientation;
            }
        }
        return base.GetSteering();
    }
}
