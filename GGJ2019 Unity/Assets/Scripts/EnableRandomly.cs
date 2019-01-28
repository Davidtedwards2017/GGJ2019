using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilites;

public class EnableRandomly : MonoBehaviour {

    public List<GameObject> Targets;

	void Awake () {
	    foreach(var target in Targets)
        {
            target.SetActive(false);
        }

        Targets.PickRandom().SetActive(true);
	}
	
}
