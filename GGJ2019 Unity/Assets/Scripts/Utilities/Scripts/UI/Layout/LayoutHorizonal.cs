using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class LayoutHorizonal : LayoutOrginizerBase
{
    public float Spacing = 1.0f;

    protected override void UpdateChildren(List<LayoutChild> children)
    {
        //float scale = Mathf.Pow(ScalingPerItem, children.Count - 1);
        float startingX = -(((children.Count * Spacing) / 2) - Spacing / 2);
        for (int i = 0; i < children.Count; i++)
        {
            var child = children[i];
            var newPos = Vector3.zero;
            newPos.x = startingX + (Spacing * i);

            child.transform.DOLocalMove(newPos, 0.2f);
            //t.DOScale(new Vector3(scale, scale, scale), 0.2f);
        }
    }
}
