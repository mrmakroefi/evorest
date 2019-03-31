using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour {

    private Rigidbody2D rb2D;

    public float maxSpeed = 2f;
    public float jumpHeight = 4f;
    public float agility = 0.2f;

    private Vector2 currentVelocity;
	void Awake () {
        rb2D = GetComponent<Rigidbody2D>();
	}
	
    public void Move(float velocity)
    {
        float targetSmoothTime = Mathf.Abs(velocity) > 0 ? 0.04f : agility;
        Vector2 targetVelocity = new Vector2(velocity, rb2D.velocity.y);
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, targetSmoothTime);
        rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -maxSpeed, maxSpeed), rb2D.velocity.y);
    }

    protected void Jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpHeight);
    }

}
