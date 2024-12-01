using System;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private string timePlayerPrefsKey = "Times";
    bool gamePaused = false;
    public bool GamePaused{
        get{
            return gamePaused;
        }
    }

    public static GameController instance = null;
    [SerializeField] CheatCodes cheatCodes;    
    private float timerStart;
    
    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
            Debug.Log("Destroying game controller, instance is ", instance?.gameObject);
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
        var times = PlayerPrefs.GetString(timePlayerPrefsKey);

        if (times != "")
        {
            times += "\n" + (Time.time - timerStart).ToString("0.00");
        }
        else
        {
            times = (Time.time - timerStart).ToString();
        }
        
        Debug.Log(times);
        
        var timesList = times.Split("\n").ToList();
        
        
        timesList.Sort((a, b) =>
        {
            var timeA = float.Parse(a);
            var timeB = float.Parse(b);
            
            return timeA.CompareTo(timeB);
        });
        
        PlayerPrefs.SetString(timePlayerPrefsKey, String.Join("\n", timesList));
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel(){
        Inventory.instance.Load();
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
        FindFirstObjectByType<PlayerController>().Heal(100);
    }

    public void LoadLevel(string level){
        Inventory.instance.Save();
        SceneManager.LoadScene(level);
    }
}
