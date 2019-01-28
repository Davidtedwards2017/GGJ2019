using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utilites;

public class WindShieldWiper : InteractableCabObject
{
    public float WipeDuration = 1f;
    public Animator AnimationCtrl;
    public WindShield WindShield;

    public List<SoundEffectData> WipeSfx;
    public SoundEffectData ButtonPress;
    public List<SoundEffectData> TenticalSquish;

    public List<SoundEffectData> BanterSfx;
    
    private float WiperCooldown;

    public override void StartInteracting()
    {
        ArmCtrl.PushButtonVisual.SetActive(true);
        if (WiperCooldown > 0) return;

        ButtonPress.PlaySfx();
        TenticalSquish.PickRandom().PlaySfx();

        MasterGameStateController.Instance.Player.TryToBanter(BanterSfx.PickRandom());

        WiperCooldown = WipeDuration;

        IdleVisual.SetActive(false);

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
        IdleVisual.SetActive(true);

        ArmCtrl.PushButtonVisual.SetActive(false);
    }

    private IEnumerator Sequence()
    {
        AnimationCtrl.SetTrigger("Wipe");
        WipeSfx.PickRandom().PlaySfx();
        yield return new WaitForSeconds(WipeDuration / 2);
        WindShield.WipeWindow();
    }

}