using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LayoutVerticalStacker : LayoutOrginizerBase
{
    //private int m_CachedActiveChildrenCount = 0;
    //private Transform[] m_ActiveChildren;
    public float Spacing = 1;

    protected override void UpdateChildren(List<LayoutChild> children)
    {
        float startingY = 0;
        for (int i = 0; i < children.Count; i++)
        {
            var child = children[i];
            var newPos = Vector3.zero;
            newPos.y = startingY + (Spacing * i);

            child.transform.DOLocalMove(newPos, 0.2f);
        }
    }
}
