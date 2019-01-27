using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : InteractableCabObject
{    
    public float SteeringForce = 100;
    private Vector3 _LastMouseWorldPosition;
    public Vector3 Vel;
    private Quaternion _LastRotation;
    public Quaternion ChangeInRotation;
    
    public float SteeringRate = 1;

    public MinMaxEventFloat SteeringAmount = new MinMaxEventFloat(-1, 1, 0);

    private Rigidbody _Rigidbody;

    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    public override void StartInteracting()
    {
        _LastMouseWorldPosition = GameInfo.Instance.MouseWorldPosition;
    }

    public override void StopInteracting()
    {

    }

    public override Vector3 InteractionUpdate()
    {
        Vel = GameInfo.Instance.MouseWorldPosition - _LastMouseWorldPosition;
        Debug.DrawLine(_LastMouseWorldPosition, GameInfo.Instance.MouseWorldPosition);
        
        _LastMouseWorldPosition = GameInfo.Instance.MouseWorldPosition;
        _Rigidbody.AddForceAtPosition(SteeringForce * Vel, _LastMouseWorldPosition);

        var currentRotation =  transform.localRotation;

        ChangeInRotation = currentRotation * Quaternion.Inverse(_LastRotation);

        var change = ChangeInRotation;
        SteeringAmount.value -= change.x * SteeringRate;

        _LastRotation = currentRotation;


        return base.InteractionUpdate();
    }

}
