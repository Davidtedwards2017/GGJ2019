using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EffectFactory<T> : Singleton<T> where T : MonoBehaviour
{    
    public void SpawnDelayedEffect(EffectData effect, Vector3 position, float delay)
    {
        StartCoroutine(SpawnDelayedEffectAsync(effect, position, delay));
    }

    private IEnumerator SpawnDelayedEffectAsync(EffectData effect, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SpawnEffect(position);
    }


}
