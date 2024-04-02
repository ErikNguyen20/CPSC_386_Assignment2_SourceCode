using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Public Variables
    public float speed = 0.1f;

    // Private Variables
    private Material material;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime * this.speed;
        material.SetTextureOffset("_MainTex", Vector2.right * this.distance);
    }
}
