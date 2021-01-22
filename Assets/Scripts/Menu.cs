using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _credit;
    [SerializeField]
    GameObject _setting;
    [SerializeField]
    GameObject _controls;

    public void StartGame(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void ShowCredit()
    {
        _credit.SetActive(true);
        _menu.SetActive(false);
    }

    public void BackToMenu()
    {
        _credit.SetActive(false);
        _setting.SetActive(false);
        _controls.SetActive(false);
        _menu.SetActive(true);
    }

    public void ShowSettings()
    {
        _setting.SetActive(true);
        _menu.SetActive(false);
    }

    public void ShowContorls()
    {
        _controls.SetActive(true);
        _menu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
