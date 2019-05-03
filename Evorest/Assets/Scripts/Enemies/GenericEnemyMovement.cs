using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyMovement : CharacterMotor {

    // patrol
    // target in sight
    // stance
    // go for target
    // close enough? attack

    public enum TargetDirection { right, bottom, left, top};

    public GameObject target { get; private set; }

    public float minDistance = 0.3f;
    public float cooldownAfterHurt = 1f;

    public TextMesh text;

    TargetDirection targetDirection;

    private float currentCooldownAfterHurt = 0;
    private bool canMove = true;

    bool leftHole = false, rightHole = false, sideCol = false;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player");
        SetCanDoubleJump(false);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        canMove = true;
        if (isHurt || Time.time < currentCooldownAfterHurt) {
            if (isHurt) {
                currentCooldownAfterHurt = Time.time + cooldownAfterHurt;
            }
            canMove = false;
        }

        float distance = GetDistanceToTarget();
        int direction = (int)Mathf.Sign(target.transform.position.x - transform.position.x); ;
        if (distance <= (isGrounded ? minDistance : 0) && (direction + getFacingDir != 0) || !canMove) {
            direction = 0;
        }

        anim.SetBool("isMoving", direction != 0);

        Move(direction);

        if (canMove) {
            if (direction == 1 && rightHole && (targetDirection == TargetDirection.right || targetDirection == TargetDirection.top)) {
                Jump();
            } else if (direction == -1 && leftHole && (targetDirection == TargetDirection.left || targetDirection == TargetDirection.top)) {
                Jump();
            }

            if (sideCol && distance > minDistance && (targetDirection == TargetDirection.top || targetDirection == TargetDirection.left || targetDirection == TargetDirection.right)) {
                Jump();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        Vector2 direction = target.transform.position - transform.position;
        int angle = Mathf.RoundToInt(Vector2.SignedAngle(Vector2.right, direction));

        anim.SetBool("grounded", isGrounded);
        anim.SetFloat("move", Mathf.Abs(rb2D.velocity.x) * 0.5f);

        // up is positive
        if ((angle > 135 && angle <= 180) || (angle < -135 && angle >= -180)) {
            targetDirection = TargetDirection.left;
            text.text = "left";
        } else if (angle >= -135 && angle < -45){
            targetDirection = TargetDirection.bottom;
            text.text = "bottom";
        } else if (angle >= -45 && angle < 45) {
            targetDirection = TargetDirection.right;
            text.text = "right";
        } else {
            targetDirection = TargetDirection.top;
            text.text = "top";
        }

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

    public float GetDistanceToTarget()
    {
        return Mathf.Abs(Vector2.Distance(transform.position, target.transform.position));
    }

    private void OnDrawGizmos()
    {
        if (!coll2D) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(
            new Vector2(coll2D.bounds.center.x, coll2D.bounds.min.y), new Vector2(coll2D.bounds.size.x - 0.03f, 0.05f)
            );
    }
}
