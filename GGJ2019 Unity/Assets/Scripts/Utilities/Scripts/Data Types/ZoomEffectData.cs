using UnityEngine;
using System.Collections;
using DG.Tweening;

[CreateAssetMenu(fileName = "Zoom Effect Data", menuName = "Effects/Zoom Effect")]
public class ZoomEffectData : ScriptableObject
{
    public float Amount;
    public float InDuration;
    public float HoldDuration;
    public float OutDuration;
    public Ease InEase;
    public Ease OutEase;

    public float minZoomAmount = 10;
}