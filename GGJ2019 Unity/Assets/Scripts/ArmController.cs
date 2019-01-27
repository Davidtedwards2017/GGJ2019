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
        Interacting
    }

    public Vector3 MouseWorldPos;
    private StateMachine<State> _StateCtrl;

    public GameObject IdleVisual;
    public GameObject HoveringVisual;
    public GameObject HurtVisual;
    public GameObject SteeringVisual;
    public GameObject PushButtonVisual;

    public List<SoundEffectData> IdleBanter;

    public MeshRenderer Renderer;
    Color m_OriginalColor;
    Color m_HoveringColor = Color.red;
    Color m_InteractingColor = Color.green;

    public InteractableCabObject _HoverTarget;
    public InteractableCabObject _InteractingTarget;

    private void Start()
    {
        m_OriginalColor = Renderer.material.color;

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
        Renderer.material.color = m_HoveringColor;
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
        Renderer.material.color = m_OriginalColor;
    }

    private void Interacting_Enter()
    {
        _InteractingTarget = _HoverTarget;
        Debug.Log("Arm interacting with "  + _InteractingTarget.gameObject.name);

        _InteractingTarget.StartInteracting();
        Renderer.material.color = m_InteractingColor;
    }

    private void Interacting_Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _StateCtrl.ChangeState(State.HoveringOver);
        }
    }

    private void Interacting_LateUpdate()
    {
        UpdateArmPosition(_InteractingTarget.InteractionUpdate());
    }

    private void Interacting_Exit()
    {
        _InteractingTarget.StopInteracting();
        _InteractingTarget = null;
        Renderer.material.color = m_OriginalColor;
    }

}
