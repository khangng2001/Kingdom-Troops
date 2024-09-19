using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateAnim
{
    Normal,
    Slash,
    Slash2,
    Slashing,
    Slashing2
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private AnimationCurve curve;

    private InputGameAction inputActions;
    private Animator animator;
    private StateAnim currentState;

    private Vector2 inputPlayer;
    private Vector3 movement;
    private bool shiftRun;
    private bool slash;
    private float currentSpeed;

    private void OnEnable()
    {
        this.inputActions.Enable();
    }

    private void OnDisable()
    {
        this.inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new InputGameAction();
        animator = GetComponent<Animator>();
        currentState = StateAnim.Normal;
    }

    private void Update()
    {
        SetInput();
        movement = new Vector3(inputPlayer.x, 0f, inputPlayer.y);
        movement.Normalize();

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
                    if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9f)
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
                    break;
                }
            case StateAnim.Slash2:
                {
                    if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9f)
                    {
                        animator.SetTrigger("Slash2");
                    }
                    break; 
                }
            case StateAnim.Slashing2:
                {
                    break;
                }

        }
    }

    private void SetInput()
    {
        inputPlayer = inputActions.Character.Move.ReadValue<Vector2>();
        shiftRun = inputActions.Character.Run.IsPressed();
        if (Input.GetMouseButtonDown(0))
        {
            slash = true;
        }
        else
        {
            slash = false;
        }
        //slash = inputActions.Character.Slash.IsPressed();
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

}
