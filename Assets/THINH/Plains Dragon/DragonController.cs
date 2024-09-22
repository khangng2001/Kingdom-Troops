using UnityEngine;
using UnityEngine.Playables;

public enum DragonState
{
    Ground,
    Die
}
public enum DragonGroundState
{
    RunToPoint,
    Ready,
    Fire,
    Rest
}
public enum DragonDieState
{
    Ready,
    Die
}

public class DragonController : MonoBehaviour
{
    private Animator _animator;

    public Transform Player;
    public DragonFire DragonFire;

    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedRotateHead;

    [SerializeField] private DragonState _state;

    [SerializeField] private DragonGroundState _groundState;
    public float timeRunningRest;
    public float timeMaxRest;

    public float MaxHealth;
    public float Health;

    public GameObject RangeActiveEnd;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        DragonFire = GetComponent<DragonFire>();

        ChangeState(DragonState.Ground);

        Health = MaxHealth;
    }

    void Update()
    {
        // Look Player
        {
            Vector3 directionToPlayer = new Vector3(Player.position.x, 0, Player.position.z) - new Vector3(DragonFire.Head.transform.position.x, 0, DragonFire.Head.transform.position.z);
            if (Vector3.Dot(-directionToPlayer, DragonFire.Head.transform.right) > 0)
            {
                DragonFire.Head.transform.rotation = Quaternion.LookRotation(Quaternion.Euler(0, -90, 0) * directionToPlayer, -Vector3.up);
            }
        }

        if (Health <= -0)
        {
            ChangeState(DragonState.Die);
        }

        switch (_state)
        {
            case DragonState.Ground:
                GroundState();
                break;
            case DragonState.Die:
                break;  
        }
    }

    void ChangeState(DragonState state)
    {
        // Last Time
        switch (_state)
        {
            case DragonState.Ground:
                _animator.SetBool("IsReady", false);
                _animator.SetBool("IsAttack", false);
                _animator.SetBool("IsRest", false);
                break;
            case DragonState.Die:
                break;
        }

        _state = state;
        _animator.SetInteger("State", (int)_state);

        // First Time
        switch (_state)
        {
            case DragonState.Ground:
                _animator.SetBool("IsReady", false);
                _animator.SetBool("IsAttack", false);
                _animator.SetBool("IsRest", false);
                break;
            case DragonState.Die:
                RangeActiveEnd.SetActive(true);
                this.enabled = false;
                break;
        }
    }

    void GroundState()
    {
        switch (_groundState)
        {
            case DragonGroundState.RunToPoint:
                float distanceFromPoint = Vector3.Distance(transform.position, DragonFire.UseSkillFirePoint[DragonFire.IndexPoint].position);
                if (distanceFromPoint <= 0.2f)
                {
                    DragonFire.IndexPoint += 1;
                    if (DragonFire.IndexPoint == DragonFire.UseSkillFirePoint.Count)
                    {
                        DragonFire.IndexPoint = 0;
                    }

                    DragonFire.FireCount = 0;
                    _animator.SetBool("IsReady", true);
                    _groundState = DragonGroundState.Ready;
                }

                Vector3 directionToPoint = DragonFire.UseSkillFirePoint[DragonFire.IndexPoint].position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPoint), Time.deltaTime * _speedRotate);
                transform.position += transform.forward * Time.deltaTime * _speed;

                break;
            case DragonGroundState.Ready:
                {
                    Vector3 directionToPlayer = Player.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * _speedRotate);

                    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(directionToPlayer)) == 0)
                    {
                        _animator.SetBool("IsReady", false);
                        _animator.SetBool("IsAttack", true);
                        _groundState = DragonGroundState.Fire;
                    }
                }
                break;
            case DragonGroundState.Fire:
                if (DragonFire.FireCount >= DragonFire.FireCountMax)
                {
                    timeRunningRest = timeMaxRest;
                    _animator.SetBool("IsAttack", false);
                    _animator.SetBool("IsRest", true);
                    _groundState = DragonGroundState.Rest;
                }
                break;
            case DragonGroundState.Rest:
                timeRunningRest -= Time.deltaTime;
                if (timeRunningRest <= 0)
                {
                    _animator.SetBool("IsRest", false);
                    _groundState = DragonGroundState.RunToPoint;
                }
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0) 
        {
            Health = 0;
            ChangeState(DragonState.Die);
        }
    }
}
