using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Public Variables
    public int behavior = 0;
    public float speed = 5f;
    public int health = 3;
    public int max_health = 3;
    public Vector2 starting_position = new Vector2(0, 0);
    public Vector2 moveDirection = new Vector2(-1, 0);

    // Private Variables
    private DmgFlashEffect flash_effect;

    [SerializeField]
    private GameObject death_effect;
    [SerializeField]
    private GameObject optional_split_prefab;
    [SerializeField]
    private int optional_split_count = 3;
    private float time_exist = 0;
    private GameObject game_manager;
    private GameController game_controller;

    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
        flash_effect = GetComponent<DmgFlashEffect>();

        if(starting_position.x == 0 && starting_position.y == 0) {
            starting_position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + 25f, Screen.height/2));
        }

        this.game_manager = GameObject.FindWithTag("GameController");
        this.game_controller = game_manager.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        time_exist += Time.deltaTime;
        // Manages movement and destroys itself after it exits screen
        if(behavior == 0 || behavior == 2) {
            transform.position = starting_position + (moveDirection * time_exist * this.speed);
            //transform.position = Vector2.MoveTowards(transform.position, starting_position + Vector2.left, Time.deltaTime * this.speed);
        }
        else if(behavior == 1) {
            transform.position = starting_position + (moveDirection * time_exist * this.speed) + (Vector2.up * Mathf.Sin(time_exist * this.speed/2));
        }

        Vector3 viewport_position = Camera.main.WorldToViewportPoint(transform.position);
        if(viewport_position.x < -0.1f || viewport_position.x > 1.1f || viewport_position.y < -0.1f || viewport_position.y > 1.1f) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount) {
        health -= amount;
        if(health <= 0) {
            if(death_effect) Instantiate(death_effect, transform.position, transform.rotation);

            if(behavior == 0 || behavior == 1) {
                if(game_controller) game_controller.PlaySFX(2);
                Destroy(gameObject);
            }
            else if(behavior == 2 && optional_split_prefab) {
                // On Death, clone and split
                for(int index = 0; index < this.optional_split_count; ++index) {
                    GameObject split = Instantiate(optional_split_prefab, transform.position, transform.rotation);
                    split.transform.Rotate(new Vector3(0, 0, Random.Range(0, 359)));

                    EnemyHandler split_enemy_handler = split.GetComponent<EnemyHandler>();
                    if(split_enemy_handler) {
                        split_enemy_handler.starting_position = transform.position;
                        split_enemy_handler.moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    }
                }

                if(game_controller) game_controller.PlaySFX(2);
                Destroy(gameObject);
            }
        }

        if(flash_effect) flash_effect.FlashDamage();
    }
}
