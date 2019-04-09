using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterMotor : MonoBehaviour {

    private Rigidbody2D rb2D;
    private Collider2D coll2D;

    public SpriteRenderer sprite;
    public LayerMask groundMask;
    public float maxSpeed = 2f;
    public float jumpPower = 4f;
    public ParticleSystem dashEffect;
    public float dashDistance = 1f;
    public float dashTime = 0.2f;
    public float agility = 0.2f;
    public float inAirAgility = 1f;

    protected bool isFacingRight = true;

    protected bool isDashing { get; private set; }

    protected bool isGrounded { get; private set; }
    private bool lastFrameGrounded = false;

    private bool doubleJumped = false;

    private float currentDashTime = 0;

    private Vector2 currentVelocity;
	protected virtual void Awake () {
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();

        isFacingRight = !sprite.flipX;

        currentDashTime = dashTime;
	}
    
    public int getFacingDir {
        get {
            return isFacingRight ? 1 : -1;
        }
    }

    public Collider2D getCollider2D {
        get {
            if (coll2D != null) {
                return coll2D;
            }
            return null;
        }
    }

    public Rigidbody2D getRb2D {
        get {
            if (rb2D != null) {
                return rb2D;
            }
            return null;
        }
    }
    

    protected virtual void Update()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(coll2D.bounds.center.x, coll2D.bounds.min.y), new Vector2(coll2D.bounds.size.x-0.02f, 0.05f), 0, groundMask);
        for (int i = 0;i < colliders.Length; i++) {
            isGrounded = true;
        }

        // reset double jump
        if (!lastFrameGrounded && isGrounded) doubleJumped = false;

        lastFrameGrounded = isGrounded;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(new Vector2(coll2D.bounds.center.x, coll2D.bounds.min.y), new Vector2(coll2D.bounds.size.x - 0.02f, 0.05f));
    //}


    protected void Move(float velocity)
    {
        // snappy movement but smooth on the brakes
        float targetSmoothTime = Mathf.Abs(velocity) > 0 ? 0.08f : agility;
        Vector2 targetVelocity = new Vector2(
            velocity * maxSpeed,
            rb2D.velocity.y
            );
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, targetSmoothTime);

        //rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -maxSpeed, maxSpeed), rb2D.velocity.y);

        if (velocity > 0 && !isFacingRight) {
            SpriteFlip();
        } else if (velocity < 0 && isFacingRight) {
            SpriteFlip();
        }
    }

    private void SpriteFlip()
    {
        sprite.flipX = !sprite.flipX;
        isFacingRight = !sprite.flipX;
    }

    protected void Jump()
    {
        if (isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        } else if (!doubleJumped) {
            doubleJumped = true;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower * 0.7f);
        }
    }

    protected bool Dash(int direction)
    {
        if (!isDashing) {
            dashEffect.gameObject.SetActive(true);
            dashEffect.Play();
        }

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        direction = (int)Mathf.Sign(direction);
        rb2D.velocity = new Vector2(direction * dashDistance * ( 1 / dashTime), rb2D.velocity.y);

        // waiting dash timer to end
        currentDashTime -= Time.deltaTime;
        if (currentDashTime <= 0) {
            currentDashTime = dashTime;
            isDashing = false;
            return false;
        }
        isDashing = true;
        return true;
    }
}
