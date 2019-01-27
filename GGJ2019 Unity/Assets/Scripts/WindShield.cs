using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

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

    public GameObject SplatVisual;
    public GameObject OneWipeVisual;
    public GameObject TwoWipeVisual;

    public float BugIncommingTime = 1.0f;

    public float MinTimeTillNextBug = 3.0f;
    public float MaxTimeTillNextBug = 10.0f;


    private StateMachine<State> _StateCtrl;


	// Use this for initialization
	void Start () {
        _StateCtrl = StateMachine<State>.Initialize(this);
        _StateCtrl.ChangeState(State.Clean);
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
    
    private IEnumerator BugIncomming_Enter()
    {
        Debug.Log("Bug incomming");
        yield return new WaitForSeconds(BugIncommingTime);
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
