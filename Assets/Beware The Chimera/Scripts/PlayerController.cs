using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 respawnPoint;

    //Inputs
    public InputAction MoveAction;

    // Player Movement
    private Vector2 moveDirection = new Vector2(1, 0);
    private Vector2 move;
    private Rigidbody2D rigidbody2d;

    // Animator
    private Animator animator;

    // Player Speed
    public float maxSpeed;
    float currentSpeed;

    // Invincibility Frames
    public float timeInvulnerable = 2.0f;
    public bool isInvulnerable;
    float damageCooldown;

    // Player Health
    public int maxHealth = 100;
    public int health
    {
        get
        {
            return currentHealth;
        }
    }
    public int currentHealth;
    public FloatingHealthBar healthBar;


    [SerializeField] private bool isRunning;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetMaxHealth(maxHealth);
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

    public void Heal(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        isInvulnerable = true;
        damageCooldown = timeInvulnerable;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Respawn());
    }

    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPoint;
        GetComponent<Collider>().enabled = true;
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
    }
}