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

        player.rb.AddForce(player.movementInput.normalized * (player.runSpeed * 0.5f) * (player.grounded.collider ? 1 : 0.25f), ForceMode.Force);
    }

    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Arms-Grab", 1);
        player.grabbing = false;
        player.StartCoroutine(player.Grab(player.grabTime * 0.5f));
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
