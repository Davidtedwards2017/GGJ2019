using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

[ExecuteInEditMode]
public class DistanceCheck : MonoBehaviour {

#if  UNITY_EDITOR
    public GameObject A;
    public GameObject B;

    private float _Distance;
    private Vector3 _Center;

    // Update is called once per frame
    void Update () {
        if (A == null || B == null) return;

        _Distance = Vector3.Distance(A.transform.position, B.transform.position);
        _Center = Vector3.Lerp(A.transform.position, B.transform.position, 0.5f);
        Debug.DrawLine(A.transform.position, B.transform.position);
	}

    void OnDrawGizmos()
    {
        Handles.Label(_Center, _Distance.ToString());
    }

#endif
}
