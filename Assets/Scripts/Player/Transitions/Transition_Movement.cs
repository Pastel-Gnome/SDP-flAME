using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Transition_Movement : PlayerTransition
{
    private float magnitudeMin, magnitudeMax = 1;
    public Transition_Movement(PlayerBehaviour playerNew, PlayerState transitionToNew) : base(playerNew, transitionToNew){
    }

    protected override bool Listener(){
        return player.movementInput.magnitude > magnitudeMin && player.movementInput.magnitude <= magnitudeMax;
    }
}
