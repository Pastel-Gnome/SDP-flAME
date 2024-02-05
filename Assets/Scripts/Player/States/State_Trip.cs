using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Trip : PlayerState
{
    private float timeElapsed;
    public State_Trip(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(timeElapsed > player.getupDuration){
            player.SetPlayerState(new State_Stand(player));
        }
        timeElapsed += Time.deltaTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        RecoverBalance(0.25f);
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
