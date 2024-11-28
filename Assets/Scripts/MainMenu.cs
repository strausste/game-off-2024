using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject scoreboardPanel;
    [SerializeField] Image[] displayedSymbols;
    
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
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenScoreboard(bool open)
    {
        scoreboardPanel.SetActive(open);
    }

    public void RandomizeLanguage(){
        Language.instance.Randomize();
        displayedSymbols[0].sprite = Language.instance.GetSymbol(Meaning.YOU)[0].getSprite();
        displayedSymbols[1].sprite = Language.instance.GetSymbol(Meaning.GO)[0].getSprite();
        displayedSymbols[2].sprite = Language.instance.GetSymbol(Meaning.LIFE)[0].getSprite();
    }
}
