using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAdaptor : MonoBehaviour {

    public void SetColor(Color color)
    {
        var particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach(var ps in particleSystems)
        {
            ParticleSystem.MainModule settings = ps.main;
            settings.startColor = color;
        }

        foreach(var trail in gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            trail.startColor = new Color(color.r, color.g, color.b, trail.startColor.a);
            trail.endColor = new Color(color.r, color.g, color.b, trail.endColor.a);
        }
    }
}

public static class ColorAdaptorExtentions
{
    public static void SetColor(this GameObject gameObject, Color color)
    {
        if (gameObject == null) return;

        foreach (var colorAdaptor in gameObject.GetComponentsInChildren<ColorAdaptor>())
        {
            colorAdaptor.SetColor(color);
        }
    }
}


