using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stumble : PlayerState
{
    private float duration;
    private Vector3 stumbleInput;
    private PlayerState previousState;
    private float timeSinceLastStep;

    public State_Stumble(PlayerBehaviour playerNew, PlayerState previousStateNew) : base(playerNew){
        previousState = previousStateNew;
    }

    public override void Chosen(){
        if(previousState is State_Jump state_Jump){
            duration = state_Jump.jumpTimeElapsed * 2;
            stumbleInput = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
        }

        PlayerTransition[] transitionsNew = {
            new Transition_Jumping(player, new State_Jump(player)),
            new Transition_Timer(player, new State_Stand(player), duration)
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

        player.rb.AddForce(stumbleInput * 2);
        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        player.balance += new Vector2(stumbleInput.x, stumbleInput.z).normalized * 0.1f;
        player.balance += new Vector2(player.movementInput.x, player.movementInput.z).normalized;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.SetTrigger("Go_Stumble");
        player.SetColliderMaterial(player.airFriction);
        
        //Debug.Log(stumbleInput);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
