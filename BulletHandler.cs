using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    // Public Variables
    public Vector2 direction = new Vector2(0, 1);
    public float speed = 12f;
    public int behavior = 0;
    public bool onPlayerTeam = true;
    public GameObject hit_effect;
    

    // Private Variables
    private float time_exist = 0;
    private GameObject game_manager;
    private GameController game_controller;

    // Start is called before the first frame update
    void Start()
    {
        // Destroys the projectile after certain number of seconds
        Destroy(gameObject, 3);

        this.game_manager = GameObject.FindWithTag("GameController");
        this.game_controller = game_manager.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time_exist += Time.fixedDeltaTime;
        Vector2 currentPos = transform.position;
        switch(this.behavior) {
            case 1:
                currentPos += this.direction * this.speed * Time.fixedDeltaTime;
                currentPos += new Vector2(0f, Mathf.Sin(time_exist) * Time.fixedDeltaTime);
                transform.position = currentPos;
                break;
            default:
                currentPos += this.direction * this.speed * Time.fixedDeltaTime;
                transform.position = currentPos;
                break;
        }
    }

    // This is called when any other object collides with this object
    void OnTriggerEnter2D(Collider2D other) 
    {
        // Checks if a bullet (fired by the player) collides.
        if(this.onPlayerTeam && other.CompareTag("Enemy")) {
            if(hit_effect) Instantiate(hit_effect, transform.position, transform.rotation);

            // Apply Damage
            EnemyHandler enemyHandler = other.gameObject.GetComponent<EnemyHandler>();
            if(enemyHandler) enemyHandler.TakeDamage(1);
            

            // Destroy the bullet
            if(game_controller) game_controller.PlaySFX(1);
            Destroy(gameObject);
        }
        else if(!this.onPlayerTeam && other.CompareTag("Player")) {
            if(hit_effect) Instantiate(hit_effect, transform.position, transform.rotation);

            // Destroy the bullet
            if(game_controller) game_controller.PlaySFX(1);
            Destroy(gameObject);
        }
    }
}
