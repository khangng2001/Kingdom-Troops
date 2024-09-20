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
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerController>())
            {   
                PlayerController playerController = other.GetComponent<PlayerController>();
                playerController.SwitchStateAnim(StateAnim.OnHit);
                playerController.ReceiveDamage(enemyController.Damage);
            }
        }
    }
}
