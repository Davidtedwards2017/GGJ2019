using UnityEngine;
using System.Collections;
using DG.Tweening;

[CreateAssetMenu(fileName = "Slow Down Data", menuName = "Effects/Slow Down")]
public class SlowDownEffectData : ScriptableObject {

    public float InDuration;
    public float holdDuration;
    public float OutDuration;

    public Ease EaseIn;
    public Ease EaseOut;

    public float Amount;
}