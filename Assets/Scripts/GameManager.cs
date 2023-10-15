using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
  [SerializeField] public List<moleGenerate> moles;

  [SerializeField] private TMPro.TextMeshProUGUI scoreText;

  private HashSet<moleGenerate> currentMoles = new HashSet<moleGenerate>();
  public int score;
  private bool playing = false;


  public void StartGame() 
  {
    for (int i = 0; i < moles.Count; i++) 
    {
      moles[i].Hide();
      moles[i].SetIndex(i);
    }
    
    currentMoles.Clear();
    score = 0;
    scoreText.text = "0";
    playing = true;
  }



  void Update() 
 {
    if (playing) 
    {      
      if (currentMoles.Count <= moles.Count/2) 
      {     
        int index = Random.Range(0, moles.Count);
        
        if (!currentMoles.Contains(moles[index])) // && !moles[index].grown) 
        {
          currentMoles.Add(moles[index]);
          moles[index].Activate(score / 100);
        }
      }
    }
  }

  public void AddScore(int moleIndex) 
  {
    score += 100;
    scoreText.text = $"{score}";
    currentMoles.Remove(moles[moleIndex]);
  }

  public void Missed(int moleIndex) 
  {
    currentMoles.Remove(moles[moleIndex]);
  }
}
