using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    private Animator animator;
    private PlayerInput input;
    private int IS_WALKING;
    private int IS_RUNNING;
    private Vector2 currentMovement;
    private bool movementPressed;
    private bool runPressed;

    private void Awake()
    {
        input = new PlayerInput();
        input.CharacterControlls.Movement.performed += ctx => {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControlls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        IS_WALKING = Animator.StringToHash("isWalking");
        IS_RUNNING = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;
        transform.LookAt(positionToLookAt);
    }

    private void HandleMovement()
    {
        bool isRunning = animator.GetBool(IS_RUNNING);
        bool isWalking = animator.GetBool(IS_WALKING);

        if (movementPressed && !isWalking) {
            animator.SetBool(IS_WALKING, true);
        }
        if (!movementPressed && isWalking) {
            animator.SetBool(IS_WALKING, false);
        }
        if ((movementPressed && runPressed) && !isRunning) {
            animator.SetBool(IS_RUNNING, true);

        }
        if ((movementPressed || runPressed) && isRunning) {
            animator.SetBool(IS_RUNNING, false);
        }
    }

    private void OnEnable()
    {
        input.CharacterControlls.Enable();
    }

    private void OnDisable()
    {
        input.CharacterControlls.Disable();
    }
}
