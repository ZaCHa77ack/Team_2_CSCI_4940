using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileTransform;
    private PlayerController player;
    public float fireRate;
    private float timer;
    private bool canFire;

    private void Update()
    {
        player = GetComponent<PlayerController>();

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        }

        else if (player.currentHealth <= 0)
        {
            canFire = false;
            
        }
    }
}
