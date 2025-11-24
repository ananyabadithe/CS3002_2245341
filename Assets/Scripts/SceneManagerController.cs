using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public EnemyShipSpawner enemyShips;
    public EnemyAlienSpawner enemyAlien;
    public GameObject menu;
    public GameObject instructions;

    private LaserBeamController laserBeam;
    [SerializeField] private AudioClip buttonClick;

    void Start()
    {
        Time.timeScale = 0f;

        menu.SetActive(false);
        instructions.SetActive(true);

        FindSceneObjects();
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;  
        FindSceneObjects();  
    }

    void FindSceneObjects()
    {
        enemyShips = FindObjectOfType<EnemyShipSpawner>();
        enemyAlien = FindObjectOfType<EnemyAlienSpawner>();
        laserBeam = FindObjectOfType<LaserBeamController>();
    }

    void Update()
    {

        int index = SceneManager.GetActiveScene().buildIndex;

        if (index == 0 && enemyShips != null && enemyShips.SpawnedShips.Count == 0)
        {
            if (laserBeam != null)
                laserBeam.DifficultyFactor = 2.0f;

            SceneManager.LoadScene("Level2");
        }

        if (index == 1
            && enemyShips.SpawnedShips.Count == 0
            && enemyAlien.SpawnedAliens.Count == 0)
        {
            SceneManager.LoadScene("Level3");
        }

        GameObject bigBoss = GameObject.FindWithTag("BigBoss");
        
        if (index == 2
            && enemyAlien.SpawnedAliens.Count == 0
            && bigBoss == null)
        {
            OnQuit();
        }
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        PlayButtonClickSound();
        if (!context.performed) return;

        Time.timeScale = 0f;
        menu.SetActive(true);
    }

    public void OnPlayButton()
    {
        PlayButtonClickSound();
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnReplay()
    {
        PlayButtonClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
        PlayButtonClickSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnInstructions()
    {
        menu.SetActive(false ); 
        instructions.SetActive(true);
    }

    public void BackMenu()
    {
        instructions.SetActive(false );
        menu.SetActive(true );
    }

    private void PlayButtonClickSound()
    {
        if (buttonClick != null)
        {
            AudioSource.PlayClipAtPoint(buttonClick, transform.position, 1.5f);
        }
    }
}
