using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    [SerializeField] private SodierSO sodierSO;

    private void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            other.GetComponent<GateController>().Health -= sodierSO.damge;
        }
    }
}
