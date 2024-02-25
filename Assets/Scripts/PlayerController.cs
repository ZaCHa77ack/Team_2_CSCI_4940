using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float maxSpeed = 3.0f;
    float currentSpeed;

    public float timeInvulnerable = 3.0f;
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

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector2 move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
        Vector2 position = (Vector2)transform.position + move * 3.0f * Time.deltaTime;
        transform.position = position;
        */
        move = MoveAction.ReadValue<Vector2>();
        //Debug.Log(move);

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
