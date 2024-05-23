using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyProjectileprefab;
    public Transform projectileSpawnPoint;
    public bool cooldownReached = true;
    public float cooldown;

    private void Update()
    {
        if (cooldownReached)
        {
            cooldownReached = false;
            var projectile = Instantiate(enemyProjectileprefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Invoke("changementCooldown", cooldown);
        }
    }

    public void changementCooldown()
    {
        cooldownReached = true;
    }
}
