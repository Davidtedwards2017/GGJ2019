﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;
using DG.Tweening;
using System.Linq;


public class WindShield : MonoBehaviour {

    private enum State
    {
        Disabled,
        Clean,
        BugIncomming,
        BugSplat,
        OneWipe,
        TwoWipe,
    }

    public SoundEffectData Buzzing;
    public List<SoundEffectData> BugSplats;

    public GameObject SplatVisual;
    public GameObject OneWipeVisual;
    public GameObject TwoWipeVisual;

    public float BugIncommingTime = 1.0f;


    public float MinTimeTillNextBug = 3.0f;
    public float MaxTimeTillNextBug = 10.0f;


    public GameObject Bug;
    public Transform BugSpawnPoint;
    public Transform BugSplatPoint;

    private StateMachine<State> _StateCtrl;


	// Use this for initialization
	void Start () {
        _StateCtrl = StateMachine<State>.Initialize(this);
        SetBugsActive(false);

    }

    public void SetBugsActive(bool active)
    {
        if(active)
        {
            _StateCtrl.ChangeState(State.Clean);
        }
        else
        {
            _StateCtrl.ChangeState(State.Disabled);
        }
    }

    private IEnumerator Clean_Enter()
    {
        SplatVisual.SetActive(false);
        OneWipeVisual.SetActive(false);
        TwoWipeVisual.SetActive(false);

        float timeTillNextBug = Random.Range(MinTimeTillNextBug, MaxTimeTillNextBug);
        yield return new WaitForSeconds(timeTillNextBug);
               

        _StateCtrl.ChangeState(State.BugIncomming);
    }

    public Ease BugFlightEase;

    private IEnumerator BugIncomming_Enter()
    {
        Debug.Log("Bug incomming");
        var audio = Buzzing.PlaySfx();
        if(audio != null)
        {
            audio.volume = 0;
            audio.DOFade(1, BugIncommingTime);
        }

        Bug.SetLocalPosition(BugSpawnPoint.localPosition);
        Bug.SetActive(true);

        yield return Bug.transform.DOLocalMove(BugSplatPoint.localPosition, BugIncommingTime)
            .SetEase(BugFlightEase)
            //BugTravelPath.Select(s => s.position).ToArray(), BugIncommingTime)
            //.SetLookAt(transform)
            .WaitForCompletion();

        Bug.SetActive(false);

        //yield return new WaitForSeconds(BugIncommingTime);
        Destroy(audio.gameObject);
        _StateCtrl.ChangeState(State.BugSplat);
    }

    public void WipeWindow()
    {
        if(_StateCtrl.State == State.BugSplat)
        {
            _StateCtrl.ChangeState(State.OneWipe);
        }
        else if (_StateCtrl.State == State.OneWipe)
        {
            _StateCtrl.ChangeState(State.TwoWipe);
        }
        else if(_StateCtrl.State == State.TwoWipe)
        {
            _StateCtrl.ChangeState(State.Clean);
        }

    }

    private void BugSplat_Enter()
    {
        SplatVisual.SetActive(true);
        BugSplats.PickRandom().PlaySfx();
    }

    private void BugSplat_Exit()
    {
        SplatVisual.SetActive(false);
    }

    private void OneWipe_Enter()
    {
        OneWipeVisual.SetActive(true);
    }

    private void OneWipe_Exit()
    {
        OneWipeVisual.SetActive(false);
    }
    
    private void TwoWipe_Enter()
    {
        TwoWipeVisual.SetActive(true);
    }

    private void TwoWipe_Exit()
    {
        TwoWipeVisual.SetActive(false);
    }



    // Update is called once per frame
    void Update () {
		
	}
}
