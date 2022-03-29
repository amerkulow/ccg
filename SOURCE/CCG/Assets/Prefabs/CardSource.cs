using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSource : MonoBehaviour
{
    public Transform cardTransform;

    //parameters
    public string Text;
    public int mana;
    public int health;
    public int attack;
    public int frame=-1;

    //positioning
    private RectTransform rectTransform;
    private MainCanvasScript canvasGenCard;
    public Vector2 posSource;
    public Vector2 posDestination;
    public Vector2 posCurrent;
    public Vector2 acc;
    public Vector2 vel;
    public float angleSource;
    public float angleDestination;
    public float angleCurrent;
    public float accAngle;
    public float velAngle;
    public float sizeDestination;
    public float sizeCurrent;
    public float accSize;
    public float velSize;
    public float alpha;

    //text objects
    private Text goMana;
    private Text goHealth;
    private Text goAttack;
    private Image imageHL;

    // Start is called before the first frame update
    void Awake()
    {
        canvasGenCard = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvasScript>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        goMana = transform.Find("Mana").gameObject.GetComponent<Text>();
        goHealth = transform.Find("Health").gameObject.GetComponent<Text>();
        goAttack = transform.Find("Attack").gameObject.GetComponent<Text>();
        imageHL = transform.Find("FrontCoverHL").gameObject.GetComponent<Image>();
        goMana.text = mana.ToString();
        goHealth.text = health.ToString();
        goAttack.text = attack.ToString();
        angleCurrent = transform.rotation.eulerAngles.z;
        sizeCurrent = 1;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (frame > 0.1f/Time.deltaTime) //every 100ms
        {
            frame = 0;

            if (goMana.text != mana.ToString())
            {
                int oldValue = System.Convert.ToInt32(goMana.text);
                goMana.text = (oldValue + Mathf.Sign(mana - oldValue)).ToString();
            }
            if (goHealth.text != health.ToString())
            {
                int oldValue = System.Convert.ToInt32(goHealth.text);
                goHealth.text = (oldValue + Mathf.Sign(health - oldValue)).ToString();
            }
            if (goAttack.text != attack.ToString())
            {
                int oldValue = System.Convert.ToInt32(goAttack.text);
                goAttack.text = (oldValue + Mathf.Sign(attack - oldValue)).ToString();
            }
        }
        if (int.Parse(goHealth.text)<1)
        {
            for (int i = 0; i < canvasGenCard.cardObjects.Count; i++)
            {
                if (GameObject.ReferenceEquals(canvasGenCard.cardObjects[i], gameObject))
                {
                    canvasGenCard.cardObjects.RemoveAt(i);
                    break;
                }
            }
            gameObject.tag = "ToRemove";
            canvasGenCard.UpdateCards();
            posDestination = new Vector2(posDestination.x, -200f);
        }
    }

    private void FixedUpdate()
    {
        posCurrent = rectTransform.anchoredPosition;
        Vector2 delta = posDestination - posCurrent;
        if (delta.magnitude < 5)
        {
            posCurrent = posDestination;
            acc = Vector2.zero;
            vel = Vector2.zero;
            if (gameObject.tag == "ToRemove")
            {
                Destroy(gameObject);
            }
        }
        else
        {
            acc = delta.normalized * 15000f;
            vel += acc * Time.fixedDeltaTime;
            vel *= 0.8f; // damping decriment
            posCurrent = posCurrent + vel * Time.fixedDeltaTime;
        }
        rectTransform.anchoredPosition = posCurrent;

        float deltaAngle = angleDestination - angleCurrent;
        if (Mathf.Abs(deltaAngle) < 1)
        {
            angleCurrent = angleDestination;
            accAngle = 0;
            velAngle = 0;
            transform.rotation = Quaternion.Euler(0, 0, angleCurrent);
        }
        else
        {
            accAngle = Mathf.Sign(deltaAngle)*10f;
            velAngle += accAngle * Time.fixedDeltaTime;
            velAngle *= 0.82f; // angular damping decriment
            angleCurrent = angleCurrent + velAngle;
            transform.rotation = Quaternion.Euler(0, 0, angleCurrent);
        }

        float deltaSize = sizeDestination - sizeCurrent;
        if (Mathf.Abs(deltaSize) < 0.05)
        {
            sizeCurrent = sizeDestination;
            accSize = 0;
            velSize = 0;
            rectTransform.localScale = new Vector2(sizeCurrent, sizeCurrent);
            alpha = Mathf.Max(0f, Mathf.Min(1f, (sizeCurrent - 1f)));
            imageHL.color = new Color(255, 255, 255, alpha);
        }
        else
        {
            if (sizeDestination <= 1f)
            {
                accSize = Mathf.Sign(deltaSize) * 1.5f;
            } else
            {
                accSize = Mathf.Sign(deltaSize) * 5f;
            }
            velSize+= accSize * Time.fixedDeltaTime;
            velSize *= 0.8f; // size damping decriment
            sizeCurrent = sizeCurrent + velSize;
            rectTransform.localScale = new Vector2(sizeCurrent, sizeCurrent);
            alpha = Mathf.Max(0f,Mathf.Min(1f, (sizeCurrent-1f)));
            imageHL.color = new Color(255, 255, 255, alpha);
        }
    }
}