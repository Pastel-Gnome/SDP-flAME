using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Run : PlayerState
{
    private float timeSinceLastStep;

    public State_Run(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_NoMovement(player, new State_Stand(player)),
            new Transition_Jumping(player, new State_Jump(player)),
            new Transition_Grabbing(player, new State_Grab(player), new State_Place(player)),
            new Transition_UnGrounded(player, new State_Jump(player))
            //new Transition_Balance(player, new State_Trip(player, player.getupDuration), 90)
        };
        transitions = transitionsNew;
        timeSinceLastStep = player.stepTimer;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        timeSinceLastStep = player.Step(timeSinceLastStep);

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * (player.grounded.collider ? 1 : 0.25f) * (player.heldObject ? 0.8f: 1f), ForceMode.Force);
        
        player.RecoverBalance(0.5f);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        player.animator.SetTrigger("Go_Run");
        player.SetColliderMaterial(player.groundFriction);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
