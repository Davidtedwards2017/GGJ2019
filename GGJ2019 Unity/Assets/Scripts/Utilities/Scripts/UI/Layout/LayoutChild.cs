using UnityEngine;

public class LayoutChild : MonoBehaviour {

    private bool m_CachedActive = false;
    public bool Active = true;

    private LayoutOrginizerBase m_Parent;
    public LayoutOrginizerBase Parent
    {
        get
        {
            if (m_Parent == null)
            {
                m_Parent = transform.parent.GetComponent<LayoutOrginizerBase>();
            }

            return m_Parent;
        } 
    }

    private void Start()
    {
        if (Parent != null)
        {
            Parent.ShouldUpdate = true;
        }
    }

    private void Update()
    {
        if(Active != m_CachedActive && Parent != null)
        {
            m_CachedActive = Active;
            Parent.ShouldUpdate = true;
        }
    }
}