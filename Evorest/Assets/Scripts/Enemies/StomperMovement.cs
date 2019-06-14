using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomperMovement : CharacterMotor {

    /*
     * - idle
     * - find the target
     * - fly toward target
     * - if close enough
     * - DROP!!
     * - back to idle
     */

    public GameObject target { get; private set; }

    public float flySpeed = 5f;
    public float flyingCooldown = 3f;
    public float flyingMaxDuration = 4f;

    public TextMesh text;

    public Vector2 minVisibleDistance;

    public bool targetAcquired { get; private set; }
    public bool inRange { get; private set; }
    

    bool sideCol = false;

    bool flying = false;

    private float currentFlyingCooldown = 0;
    private float currentFlyingSpeed = 0;
    private float currentFlyingDuration = 0;

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
        if (targetAcquired && (direction + getFacingDir != 0)) {
            direction = 0;
        }

        if (flying) {
            // optimize expensive calculation PLEASE
            float distance = Mathf.Abs(transform.position.x - target.transform.position.x);

            // DROP HARD!
            if (distance <= 0.05f || Time.time >= currentFlyingDuration) {
                flying = false;
                rb2D.isKinematic = false;
                currentFlyingCooldown = Time.time + flyingCooldown;
                return;
            }

            Vector2 flyDir = ((target.transform.position - (Vector3.up * -0.75f)) - transform.position).normalized;

            currentFlyingSpeed = Mathf.Lerp(currentFlyingSpeed, flySpeed, flySpeed * Time.deltaTime);

            rb2D.velocity = flyDir * currentFlyingSpeed * Time.deltaTime;
        } else {

            // in patrol
            if (!targetAcquired) {
                //if (!canMove) {
                //    SetCanMove(true);
                //}

                //if (direction == 1 && ( sideCol)) {
                //    direction = -1;
                //    Move(direction);
                //    SetCanMove(false);
                //} else if (direction == -1 && (sideCol)) {
                //    direction = 1;
                //    Move(direction);
                //    SetCanMove(false);
                //}
            } else {
                FaceTarget(direction);
                if (Time.time >= currentFlyingCooldown) {
                    rb2D.isKinematic = true;
                    flying = true;
                    currentFlyingSpeed = 0;
                    currentFlyingDuration = Time.time + flyingMaxDuration;
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (isTargetAvailable()) {
            targetAcquired = true;
            inRange = true;
        } else {
            inRange = false;
            targetAcquired = false;
        }

        text.gameObject.SetActive(targetAcquired);

        anim.SetFloat("move", Mathf.Abs(rb2D.velocity.x) * 0.5f);
        
        /*
         * ENVIRONMENT CHECKING
         */
        sideCol = false;
        Collider2D coll = Physics2D.OverlapBox(
            new Vector2(isFacingRight ? coll2D.bounds.max.x + 0.025f : coll2D.bounds.min.x - 0.025f, coll2D.bounds.center.y),
            new Vector2(0.05f, coll2D.bounds.size.y - 0.2f), 0, groundMask
            );
        if (coll != null) {
            sideCol = true;
        }
    }

    public float GetDistanceToStuff(Vector2 position)
    {
        return Vector2.Distance(transform.position, position);
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
