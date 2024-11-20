using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject scoreboardPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void OpenOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void StartGame()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Luca");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenScoreboard(bool open)
    {
        scoreboardPanel.SetActive(open);
    }
}
