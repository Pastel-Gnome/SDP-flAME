using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition
{
    protected PlayerBehaviour player;
    protected PlayerState transitionTo;

    protected PlayerTransition(PlayerBehaviour playerNew, PlayerState transitionToNew){
        player = playerNew;
        transitionTo = transitionToNew;
    }

    protected virtual bool Listener(){
        return false;
    }

    public virtual void Transition(){
        if(Listener()){
            transitionTo.Chosen();
            player.SetPlayerState(transitionTo);
        }
    }
}
