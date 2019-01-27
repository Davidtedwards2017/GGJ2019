using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilites;
using DG.Tweening;

public class BattleDamageCtrl : MonoBehaviour {

    public TrunkController Trunk;

    public List<GameObject> DamageElementEffects;

    private List<GameObject> _BattleDamageElementPool;
    private List<GameObject> _EnabledBattleDamage = new List<GameObject>();

    public GameObject WarningLight;

    public GameObject Head;
    public float HeadPunchScale = 0.05f;

    public SoundEffectData WarningSfx;
    private GameObject warningSfxInstance;

    private float DisplayWarningLightTime;
    public float DelayBeforeTurningOffWarningLight = 0.3f;

    private void Start()
    {
        _BattleDamageElementPool = new List<GameObject>(DamageElementEffects);
        foreach(var element in _BattleDamageElementPool)
        {
            element.SetActive(false);
        }

        Trunk.Health.OnValueChangeTo += OnHealthValueChangeTo;
        Trunk.Health.OnValueChangeBy += OnHealthValueChangeBy;
    }

    private void OnHealthValueChangeTo(float value)
    {
        DisplayWarningLightTime = DelayBeforeTurningOffWarningLight;

        var count = GetShouldEnableCount(value);
        Debug.Log("should have " + count + " damage elements enabled");

        var elementsToEnable = count - _EnabledBattleDamage.Count;
        while(elementsToEnable -- > 0)
        {
            var instance = _BattleDamageElementPool.PickRandom();
            instance.SetActive(true);
            _BattleDamageElementPool.SafeRemove(instance);
            _EnabledBattleDamage.AddIfUnique(instance);
        }
    }

    private void OnHealthValueChangeBy(float value)
    {
        PerformCameraShake(value);
    }

    private void Update()
    {        
        if(DisplayWarningLightTime > 0 || Trunk.Health.percentage < 0.2)
        {
            WarningLight.SetActive(true);
            if(warningSfxInstance == null)
            {
                warningSfxInstance = WarningSfx.PlaySfx(true).gameObject;
                warningSfxInstance.transform.SetParent(transform);
            }
        }
        else
        {
            WarningLight.SetActive(false);
            if (warningSfxInstance != null)
            {
                Destroy(warningSfxInstance.gameObject);
            }
        }

        DisplayWarningLightTime -= Time.deltaTime;
    }

    private int GetShouldEnableCount(float health)
    {
        return (int) Mathf.Lerp(0, DamageElementEffects.Count, 1 - (health / 100));
    }

    public void PerformCameraShake(float value)
    {
        Head.transform.DOPunchRotation(Vector3.zero.RandomOffset(Vector3.one * value * HeadPunchScale ), 0.5f);
    }





}
