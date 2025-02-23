using Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform ghostTransform;
    [SerializeField] private float SmoothSpeed = 0.03f;
    void Start()
    {
        ghostTransform = FindObjectOfType<GhostScript>().transform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(ghostTransform.position.x, transform.position.y, ghostTransform.position.z), SmoothSpeed);
    }
}
