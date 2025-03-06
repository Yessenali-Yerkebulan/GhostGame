using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float coinRotationSpeed = 200f;

    private RewardSystem _rewardSystem;

    private float _coinRotateOffset = 50;

    private void Start()
    {
        _rewardSystem = GetComponentInParent<RewardSystem>();
        coinRotationSpeed = Random.Range(coinRotationSpeed - _coinRotateOffset, coinRotationSpeed + _coinRotateOffset);
    }

    private void Update()
    {
        transform.Rotate(coinRotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        _rewardSystem.AddCoin();
        Destroy(gameObject);
    }
}
