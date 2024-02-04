using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private Vector2 balanceRecoveryVelocity = Vector2.zero;

    protected PlayerBehaviour player;

    // Update is called once per frame
    virtual public void Update()
    {
    }

    // Update is called once per frame
    virtual public void FixedUpdate()
    {
    }

    virtual public void OnEnterState()
    {
    }

    virtual public void OnExitState()
    {
    }

    protected void RecoverBalance(float recoveryMod = 1){
        player.balance = new Vector2(
            player.balance.x > 0.2 ? player.balance.x - player.balanceRecoverRate : player.balance.x + player.balanceRecoverRate, 
            player.balance.y > 0.2 ? player.balance.y - player.balanceRecoverRate : player.balance.y + player.balanceRecoverRate
        );

        //player.balance = Vector3.Slerp(player.balance, Vector2.zero, player.balanceRecoverRate * recoveryMod);

        //player.balance = Vector2.SmoothDamp(player.balance, Vector2.zero, ref balanceRecoveryVelocity, 0, player.balanceRecoverRate * recoveryMod);
    }
}
