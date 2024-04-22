using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_UnGrounded : PlayerTransition
{
    public Transition_UnGrounded(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return !player.grounded.collider;
    }

    public override void Transition()
    {
        base.Transition();
    }
}
