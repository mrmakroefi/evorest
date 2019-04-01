using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float verticalOffset;
    [Tooltip("Jarak pandang depan")]
    public float lookAheadDistanceX;
    [Tooltip("Waktu transisi melihat kedepan")]
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;

    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    void Start()
    {
        focusArea = new FocusArea(GameManager.gm.player.getCollider2D.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(GameManager.gm.player.getCollider2D.bounds);  //udpate focus area position

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset; // the focus area position

        if (focusArea.velocity.x != 0) {    //when the velocity of focus area is not zero, which mean it's moving
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(GameManager.gm.player.horizontalInput) == Mathf.Sign(focusArea.velocity.x) && GameManager.gm.player.horizontalInput != 0) { // if we are heading full forward then
                lookAheadStopped = false;   // this is used for stop looking ahead progress if we not press forward input button
                targetLookAheadX = lookAheadDirX * lookAheadDistanceX;  // initialize the target to look ahead
            } else { // if we are cancel heading forward before looking forward is finish
                if (!lookAheadStopped) {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistanceX - currentLookAheadX) / 4f; // update the target position looking ahead to current position PLUS 1/4 distance to real target for smooth things
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);    // the current position

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime); // move to the center of focus area vertically
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10; //set the the position to focus area
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1f, 0.2f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center, velocity;
        public float left, right, top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {   //target bounds is player collider bounds, and size is the size of focus area that we can edit it on inspector
            //Initialize the edge each side focus area
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            top = targetBounds.min.y + size.y;
            bottom = targetBounds.min.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);   //calculate the center, make sense
        }

        //Update the position of the focus area
        public void Update(Bounds targetBounds)
        { //target bounds is player collider bounds
            float shiftX = 0;
            if (targetBounds.min.x < left) {    //if player go through the left side of focus area then
                shiftX = targetBounds.min.x - left; //shiftX cointain the delta distance of the player pass the focus area bounds
            } else if (targetBounds.max.x > right) {    // same story
                shiftX = targetBounds.max.x - right;
            }
            //reinitialize side position of focus area
            left += shiftX;
            right += shiftX;

            // Same things as above, but it's top and bottom
            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            } else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);   //recalculate the center of focus area
            velocity = new Vector2(shiftX, shiftY); //recalculate the velocity
        }
    }
}
