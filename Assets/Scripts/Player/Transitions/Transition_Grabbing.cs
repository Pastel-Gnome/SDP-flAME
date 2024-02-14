using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Transition_Grabbing : PlayerTransition
{
    public Transition_Grabbing(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.grabbing;
    }
}
