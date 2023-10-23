using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantGenerate : MonoBehaviour
{
    [SerializeField] private Sprite plant;
    [SerializeField] private GameManager gameManager;

    public float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    // Plant Parameters
    float timerGrown = 180.0f;
    private bool attack = false;
    private bool grown = false;
    private int plantIndex = 0;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;
    }

    public void PlacePlant()
    {
        StopAllCoroutines();
        grown = true;
        spriteRenderer.enabled = true;
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
            if (!gameManager.CheckIsMoleCurrent(plantIndex) && gameManager.score >= 150 && !grown)
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

    public bool GetGrownState()
    {
        return grown;
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
