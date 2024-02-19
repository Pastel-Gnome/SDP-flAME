using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Trip : PlayerState
{ 
    private float duration;
    public State_Trip(PlayerBehaviour playerNew, float durationNew) : base(playerNew){
        duration = durationNew;
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Timer(player, new State_Stand(player), duration)
        };
        transitions = transitionsNew;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.RecoverBalance(0.25f);
    }

    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Trip");

        player.Drop(false, player.rb.velocity);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
