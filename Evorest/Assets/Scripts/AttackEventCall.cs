using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventCall : MonoBehaviour {
    
    public void Attack(int index)
    {
        GameManager.gm.getPlayerAttack.Attack(index);
    }

    public void Dash(int index)
    {
        GameManager.gm.getPlayer.Dash(
            GameManager.gm.getPlayer.getFacingDir,
            GameManager.gm.getPlayerAttack.meleeCombos[index].dashTime,
            GameManager.gm.getPlayerAttack.meleeCombos[index].dashDistance
            );
    }



}
