using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    private PlayerInput playerController;

    private void Awake()
    {
        if (gm == null) {
            gm = this;
        } else {
            Destroy(gameObject);
        }
    }

    public PlayerInput player {
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

}
