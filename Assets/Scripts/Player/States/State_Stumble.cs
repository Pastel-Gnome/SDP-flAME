using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Stumble : PlayerState
{
    private float stumbleDuration = 0.5f;
    private float stumbleElapsed = 0;
    private Vector3 stumbleInput;

    public State_Stumble(PlayerBehaviour playerNew, float stumbleDurationNew){
        player = playerNew;
        stumbleInput = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
        stumbleDuration = stumbleDurationNew;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(stumbleElapsed < stumbleDuration){
            if(player.grounded){
                stumbleElapsed += Time.deltaTime;
            }
        }
        else{
            player.SetPlayerState(new State_Stand(player));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.rb.AddForce(stumbleInput);

        player.rb.AddForce(player.movementInput.normalized * player.runSpeed * 0.25f, ForceMode.Force);

        player.balance += new Vector2(stumbleInput.x, stumbleInput.z).normalized;
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
