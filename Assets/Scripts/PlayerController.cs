using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;

    public int maxHealth = 5;
    int currentHealth;

    public float maxSpeed = 3.0f;
    float currentSpeed;

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
        Debug.Log(move);
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * currentSpeed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    void changeHealth (int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

}
