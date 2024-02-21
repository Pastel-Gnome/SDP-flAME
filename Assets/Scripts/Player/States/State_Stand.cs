using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stand : PlayerState
{
    public State_Stand(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Movement(player, new State_Run(player)),
            new Transition_Jumping(player, new State_Jump(player)),
            new Transition_Grabbing(player, new State_Grab(player), new State_Place(player)),
            new Transition_Balance(player, new State_Trip(player, player.getupDuration), 90)
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

        player.RecoverBalance();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        player.animator.Play("Stand");
        player.SetColliderMaterial(player.groundFriction);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
