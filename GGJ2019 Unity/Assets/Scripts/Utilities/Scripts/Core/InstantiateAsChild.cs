using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAsChild : MonoBehaviour {

    public List<GameObject> Prefabs;
    public void Awake()
    {
        foreach(var prefab in Prefabs)
        {
            if (prefab == null) continue;

            var spawned = Instantiate(prefab, transform, false);
            spawned.name = prefab.name;
        }
    }
}
