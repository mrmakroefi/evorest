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

    void Update()
    {
        if (GameManager.gm.motor.isHurt) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        dashInput = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button1);
        
        jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button2); ;

        lightAttackInput = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button3);
        heavyAttackInput = Input.GetKeyDown(KeyCode.C);
        
    }
}
