using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Balance : PlayerTransition
{
    private float magnitudeMin, magnitudeMax;
    public Transition_Balance(PlayerBehaviour playerNew, PlayerState transitionToNew, float magnitudeMinNew, float magnitudeMaxNew = 180) : base(playerNew, transitionToNew){
        magnitudeMin = magnitudeMinNew;
        magnitudeMax = magnitudeMaxNew;
    }

    protected override bool Listener(){
        return player.movementInput.magnitude > magnitudeMin && player.movementInput.magnitude <= magnitudeMax;
    }
}
