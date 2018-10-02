using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public List<Transition> transitions;
    public GameObject target;
    protected Agent agent;

	public virtual void Awake ()
    {
        transitions = new List<Transition>();
        agent = gameObject.GetComponent<Agent>();
        //setup transitions
	}
	
	public virtual void Update ()
    {
        agent.SetSteering(GetSteering());
	}

    public virtual Steering GetSteering()
    {
        return new Steering();
    }

    public virtual void OnEnable()
    {
        //initialise
    }

    public virtual void OnDisable()
    {
        //finalize
    }

    public void LateUpdate()
    {
        foreach(Transition t in transitions)
        {
            if(t.condition.Test())
            {
                State target;
                target = t.target;

                target.enabled = true;
                this.enabled = false;
                return;
            }
        }
    }

    public Vector2 OriToVec(float orientation)
    {
        Vector2 vector = Vector2.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.y = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
