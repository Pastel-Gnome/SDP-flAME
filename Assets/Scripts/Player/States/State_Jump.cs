using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class State_Jump : PlayerState
{
    public float jumpTimeElapsed;
    private Vector2 balanceBuildup;

    public State_Jump(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Landing(player, new State_Stumble(player, this))
            //new Transition_Landing(player, new State_Stand(player))
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
        else if(!Input.GetButton("Jump")){
            player.rb.AddForce(-player.orientation.up * player.dropSpeed, ForceMode.Impulse);
        }

        balanceBuildup += new Vector2(player.movementInput.x, player.movementInput.z).normalized * 0.05f;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.animator.SetTrigger("Go_Jump");
        player.SetColliderMaterial(player.airFriction);

        player.jumping = true;
    }

    public override void OnExitState()
    {
        player.balance += balanceBuildup;

        base.OnExitState();
    }
}
