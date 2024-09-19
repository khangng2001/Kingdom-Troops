using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnemy
{
    Normal,
    Chasing,
    Attack
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask mask;

    private bool hasDetect;
    private Transform targetTransform;
    private Animator animator;


    private StateEnemy currentStateEnemy;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SwitchStateEnemy(StateEnemy.Normal);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * 1f, Color.green);
        EnemyStateMachine();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward,out var hit, 1, mask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red);
            Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), hit.point);
        }
    }

    private void EnemyStateMachine()
    {
        switch (currentStateEnemy)
        {
            case StateEnemy.Normal:
                {
                    if (hasDetect)
                    {
                        SwitchStateEnemy(StateEnemy.Chasing);
                    }
                    break;
                }
            case StateEnemy.Chasing:
                {
                    Vector3 newRotateDirection = targetTransform.position - transform.position;
                    Quaternion toRotation = Quaternion.LookRotation(newRotateDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed);
                    animator.SetBool("Chase", true);

                    if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, 1.5f, mask))
                    {
                        SwitchStateEnemy(StateEnemy.Attack);
                    }

                    break;
                }
            case StateEnemy.Attack:
                {
                    break;
                }
        }
    }

    public void SwitchStateEnemy(StateEnemy newStateEnemy)
    {
        currentStateEnemy = newStateEnemy;

        switch (currentStateEnemy)
        {
            case StateEnemy.Normal:
                {
                    break;
                }
            case StateEnemy.Chasing:
                {
                    break;
                }
            case StateEnemy.Attack:
                {
                    animator.SetTrigger("Attack");
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasDetect = true;
            targetTransform = other.gameObject.transform;
        }
    }
}
