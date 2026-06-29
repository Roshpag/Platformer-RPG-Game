using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transitionAnim;
    private static bool isPaused;
    public GameObject menu;
    private CursorLockMode cursorBefore;
    private bool isVisibleCursor;
    public static int playerSkin = 0;
    public bool isGameLevel = true;
    private void Awake()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isGameLevel)
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        ShowMenu();
        cursorBefore = Cursor.lockState;
        isVisibleCursor = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        HideMenu();
        Cursor.lockState = cursorBefore;
        Cursor.visible = isVisibleCursor;
    }

    public void Restart()
    {
        Resume();
        int scene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Transition(1f, scene));
    }

    public void HideMenu()
    {
        if(menu != null)
        {
            menu.SetActive(false);
        }
    }
    public void ShowMenu()
    {
        if(menu != null)
        {
            menu.SetActive(true);
        }
    }

    IEnumerator Transition(float sec,int scene)
    {
        transitionAnim.SetTrigger("Transition");
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(scene);
    }

    public void ChangeSkin()
    {
        if(playerSkin == 0)
        {
            playerSkin = 1;
        }
        else if(playerSkin == 1)
        {
            playerSkin = 0;
        }
    }

    public void MainMenu()
    {
        StartCoroutine(Transition(1f, 0));
    }

    public void LevelSelect()
    {
        StartCoroutine(Transition(1f, 1));
    }
    public void LVL1()
    {
        StartCoroutine(Transition(1f, 2));
    }
}
