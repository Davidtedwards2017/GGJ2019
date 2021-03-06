﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public EffectData BlowupEffect;
    public float Damage { get { return 10.0f; } }
    public float SizeOffset;

    private Rigidbody _RB;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
        _RB.AddRelativeTorque(Vector3.forward * 1000.RandomOffset(500), ForceMode.Impulse);

        var orginialSize = transform.localScale.x;
        var newScale = orginialSize.RandomOffset(SizeOffset);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        //collision.gameObject.SendMessage("HitAsteroid", SendMessageOptions.DontRequireReceiver);
        BlowUp();
    }

    public void BlowUp()
    {
        BlowupEffect.Spawn(transform.position);
        Destroy(gameObject);
    }
}
