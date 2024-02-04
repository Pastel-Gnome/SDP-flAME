using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Base : PlayerState
{

    public State_Base(PlayerBehaviour playerNew){
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
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
