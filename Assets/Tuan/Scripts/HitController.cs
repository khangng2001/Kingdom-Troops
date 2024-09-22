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
        IGetHealthSystem getHealthSystem = other.GetComponent<IGetHealthSystem>();
        if (getHealthSystem != null)
        {
            if (other.GetComponent<PlayerController>())
            {
                return;
            }
            getHealthSystem.TakeDamage(playerController.Damage);
        }
    }
}
