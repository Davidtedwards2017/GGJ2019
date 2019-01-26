using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class RandomAnimationTrigger : MonoBehaviour {

    public string AnimationTriggerName;

    public float MinDelayTime = 0;
    public float MaxDelayTime = 1;

    private Animator _animator;
    private Coroutine _Sequence;

    void Start () {
        _animator = GetComponent<Animator>();
	}
    private void OnEnable()
    {
        _Sequence = StartCoroutine(TiggeringSequence());
    }

    private void OnDisable()
    {
        if(_Sequence != null)
        {
            StopCoroutine(_Sequence);
            _Sequence = null;
        }
    }

    private IEnumerator TiggeringSequence()
    {
        while(true)
        {
            yield return WaitTillNextTriggerTime();
            _animator.SetTrigger(AnimationTriggerName);
        }
    }

    private IEnumerator WaitTillNextTriggerTime()
    {
        var waitTime = Random.Range(MinDelayTime, MaxDelayTime);
        yield return new WaitForSeconds(waitTime);
    }
}
