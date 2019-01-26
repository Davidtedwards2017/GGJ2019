using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class TrunkController : MonoBehaviour
{
    public enum States
    {
        Init,
        WaitingToStart,
        Coasting,
        Steering,
        RechargingEnergy,
        WippingWindShield,
        Crashing,
        Winning,
    }

   
    public float MinEnergy = 0;
    public float MaxEnergy = 100;
    public float EnergyDecayRate = 2.5f;

    public float NormalForce;
    public float LowEnergySpeed;

    public Vector3 _SteeringDirection;
    public float SteeringSpeed = 0.5f;

    private StateMachine<States> _StateCtrl;

    private Rigidbody _RigidBody;

    public Vector3 Steer_FullLeft;
    public Vector3 Steer_FullRight;
    public float _Speed;

    public Vector3 currentVelocity;

    private float _CurrentSteering = 0.5f;

    public void Start()
    {
        _StateCtrl = StateMachine<States>.Initialize(this);
        _StateCtrl.ChangeState(States.Init);
    }

    void FixedUpdate()
    {

    }

    private void UpdateSteering()
    {
        float steering = Input.GetAxis("Horizontal");
        if (steering > 0)
        {
            _CurrentSteering = Mathf.Lerp(_CurrentSteering, 1, Time.deltaTime);
        }
        else if (steering < 0)
        {
            _CurrentSteering = Mathf.Lerp(_CurrentSteering, 0, Time.deltaTime);
        }

        _SteeringDirection = Vector3.Lerp(Steer_FullLeft, Steer_FullRight, _CurrentSteering) * SteeringSpeed * Time.deltaTime;
    }

    private void UpdateMovement()
    {
        currentVelocity = (new Vector3 (1,0,0) + _SteeringDirection).normalized * _Speed * Time.deltaTime;

        _RigidBody.AddForce(currentVelocity);
        transform.rotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(currentVelocity);
    }

    public void ReachDestination()
    {
        _StateCtrl.ChangeState(States.Winning);
    }

    
    private void Init_Enter()
    {
        if(MasterGameStateController.Instance.Player != this)
        {
            Debug.LogWarning("Player instance already exist.");
            Destroy(gameObject);
            return;
        }

        _RigidBody = GetComponent<Rigidbody>();
    }

    public void StartEngines()
    {
        _StateCtrl.ChangeState(States.Coasting);
    }
        
    private void Coasting_Enter()
    {

    }

    private void Coasting_FixedUpdate()
    {
        UpdateSteering();
        UpdateMovement();
    }

    private void WaitingToStart_Enter()
    {

    }

    private void Steering_Enter()
    {

    }

    private void Steering_FixedUpdate()
    {
        UpdateSteering();
        UpdateMovement();
    }

    private void RechargingEnergy_Enter()
    {

    }

    private void RechargingEnergy_FixedUpdate()
    {
        UpdateMovement();
    }

    private void WippingWindShield_Enter()
    {

    }

    private void WippingWindShield_FixedUpdate()
    {
        UpdateMovement();
    }

    private void Crashing_Enter()
    {
        Debug.Log("Player Crashed");
        MasterGameStateController.Instance.PlayerLost();
    }

    private void Winning_Enter()
    {
        Debug.Log("Player Winning");
        MasterGameStateController.Instance.PlayerWon();
    }




}
