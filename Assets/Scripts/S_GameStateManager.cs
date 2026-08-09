using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("State variables")]
    [SerializeField]
    private static bool gamePaused;

    [Header("Input Action Reference")]
    [SerializeField]
    private InputActionReference pauseActionReference;
    [SerializeField]
    private InputActionReference interactActionReference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        gamePaused = false;
    }

    void Update()
    {
        //if (pauseActionReference.action.triggered || interactActionReference.action.triggered)
        //{
        //    if (gamePaused)
        //    {
        //        gamePaused = false;
        //        Cursor.lockState = CursorLockMode.Locked;
        //    }
        //    else
        //    {
        //        gamePaused = true;
        //        Cursor.lockState = CursorLockMode.None;
        //    }
        //}

        //if (gamePaused)
        //{
        //    if (Time.timeScale != 0f)
        //        Time.timeScale = 0f;
        //}
        //else
        //{
        //    if (Time.timeScale != 1f)
        //        Time.timeScale = 1f;
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the currently active scene
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void UnpauseGame()
    {
        gamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
