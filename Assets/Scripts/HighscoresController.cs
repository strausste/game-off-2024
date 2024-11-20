using UnityEngine;

public class HighscoresController : MonoBehaviour
{
    private string timePlayerPrefsKey = "Times";
    [SerializeField] TMPro.TextMeshProUGUI timeText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var previousTimes = PlayerPrefs.GetString(timePlayerPrefsKey);

        if (previousTimes == string.Empty)
        {
            timeText.SetText("No scores found");
            return;
        }
        
        var times = previousTimes != "" ? previousTimes.Split("\n") : null;
        
        var formattedTime = "";

        int position = 1;
        foreach (var time in times)
        {
            formattedTime +=  $"{position}. {time}\n";
            
            position++;
        }
        
        timeText.SetText(formattedTime);
    }
}
