using UnityEngine;
using System.Collections;

public class RandomRescaler : MonoBehaviour {

    public float MaxScaleAmt = 1;
    public float MinScaleAmt = 1;

    void Start () {

        var baseX = transform.localScale.x;
        var baseY = transform.localScale.y;
        var baseZ = transform.localScale.z;

        var scaleAmt = Random.Range(MinScaleAmt, MaxScaleAmt);
        transform.localScale = new Vector3(baseX * scaleAmt, baseY * scaleAmt, baseZ * scaleAmt);

    }
	
}
