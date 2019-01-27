using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackArea : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        var trunk = other.GetComponent<TrunkController>();
        if(trunk != null)
        {
            trunk.OnEnterTrackArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var trunk = other.GetComponent<TrunkController>();
        if (trunk != null)
        {
            trunk.OnLeaveTrackArea(this);
        }
    }
}
