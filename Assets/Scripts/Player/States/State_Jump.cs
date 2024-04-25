using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class State_Jump : PlayerState
{
    private Vector2 balanceBuildup;
    private int jumpImpulses;
    public float jumpTimeElapsed;

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
        
        if(Input.GetButton("Jump") && jumpImpulses < player.jumpImpulsesMax){
            player.jumping = true;
            jumpTimeElapsed += Time.deltaTime;
        }       
        else{
            player.jumping = false;
            jumpImpulses = player.jumpImpulsesMax;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        if(player.jumping){
            player.rb.AddForce(Vector3.up * player.jumpSpeed, ForceMode.Impulse);
            jumpImpulses++;
        }
        else if(!Input.GetButton("Jump")){
            player.rb.AddForce(-Vector3.up * player.dropSpeed, ForceMode.Impulse);
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
