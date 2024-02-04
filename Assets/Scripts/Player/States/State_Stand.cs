using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stand : PlayerState
{
    public State_Stand(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(player.movementInput != Vector3.zero){
            player.SetPlayerState(new State_Run(player));
        }
        else if(player.jumping){
            player.SetPlayerState(new State_Jump(player));
        }
        else if(player.jumping){
            player.SetPlayerState(new State_Jump(player));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        RecoverBalance();
        //player.balance = Vector3.Slerp(player.balance, Vector2.zero, 0.25f);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        player.animator.Play("Stand");
        player.SetColliderMaterial(player.groundFriction);
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
