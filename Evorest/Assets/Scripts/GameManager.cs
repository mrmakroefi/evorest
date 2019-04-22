using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    private PlayerMovement playerController;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        if (gm == null) {
            gm = this;
        } else {
            Destroy(gameObject);
        }
    }

    public PlayerMovement getPlayer {
        get {
            if (playerController != null) {
                return playerController;
            }
            return null;
        }

        set {
            playerController = value;
        }
    }

    public PlayerAttack getPlayerAttack {
        get {
            if (playerAttack != null) {
                return playerAttack;
            }
            return null;
        }

        set {
            playerAttack = value;
        }
    }

}
