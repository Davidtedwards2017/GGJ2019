using UnityEngine;

public static class TransformExtentions  {

	public static bool HasComponent<T>(this Transform transform)
	{
		T component = transform.GetComponent<T>();
		return component != null;
	}

}
