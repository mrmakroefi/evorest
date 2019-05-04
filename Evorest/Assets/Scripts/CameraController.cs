using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    public Bounds boundary;

    public float verticalOffset;
    public float lookAheadDistanceX;
    public float lookAheadDistanceY;
    public float lookSmoothTimeX;
    public float lookSmoothTimeY;
    
    float currentLookAheadX;
    float currentLookAheadY;
    float targetLookAheadX;
    float targetLookAheadY;
    float lookAheadDirX;
    float lookAheadDirY;
    float smoothLookVelocityX;
    float smoothLookVelocityY;

    float horizontalCamExtent, verticalCamExtent;

    [HideInInspector]
    public bool preview;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        horizontalCamExtent = (topRight.x - bottomLeft.x) * 0.5f;
        verticalCamExtent = (topRight.y - bottomLeft.y) * 0.5f;
    }

    void LateUpdate()
    {
        Vector2 targetPosition = GameManager.gm.motor.coll2D.bounds.center + Vector3.up * verticalOffset;

        lookAheadDirX = GameManager.gm.motor.getFacingDir;
        lookAheadDirY = Mathf.Sign(GameManager.gm.motor.rb2D.velocity.y);

        if (Mathf.Abs(PlayerInput.horizontalInput) > 0) {
            targetLookAheadX = 0;
        } else {
            targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
        }

        if (Mathf.Abs(GameManager.gm.motor.rb2D.velocity.y) > 0) {
            targetLookAheadY = lookAheadDirY * lookAheadDistanceY;
        } else {
            targetLookAheadY = 0;
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        currentLookAheadY = Mathf.SmoothDamp(currentLookAheadY, targetLookAheadY, ref smoothLookVelocityY, lookSmoothTimeY);
        targetPosition += new Vector2(currentLookAheadX, currentLookAheadY);

        transform.position = (Vector3)targetPosition + Vector3.forward * -10;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, boundary.min.x + horizontalCamExtent, boundary.max.x - horizontalCamExtent),
            Mathf.Clamp(transform.position.y, boundary.min.y + verticalCamExtent, boundary.max.y - verticalCamExtent),
            transform.position.z
            );
    }

    // debugging
    private void OnDrawGizmos()
    {
        if (preview) {
            Gizmos.color = new Color(1, 1, 0, 0.1f);
            Gizmos.DrawCube(boundary.center, boundary.size);
        }
    }
}
