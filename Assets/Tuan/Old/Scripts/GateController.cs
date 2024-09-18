using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private float health = 30;
    public float Health {  get { return health; } set {  health = value; } }
}
