using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;
using DG.Tweening;
using System.Linq;

public class TrunkController : MonoBehaviour
{
    public enum States
    {
        Init,
        WaitingToStart,
        Coasting,
        //Steering,
        //RechargingEnergy,
        //WippingWindShield,
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
    public float MaxTurningAngle = 20.0f;

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

    public ArmController Arm;

    public SoundEffectData Ambiant;
    public WindShield WindShield;



    public void Start()
    {
        Ambiant.PlaySfx(true).transform.SetParent(transform);
        _StateCtrl = StateMachine<States>.Initialize(this);
        _StateCtrl.ChangeState(States.Init);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (_SteeringDirection * 100) , Color.red);
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
        Arm.HitRock();
    }

    private void UpdateSteering()
    {
        _CurrentSteering = Wheel.SteeringAmount.value;
        var steeringAngle = Mathf.Lerp(-MaxTurningAngle, MaxTurningAngle, (_CurrentSteering / 2) + 0.5f) * Time.deltaTime;
        _SteeringDirection = Quaternion.AngleAxis(steeringAngle, Vector3.up) * transform.right;
        //_SteeringDirection = Quaternion.AngleAxis(_CurrentSteering * SteeringSpeed, Vector3.up) * transform.right;
        transform.rotation = Quaternion.Euler(0, -90, 0) * Quaternion.LookRotation(_SteeringDirection);
    }

    private void UpdateMovement()
    {
        UpdateSteering();
        currentVelocity = (transform.right).normalized * _Speed * Time.deltaTime;
        _RigidBody.AddForce(currentVelocity);
    }

    private FinishTrigger _FinishingTrigger;
    public void ReachDestination(FinishTrigger trigger)
    {
        _FinishingTrigger = trigger;
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
        //Wheel.SteeringAmount.OnValueChanged += UpdateSteering;

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
        WindShield.SetBugsActive(true);
    }

    private void Coasting_Update()
    {
        if (_TrackArea == null)
        {
            Health.value -= OutOfBoundsDamagePerSecond * Time.deltaTime;
        }
    }

    private void Coasting_FixedUpdate()
    {
        //UpdateSteering();
        UpdateMovement();
    }

    private void Coasting_Exit()
    {
        WindShield.SetBugsActive(false);
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

    private float CrashingDuration = 5.0f;
    private IEnumerator Crashing_Enter()
    {
        MasterGameStateController.Instance.FadeOutAllExistingAudio(3.0f);

        Debug.Log("Player Crashed");

        var lostingSfx = MasterGameStateController.Instance.LosingSfx.PlaySfx();
        lostingSfx.transform.SetParent(transform);
        lostingSfx.volume = 0;
        lostingSfx.DOFade(1, MinMusicFadeInDuration);

        MasterGameStateController.Instance.DisableNewAudio = true;

        ScreenEffect.StartFadeEffect(Color.black, CrashingDuration);

        yield return new WaitForSeconds(CrashingDuration);


        yield return new WaitForSeconds(3.0f);
        MasterGameStateController.Instance.PlayerLost();

    }

    float MinMusicFadeInDuration = 3.0f;

    private IEnumerator Winning_Enter()
    {
        MasterGameStateController.Instance.FadeOutAllExistingAudio(3.0f);

        Debug.Log("Player Winning");
        var winningSfx = MasterGameStateController.Instance.WinningSfx.PlaySfx();
        winningSfx.transform.SetParent(transform);
        winningSfx.volume = 0;
        winningSfx.DOFade(1, MinMusicFadeInDuration);
        MasterGameStateController.Instance.DisableNewAudio = true;
        yield return FlyInSequence();


        yield return new WaitForSeconds(3.0f);
        MasterGameStateController.Instance.PlayerWon();
    }

    public ScreenFlash ScreenEffect;
    private float DurationBeforeStartWinScreenFade = 15.0f;
    public IEnumerator FlyInSequence()
    {
        Vector3[] path = _FinishingTrigger.FlyPath.Select(s => s.position).ToArray();

        yield return  transform.DOMove(path[0], _FinishingTrigger.StartPathDuration).WaitForCompletion();

        transform.DOPath(path, _FinishingTrigger.FlyDownDuration)
            .SetEase(_FinishingTrigger.EndSequenceEase)
            .SetLookAt(1f, new Vector3(-1,0,0))
            .WaitForCompletion();

        yield return new WaitForSeconds(DurationBeforeStartWinScreenFade);
        var fadeDuration = _FinishingTrigger.FlyDownDuration - DurationBeforeStartWinScreenFade;

        ScreenEffect.StartFadeEffect(Color.white, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

    }
}
