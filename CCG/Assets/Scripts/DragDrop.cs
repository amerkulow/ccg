using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private MainCanvasScript mainCanvasScript;
    private CardSource cardSource;
    private Vector2 posDestination;
    private float angleDestination;
    public int storedOrderPosition;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetComponent<Canvas>();
        mainCanvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvasScript>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        cardSource = GetComponent<CardSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cardSource.tag == "OnTable") return;
        canvasGroup.blocksRaycasts = false;
        for (int i=0; i< mainCanvasScript.cardObjects.Count; i++)
        {
            if (GameObject.ReferenceEquals(mainCanvasScript.cardObjects[i], gameObject))
            {
                storedOrderPosition = i;
                mainCanvasScript.cardObjects.RemoveAt(i);
            }
        }
        posDestination = cardSource.posDestination;
        angleDestination = cardSource.angleDestination;
        cardSource.tag = "CardDragged";
        mainCanvasScript.UpdateCards();
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Reorder").transform, false);
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cardSource.tag == "OnTable") return;
        cardSource.posCurrent += eventData.delta / canvas.scaleFactor;
        cardSource.posDestination = cardSource.posCurrent;
        cardSource.sizeDestination = 2f;
        cardSource.angleCurrent = 0;
        cardSource.angleDestination = 0;
        rectTransform.anchoredPosition = cardSource.posCurrent;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardSource.tag == "OnTable") return;
        canvasGroup.blocksRaycasts = true;
        cardSource.posDestination = posDestination;
        cardSource.angleDestination = angleDestination;
        mainCanvasScript.cardObjects.Insert(storedOrderPosition, gameObject);
        cardSource.tag = "MyHand";
        cardSource.sizeDestination = 1f;
        mainCanvasScript.UpdateCards();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
