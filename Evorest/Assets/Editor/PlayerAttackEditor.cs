using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackController),true)]
public class MeleeAttackControllerEditor : Editor {
    private bool preview = false;
    private int index = 0;

    public override void OnInspectorGUI()
    {
        AttackController melee = (AttackController)target;

        index = EditorGUILayout.IntSlider("Element Index", index, 0, melee.meleeCombos.Length - 1);
        AttackController.Combos combo = melee.meleeCombos[index];

        EditorGUILayout.LabelField((preview ? "(On) " : "(Off) ") + combo.name);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview")) {
            preview = true;
        }

        if (GUILayout.Button("Clear")) {
            preview = false;
            melee.UpdatePreviewHitbox(combo, false);
        }
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();

        if (preview) {
            melee.UpdatePreviewHitbox(combo, true);
        }
    }
    

}
