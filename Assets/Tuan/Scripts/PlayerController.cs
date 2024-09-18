using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float run;

    private InputGameAction inputActions;
    private Animator animator;

    [SerializeField] private Vector2 inputPlayer;
    [SerializeField] private Vector3 movement;

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
    }

    private void Update()
    {
        inputPlayer = inputActions.Character.Move.ReadValue<Vector2>();
        movement = new Vector3(inputPlayer.x, 0f, inputPlayer.y);
        movement.Normalize();

        float move = 0;

        if (movement.magnitude > 0f)
        {
            Vector3 newRotateDirection = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * movement;
            Quaternion toRotation = Quaternion.LookRotation(newRotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed);
            move = speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                //speed += 5;
                move = run;
            }
            //else
            //{
            //    animator.SetBool("Run", true);
            //}

            //transform.Translate(Vector3.forward * Time.deltaTime * move);
        }

        animator.SetFloat("Speed", movement.magnitude);
        animator.SetFloat("Run", move);
        //animator.SetBool("Run", false);
    }
}
