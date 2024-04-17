using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Timer : PlayerTransition
{
    private float timeElapsed = 0;
    private float duration;
    public Transition_Timer(PlayerBehaviour playerNew, PlayerState transitionToNew, float durationNew) : base(playerNew, transitionToNew){
        duration = durationNew;
    }

    protected override bool Listener(){
        timeElapsed += Time.deltaTime;

        return timeElapsed > duration;
    }
}
