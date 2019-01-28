using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;

public class ArmController : MonoBehaviour {
    private enum State
    {
        Idle,
        HoveringOver,
        Interacting,
        HitSomething,
    }

    public Vector3 MouseWorldPos;
    private StateMachine<State> _StateCtrl;

    public GameObject IdleVisual;
    public GameObject HoveringVisual;
    public GameObject HurtVisual;
    public GameObject SteeringVisual;
    public GameObject PushButtonVisual;

    public List<SoundEffectData> IdleBanter;

    public InteractableCabObject _HoverTarget;
    public InteractableCabObject _InteractingTarget;

    private void Start()
    {
        _StateCtrl = StateMachine<State>.Initialize(this);
        _StateCtrl.ChangeState(State.Idle);
    }

    private void Update()
    {
        RaycastHit[] allHit = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach (RaycastHit hit in allHit)
        {
            _HoverTarget = hit.collider.gameObject.GetComponent<InteractableCabObject>();
            if (_HoverTarget != null)
            {
                //do something here
                break;
            }
        }
    }
    public void HitRock()
    {
        _StateCtrl.ChangeState(State.HitSomething);
    }

    private void UpdateArmPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Idle_Enter()
    {
        IdleVisual.SetActive(true);
        Debug.Log("Arm Idle");
    }

    private float MinIdlePanterInvokeTime = 7.0f;
    private float TimeTillIdleBanter = -1;

    private void Idle_Update()
    {
        if(TimeTillIdleBanter <= 0)
        {
            MasterGameStateController.Instance.Player.TryToBanter(IdleBanter.PickRandom());
            TimeTillIdleBanter = MinIdlePanterInvokeTime.RandomOffset(MinIdlePanterInvokeTime / 2);
        }
        else
        {
            TimeTillIdleBanter -= Time.deltaTime;
        }


        if (_HoverTarget != null)
        {
            _StateCtrl.ChangeState(State.HoveringOver);
        }
    }
    
    private void UpdateArmToMousePosition()
    {
        UpdateArmPosition(GameInfo.Instance.MouseWorldPosition);
    }

    private void Idle_LateUpdate()
    {
        UpdateArmToMousePosition();
    }

    private void Idle_Exit()
    {
        IdleVisual.SetActive(false);
    }
    
    private void HoveringOver_Enter()
    {
        Debug.Log("Arm hovering");
        HoveringVisual.SetActive(true);
    }

    private void HoveringOver_Update()
    {
        if (_HoverTarget == null)
        {
            _StateCtrl.ChangeState(State.Idle);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            _StateCtrl.ChangeState(State.Interacting);
        }
    }

    private void HoveringOver_LateUpdate()
    {
        UpdateArmToMousePosition();
    }

    private void HoveringOver_Exit()
    {
        HoveringVisual.SetActive(false);
    }

    private void Interacting_Enter()
    {
        _InteractingTarget = _HoverTarget;

        if(_InteractingTarget == null)
        {
            _StateCtrl.ChangeState(State.HoveringOver);
        }
        else
        {
            _InteractingTarget.StartInteracting();
        }
    }

    private void Interacting_Update()
    {
        if (_InteractingTarget == null)
        {
            _StateCtrl.ChangeState(State.HoveringOver);
        }

        if (_InteractingTarget == null || Input.GetMouseButtonUp(0))
        {
            _StateCtrl.ChangeState(State.HoveringOver);
        }
    }

    private void Interacting_LateUpdate()
    {
        if(_InteractingTarget != null)
        {
            UpdateArmPosition(_InteractingTarget.InteractionUpdate());
        }
    }    

    private void Interacting_Exit()
    {
        if(_InteractingTarget != null)
        {
            _InteractingTarget.StopInteracting();
        }
    }

    public float HitSomethingSpazDuration = 0.5f;

    private IEnumerator HitSomething_Enter()
    {
        HurtVisual.SetActive(true);
        yield return new WaitForSeconds(HitSomethingSpazDuration);
        HurtVisual.SetActive(false);
        _StateCtrl.ChangeState(_StateCtrl.LastState);
    }

    private void HitSomething_LateUpdate()
    {
        UpdateArmToMousePosition();
    }

}
