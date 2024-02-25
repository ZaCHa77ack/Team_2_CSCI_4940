using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Object that entered the trigger: " + other);
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(20);
            Destroy(gameObject);
        }
    }
}
