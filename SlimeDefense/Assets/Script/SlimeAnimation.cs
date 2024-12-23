using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum SlimeAnimationName { Idle, Walk, Jump, Attack, Damage0, Damage1, Damage2 }
public class SlimeAnimation : MonoBehaviour
{
    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;
    private SlimePlacement placement;

    public Animator animator;
    public NavMeshAgent agent;
    public int damType;
    public Transform[] waypoints;
    private int m_CurrentWaypointIndex;
    private Material faceMaterial;
    private Vector3 originPos;

    public enum WalkType { Patroll, ToOrigin }
    private WalkType walkType;

    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().material;
        originPos = transform.position;
        currentState = SlimeAnimationState.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                SetFace(faces.Idleface);
                break;
            case SlimeAnimationState.Walk:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

                agent.isStopped = false;
                agent.updateRotation = true;

                if (walkType == WalkType.ToOrigin)
                {
                    agent.SetDestination(originPos);
                    SetFace(faces.WalkFace);
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        walkType = WalkType.Patroll;

                        transform.rotation = Quaternion.identity;

                        currentState = SlimeAnimationState.Idle;
                    }

                }
                else
                {
                    if (waypoints[0] == null) return;

                    agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        currentState = SlimeAnimationState.Idle;

                        Invoke(nameof(WalkToNextDestination), 2f);
                    }

                }
                animator.SetFloat("Speed", agent.velocity.magnitude);


                break;

            case SlimeAnimationState.Jump:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;

                StopAgent();
                SetFace(faces.jumpFace);
                animator.SetTrigger("Jump");

                break;

            case SlimeAnimationState.Attack:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                StopAgent();
                SetFace(faces.attackFace);
                animator.SetTrigger("Attack");

                break;
            case SlimeAnimationState.Damage:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                     || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                     || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;

                StopAgent();
                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", damType);
                SetFace(faces.damageFace);

                break;

        }
    }

    public void WalkToNextDestination()
    {
        currentState = SlimeAnimationState.Walk;
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        SetFace(faces.WalkFace);
    }
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }
    private void StopAgent()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
        agent.updateRotation = false;
    }
    public void SetSlimeAnimationSate(SlimeAnimationState state)
    {
        currentState = state;
    }

}
