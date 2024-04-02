using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Public Variables
    public GameObject pause_menu;
    public Slider volume_slider;
    public GameObject death_menu;
    public GameObject win_menu;
    public AudioSource sfx_source;
    public AudioClip[] sounds;
    public float[] volumes;

    // Private Variables
    private bool won_game = false;
    private bool isDead = false;
    private SceneTransition transition;
    private AudioSource music;
    private float original_volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Activate In Transition
        transition = GetComponent<SceneTransition>();
        transition.TransitionIn(0.5f);

        // Disable menu items
        pause_menu.SetActive(false);
        death_menu.SetActive(false);
        win_menu.SetActive(false);


        // Updates the volume
        music = GetComponent<AudioSource>();
        original_volume = music.volume;
        volume_slider.value = PlayerPrefs.GetFloat("Volume", 1f);
        music.volume = original_volume * volume_slider.value;
        volume_slider.onValueChanged.AddListener((v) => {
            music.volume = original_volume * v;
            PlayerPrefs.SetFloat("Volume", v);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !won_game && !isDead) {
            if(Time.timeScale == 0) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void PlaySFX(int index) {
        if(index < sounds.Length && index >= 0) {
            sfx_source.PlayOneShot(sounds[index], volumes[index] * volume_slider.value);
        }
    }

    // Pauses the game
    void PauseGame() {
        pause_menu.SetActive(true);
        Time.timeScale = 0;
    }

    // Resumes the game
    void ResumeGame() {
        pause_menu.SetActive(false);
        Time.timeScale = 1;
    }

    // Calls to activate death screen
    public void ActivateDeathScreen() {
        Time.timeScale = 0;
        death_menu.SetActive(true);
        isDead = true;
    }


    // Button Events
    public void OnButtonResumePressed() {
        ResumeGame();
    }

    public void OnButtonMainMenuPressed() {
        LoadMainMenu();
        ResumeGame();
    }

    // Activates Win Screen
    public void OnWinScreen() {
        won_game = true;
        win_menu.SetActive(true);
        Invoke("LoadMainMenu", 4f);
    }

    // Loads Scene to Main menu
    void LoadMainMenu() {
        transition.TransitionOut(0.5f, "TitleScreen");
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetFloat("Volume", volume_slider.value);
    }
 }
