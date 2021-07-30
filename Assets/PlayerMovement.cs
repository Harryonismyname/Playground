using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform destinationNode;
    private readonly float MOVEMENTSPEED = 3f;
    public LayerMask whatStopsMovement;
    public Animator animator;
    public bool visuals;
    // Start is called before the first frame update
    void Start()
    {
        destinationNode.parent = null;
        if (!visuals)
        {
            animator = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (visuals)
        {
            if (Vector3.Distance(transform.position, destinationNode.position) < 0.1f)
            {
                animator.SetFloat("HorizontalSpeed", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("VerticalSpeed", Input.GetAxisRaw("Vertical"));
            }
        }
        Tools.MoveObjectToLocation(gameObject, destinationNode.position, MOVEMENTSPEED);
        if (Tools.PositionCheck(transform.position, destinationNode.position))
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                
                if (!Physics2D.OverlapCircle(destinationNode.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), 0.3f, whatStopsMovement))
                {
                    destinationNode.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(destinationNode.position + new Vector3(0, Input.GetAxisRaw("Vertical"), 0), 0.3f, whatStopsMovement))
                {
                    destinationNode.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                }
                
            }
        }
    }
}
