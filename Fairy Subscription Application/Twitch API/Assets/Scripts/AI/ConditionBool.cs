using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBool : Condition
{
    public Agent valueCondition;
    public bool valueTest;

    public override bool Test()
    {
        if(valueTest == valueCondition.called)
        {
            return true;
        }
        return false;
    }
}
