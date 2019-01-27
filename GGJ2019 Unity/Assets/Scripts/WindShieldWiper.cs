using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindShieldWiper : InteractableCabObject
{
    public float WipeDuration = 1f;
    public Animator AnimationCtrl;
    public WindShield WindShield;

    private float WiperCooldown;

    public override void StartInteracting()
    {
        if (WiperCooldown > 0) return;
        WiperCooldown = WipeDuration;

        StartCoroutine(Sequence());

    }

    private void Update()
    {
        if(WiperCooldown > 0)
        {
            WiperCooldown -= Time.deltaTime;
        }
    }

    public override void StopInteracting()
    {

    }
    private IEnumerator Sequence()
    {
        AnimationCtrl.SetTrigger("Wipe");
        yield return new WaitForSeconds(WipeDuration / 2);
        WindShield.WipeWindow();
    }

}