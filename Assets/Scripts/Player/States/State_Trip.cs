using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Trip : PlayerState
{
    public State_Trip(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Trip");
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
