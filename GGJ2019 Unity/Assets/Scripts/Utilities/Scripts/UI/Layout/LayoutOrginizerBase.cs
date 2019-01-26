using System.Collections.Generic;
using UnityEngine;

public abstract class LayoutOrginizerBase : MonoBehaviour {

    public bool ShouldUpdate = true;

    private void Update()
    {
        if(ShouldUpdate)
        {
            UpdateChildren();
            ShouldUpdate = false;
        }
    }

	private void UpdateChildren ()
    {        
        var activeChildren = new List<LayoutChild>();
        foreach (Transform child in transform)
        {
            var layoutChild = child.GetComponent<LayoutChild>();
            if(layoutChild != null && layoutChild.Active)
            {
                activeChildren.Add(layoutChild);
            }
        }

        UpdateChildren(activeChildren);
    }


    protected abstract void UpdateChildren(List<LayoutChild> children);
}
