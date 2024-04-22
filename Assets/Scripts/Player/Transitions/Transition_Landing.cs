using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Landing : PlayerTransition
{
    public Transition_Landing(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.grounded.collider && !player.jumping;
    }

    public override void Transition()
    {
        base.Transition();
    }
}
