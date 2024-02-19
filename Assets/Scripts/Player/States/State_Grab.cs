using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grab : PlayerState
{
    public State_Grab(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Timer(player, new State_Stand(player), player.grabTime)
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
    }

    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.grabbing = false;

        Transform droppedObject = player.Drop(true, Vector3.zero);

        player.Grab(droppedObject);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
