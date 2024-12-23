using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeCharacterManage : MonoBehaviour
{
    private bool move;
    private bool isDrag;
    private Vector3 originPos;
    private SlimePlacement slimePlacement;

    void OnMouseDown()
    {
        if (IsMouseOverSlime())
        {
            isDrag = true;
            StartCoroutine(DragSlime());
        }
    }

    void OnMouseUp()
    {
        if (isDrag)
        {
            isDrag = false;
            DropSlime();
        }
    }

    bool IsMouseOverSlime()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    IEnumerator DragSlime()
    {
        originPos = transform.position;
        RaycastHit hit;
        while (isDrag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlimeArea")))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Log : find SlimeArea success;");
                    transform.position = hit.point;
                }
                else
                {
                    Debug.Log("Log : find SlimeArea failed;");
                    transform.position = originPos;
                }
            }
            else
            {
                Debug.Log("Log : Raycast failed;");
                transform.position = originPos;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void DropSlime()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlimePlacement")))
        {
            if (hit.collider != null)
            {
                SlimePlacement placement = hit.collider.gameObject.GetComponent<SlimePlacement>();
                if (placement != null)
                {
                    Debug.Log("Log : find slime placement success;");
                    placement.SetSlime(this);
                }
                else
                {
                    Debug.Log("Log : SlimePlacement component not found;");
                    transform.position = originPos;
                }
            }
            else
            {
                Debug.Log("Log : find slime placement failed;");
                transform.position = originPos;
            }
        }
        else
        {
            Debug.Log("Log : Raycast failed;");
            transform.position = originPos;
        }
    }
}
