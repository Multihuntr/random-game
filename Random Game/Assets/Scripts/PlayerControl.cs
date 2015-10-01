using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    Rigidbody2D rb;
    Mesh mesh;

    public float runSpd;
    public float runSmoothing;
    public float jumpHeight;
    public float jumpTriggerHeight;

    private bool jumpHeld;
    
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        mesh = GetComponent<MeshFilter>().mesh;
        jumpTriggerHeight += mesh.bounds.size.y;
	}

    bool isGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, jumpTriggerHeight);
    }
	
	void LateUpdate () {
        float targXVel = Input.GetAxis("Horizontal") != 0 ? Input.GetAxis("Horizontal")*runSpd : 0;
        float targYVel = rb.velocity.y;

        if (Input.GetAxis("Vertical") > 0)
        {
            if (isGrounded() && !jumpHeld)
            {
                targYVel = jumpHeight;
            }
            jumpHeld = true;
        } else
        {
            jumpHeld = false;
        }
        Debug.Log(Input.GetAxis("Vertical"));
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(targXVel, targYVel), runSmoothing);
    }
}
