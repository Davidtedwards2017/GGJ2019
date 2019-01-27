using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;

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

    public List<SoundEffectData> RockHitSfx;

    public float DamagePerHit = 1.0f;
    public MinMaxEventFloat Health = new MinMaxEventFloat(0, 100, 100);
       
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

    public SteeringWheel Wheel;

    private float _CurrentSteering = 0.5f;

    private TrackArea _TrackArea;

    public float OutOfBoundsDamagePerSecond = 5.0f;

    public List<SoundEffectData> TakeDamageVoices;

    public Transform BanterTransform;



    public void Start()
    {
        _StateCtrl = StateMachine<States>.Initialize(this);
        _StateCtrl.ChangeState(States.Init);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (_SteeringDirection * 100) , Color.red);

        //if (_TrackArea == null)
        if(_TrackArea == null)
        {
            Health.value -= OutOfBoundsDamagePerSecond * Time.deltaTime;
        }
    }

    public void OnEnterTrackArea(TrackArea track)
    {
        if(_TrackArea != track)
        {
            _TrackArea = track;
        }
        Debug.Log("Trunk Enter track area " + track.gameObject.name);
    }



    public void TryToBanter(SoundEffectData sfx, bool force = false)
    {
        if(force)
        {
            if(BanterTransform.childCount <= 0)
            {
                foreach(Transform child in BanterTransform)
                {
                    Destroy(child.gameObject);
                }
            }

            sfx.PlaySfx().transform.SetParent(BanterTransform);
        }
        else if(BanterTransform.childCount == 0)
        {
            sfx.PlaySfx().transform.SetParent(BanterTransform);
        }
    }

    public void OnLeaveTrackArea(TrackArea track)
    {
        if(_TrackArea == track)
        {
            _TrackArea = null;
        }
        Debug.Log("Trunk left track area " + track.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var asteroid = collision.gameObject.GetComponent<Asteroid>();
        if(asteroid != null)
        {
            RockHitSfx.PickRandom().PlaySfx();
            TryToBanter(TakeDamageVoices.PickRandom(), true);
            HitSomthing(asteroid.Damage);
        }
    
        //collision.gameObject.SendMessage("HitAsteroid", SendMessageOptions.DontRequireReceiver);
    }

    public void HitSomthing(float dmgAmount)
    {
        Debug.Log("Took damage " + dmgAmount);
        Health.value -= dmgAmount;
    }

    private void UpdateSteering()
    {
        _CurrentSteering = Wheel.SteeringAmount.value;
        _SteeringDirection = Quaternion.AngleAxis(_CurrentSteering * SteeringSpeed, Vector3.up) * transform.right;
        transform.rotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(_SteeringDirection);    
    }

    private void UpdateMovement()
    {
        currentVelocity = (transform.right).normalized * _Speed * Time.deltaTime;
        _RigidBody.AddForce(currentVelocity);
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

        Health.OnValueChangeTo += OnHealthValueChangeTo;
        Health.OnValueMin += OnHealthDepleted;
        Wheel.SteeringAmount.OnValueChanged += UpdateSteering;

        _RigidBody = GetComponent<Rigidbody>();
    }

    private void OnHealthValueChangeTo(float value)
    {

    }

    private void OnHealthDepleted()
    {
        _StateCtrl.ChangeState(States.Crashing);
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
        //UpdateSteering();
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
