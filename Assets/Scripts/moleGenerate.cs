using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class moleGenerate : MonoBehaviour
{
    [SerializeField] private Sprite mole;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject plant;

    public BoxCollider2D plantBox;


    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    
    public float showDuration = 0.5f;
    public float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    // Mole Parameters 
    private bool hittable = true;
    public enum MoleType { Standard };
    private MoleType moleType;
    private int lives;
    private int moleIndex = 0;



    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        plantBox.enabled = false;
        transform.localPosition = start;

        
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;        
        
        plantBox.enabled = true;
        yield return new WaitForSeconds(duration);

        if (hittable)
        {
            hittable = false;
            gameManager.Missed(moleIndex);
        }
    }

    float timerGrown = 180.0f;
    public bool attack = false;
    public bool grown;
    public void placePlant()
    {
        StopAllCoroutines();
        grown = true;

        plant.SetActive(true);
        float setTime = 180.0f;
        StartCoroutine(grownPlant(setTime));

    }

    private IEnumerator grownPlant(float setTime)
    {
        while(grown)
        {
            Debug.Log(grown);
            float curTime1 = timerGrown;
            float curTime2 = setTime;
            while (curTime1 >= 0)
            {
                curTime1 -= Time.deltaTime;
            }
            Debug.Log(curTime1);
            yield return new WaitForSeconds(duration * 2);
            if (curTime1 <= 0)
            {
                attack = true;
            }
            Debug.Log(attack);
            while (attack && (curTime2 > 0))
            {
                plant.GetComponent<SpriteRenderer>().color = Color.black;                
                curTime2 -= Time.deltaTime;
            }
            yield return new WaitForSeconds(duration * 2);
            Debug.Log(curTime2);
            if (attack && curTime2 <= 0)
            {
                Debug.Log("st");
                plant.SetActive(false);
                grown = false;
                attack= false;
                StopAllCoroutines();
                Activate(2);
                Debug.Log("fn");
            }
            else
            {
                plant.GetComponent<SpriteRenderer>().color = Color.green;
                curTime2 = setTime;
                curTime1 = timerGrown;
                attack = false;
            }
            yield return null;
        }
        

        
    }

    public void Hide()
    {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);
        if (!hittable)
        {
            Hide();
        }
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            if (hittable)
            {
                gameManager.AddScore(moleIndex);
                StopAllCoroutines();
                StartCoroutine(QuickHide());
                hittable = false;
            }
        }

    }
    

    private void CreateNext()
    {
        moleType = MoleType.Standard;
        spriteRenderer.sprite = mole;
        lives = 1;        
        hittable = true;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);
    }

    public void Activate(int level)
    {
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    public void SetIndex(int index)
    {
        moleIndex = index;
    }
}
