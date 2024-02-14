using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stumble : PlayerState
{
    private float duration = 0.5f;
    private Vector3 stumbleInput;

    public State_Stumble(PlayerBehaviour playerNew, float durationNew, Vector3 stumbleInputNew) : base(playerNew){
        duration = durationNew;
        stumbleInput = stumbleInputNew;
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Jumping(player, new State_Jump(player)),
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

        player.rb.AddForce(stumbleInput);
        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        player.balance += new Vector2(stumbleInput.x, stumbleInput.z).normalized * 0.1f;
        player.balance += new Vector2(player.movementInput.x, player.movementInput.z).normalized;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Stumble");
        player.SetColliderMaterial(player.airFriction);
        
        Debug.Log(stumbleInput);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
