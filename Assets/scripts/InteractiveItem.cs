using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    [SerializeField] int id;
    void OnTriggerEnter(Collider other)
    {
        PlayerController pc;
        // null -> bool => false
        // not null -> bool => true
        if (pc = other.GetComponent<PlayerController>())
        {
            pc.ChangeMats(id, 1);
            
            Destroy(gameObject);
        }
    }
}
