using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(tk2dBaseSprite))]
public class RandomSpriteFlip : MonoBehaviour {

    public bool CanHorizontalFlip;
    private tk2dBaseSprite _Sprite;

	void Start ()
    {
        _Sprite = GetComponent<tk2dBaseSprite>();

        if (CanHorizontalFlip)
        {
            _Sprite.FlipX = Random.Range(0, 2) == 0 ? true : false;
        }
	}
	
}
