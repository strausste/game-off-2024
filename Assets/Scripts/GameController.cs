using System;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameController : MonoBehaviour
{
    [SerializeField] private string timePlayerPrefsKey = "Times";
    bool gamePaused = false;
    public bool GamePaused{
        get{
            return gamePaused;
        }
    }

    public static GameController instance;
    [SerializeField] CheatCodes cheatCodes;    
    private float timerStart;
    
    void Awake(){
        if(instance && instance != this){
            Destroy(gameObject);

            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timerStart = Time.time;
    }

    void FixedUpdate()
    {
        var timeSinceStart = Time.time - timerStart;
        
        UIController.instance.SetTimer(timeSinceStart);
    }
    
    public void PauseGame(bool pause){
        this.gamePaused = pause;
    }

    public CheatCodes GetCheatCodes()
    {
        return cheatCodes;
    }

    //Call this when the player reachs the end of the game
    public void EndGame()
    {
        var times = PlayerPrefs.GetString(timePlayerPrefsKey) + "\n" + (Time.time - timerStart);

        var timesList = times.Split("\n").ToList();
        
        timesList.Sort((a, b) =>
        {
            var timeA = float.Parse(a);
            var timeB = float.Parse(b);
            
            return timeB.CompareTo(timeA);
        });
        
        PlayerPrefs.SetString(timePlayerPrefsKey, String.Join("\n", timesList));
    }
}
