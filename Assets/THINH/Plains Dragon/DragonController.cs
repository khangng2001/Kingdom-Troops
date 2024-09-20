using System.Collections.Generic;
using UnityEngine;

public enum DragonState
{
    RunToPoint,
    Ready,
    Fire,
}

public class DragonController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private Transform player;

    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotate;

    [SerializeField] private DragonState _state;

    [SerializeField] private List<Transform> FirePoint = new List<Transform>();
    [SerializeField] private int indexPoint = 0;
    public int FireCount = 0;
    public int FireCountMax = 3;
    public GameObject FireBall;
    public Transform FireBallPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _distance = Vector3.Distance(transform.position, player.position);


        switch (_state)
        {
            case DragonState.RunToPoint:
                float distanceFromPoint = Vector3.Distance(transform.position, FirePoint[indexPoint].position);
                if (distanceFromPoint <= 0.2f)
                {
                    indexPoint += 1;
                    if (indexPoint == FirePoint.Count)
                    {
                        indexPoint = 0;
                    }

                    FireCount = 0;
                    _animator.SetBool("IsAttack", true);
                    _state = DragonState.Fire;
                }

                Vector3 directionToPoint = FirePoint[indexPoint].position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPoint), Time.deltaTime * _speedRotate);
                transform.position += transform.forward * Time.deltaTime * _speed;

                break;
            case DragonState.Ready:
                break;
            case DragonState.Fire:
                Vector3 directionToPlayer = player.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * _speedRotate);

                if (FireCount >= FireCountMax)
                {
                    _animator.SetBool("IsAttack", false);
                    _state = DragonState.RunToPoint;
                }
                break;
        }
    }

    public void Fire()
    {
        Vector3 directionToPlayer = player.position - FireBallPoint.position;
        GameObject fireBall = Instantiate(FireBall, FireBallPoint.position, Quaternion.LookRotation(directionToPlayer));
    }
}
