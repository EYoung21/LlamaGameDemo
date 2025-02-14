using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text uiText;

    private static UIManager _instance;

    public void Start()
    {
        _instance = this;
    }

    public static void UpdateScore()
    {
        _instance.uiText.text = ScoreManager.score.ToString();
    }
}
