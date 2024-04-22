using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class State_Jump : PlayerState
{
    public float jumpTimeElapsed;
    private Vector2 balanceBuildup;
    private int index;
    private float timeElapsed;
    private int jumpImpulses, jumpImpulsesMax;

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
            //timeElapsed += Time.deltaTime;
        }       
        else{
            if(player.jumping){
                /*
                Debug.Log("jump " + index + ": " + timeElapsed + ", jumpImpulses: " + jumpImpulses);
                timeElapsed = 0;
                index++;
                */
                if(jumpImpulses == jumpImpulsesMax-1 && Input.GetButton("Jump")){player.rb.AddForce(Vector3.up * player.jumpSpeed, ForceMode.Impulse);}
                jumpImpulses = 0;
                player.jumping = false;
            }
            jumpTimeElapsed = player.jumpDuration;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        if(player.jumping){
            player.rb.AddForce(Vector3.up * player.jumpSpeed, ForceMode.Impulse);
            jumpImpulses++;
            if(jumpImpulses > jumpImpulsesMax){jumpImpulsesMax = jumpImpulses;}
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
