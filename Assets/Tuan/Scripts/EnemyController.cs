using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum StateEnemy
{
    Normal,
    Chasing,
    Attack,
    OnHit,
    OnDead
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask mask;
    [SerializeField, ReadOnly] private GameObject sword;
    [SerializeField] private EnemySO enemySO;

    private bool hasDetect;
    //privatet targetTransform;
    private Animator animator;
    private NavMeshAgent agent;

    private HealthSystem healthSystem;
    private StaminaSystem staminaSystem;
    private PlayerStatSystem playerStatSystem;
    [SerializeField]
    private Transform playerT;

    // Info
    private int damage;
    public int Damage => damage;

    private StateEnemy currentStateEnemy;

    private void OnEnable()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamageEnemy;
        healthSystem.OnDead += HealthSystem_OnDeadEnemy;
    }

    private void OnDisable()
    {
        healthSystem.OnDamaged -= HealthSystem_OnDamageEnemy;
        healthSystem.OnDead -= HealthSystem_OnDeadEnemy;
    }

    private void Awake()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

        healthSystem = new HealthSystem(enemySO.MaxHealth);
        playerStatSystem = GetComponent<PlayerStatSystem>();
        playerStatSystem.GetData(healthSystem, staminaSystem);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        sword = GetComponentInChildren<EnemyHitController>().gameObject;
    }

    private void Start()
    {
        SwitchStateEnemy(StateEnemy.Normal);

        damage = enemySO.Damage;
    }
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerT.position, transform.position);
        if (distance < 5)
        {
            if (Vector3.Dot(transform.forward.normalized, playerT.position - transform.position) > 0.7)
            {
                hasDetect = true;
            }
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * 1f, Color.green);
        EnemyStateMachine();
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
                    Vector3 newRotateDirection = playerT.position - transform.position;
                    Quaternion toRotation = Quaternion.LookRotation(newRotateDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed);
                    agent.SetDestination(playerT.position);

                    animator.SetBool("Chase", true);

                    if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, 1.0f, mask))
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
            case StateEnemy.OnDead:
                {
                    break;
                }
        }
    }

    public void SwitchStateEnemy(StateEnemy newStateEnemy)
    {
        currentStateEnemy = newStateEnemy;
        agent.SetDestination(transform.position);

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
            case StateEnemy.OnHit:
                {
                    animator.SetTrigger("OnHit");
                    break;
                }
            case StateEnemy.OnDead:
                {
                    animator.SetTrigger("OnDead");
                    this.gameObject.GetComponent<CharacterController>().enabled = false;
                    break;  
                }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        hasDetect = true;
    //        targetTransform = other.gameObject.transform;
    //    }
    //}

    public void EnableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;
    }
    public void DisableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = false;
    }

    
    // Health System
    private void HealthSystem_OnDamageEnemy(object sender, System.EventArgs e)
    {
        SwitchStateEnemy(StateEnemy.OnHit);
    }

    private void HealthSystem_OnDeadEnemy(object sender, System.EventArgs e)
    {
        SwitchStateEnemy(StateEnemy.OnDead);
    }
}
