using UnityEngine;
using System.Collections;

 public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    
    public void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}