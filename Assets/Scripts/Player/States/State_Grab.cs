using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grab : PlayerState
{
    public State_Grab(PlayerBehaviour playerNew) : base(playerNew){
    }

    public override void Chosen(){
        PlayerTransition[] transitionsNew = {
            new Transition_Timer(player, new State_Stand(player), player.grabTime)
        };
        transitions = transitionsNew;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.grabbing = false;

        if(player.heldObject){
            player.heldObject.dropped();
            player.heldObject.transform.SetPositionAndRotation(player.placePosition.position, Quaternion.identity);
            player.heldObject = null;
        } else { 

        Transform closestGrab = null;
        Collider[] grabHits = Physics.OverlapSphere(player.orientation.position, player.grabRadius, player.holdableMask);
        foreach(Collider i in grabHits){
            if(closestGrab == null || Vector3.Distance(i.transform.position, player.orientation.position) < Vector3.Distance(closestGrab.position, player.orientation.position)){
                closestGrab = i.transform;
            }
        }

        if(closestGrab){
            player.heldObject = closestGrab.GetComponent<Holdable>();
            player.heldObject.grabbed(player.CarryAnchorpoint);
        }
		}
	}

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
