using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterMotor : MonoBehaviour
{

    public Rigidbody2D rb2D { get; private set; }
    public Collider2D coll2D { get; private set; }
    public Animator anim { get; private set; }
    public AttackController attackController { get; private set; }

    public SpriteRenderer sprite;                   // main sprite renderer
    public Image dashMeter;
    public LayerMask groundMask;
    public float maxSpeed = 2f;
    public float jumpPower = 4f;
    public ParticleSystem dashEffect;
    public float dashDistance = 1f;
    public float dashTime = 0.2f;
    public float dashMeterFillingTime = 10f;
    public float agility = 0.2f;
    public float inAirAgility = 1f;

    private bool isJumping = false;
    protected bool isFacingRight = true;
    public bool isDashing { get; private set; }
    protected bool isGrounded { get; private set; }
    private bool lastFrameGrounded = false;
    public bool isHurt { get; private set; }
    public bool canMove { get; private set; }

    private int dashDirection = 1;

    private bool canDoubleJump = true;
    private bool doubleJumped = false;
    private float jumpAssistTime = 0.5f;
    private float currentJumpAssistTime = 0;

    private float tempDashTime = 0;
    private float currentDashTime = 0;
    private float currentDashDistance = 0;
    private float currentDashMeter = 0;

    private bool customDash = false;

    private AttackStateUpdater[] attackStateUpdaters;
    private HurtStateUpdater[] hurtStateUpdaters;

    private Vector2 currentVelocity;
    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        attackController = GetComponent<AttackController>();
        anim = GetComponentInChildren<Animator>();

        if (attackController != null) {
            attackController.SetAnimator(anim);
            attackController.SetMotor(this);
        }
        canMove = true;

        isFacingRight = !sprite.flipX;

        currentDashTime = dashTime;

        if (dashMeter != null) {
            dashMeter.fillAmount = 1f;
            currentDashMeter = 100f;
        }
    }

    protected void Start()
    {
        // intialize attackController on Mechanim behaviour
        if (attackController != null) {
            attackStateUpdaters = anim.GetBehaviours<AttackStateUpdater>();
            foreach (AttackStateUpdater atkUptr in attackStateUpdaters) {
                atkUptr.attackController = attackController;
            }

            hurtStateUpdaters = anim.GetBehaviours<HurtStateUpdater>();
            foreach (HurtStateUpdater hrtUptr in hurtStateUpdaters) {
                hrtUptr.attackController = attackController;
            }
        }
    }

    public int getFacingDir {
        get {
            return isFacingRight ? 1 : -1;
        }
    }
    
    protected virtual void Update()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(coll2D.bounds.center.x, coll2D.bounds.min.y), new Vector2(coll2D.bounds.size.x - 0.03f, 0.05f), 0, groundMask);
        for (int i = 0; i < colliders.Length; i++) {
            isGrounded = true;
        }

        // if we are not grounded because of falling off ledge instead of jumping then . . .
        if (!isGrounded && !isJumping) {
            currentJumpAssistTime += Time.deltaTime;
            currentJumpAssistTime = currentJumpAssistTime >= jumpAssistTime ? jumpAssistTime : currentJumpAssistTime;       // clamp the value
        }

        // first frame of grounded
        if (isGrounded && !lastFrameGrounded) {
            isJumping = false;   // we are not jumping anymore
            currentJumpAssistTime = 0; // reset jumpAssistTimer
        }

        // reset double jump
        if (!lastFrameGrounded && isGrounded) doubleJumped = false;

        lastFrameGrounded = isGrounded;

        if (dashMeter != null) {
            if (currentDashMeter < 100) {
                currentDashMeter += dashMeterFillingTime * Time.deltaTime;
            }
            currentDashMeter = Mathf.Clamp(currentDashMeter, 0f, 100f);
            dashMeter.fillAmount = currentDashMeter / 100;
        }
    }

    protected virtual void FixedUpdate()
    {
        // move overtime
        if (isDashing) {
            Dashing(dashDirection);
        }
    }

    public void Dash(int direction, float dashTime, float dashDistance, bool nonMovementDash, bool useEffect = false)
    {
        if (dashMeter != null) {
            // if its regular dash and doesnt have enough dash meter. forget about dashing man!
            if (!customDash) if (currentDashMeter - 50 < 0) return;
        }

        // let dash function know what kind of dash is this. dash from attacking or regular dash.
        customDash = nonMovementDash; isDashing = false;
        dashDirection = direction;
        currentDashDistance = dashDistance;

        if (!customDash) {
            if (dashMeter != null) {
                currentDashMeter -= 50f;
            }
            isDashing = true;
        }

        if (useEffect && dashEffect != null) {
            dashEffect.gameObject.SetActive(true);
            dashEffect.Play();
        }
        // set dash time, determine how long dash last
        currentDashTime = dashTime;
        tempDashTime = currentDashTime;
        // dash as player press button (followed by fixedUpdate call)
        Dashing(dashDirection);
    }

    protected void Move(float direction)
    {
        if (!canMove || isDashing) return;
        // snappy movement but smooth on the brakes
        float targetSmoothTime = Mathf.Abs(direction) > 0 ? 0.08f : agility;
        Vector2 targetVelocity = new Vector2(
            direction * maxSpeed,
            rb2D.velocity.y
            );
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, targetSmoothTime);

        //rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -maxSpeed, maxSpeed), rb2D.velocity.y);

        FaceDirection(direction);
    }

    protected void FaceDirection(float direction)
    {
        if (direction > 0 && !isFacingRight) {
            SpriteFlip();
        } else if (direction < 0 && isFacingRight) {
            SpriteFlip();
        }
    }

    private void SpriteFlip()
    {
        sprite.flipX = !sprite.flipX;

        if (dashEffect != null) {
            ParticleSystem.TextureSheetAnimationModule dashSheet = dashEffect.textureSheetAnimation;
            dashSheet.flipU = sprite.flipX ? 1 : 0;
        }

        isFacingRight = !sprite.flipX;
    }

    protected void Jump()
    {
        if (isGrounded || currentJumpAssistTime < jumpAssistTime) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
            isJumping = true;
            isDashing = false;  // cancel dash
            currentJumpAssistTime = jumpAssistTime;
        } else if (!doubleJumped && canDoubleJump) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower * 0.7f);
            doubleJumped = true;
            isJumping = true;
            isDashing = false;
        }
    }

    protected bool Dashing(int direction)
    {
        direction = (int)Mathf.Sign(direction);

        // cancel dash by going to the other direction (create dash effect as new instance)
        //if (direction - Mathf.RoundToInt(-PlayerInput.horizontalInput) == 0) {
        //    isDashing = false;
        //    return false;
        //}

        rb2D.velocity = new Vector2(direction * currentDashDistance * (1 / currentDashTime), customDash ? rb2D.velocity.y : 0);

        // waiting dash timer to end
        tempDashTime -= Time.fixedDeltaTime;
        if (tempDashTime <= 0) {
            tempDashTime = currentDashTime;
            if (!customDash) isDashing = false;
            return false;
        }
        if (!customDash) isDashing = true;
        return true;
    }

    public void SetCanDoubleJump (bool flag)
    {
        canDoubleJump = flag;
    }
    
    public void SetIsHurt(bool flag)
    {
        isHurt = flag;
    }

    public void SetCanMove(bool flag)
    {
        canMove = flag;
    }

    public void SetAttackController(AttackController controller)
    {
        attackController = controller;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(new Vector2(coll2D.bounds.center.x, coll2D.bounds.min.y), new Vector2(coll2D.bounds.size.x - 0.02f, 0.05f));
    //}
}
