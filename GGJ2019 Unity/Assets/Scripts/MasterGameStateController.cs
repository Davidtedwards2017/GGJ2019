using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;
using DG.Tweening;

public class MasterGameStateController : Singleton<MasterGameStateController> {

    public enum States
    {
        Init,
        WaitingToStart,
        Starting,
        Playing,
        Losing,
        Winning,
        Cleanup,
    }

    public enum SpawnGroupDifficulty
    {
        Easy,
        Medium,
        Hard,
        None,
    }


    public CanvasGroup TitleGroup;
    public SoundEffectData WinningSfx;
    public SoundEffectData LosingSfx;

    public List<GameObject> EasyGroups;
    public FilteredRandom<GameObject> EasyGroupRnd;

    public List<GameObject> MediumGroups;
    public FilteredRandom<GameObject> MediumGroupRnd;
    
    public List<GameObject> HardGroups;
    public FilteredRandom<GameObject> HardGroupRnd;


    private StateMachine<States> _StateCtrl;

    public GameObject PlayerPrefab;
    public GameObject PlayerSpawnPoint;

    public TrunkController Player;

    public List<SoundEffectData> TrackData;

    public AudioSource[] trackInstances;
    public float[] TrackVolumes;

    public GameObject Track;
    public GameObject TrackPrefab;

    public bool DisableNewAudio = false;
    public void FadeOutAllExistingAudio(float duration)
    {
        foreach (var audio in FindObjectsOfType<AudioSource>())
        {
            audio.DOFade(0, duration);
        }
    }

    public void Awake()
    {
        EasyGroupRnd = new FilteredRandom<GameObject>(EasyGroups, 3);
        MediumGroupRnd = new FilteredRandom<GameObject>(MediumGroups, 3);
        HardGroupRnd = new FilteredRandom<GameObject>(HardGroups, 3);

        //Physics.IgnoreLayerCollision(0, 9);
        //Physics.IgnoreLayerCollision(1, 9);
        //
        //Physics.IgnoreLayerCollision(2, 9);
        //Physics.IgnoreLayerCollision(3, 9);
        //Physics.IgnoreLayerCollision(4, 9);
        //Physics.IgnoreLayerCollision(5, 9);
        //Physics.IgnoreLayerCollision(6, 9);
        //Physics.IgnoreLayerCollision(7, 9);
        //Physics.IgnoreLayerCollision(8, 9);

        _StateCtrl = StateMachine<States>.Initialize(this);
        _StateCtrl.ChangeState(States.Cleanup);

        InitRadioTracks();
    }

    private void InitRadioTracks()
    {
        trackInstances = new AudioSource[TrackData.Count + 1];
        TrackVolumes = new float[TrackData.Count];

        var i = 0;
        foreach (var track in TrackData)
        {
            var instance = track.PlaySfx(true);
            instance.transform.SetParent(transform);
            instance.volume = 0;

            trackInstances[i] = instance;
            TrackVolumes[i] = track.Volume;

            i++;
        }

    }

    public GameObject GetNextAsteroidGroup(SpawnGroupDifficulty difficulty)
    {
        switch (difficulty)
        {
            case SpawnGroupDifficulty.Hard:
                return EasyGroupRnd.GetNextRandom();
            case SpawnGroupDifficulty.Medium:
                return MediumGroupRnd.GetNextRandom();
            case SpawnGroupDifficulty.Easy:
                return HardGroupRnd.GetNextRandom();
        }

        return null;

    }

    private void SpawnPlayer()
    {
        Debug.Log("Spawning player");
        var go = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, Quaternion.identity);
        Player = go.GetComponent<TrunkController>();
    }

    public void Init_Enter()
    {
        SpawnPlayer();
        DisableNewAudio = false;
        _StateCtrl.ChangeState(States.WaitingToStart);
    }

    public void WaitingToStart_Enter()
    {
        TitleGroup.DOFade(1, 0.5f);
    }

    public void WaitingToStart_Update()
    {
        if(Input.anyKeyDown)
        {
            _StateCtrl.ChangeState(States.Playing);
        }
    }

    public void WaitingToStart_Exit()
    {
        TitleGroup.DOFade(0, 0.5f);
    }

    public void Playing_Enter()
    {
        Track = Instantiate(TrackPrefab);

        Player.StartEngines();
    }

    public void PlayerLost()
    {
        _StateCtrl.ChangeState(States.Losing);
    }

    public void Losing_Enter()
    {
        _StateCtrl.ChangeState(States.Cleanup);
    }

    public void PlayerWon()
    {
        _StateCtrl.ChangeState(States.Winning);
    }

    public void Winning_Enter()
    {
        _StateCtrl.ChangeState(States.Cleanup);
    }

    public void Cleanup_Enter()
    {
        Debug.Log("Despawning player");
        Destroy(Player.gameObject);

        if(Track != null)
        {
            Destroy(Track);
        }

        _StateCtrl.ChangeState(States.Init);
    }
}
