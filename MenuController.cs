using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Public Variables
    public GameObject title_screen;
    public GameObject play_button;
    public GameObject level_screen;
    public Slider volume_slider;

    public TextMeshProUGUI[] levelTiles;

    // Private Variables
    private SceneTransition transition;
    private int unlocked_levels = 1;
    private AudioSource music;
    private float original_volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Activate In Transition
        transition = GetComponent<SceneTransition>();
        transition.TransitionIn(0.5f);

        title_screen.SetActive(true);
        level_screen.SetActive(false);

        // Updates the volume
        music = GetComponent<AudioSource>();
        original_volume = music.volume;
        volume_slider.value = PlayerPrefs.GetFloat("Volume", 1f);
        music.volume = original_volume * volume_slider.value;
        volume_slider.onValueChanged.AddListener((v) => {
            music.volume = original_volume * v;
            PlayerPrefs.SetFloat("Volume", v);
        });


        unlocked_levels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        // Sets the level tiles
        for(int index = 0; index < levelTiles.Length; ++index) {
            if(index >= unlocked_levels) {
                if(levelTiles[index]) levelTiles[index].SetText("(Locked)");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        play_button.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 1f) * 6f);
    }

    public void OnTileClick(int level) {
        if(level <= unlocked_levels) {
            // Send player to the level
            PlayerPrefs.SetInt("CurrentLevel", level);
            transition.TransitionOut(0.5f, "Gameplay");
        }
    }

    public void OnButtonPlay() {
        level_screen.SetActive(true);
        title_screen.SetActive(false);
    }

    public void OnBackButton() {
        level_screen.SetActive(false);
        title_screen.SetActive(true);
    }

    public void OnButtonQuit() {
        Application.Quit();
    }

    

    void OnApplicationQuit() {
        PlayerPrefs.SetFloat("Volume", volume_slider.value);
    }
}
