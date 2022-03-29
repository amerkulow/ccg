using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableSlot : MonoBehaviour, IDropHandler
{
    private Canvas canvas;
    private MainCanvasScript mainCanvasScript;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CardSource cardSource = eventData.pointerDrag.GetComponent<CardSource>();
            mainCanvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvasScript>();
            cardSource.tag = "OnTable";
            if (cardSource.posCurrent.x< GetComponent<RectTransform>().anchoredPosition.x)
            {
                mainCanvasScript.myTableCards.Insert(0, eventData.pointerDrag);
            } else
            {
                mainCanvasScript.myTableCards.Add(eventData.pointerDrag);
            }
            cardSource.posDestination = GetComponent<RectTransform>().anchoredPosition;
            cardSource.angleDestination = 0;
            cardSource.sizeDestination = 1f;
            mainCanvasScript.UpdateTableCards();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetComponent<Canvas>();
        mainCanvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvasScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
