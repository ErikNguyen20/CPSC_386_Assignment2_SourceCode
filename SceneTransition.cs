using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Public Variables
    public GameObject transition;

    // Private Variables
    private float in_transition_duration = -1f;
    private float out_transition_duration = -1f;
    private float transition_timing = 0;
    private RectTransform transitionRect;
    private string out_scene = "";

    // Start is called before the first frame update
    void Start()
    {
        transitionRect = transition.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Control animation for in transition
        if(in_transition_duration >= 0) {
            in_transition_duration += Time.deltaTime;
            transitionRect.localScale = new Vector2(1f, 1f) * Mathf.Abs(1f - (in_transition_duration / transition_timing));

            if(in_transition_duration >= transition_timing) {
                in_transition_duration = -1f;
                transition.SetActive(false);
                
            }
        }

        // Control animation for out transition
        if(out_transition_duration >= 0) {
            out_transition_duration += Time.deltaTime;
            transitionRect.localScale = new Vector2(1f, 1f) * (out_transition_duration / transition_timing);

            if(out_transition_duration >= transition_timing) {
                out_transition_duration = -1f;

                SceneManager.LoadScene(out_scene);
            }
        }
    }

    public void TransitionIn(float duration) {
        if(transitionRect == null) {
            transitionRect = transition.GetComponent<RectTransform>();
        }
        transitionRect.localScale = new Vector2(1f, 1f);
        transition_timing = duration;
        transition.SetActive(true);
        in_transition_duration = 0f;
    }

    public void TransitionOut(float duration, string scene) {
        transitionRect.localScale = new Vector2(0, 0);
        transition_timing = duration;
        transition.SetActive(true);
        out_scene = scene;
        out_transition_duration = 0f;
    }
}
