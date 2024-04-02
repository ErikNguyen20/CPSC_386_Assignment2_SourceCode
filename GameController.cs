using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    // Public Variables
    public GameObject[] enemyPrefabs; // In order of ascending difficulty
    public GameObject bossPrefab;
    public int boss_level = 5;
    public float[] level_length = {70f, 75f, 80f, 85f, 60f};

    // Private Variables
    [SerializeField]
    public Slider progress_bar;
    private int current_level = 1;
    private float current_level_length;
    private float attempt_curr_duration = 0;
    private float time_exist = 0;
    private float level_progress_time = 0;
    private SettingsMenu settingsMenu;
    private bool exiting_level = false;



    // Start is called before the first frame update
    void Start()
    {
        current_level = PlayerPrefs.GetInt("CurrentLevel", 1);
        current_level_length = level_length[current_level-1];

        if(progress_bar)  {
            progress_bar.gameObject.SetActive(true);
            progress_bar.enabled = false;
        }

        settingsMenu = GetComponent<SettingsMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        time_exist += Time.deltaTime;

        // Attempt to spawn an enemy 20 times a second
        attempt_curr_duration += Time.deltaTime;
        if(attempt_curr_duration >= 0.05f && level_progress_time <= current_level_length) {
            attempt_curr_duration = 0;
            AttemptSpawn();
        }

        // Updates progress bar
        level_progress_time += Time.deltaTime;
        if(level_progress_time < current_level_length) {
            if(progress_bar) progress_bar.value = level_progress_time / current_level_length;
        }
        else if(this.current_level == boss_level) {
            // Beat boss first

        }
        else if(level_progress_time >= current_level_length + 5f) {
            // Beat level condition
            if(!exiting_level) {
                exiting_level = true;
                if(current_level + 1 > PlayerPrefs.GetInt("UnlockedLevels", 1)) {
                    PlayerPrefs.SetInt("UnlockedLevels", current_level + 1);
                }
                settingsMenu.OnWinScreen();
            }
            
        }
    }

    void SpawnEnemy() {
        // Randomly select enemy based on level
        float spawn_chance = Random.Range(0f, 100.0f);
        int selectedSpawnIndex = 0;
        switch(this.current_level) {
            case 1:
                selectedSpawnIndex = 0;
                break;
            case 2:
                if(spawn_chance <= 20f) {
                    selectedSpawnIndex = 1;
                }
                else {
                    selectedSpawnIndex = 0;
                }
                break;
            case 3:
                if(spawn_chance <= 10f) {
                    selectedSpawnIndex = 2;
                }
                else if(spawn_chance <= 60f){
                    selectedSpawnIndex = 0;
                }  
                else {
                    selectedSpawnIndex = 1;
                }
                break;
            case 4:
                if(spawn_chance <= 10f) {
                    selectedSpawnIndex = 3;
                }
                else if(spawn_chance <= 45f){
                    selectedSpawnIndex = 2;
                }  
                else if(spawn_chance <= 65f){
                    selectedSpawnIndex = 1;
                }
                else {
                    selectedSpawnIndex = 0;
                }
                break;
            case 5:
                if(spawn_chance <= 30f) {
                    selectedSpawnIndex = 3;
                }
                else if(spawn_chance <= 60f){
                    selectedSpawnIndex = 2;
                }  
                else if(spawn_chance <= 75f){
                    selectedSpawnIndex = 1;
                }
                else {
                    selectedSpawnIndex = 0;
                }
                break;
        }

        // Ensure that we do not access anything out of bounds.
        if(this.enemyPrefabs.Length <= selectedSpawnIndex) {
            selectedSpawnIndex = 0;
        }

        // Spawn the enemy
        float randomY = Random.Range(20f, Screen.height - 20f);
        Vector2 spawn_position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + 25f, randomY));
        GameObject enemy = Instantiate(this.enemyPrefabs[selectedSpawnIndex], spawn_position, transform.rotation);

        EnemyHandler enemy_handler = enemy.GetComponent<EnemyHandler>();
        if(enemy_handler) {
            enemy_handler.starting_position = spawn_position;
        }

        // If it is an asteroid, then apply random rotation
        if(selectedSpawnIndex == 0) {
            enemy.transform.Rotate(new Vector3(0, 0, Random.Range(0, 359)));
        }
    }

    void AttemptSpawn() {
        // Spawn Chance progressive difficulty
        float spawn_chance = Random.Range(0f, 600.0f / (this.current_level * 0.2f + 1));
        if(spawn_chance < (time_exist / 4) + 40f) {
            SpawnEnemy();
        }
    }

    // References the settings menu to play a sound effect
    public void PlaySFX(int index) {
        settingsMenu.PlaySFX(index);
    }

    // Method that is called when the player is dead
    public void HandlePlayerDeath() {
        if(!exiting_level) {
            exiting_level = true;
            settingsMenu.ActivateDeathScreen();
        }
    }
}
