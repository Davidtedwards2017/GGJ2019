using UnityEngine;
using System.Collections;

public static class MathExtensions  {

	public static float Round(this float val, float multiple)
    {
        if (multiple == 0)
        {
            return val;
        }

        return Mathf.Round(val * multiple) / multiple;

        //return (val % multiple) * multiple;
    }

    public static float RandomOffset(this float seedValue, float allowedOffset)
    {
        var min = seedValue - allowedOffset;
        var max = seedValue + allowedOffset;

        return Random.Range(min, max);
    }

    public static float RandomOffset(this int seedValue, int allowedOffset)
    {
        var min = seedValue - allowedOffset;
        var max = seedValue + allowedOffset;

        return Random.Range(min, max);
    }


    public static Vector3 RandomOffset(this Vector3 seedValue, float x, float y, float z)
    {
        return seedValue.RandomOffset(new Vector3(x, y, z));
    }

    public static Vector3 RandomOffset(this Vector3 seedValue, Vector3 allowedOffset)
    {
        return new Vector3(
            seedValue.x.RandomOffset(allowedOffset.x),
            seedValue.y.RandomOffset(allowedOffset.y),
            seedValue.z.RandomOffset(allowedOffset.z)
            );
    }
}
