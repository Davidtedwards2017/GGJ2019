using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scrolling Text Data", menuName = "Scrolling Text")]
public class ScrollingTextData : ScriptableObject
{
    public Vector3 TargetRelativeOffset;
    public ScrollingCombatText Prefab;
    public Color Color;

    public void Spawn(Vector3 position, string text, float duration)
    {
        ScrollingCombatText instance = Instantiate(Prefab, position, Quaternion.identity);
        instance.Initialize(text, duration, Color, TargetRelativeOffset);
    }
}