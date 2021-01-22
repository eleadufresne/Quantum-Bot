using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private bool _pause;
    [SerializeField]
    private float _currentTimeScale;

    private void Start()
    {
        Time.timeScale = 1;
    }


    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            _pause = !_pause;
        }


        if (_pause)
        {
            if(Time.timeScale > 0)
            {
                _currentTimeScale = Time.timeScale;
            }
            Cursor.visible = true;
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
        }
        else
        {
            Cursor.visible = false;
            if (Time.timeScale == 0)
            {
                Time.timeScale = _currentTimeScale;
            }
            _pauseMenu.SetActive(false);
        }

    }


    public void ResumeGame()
    {
        _pause = false;
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameObject.Find("Music").GetComponent<DontDestroyOnLoad>().DestroyObject();
    }
}
