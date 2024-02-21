using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Place : PlayerState
{
    public State_Place(PlayerBehaviour playerNew) : base(playerNew){
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

        player.animator.Play("Arms-Grab", 1);
        player.StartCoroutine(player.Drop(true, Vector3.zero, player.grabTime * 0.5f));
        
        player.grabbing = false;
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
