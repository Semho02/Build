using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public string menuSceneName = "MainMenu";

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        LoadMainMenu();
    //        Cursor.visible = true;
    //    }
    //}

    //public void LoadMainMenu()
    //{
    //    SceneManager.LoadScene(menuSceneName);
    //}
    public GameObject pauseMenu;
    public bool isPaused = false;
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;
    public AudioSource gameAudioSource;
    public AudioClip pauseAudioClip;
    public AudioClip gameAudioClip;
    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryMenu.activeSelf || equipmentMenu.activeSelf)
            {
                inventoryManager.isMenuOpen = false;
                CloseMenu();
            }
            else if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Останавливаем время
        isPaused = true;
        pauseMenu.SetActive(true); // Показываем меню паузы
        Cursor.visible = true;
        inventoryManager.isMenuOpen=true;
        gameAudioSource.clip = pauseAudioClip;
        gameAudioSource.Play();
        Debug.Log("Game paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false;
        pauseMenu.SetActive(false); // Скрываем меню паузы
        inventoryManager.isMenuOpen = false;
        if (!inventoryMenu.activeSelf && !equipmentMenu.activeSelf) // Проверяем, нет ли открытых других меню
        {
            Cursor.visible = false; // Скрываем курсор только если нет открытых меню
        }
        gameAudioSource.clip = gameAudioClip;
        gameAudioSource.Play();
        Debug.Log("Game resumed");
    }

    public void CloseMenu()
    {
        if (inventoryMenu.activeSelf)
        {
            // Закрываем меню оружия
            inventoryMenu.SetActive(false);
        }
        else if (equipmentMenu.activeSelf)
        {
            // Закрываем меню снаряжения
            equipmentMenu.SetActive(false);

            Debug.Log("Equipment menu closed");
        }
        if (isPaused)
        {
            Time.timeScale = 1f; // Возобновляем время
            isPaused = false;
            pauseMenu.SetActive(false); // Скрываем меню паузы
            Debug.Log("Game resumed after closing equipment menu");
        }

        Debug.Log("isPaused in GameManager: " + isPaused);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("App has quit");
    }
}
