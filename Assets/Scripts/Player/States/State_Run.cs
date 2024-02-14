using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Run : PlayerState
{
    public State_Run(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_NoMovement(player, new State_Stand(player)),
            new Transition_Jumping(player, new State_Jump(player)),
            new Transition_Grabbing(player, new State_Grab(player)),
            new Transition_Balance(player, new State_Trip(player, player.getupDuration), 90)
        };
        transitions = transitionsNew;
    }

    public override void Update()
    {
        base.Update();

        player.animator.transform.forward = Vector3.Slerp(player.animator.transform.forward, player.movementInput, Time.deltaTime * 7);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * (player.grounded ? 1 : 0.25f), ForceMode.Force);

        player.RecoverBalance(0.5f);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Run");
        player.SetColliderMaterial(player.groundFriction);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
