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
    [SerializeField] private GameObject hammer;


    public Vector2 startPosition;
    public Vector2 endPosition;
    
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

        gameManager.plants[moleIndex].ChangeBoxCollider2DState();
        yield return new WaitForSeconds(duration);

        if (hittable)
        {
            hittable = false;
            gameManager.Missed(moleIndex);
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
        hammer.GetComponent<SpriteRenderer>().enabled = true;

        float elapsed = 0f;
        Quaternion start = hammer.transform.rotation;
        Quaternion end = Quaternion.AngleAxis(60f, new Vector3(0f, 0f, 1f));
        while (elapsed < showDuration)
        {
            hammer.transform.rotation = Quaternion.Lerp(start, end, elapsed / showDuration - 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        hammer.GetComponent<SpriteRenderer>().enabled = false;
        hammer.transform.rotation = start;
        //yield return new WaitForSeconds(0.25f);

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
                gameManager.plants[moleIndex].ChangeBoxCollider2DState();
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
        hammer.GetComponent <SpriteRenderer>().enabled = false;
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

    public bool GetHittableState()
    {
        return hittable;
    }
}
