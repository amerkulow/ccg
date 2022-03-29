using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    private int currentCard = 0;
    private MainCanvasScript mainCanvasScript;
    private void Awake()
    {
        mainCanvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvasScript>();
    }

    public void ClickShuffleParams()
    {
        if (mainCanvasScript.cardObjects.Count == 0) return;
        if (currentCard >= mainCanvasScript.cardObjects.Count) currentCard = 0;
        GameObject currentCardObject = mainCanvasScript.cardObjects[currentCard];
        CardSource currentCardCS = currentCardObject.GetComponent<CardSource>();
        switch (Random.Range(0, 3))
        {
            case 0:
                currentCardCS.attack = Random.Range(-2, 10);
                break;
            case 1:
                currentCardCS.mana = Random.Range(-2, 10);
                break;
            case 2:
                currentCardCS.health = Random.Range(-2, 10);
                break;
        }
        currentCard++;
    }

    public void ClickTakeOneCard()
    {
        mainCanvasScript.AddNewCard();
    }

    public void ClickResetToDefault()
    {
        mainCanvasScript.cardObjects.ForEach(gameObject => Destroy(gameObject));
        mainCanvasScript.cardObjects.Clear();
        mainCanvasScript.myTableCards.ForEach(gameObject => Destroy(gameObject));
        mainCanvasScript.myTableCards.Clear();
        currentCard = 0;
        mainCanvasScript.Init();
    }
}
