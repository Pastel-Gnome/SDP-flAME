using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerTransition[] transitions = {};
    public PlayerBehaviour player;

    public virtual void Chosen(){
    }

    protected PlayerState(PlayerBehaviour playerNew){
        player = playerNew;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        foreach(PlayerTransition i in transitions){
            i.Transition();
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
    }

    public virtual void OnEnterState()
    {
    }

    public virtual void OnExitState()
    {
    }
}
