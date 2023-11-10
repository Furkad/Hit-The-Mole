using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
  [SerializeField] public List<moleGenerate> moles;
  [SerializeField] public List<plantGenerate> plants;

  [SerializeField] private TMPro.TextMeshProUGUI scoreText;

  private HashSet<moleGenerate> currentMoles = new HashSet<moleGenerate>();
  private HashSet<plantGenerate> currentPlants = new HashSet<plantGenerate>();
  public int score;
  private bool playing = false;

  private float timer = 7f;
  private float currentTime = 0f;

  public void StartGame() 
  {
    for (int i = 0; i < moles.Count; i++) 
    {
      moles[i].Hide();
      moles[i].SetIndex(i);
      plants[i].SetIndex(i);
    }
    
    currentMoles.Clear();
    currentPlants.Clear();
    score = 0;
    scoreText.text = "0";
    playing = true;
  }



  void Update() 
 {
    if (playing) 
    {
      bool isWon = true;
      foreach (plantGenerate plant in plants)
      {
                if (plant.GetGrowState() != 3)
                {
                    isWon = false;
                    break;
                }
      }
            if (isWon)
                return;
      currentTime += Time.deltaTime;
      //if (currentMoles.Count <= moles.Count/2)
      if (currentTime >= timer)
      {
        timer = Random.Range(0, 3f);
        currentTime = 0f;
        int index = Random.Range(0, moles.Count);
        while (currentPlants.Contains(plants[index]))
        {
            index = Random.Range(0, moles.Count);
        }    
        
        if (!currentMoles.Contains(moles[index]) && !currentPlants.Contains(plants[index])) 
        {
          currentMoles.Add(moles[index]);
          moles[index].Activate(score / 100);
          plants[index].ChangeBoxCollider2DState(false);
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

  public void SubtractScore(int plantIndex)
  {
        score -= 150;
        scoreText.text = $"{score}";
        currentPlants.Add(plants[plantIndex]);
        currentMoles.Remove(moles[plantIndex]);
  }

  public void Missed(int moleIndex) 
  {
    currentMoles.Remove(moles[moleIndex]);
  }

  public bool CheckIsMoleCurrent(int plantIndex)
  {
        return currentMoles.Contains(moles[plantIndex]);
  }
}
