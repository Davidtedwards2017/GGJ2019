using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class FinishTrigger : MonoBehaviour {

    public Transform LookAtTarget;
    public List<Transform> FlyPath;
    public float FlyDownDuration = 5.0f;

    public Ease EndSequenceEase;
    public float StartPathDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        var trunk = other.GetComponent<TrunkController>();   
        if(trunk != null)
        {
            Debug.Log("Trunk entered exit trigger");
            trunk.ReachDestination(this);
        }
    }

}
