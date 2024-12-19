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
        isDrag = true;
        StartCoroutine(DragSlime());
    }

    void OnMouseUp()
    {
        isDrag = false;
        DropSlime();
    }
    IEnumerator DragSlime()
    {
        originPos = transform.position;
        RaycastHit hit;
        while (isDrag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlimeArea"));
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
            yield return new WaitForFixedUpdate();
        }
    }
    void DropSlime()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlimePlacement"));
        if (hit.collider != null)
        {
            Debug.Log("Log : find slime placement success;");
            hit.collider.gameObject.GetComponent<SlimePlacement>().SetSlime(this);
        }
        else
        {
            Debug.Log("Log : find slime placement failed;");
            transform.position = originPos;
        }
    }
}
