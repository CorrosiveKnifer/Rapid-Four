using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// William de Beer
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static bool cheatsEnabled = false;
    public static bool loadingNextArea = false;

    public GameObject CompleteLoadUI;

    public Toggle cheatToggle;

    public Animator transition;

    public float transitionTime = 1.0f;

    private void Start()
    {
        ApplicationManager.GetInstance();
        DontDestroyOnLoad(this.gameObject);
        loadingNextArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuScreen") // Back to menu
        {
            /*
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(LoadLevel(0));
            }
            */
        }

        if (cheatToggle != null)
        {
            cheatsEnabled = cheatToggle.isOn;
        }
        /*
        if (Input.GetKeyDown(KeyCode.O)) // Reset scene
        {
            ResetScene();
        }
        */
    }

    private void OnDestroy()
    {
        ApplicationManager.DestroyInstance();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    private void OnLevelWasLoaded(int level)
    {
        GetComponentInChildren<Animator>().SetTrigger("Blink");
    }

    public void LoadNextLevel()
    {
        loadingNextArea = true;
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1) // Check if index exceeds scene count
        {
            StartCoroutine(LoadLevel(0)); // Load menu
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); // Loade next scene
        }
    }
    public void ResetScene()
    {
        loadingNextArea = true;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevelAsync(int levelIndex, float maxTime)
    {
        StartCoroutine(OperationLoadLevelAsync(levelIndex, maxTime));
    }

    IEnumerator OperationLoadLevelAsync(int levelIndex, float maxTime)
    {
        AsyncOperation gameLoad = SceneManager.LoadSceneAsync(levelIndex);
        gameLoad.allowSceneActivation = false;
        float time = 0.0f;

        while (!gameLoad.isDone)
        {
            time += Time.deltaTime;
            if (gameLoad.progress >= 0.9f)
            {
                CompleteLoadUI.SetActive(true);

                if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, 0))
                {
                    gameLoad.allowSceneActivation = true;
                }
                if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, 1))
                {
                    gameLoad.allowSceneActivation = true;
                }
                if(time >= maxTime)
                {
                    gameLoad.allowSceneActivation = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }

        CompleteLoadUI.SetActive(false);
        yield return null;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        loadingNextArea = true;
        // Play Animation
        transition.SetTrigger("Start");

        // Wait to let animation finish playing
        yield return new WaitForSeconds(transitionTime);

        if (levelIndex == 0 || levelIndex == SceneManager.sceneCountInBuildSettings - 1) // Check if either in menu or end screen
        {
            Cursor.lockState = CursorLockMode.None; // Make cursor usable.
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined; // Make cursor unusable.
            Cursor.visible = false;
        }

        // Load Scene
        SceneManager.LoadScene(levelIndex);
    }
}
