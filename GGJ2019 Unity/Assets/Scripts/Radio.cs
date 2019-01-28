using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilites;

public class Radio : InteractableCabObject
{
    public SoundEffectData ButtonPress;
    public List<SoundEffectData> TenticalSquish;

    public List<SoundEffectData> BanterSfx;

    public GameObject CurrentTrack;
    public float RadioCooldown;
    private float _RadioCooldownDuration = 0.5f;

    public int Index = -1;

    private void Start()
    {
        Index = Random.Range(0, MasterGameStateController.Instance.TrackData.Count - 1);

        PlayNext();
    }

    public override void StartInteracting()
    {
        ArmCtrl.PushButtonVisual.SetActive(true);
        if (RadioCooldown > 0) return;

        ButtonPress.PlaySfx();
        TenticalSquish.PickRandom().PlaySfx();

        MasterGameStateController.Instance.Player.TryToBanter(BanterSfx.PickRandom());

        PlayNext();

        IdleVisual.SetActive(false);
    }

    private void Update()
    {
        if (RadioCooldown > 0)
        {
            RadioCooldown -= Time.deltaTime;
        }
    }

    public override void StopInteracting()
    {
        IdleVisual.SetActive(true);
        ArmCtrl.PushButtonVisual.SetActive(false);
    }

    public void PlayNext()
    {
        var current = MasterGameStateController.Instance.trackInstances[Index];
        if(current != null)
        {
            current.volume = 0;
        }

        Index++;
        if(Index >= MasterGameStateController.Instance.trackInstances.Length)
        {
            Index = 0;
        }

        var next = MasterGameStateController.Instance.trackInstances[Index];
        if(next != null)
        {
            next.volume = MasterGameStateController.Instance.TrackVolumes[Index];
        }
        RadioCooldown = _RadioCooldownDuration;
    }
}
