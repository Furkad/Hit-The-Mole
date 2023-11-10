using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantGenerate : MonoBehaviour
{
    [SerializeField] private List<Sprite> plant;
    [SerializeField] private GameManager gameManager;

    public float timer = 0f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    // Plant Parameters
    float timerGrown = 10.0f;
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
    }

    private void Update()
    {
        if (placed && growState != 3)
        {
            timer += Time.deltaTime;
            if (timer >= timerGrown && growState != 3)
            {
                Grow();
                timer = 0f;
            }
        }
    }

    public void PlacePlant()
    {
        StopAllCoroutines();
        placed = true;
        spriteRenderer.enabled = true;
    }

    public void Attack()
    {

    }

    public void Grow()
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
                gameManager.SubtractScore(plantIndex);
                StopAllCoroutines();
                PlacePlant();
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

    public void ChangeBoxCollider2DState()
    {
        boxCollider2D.enabled = !boxCollider2D.enabled;
    }

    public void ChangeBoxCollider2DState(bool state)
    {
        boxCollider2D.enabled = state;
    }
}
