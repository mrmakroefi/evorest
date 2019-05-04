using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static float horizontalInput { get; private set; }
    public static bool jumpInput { get; private set; }
    public static bool dashInput { get; private set; }
    public static bool lightAttackInput { get; private set; }
    public static bool heavyAttackInput { get; private set; }
   
    private int inputFrame = 0;

    void Update()
    {
        if (GameManager.gm.motor.isHurt) return;

        if (Time.frameCount == inputFrame) {
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        dashInput = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button1);
        if (dashInput) {
            print("ye");
        }
        
        jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2);

        lightAttackInput = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button3);
        heavyAttackInput = Input.GetKeyDown(KeyCode.C);
    }

    public void DoDash ()
    {
        if (GameManager.gm.motor.isHurt) return;
        dashInput = true;
        inputFrame = Time.frameCount;
    }

    public void DoJump()
    {
        if (GameManager.gm.motor.isHurt) return;
        jumpInput = true;
        inputFrame = Time.frameCount;
    }

    public void DoAttack()
    {
        if (GameManager.gm.motor.isHurt) return;
        lightAttackInput = true;
        inputFrame = Time.frameCount;
    }

    public void SetHorizontalInput(float input)
    {
        if (GameManager.gm.motor.isHurt) return;
        horizontalInput = input;
        inputFrame = Time.frameCount;
    }
}
