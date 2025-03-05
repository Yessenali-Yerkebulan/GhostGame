using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text deaths;
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        coins.text = DataContainer._coins.ToString();
        deaths.text = DataContainer._deaths.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
