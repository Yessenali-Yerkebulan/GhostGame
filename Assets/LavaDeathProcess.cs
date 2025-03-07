using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample;

public class LavaDeathProcess : MonoBehaviour
{
    private GhostScript ghostScript;

    private void OnTriggerEnter(Collider other)
    {
        if(ghostScript = other.GetComponent<GhostScript>())
        {
            ghostScript.Damage();
        }
    }
}
