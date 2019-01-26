using UnityEngine;
using System.Collections;
using System.Linq;

public static class EffectExtensions
{
    public static void Play(this SlowDownEffectData data)
    {
        if (data == null) return;
        EffectsController.Instance.PlaySlowDownEffect(data);
    }

    public static GameObject Spawn(this EffectData effect, Vector3 position)
    {
        if (effect == null) return null;

        return effect.SpawnEffect(position);
    }

    public static GameObject SpawnEffect(this  EffectData effect, Vector3 position)
    {
        return SpawnEffectInstance(effect, position);
    }

    private static GameObject SpawnEffectInstance(this  EffectData effect, Vector3 position)
    {
        if (effect == null || effect.Prefab == null) return null;

        var go = GameObject.Instantiate(effect.Prefab, position + effect.Offset, Quaternion.identity);

        if (effect.OverrideColor)
        {
            go.SetColor(effect.Color);
        }

        if (effect.Duration > 0)
        {
            go.Fizzle(effect.Duration, effect.FizzleDuration);
        }

        return go;
    }

    //public static void SpawnDelayedEffect(this  EffectData effect, Vector3 position, float delay)
    //{
    //    StartCoroutine(SpawnDelayedEffectAsync(effect, position, delay));
    //}

    public static IEnumerator SpawnDelayedEffectAsync(this EffectData effect, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEffectInstance(effect, position);
    }
}
