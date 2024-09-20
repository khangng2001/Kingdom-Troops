using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum StateEnemy
{
    Normal,
    Chasing,
    Attack,
    OnHit
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask mask;
    [SerializeField, ReadOnly] private GameObject sword;
    [SerializeField] private EnemySO enemySO;

    private bool hasDetect;
    private Transform targetTransform;
    private Animator animator;
    private NavMeshAgent agent;

    // Info
    [SerializeField] private int currentHealth;
    private int currentStamina;
    private int damage;
    public int Damage => damage;

    private StateEnemy currentStateEnemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        sword = GetComponentInChildren<EnemyHitController>().gameObject;
    }

    private void Start()
    {
        SwitchStateEnemy(StateEnemy.Normal);

        currentHealth = enemySO.MaxHealth;
        damage = enemySO.Damage;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * 1f, Color.green);
        EnemyStateMachine();

        if (currentHealth < 0)
        {
            transform.gameObject.SetActive(false);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward,out var hit, 1, mask))
    //    {
    //        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.red);
    //        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), hit.point);
    //    }
    //}

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
                    agent.SetDestination(targetTransform.position);

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
            case StateEnemy.OnHit:
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
                    agent.isStopped = true;
                    break;
                }
            case StateEnemy.Chasing:
                {
                    agent.isStopped = false;
                    break;
                }
            case StateEnemy.Attack:
                {
                    agent.isStopped = true;
                    animator.SetTrigger("Attack");
                    break;
                }
            case StateEnemy.OnHit:
                {
                    agent.isStopped = true;
                    animator.SetTrigger("OnHit");
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

    public void EnableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = false;
    }

    public void ReceviedDamage(int damage)
    {
        currentHealth -= damage;
    }
}
