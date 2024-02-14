using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Transition_NoMovement : PlayerTransition
{
    public Transition_NoMovement(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.movementInput == Vector3.zero;
    }
}
