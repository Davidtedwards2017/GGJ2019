using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleSystemFizzle : MonoBehaviour {

    public bool ScaleDown = true;
    public float DelayBeforeStart;    
    public float Duration;
    private Tweener _ScalingTweener;
    private Coroutine _FizzleCoroutine;

    public void StartFizzle(bool destroyAtEnd = true)
    {
        _FizzleCoroutine = StartCoroutine(Fizzle(destroyAtEnd));
    }
    public IEnumerator Fizzle(bool destroyAtEnd = true)
    {
        if (destroyAtEnd)
        {
            Destroy(gameObject, DelayBeforeStart + Duration);
        }
        
        if(!gameObject.activeSelf || !gameObject.activeInHierarchy)
        {
            yield break;
        }

        yield return new WaitForSeconds(DelayBeforeStart);

        if(ScaleDown)
        {
            _ScalingTweener = gameObject.transform.DOScale(0, Duration);
        }

        foreach(var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            var module = particleSystem.main;
            module.loop = false;
        }
    }


    private void OnDestroy()
    {
        if(_FizzleCoroutine != null)
        {
            StopCoroutine(_FizzleCoroutine);
        }

        if(_ScalingTweener != null)
        {
            _ScalingTweener.Kill();
        }
    }
}

public static class FizzleExtenstions
{
    public static void Fizzle(this GameObject go, float delayBeforeStart, float fizzleDuration = 0, bool destroyAtEnd = true)
    {
        if (go == null) return;
        go.transform.SetParent(null);
        var fizzleEffect = go.GetComponent<ParticleSystemFizzle>();
        if (fizzleEffect == null)
        {
            fizzleEffect = go.AddComponent<ParticleSystemFizzle>();
            fizzleEffect.DelayBeforeStart = delayBeforeStart;
            fizzleEffect.Duration = fizzleDuration;
        }
            
        fizzleEffect.StartFizzle(destroyAtEnd);
    }
}

