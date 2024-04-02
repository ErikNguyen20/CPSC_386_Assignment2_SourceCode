using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    // Public Variables
    public Sprite[] spritesList;
    public int frame_per_second = 4;

    // Private Variables
    private SpriteRenderer sprite_renderer;
    private float exist_time = 0;
    private int current_frame = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite_renderer = GetComponent<SpriteRenderer>();
        if(this.sprite_renderer && this.spritesList.Length > 0) {
            this.sprite_renderer.sprite = spritesList[0];
            this.current_frame++;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Provides one-time effect animation
        this.exist_time += Time.deltaTime;
        if(this.exist_time >= 1f/frame_per_second) {
            this.exist_time = 0;

            if(this.sprite_renderer && current_frame < this.spritesList.Length) {
                this.sprite_renderer.sprite = spritesList[current_frame++];
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
