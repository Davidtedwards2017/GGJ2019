using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public MasterGameStateController.SpawnGroupDifficulty Difficulty;
    public float MinDistanceToSpawn = 500;
    public bool CanSpawn = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var player = MasterGameStateController.Instance.Player;
        if (player == null || !CanSpawn) return;

        var distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance <= MinDistanceToSpawn)
        {
            Spawn();
        }
	}

    private void Spawn()
    {
        var prefab = MasterGameStateController.Instance.GetNextAsteroidGroup(Difficulty);
        if (prefab != null)
        {
            Instantiate(prefab, transform);
        }

        CanSpawn = false;
    }

    public void Despawn()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }
}
