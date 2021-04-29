using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Michael Jordan
/// </summary>
public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
