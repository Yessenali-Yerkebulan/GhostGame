using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text deaths;
    [SerializeField] private GameObject winnerUI;
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        coins.text = DataContainer._coins.ToString();
        deaths.text = DataContainer._deaths.ToString();
    }

    public void FinishGame()
    {
        winnerUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
