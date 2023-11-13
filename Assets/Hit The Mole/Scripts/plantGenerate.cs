using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantGenerate : MonoBehaviour
{
    [SerializeField] private List<Sprite> plant;
    [SerializeField] private GameManager gameManager;

    public float timeGrow = 0f;
    public float timeAttack = 0f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    // Plant Parameters
    float timerGrown = 10.0f;
    float timerDeath = 7f;
    float timerAttack;
    private int growState = 0;
    private bool attack = false;
    private bool placed = false;
    private int plantIndex = 0;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = plant[0];
        spriteRenderer.enabled = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;
        timerAttack = Random.Range(5f, 15f);
    }

    private void Update()
    {
        if (placed && growState != 3)
        {
            timeGrow += Time.deltaTime;
            if (timeGrow >= timerGrown && growState != 3)
            {
                Grow();
                timeGrow = 0f;
            }
        }

        if (placed && !attack)
        {
            timeAttack += Time.deltaTime;
            if (timeAttack >= timerAttack)
            {
                StartCoroutine(Attack());
                attack = true;
                timeAttack = 0f;
            }
        }
    }

    public void PlacePlant()
    {
        placed = true;
        spriteRenderer.enabled = true;
    }

    private IEnumerator Attack()
    {
        Color beginColor = spriteRenderer.color;
        float deathTime = 0f;
        float colorTimer = 0.5f;
        float colorTime = 0f;
        while (deathTime < timerDeath)
        {
            deathTime += Time.deltaTime;
            colorTime += Time.deltaTime;
            if (colorTime >= colorTimer)
            {
                if (spriteRenderer.color == beginColor)
                    spriteRenderer.color = Color.red;
                else
                    spriteRenderer.color = beginColor;
                colorTime = 0f;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        attack = false;
        placed = false;
        growState = 0;
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = plant[0];
        spriteRenderer.enabled = false;
        timerAttack = Random.Range(5f, 15f);
        gameManager.SubtractScore(plantIndex, false);
    }

    private void Grow()
    {
        growState++;
        spriteRenderer.sprite = plant[growState];
    }

    //private IEnumerator GrownPlant(float setTime)
    //{
    //    while (grown)
    //    {
    //        Debug.Log(grown);
    //        float curTime1 = timerGrown;
    //        float curTime2 = setTime;
    //        while (curTime1 >= 0)
    //        {
    //            curTime1 -= Time.deltaTime;
    //        }
    //        Debug.Log(curTime1);
    //        yield return new WaitForSeconds(duration * 2);
    //        if (curTime1 <= 0)
    //        {
    //            attack = true;
    //        }
    //        Debug.Log(attack);
    //        while (attack && (curTime2 > 0))
    //        {
    //            plant.GetComponent<SpriteRenderer>().color = Color.black;
    //            curTime2 -= Time.deltaTime;
    //        }
    //        yield return new WaitForSeconds(duration * 2);
    //        Debug.Log(curTime2);
    //        if (attack && curTime2 <= 0)
    //        {
    //            Debug.Log("st");
    //            plant.SetActive(false);
    //            grown = false;
    //            attack = false;
    //            StopAllCoroutines();
    //            Activate(2);
    //            Debug.Log("fn");
    //        }
    //        else
    //        {
    //            GetComponent<SpriteRenderer>().color = Color.green;
    //            curTime2 = setTime;
    //            curTime1 = timerGrown;
    //            attack = false;
    //        }
    //        yield return null;
    //    }
    //}

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!gameManager.CheckIsMoleCurrent(plantIndex) && gameManager.score >= 150 && !placed)
            {
                gameManager.SubtractScore(plantIndex, true);
                StopAllCoroutines();
                PlacePlant();
                return;
            }

            if (placed && attack)
            {
                StopAllCoroutines();
                attack = false;
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void SetIndex(int index)
    {
        plantIndex = index;
    }

    public int GetGrowState()
    {
        return growState;
    }

    public bool GetPlacedState()
    {
        return placed;
    }

    public void ChangeBoxCollider2DState()
    {
        boxCollider2D.enabled = !boxCollider2D.enabled;
    }

    public void ChangeBoxCollider2DState(bool state)
    {
        boxCollider2D.enabled = state;
    }
}
