using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantMan : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private moleGenerate molgen;
    private void OnMouseDown()
    {

        if (Input.GetMouseButtonDown(0))
        {
                if (gameManager.score > 150)
                {
                    gameManager.score -= 150;
                    //molgen.placePlant();
                }
            
        }       

    }
}
