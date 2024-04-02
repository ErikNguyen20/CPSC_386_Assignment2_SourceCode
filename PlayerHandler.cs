using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Public Variables
    public float speed = 3f;
    public float shoot_delay = 0.2f;
    public GameObject shoot_effect;
    public GameObject bullet_prefab;
    

    // Private Variables
    private float shoot_timer = 0f;
    private Rigidbody2D rigid_body;
    private GameObject game_manager;
    private GameController game_controller;


    // Start is called before the first frame update
    void Start()
    {
        this.rigid_body = GetComponent<Rigidbody2D>();
        this.game_manager = GameObject.FindWithTag("GameController");
        this.game_controller = game_manager.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement controls for the ship
        float input_horizontal = Input.GetAxisRaw("Horizontal");
        float input_vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(input_horizontal, input_vertical, 0f).normalized;

        if(this.rigid_body) {
            rigid_body.velocity = direction * this.speed;
        }

        // Constrain movement to screen bounds
        Vector3 screen_position = Camera.main.WorldToViewportPoint(transform.position);
        screen_position.x = Mathf.Clamp01(screen_position.x);
        screen_position.y = Mathf.Clamp01(screen_position.y);
        transform.position = Camera.main.ViewportToWorldPoint(screen_position);

        // Handling for the shooting
        this.shoot_timer += Time.deltaTime;
        if(Input.GetKey(KeyCode.Space) && this.shoot_timer >= this.shoot_delay) {
            this.shoot_timer = 0f;
            // Creates flash effect
            GameObject effect = Instantiate(shoot_effect, transform);
            effect.transform.localPosition = transform.right * 0.7f + transform.up * -0.1f;
            effect.transform.localRotation = transform.rotation;

            GameObject bullet = Instantiate(bullet_prefab, transform.position + transform.right * 0.7f + transform.up * -0.1f, transform.rotation);
            if(game_controller) game_controller.PlaySFX(0);
        }
    }

    // This is called when any other object collides with this object
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy")) {
            game_controller.HandlePlayerDeath();
        }
    }
}
