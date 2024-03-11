using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    public InputAction MoveAction;
    private Rigidbody2D rigidbody2d;
    private Vector2 move;
    private Animator animator;
    private Vector2 moveDirection = new Vector2(1, 0);

    public float maxSpeed;
    float currentSpeed;

    public float timeInvulnerable = 2.0f;
    bool isInvulnerable;
    float damageCooldown;

    public int maxHealth = 100;
    public int health
    {
        get
        {
            return currentHealth;
        }
    }
    public int currentHealth;

    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvulnerable)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvulnerable = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * currentSpeed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth (int amount)
    {
        if (amount < 0)
        {
            if (isInvulnerable)
            {
                return;
            }
            isInvulnerable = true;
            damageCooldown = timeInvulnerable;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

        //Debug.Log(currentHealth + "/" + maxHealth);
        
    }

}
