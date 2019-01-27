﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Utilites;

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

    public void Start()
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
        _StateCtrl.ChangeState(States.WaitingToStart);
    }

    public void WaitingToStart_Update()
    {
        if(Input.anyKeyDown)
        {
            _StateCtrl.ChangeState(States.Playing);
        }
    }

    public void Playing_Enter()
    {
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

        _StateCtrl.ChangeState(States.Init);
    }
}
