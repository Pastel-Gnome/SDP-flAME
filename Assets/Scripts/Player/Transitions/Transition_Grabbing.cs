using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_Grabbing : PlayerTransition
{
    PlayerState transitionToAlt;
    public Transition_Grabbing(PlayerBehaviour playerNew, PlayerState transitionToNew, PlayerState transitionToAltNew) : base(playerNew, transitionToNew){
        transitionToAlt = transitionToAltNew;
    }

    protected override bool Listener(){
        return player.grabbing;
    }

    public override void Transition(){
        if(Listener()){
            if(player.heldObject){
                transitionToAlt.Chosen();
                player.SetPlayerState(transitionToAlt);
            }
            else{
                transitionTo.Chosen();
                player.SetPlayerState(transitionTo);
            }
        }
    }
}
