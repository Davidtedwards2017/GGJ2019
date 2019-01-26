using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeScaleIndependent : CustomYieldInstruction
{
    private float _EndTime; 
    public override bool keepWaiting
    {
        get
        {
            return Time.realtimeSinceStartup < _EndTime;
        }
    }

    public WaitTimeScaleIndependent(float seconds)
    {
        _EndTime = Time.realtimeSinceStartup + seconds;
    }
}
