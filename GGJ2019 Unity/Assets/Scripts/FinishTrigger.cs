using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        var trunk = other.GetComponent<TrunkController>();   
        if(trunk != null)
        {
            Debug.Log("Trunk entered exit trigger");
            trunk.ReachDestination();
        }
    }
}
