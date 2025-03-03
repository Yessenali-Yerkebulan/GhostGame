using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float coinRotationSpeed = 200f;
    private void Update()
    {
        transform.Rotate(coinRotationSpeed * Time.deltaTime, 0, 0);
    }
}
