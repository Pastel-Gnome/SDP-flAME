using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Jump : PlayerState
{
    private float jumpTimeElapsed;
    private Vector2 balanceBuildup;

    public State_Jump(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Landing(player, new State_Stumble(player, jumpTimeElapsed * 2, new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z)))
        };
        transitions = transitionsNew;
    }

    public override void Update()
    {
        base.Update();
        
        if(Input.GetButton("Jump") && jumpTimeElapsed < player.jumpDuration){
            player.jumping = true;
            jumpTimeElapsed += Time.deltaTime;
        }       
        else{
            player.jumping = false;
            jumpTimeElapsed = player.jumpDuration;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        if(player.jumping){
            player.rb.AddForce(player.orientation.up * player.jumpSpeed, ForceMode.Impulse);
        }

        balanceBuildup += new Vector2(player.movementInput.x, player.movementInput.z).normalized * 0.05f;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.Play("Jump");
        player.SetColliderMaterial(player.airFriction);

        player.jumping = true;
    }

    public override void OnExitState()
    {
        player.balance += balanceBuildup;

        base.OnExitState();
    }
}
