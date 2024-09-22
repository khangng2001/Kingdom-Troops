using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitController : MonoBehaviour
{
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IGetHealthSystem getHealthSystem = other.GetComponent<IGetHealthSystem>();
        if (getHealthSystem != null)
        {
            if (other.GetComponent<EnemyController>() == enemyController)
            {
                return;
            }
            getHealthSystem.TakeDamage(enemyController.Damage);
        }
    }
}
