using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public PlayerBehaviour player;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 viewDirection = (player.orientation.position - new Vector3(transform.position.x, player.orientation.position.y, transform.position.z)).normalized;
        //print(viewDirection);
        Vector3 slopeForward = Vector3.Slerp(viewDirection.normalized, Vector3.ProjectOnPlane(viewDirection.normalized, player.grounded.normal).normalized, 0.75f);
        player.orientation.forward = slopeForward;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.rb.position + player.orientation.forward, 0.5f);
        Gizmos.DrawSphere(player.rb.position + (player.orientation.forward * 1.5f), 0.3f);
    }
}
