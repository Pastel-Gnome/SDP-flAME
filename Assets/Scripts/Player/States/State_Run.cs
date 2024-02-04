using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Run : PlayerState
{
    public State_Run(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        player.animator.transform.forward = Vector3.Slerp(player.animator.transform.forward, player.movementInput, Time.deltaTime * 7);

        if(player.jumping){
            player.SetPlayerState(new State_Jump(player));
        }
        else if(player.movementInput == Vector3.zero){
            player.SetPlayerState(new State_Stand(player));
        }
    }

    public override void FixedUpdate()
    {
        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * (player.grounded ? 1 : 0.25f), ForceMode.Force);

        RecoverBalance(0.5f);

        base.FixedUpdate();
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
