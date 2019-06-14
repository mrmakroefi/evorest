using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : CharacterMotor
{
    public GameObject target { get; private set; }

    public TextMesh text;

    public float minDistance = 0.3f;
    public float stayTimer = 3f;
    public float lostTimer = 4f;

    public Vector2 minVisibleDistance;

    public bool targetAcquired { get; private set; }
    public bool inRange { get; private set; }
    private float deltaMovement = 0;

    private float currentStayTime = 0;
    private float currentLostTime = 0;
    private Vector2 lastFramePos;

    bool leftHole = false, rightHole = false, sideCol = false;
    float turnCooldown = 0;


    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // set target direction
        int direction = targetAcquired ? (int)Mathf.Sign(target.transform.position.x - transform.position.x) : getFacingDir;

        // if enemy is closer than minDistance AND not facing oposite direction of target direction OR enemy is attacking
        if (targetAcquired && (GetDistanceToTargetX() <= minDistance && (direction + getFacingDir != 0) || attackController.isAttacking)) {
            direction = 0;
        }

        // in patrol
        if (!targetAcquired && Time.time >= turnCooldown) {
            if (!canMove) {
                SetCanMove(true);
            }

            if (direction == 1 && (rightHole || sideCol)) {
                turnCooldown = Time.time + 0.7f;
                direction = -1;
                Move(direction);
                SetCanMove(false);
            } else if (direction == -1 && (leftHole || sideCol)) {
                turnCooldown = Time.time + 0.7f;
                direction = 1;
                Move(direction);
                SetCanMove(false);
            }
        }

        Move(direction);
    }

    protected override void Update()
    {
        base.Update();
        if (isTargetAvailable()) {
            currentLostTime = lostTimer;
            targetAcquired = true;
            inRange = true;
            SetCanMove(true);
        } else {
            currentLostTime -= Time.deltaTime;
            if (currentLostTime <= 0) {
                targetAcquired = false;
            }
            inRange = false;
        }

        text.gameObject.SetActive(targetAcquired);

        anim.SetFloat("move", Mathf.Abs(rb2D.velocity.x) * 0.5f);

        deltaMovement = Mathf.Abs(transform.position.x - lastFramePos.x);
        lastFramePos = transform.position;

        // if not moving for few seconds, assume that enemy can't follow player and lost player as its target
        if (deltaMovement < 0.01f && targetAcquired) {
            currentStayTime -= Time.deltaTime;
            if (currentStayTime <= 0) {
                targetAcquired = false;
            }
        } else {
            currentStayTime = stayTimer;
        }

        /*
         * ENVIRONMENT CHECKING
         */
        leftHole = true; rightHole = true;
        Collider2D coll = Physics2D.OverlapBox(new Vector2(coll2D.bounds.min.x - 0.15f, coll2D.bounds.min.y - 0.05f), new Vector2(0.1f, 0.1f), 0, groundMask);
        if (coll != null) {
            leftHole = false;
        }

        coll = Physics2D.OverlapBox(new Vector2(coll2D.bounds.max.x + 0.15f, coll2D.bounds.min.y - 0.05f), new Vector2(0.1f, 0.1f), 0, groundMask);
        if (coll != null) {
            rightHole = false;
        }

        sideCol = false;
        coll = Physics2D.OverlapBox(
            new Vector2(isFacingRight ? coll2D.bounds.max.x + 0.025f : coll2D.bounds.min.x - 0.025f, coll2D.bounds.center.y),
            new Vector2(0.05f, coll2D.bounds.size.y - 0.2f), 0, groundMask
            );
        if (coll != null) {
            sideCol = true;
        }
    }

    public float GetDistanceToTargetX()
    {
        return Mathf.Abs(transform.position.x - target.transform.position.x);
    }

    // check if target is in range to move toward to or attack
    bool isTargetAvailable()
    {
        float xDistance = Mathf.Abs(target.transform.position.x - transform.position.x);
        float yDistance = Mathf.Abs(target.transform.position.y - transform.position.y);

        if (xDistance <= minVisibleDistance.x && yDistance <= minVisibleDistance.y) {
            return true;
        }
        return false;
    }
}
