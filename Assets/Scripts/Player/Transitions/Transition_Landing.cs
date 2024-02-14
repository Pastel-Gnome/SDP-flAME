using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Transition_Landing : PlayerTransition
{
    public Transition_Landing(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.grounded && !player.jumping;
    }

    public override void Transition()
    {
        Debug.Log("hitting the ground");
        base.Transition();
    }
}
