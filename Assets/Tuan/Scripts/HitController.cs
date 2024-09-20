using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyController>())
            {
                EnemyController enemyController = other.GetComponent<EnemyController>();
                enemyController.SwitchStateEnemy(StateEnemy.OnHit);
                enemyController.ReceviedDamage(playerController.Damage);
            }
        }
    }
}
