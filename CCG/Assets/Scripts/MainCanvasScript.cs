using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainCanvasScript : MonoBehaviour
{
    public List<GameObject> cardObjects = new List<GameObject>();
    public List<GameObject> myTableCards = new List<GameObject>();
    public GameObject cardPrefab;
    private GameObject goTable;

    // Start is called before the first frame update
    void Awake()
    {
        goTable = GameObject.FindGameObjectWithTag("Table");
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void Init()
    {
        int totalCards = Random.Range(4, 6);
        for (int i = 0; i < totalCards; i++)
        {
            AddNewCard();
        }
    }

    async public void AddNewCard()
    {
        if (cardObjects.Count >= 8) return;
        GameObject newCard = Instantiate(cardPrefab);
        newCard.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        newCard.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 350);
        cardObjects.Add(newCard);
        GameObject imagePH = newCard.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        Texture2D tex = await GetRemoteTexture("https://picsum.photos/200/300");
        Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePH.GetComponent<Image>().sprite = fromTex;
        CardSource newCardCardSource = newCard.GetComponent<CardSource>();
        newCard.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 350);
        newCardCardSource.posSource = new Vector2(600, 350);
        newCardCardSource.posCurrent = new Vector2(600, 350);
        newCardCardSource.angleSource = -720f;
        newCardCardSource.sizeCurrent = 2f;
        newCardCardSource.sizeDestination = 1f;
        newCardCardSource.mana = Random.Range(1,9);
        newCardCardSource.health = Random.Range(1, 9);
        newCardCardSource.attack = Random.Range(1, 9);
        UpdateCards();
    }

    public void UpdateCards()
    {
        if (cardObjects.Count == 0) return;
        for (int index=0; index<cardObjects.Count;index++)
        {
            float handRadiusX = 800f;
            float handRadiusY = 400f;
            float verticalShift = -250f;
            float positionShift = 0f;
            cardObjects[index].transform.SetParent(GameObject.FindGameObjectWithTag("Reorder").transform, false);
            cardObjects[index].transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            if (cardObjects.Count > 1)
            {
                positionShift = (2f * index / (float)(cardObjects.Count - 1) - 1f); // float value from -1 to 1
            }
            float angularShift = positionShift * cardObjects.Count * 4; // -20deg to 20deg
            float x = handRadiusX * Mathf.Sin(angularShift * Mathf.PI / 180f);
            float y = handRadiusY * Mathf.Cos(angularShift * Mathf.PI / 180f);
            CardSource newCardCardSource = cardObjects[index].GetComponent<CardSource>();
            newCardCardSource.posDestination = new Vector2(x, y + verticalShift);
            newCardCardSource.angleDestination = -angularShift/2;
        }
    }

    public void UpdateTableCards()
    {
        if (myTableCards.Count == 0) return;
        for (int index = 0; index < myTableCards.Count; index++)
        {
            float positionShift = 0f;
            if (myTableCards.Count > 1)
            {
                positionShift = (float)index - 0.5f*(float)(myTableCards.Count-1);
            }
            CardSource tableCardSource = myTableCards[index].GetComponent<CardSource>();
            tableCardSource.posDestination = goTable.GetComponent<RectTransform>().anchoredPosition;
            tableCardSource.posDestination.x += positionShift * 130;
        }
    }

    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            switch (www.result)
            {
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
#if DEBUG
                    Debug.Log($"{www.error}, URL:{www.url}");
#endif
                    return null;
                default:
                    return DownloadHandlerTexture.GetContent(www);
            }
        }
    }

}
