using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgFlashEffect : MonoBehaviour
{
    // Public Variables
    public Material flash_material;
    public float flash_duration = 0.2f;

    // Private Variables
    private SpriteRenderer sprite_renderer;
    private Material original_material;
    private float current_duration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite_renderer = GetComponent<SpriteRenderer>();
        this.original_material = this.sprite_renderer.material;
        this.current_duration = this.flash_duration + 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.current_duration += Time.deltaTime;
        if(this.current_duration >= this.flash_duration) {
            this.sprite_renderer.material = this.original_material;
        }
    }

    public void FlashDamage() {
        this.current_duration = 0f;
        this.sprite_renderer.material = flash_material;
    }
}
