using UnityEngine;

public class HighscoresController : MonoBehaviour
{
    [SerializeField] private string timePlayerPrefsKey = "Times";
    [SerializeField] TMPro.TextMeshProUGUI timeText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var previousTimes = PlayerPrefs.GetString(timePlayerPrefsKey);
        
        timeText.SetText(previousTimes);
    }
}
