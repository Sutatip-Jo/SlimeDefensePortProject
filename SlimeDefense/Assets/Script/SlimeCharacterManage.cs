using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeCharacterManage : MonoBehaviour
{
    private bool move;
    private bool isDrag;
    private Vector3 originPos;
    private SlimeAnimation slimeAnimation;
    public float attackRange = 1f;

    void Start()
    {
        // slimeAnimation = GetComponent<SlimeAnimation>();
        slimeAnimation = GetComponentInChildren<SlimeAnimation>();
    }
    void Update()
    {
        Attack();
    }

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
    public void SetSlimeAnimationSate(SlimeAnimationState state)
    {
        slimeAnimation.currentState = state;
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

    public void Attack()
    {
        RaycastHit hit = RaycastForMonster();
        if (hit.collider == null)
        {
            return;
        }
        SetSlimeAnimationSate(SlimeAnimationState.Attack);
    }

    RaycastHit RaycastForMonster()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.red, 1f);
        if (Physics.Raycast(ray, out hit, attackRange, LayerMask.GetMask("Monster")))
        {
            Debug.Log("Monster detected: " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<MonsterManage>().TakeDamage();
        }
        else
        {
            Debug.Log("No monster detected.");
        }
        Debug.Log("Raycast origin: " + ray.origin + ", direction: " + ray.direction);
        return hit;
    }
}
