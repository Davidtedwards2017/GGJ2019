using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

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

    private StateMachine<States> _StateCtrl;

    public GameObject PlayerPrefab;
    public GameObject PlayerSpawnPoint;

    public TrunkController Player;

    public void Start()
    {
        _StateCtrl = StateMachine<States>.Initialize(this);
        _StateCtrl.ChangeState(States.Cleanup);
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
