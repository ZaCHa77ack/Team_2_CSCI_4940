using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical = true;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;

    // Start is called before the first frame update

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        vertical = Random.Range(0, 2) == 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;

            vertical = !vertical;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
        }
       
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        direction = -direction;
        vertical = !vertical;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(20);
        }
    }
}
