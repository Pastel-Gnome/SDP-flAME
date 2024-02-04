using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Jump : PlayerState
{
    private float jumpTimeElapsed;

    public State_Jump(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
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

        if(player.grounded && jumpTimeElapsed >= player.jumpDuration){
            player.SetPlayerState(new State_Stumble(player, jumpTimeElapsed * 2));
        }
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
        base.OnExitState();
    }
}
