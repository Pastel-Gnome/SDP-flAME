using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Jumping : PlayerTransition
{
    public Transition_Jumping(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.jumping;
    }
}
