using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UIInput bond;

    public UnityEngine.Events.UnityEvent OnTap;
    public UnityEngine.Events.UnityEvent OnHold;
    public UnityEngine.Events.UnityEvent OnRelease;

    Vector2 touchStartPos;                  //posisi touch pada saat frame pertama menyentuh layar input (Screen Space)

    [HideInInspector]
    public bool inside = false;
    [HideInInspector]
    bool touched = false;            //Is the input touched?
    [HideInInspector]
    public Vector2 fingerPosition;          //posisi touch sekarang (Screen space)

    [HideInInspector]
    public int pointerID;

    public void OnPointerDown(PointerEventData data)
    {
        if (!touched) {
            touched = true;
            pointerID = data.pointerId;
            inside = true;
            if (bond) {
                bond.touched = true;
                bond.pointerID = data.pointerId;
                bond.inside = false;
            }

            if (OnTap != null) {
                OnTap.Invoke();
            }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!bond) return;

        if (touched ) {
            inside = true;
        }
    }

    void Update()
    {
        if (touched && OnHold != null && inside) {
            OnHold.Invoke();
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == pointerID) {

            fingerPosition = data.position;

        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == pointerID) {
            touched = false;
            inside = false;

            if (bond) {
                bond.touched = false;
                bond.inside = false;
            }

            if (OnRelease != null) {
                OnRelease.Invoke();
            }
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (!bond) return;

        inside = false;
    }
}
