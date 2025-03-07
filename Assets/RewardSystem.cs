using Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private Coin[] coins;
    private UIController uiController;
    private GhostScript ghostScript;

    // Start is called before the first frame update
    void Start()
    {
        ghostScript = FindObjectOfType<GhostScript>();
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        DataContainer._coins++;
        uiController.UpdateUI();
        if(DataContainer._coins >= coins.Length)
        {
            uiController.FinishGame();
            ghostScript.LockInput();
        }
    }
}
