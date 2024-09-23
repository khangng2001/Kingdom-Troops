using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum StateAnim
{
    Normal,
    Slash,
    Slash2,
    Slashing,
    Slashing2,
    OnHit,
    OnDead,
    BlockMove
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private GameObject sword;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Animation youDie;

    private InputGameAction inputActions;
    private Animator animator;
    private StateAnim currentState;

    private Vector2 inputPlayer;
    private Vector3 movement;
    private bool shiftRun;
    private bool slash;
    private float currentSpeed;
    private bool hasMove;

    private HealthSystem healthSystem;
    private StaminaSystem staminaSystem;
    private PlayerStatSystem playerStatSystem;

    // Info
    private int damage;
    public int Damage => damage;

    private void OnEnable()
    {
        this.inputActions.Enable();
        healthSystem.OnDamaged += HealthSystem_OnDamagePlayer;
        healthSystem.OnDead += HealthSystem_OnDeadPlayer;
    }

    private void OnDisable()
    {
        this.inputActions.Disable();
        healthSystem.OnDamaged -= HealthSystem_OnDamagePlayer;
        healthSystem.OnDead -= HealthSystem_OnDeadPlayer;
    }

    private void Awake()
    {
        healthSystem = new HealthSystem(playerSO.MaxHealth);
        staminaSystem = new StaminaSystem(playerSO.MaxStamina);
        playerStatSystem = GetComponent<PlayerStatSystem>();
        playerStatSystem.GetData(healthSystem, staminaSystem);

        inputActions = new InputGameAction();
        animator = GetComponent<Animator>();
        SwitchStateAnim(StateAnim.Normal);
        sword = GetComponentInChildren<HitController>().gameObject;
    }

    private void Start()
    {
        //currentHealth = playerSO.MaxHealth;
        damage = playerSO.Damage;

        CanMove(true);

        //StartCoroutine(StaminaRecovering());
    }

	private void Update()
    {
        SetInput();
        movement = new Vector3(inputPlayer.x, 0f, inputPlayer.y);
        movement.Normalize();

        //if (!hasMove)
        //{
        //    SwitchStateAnim(StateAnim.BlockMove);
        //}


        StateMachine();
    }

    private void StateMachine()
    {
        switch (currentState)
        {
            case StateAnim.Normal:
                {
                    Movement();

                    if (slash)
                    {
                        SwitchStateAnim(StateAnim.Slash);
                    }
                    break;
                }
            case StateAnim.Slash:
                {
                    SwitchStateAnim(StateAnim.Slashing);
                    break;
                }
            case StateAnim.Slashing:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
                    {
                        if (slash)
                        {
                            SwitchStateAnim(StateAnim.Slash2);
                        }
                    }
                    break;
                }
            case StateAnim.Slash2:
                {
                    SwitchStateAnim(StateAnim.Slashing2);
                    break;
                }
            case StateAnim.Slashing2:
                {
                    break;
                }
            case StateAnim.OnHit:
                {
                    break;
                }
            case StateAnim.OnDead:
                {
                    break;  
                }
            case StateAnim.BlockMove:
                {
                    break;
                }
        }
    }

    public void SwitchStateAnim(StateAnim newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case StateAnim.Normal:
                {
                    break;
                }
            case StateAnim.Slash:
                {
                    animator.SetTrigger("Slash");
                    break;
                }
            case StateAnim.Slashing:
                {
					//playerStatSystem.UseStamina(10);
					break;
                }
            case StateAnim.Slash2:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
                    {
                        animator.SetTrigger("Slash2");
                    }
                    break;
                }
            case StateAnim.Slashing2:
                {
					//playerStatSystem.UseStamina(10);
					break;
                }
            case StateAnim.OnHit:
                {
                    animator.SetTrigger("OnHit");
                    break;
                }
            case StateAnim.OnDead:
                {
                    animator.SetTrigger("OnDead");
                    transform.GetComponent<CharacterController>().enabled = false;
					break;  
                }
            case StateAnim.BlockMove:
                {
                    animator.SetFloat("Speed", curve.Evaluate(0));
                    break;
                }
        }
    }

    public void CanMove(bool can)
    {
        hasMove = can;

        if (!hasMove)
        {
            SwitchStateAnim(StateAnim.BlockMove);
        }
        else
        {
            SwitchStateAnim(StateAnim.Normal);
        }
    }

    private void SetInput()
    {
        inputPlayer = inputActions.Character.Move.ReadValue<Vector2>();
        shiftRun = inputActions.Character.Run.IsPressed();
        slash = inputActions.Character.Slash.triggered;
    }

    private void Movement()
    {
        //float move = 0;
        float valuesCurve = movement.magnitude;

        if (movement.magnitude > 0f)
        {
            Vector3 newRotateDirection = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * movement;
            Quaternion toRotation = Quaternion.LookRotation(newRotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed);
            //move = speed;

            if (shiftRun)
            {
                //move = run;
                valuesCurve = 2;
            }
        }

        currentSpeed = Mathf.Lerp(currentSpeed, valuesCurve, Time.deltaTime * 2.5f);
        //Debug.Log(currentSpeed);

        animator.SetFloat("Speed", curve.Evaluate(currentSpeed));
    }

    //private IEnumerator StaminaRecovering()
    //{
    //    yield return new WaitForSeconds(1);
    //    playerStatSystem.Recovering(5);
    //}


    public void EnableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;
    }

    public void DisableCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = false;
    }

    public void ShowYouDie()
    {
		youDie.gameObject.SetActive(true);
		youDie.Play();

        //Load To MainScene
        StartCoroutine(GameSceneLoading.Instance.LoadChildGame(StringConstants.MAIN, () =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }));
    }

    // Health System
    private void HealthSystem_OnDamagePlayer(object sender, System.EventArgs e)
    {
        SwitchStateAnim(StateAnim.OnHit);
    }

    private void HealthSystem_OnDeadPlayer(object sender, System.EventArgs e)
    {
        SwitchStateAnim(StateAnim.OnDead);
    }
}
