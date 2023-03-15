using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private TMP_Text playerPointsText;
    [SerializeField] private PlayerPoints playerPoints;

    [SerializeField] private GameObject end;
    [SerializeField] private TMP_Text endPoints;
    
    void Update()
    {
        playerPointsText.text = playerPoints.Points.ToString();
    }

    public void ShowEnd()
    {
        endPoints.text = playerPoints.Points.ToString();
        end.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
