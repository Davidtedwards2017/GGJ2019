using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableCabObject : MonoBehaviour {

    public GameObject IdleVisual;

    public Transform Anchor;

    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = Color.red;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    public ArmController ArmCtrl;

    public virtual void StartInteracting()
    {

    }

    public virtual void StopInteracting()
    {

    }

    public virtual Vector3 InteractionUpdate()
    {
        return Anchor.position;
    }


}
