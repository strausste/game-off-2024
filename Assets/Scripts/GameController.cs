using UnityEngine;

public class GameController : MonoBehaviour
{
    bool gamePaused = false;
    public bool GamePaused{
        get{
            return gamePaused;
        }
    }

    public static GameController instance;

    void Awake(){
        if(instance && instance != this){
            Destroy(gameObject);

            return;
        }

        instance = this;
    }

    public void PauseGame(bool pause){
        this.gamePaused = pause;
    }
}