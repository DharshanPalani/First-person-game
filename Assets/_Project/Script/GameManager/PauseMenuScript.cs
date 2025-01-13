using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public UnityEvent pauseMenuShow;

    private void Start()
    {
        SetTimeScale(1.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuShow.Invoke();
        SetTimeScale(0f);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
