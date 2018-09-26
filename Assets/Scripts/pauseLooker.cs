using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseLooker : MonoBehaviour {

    public GameObject pauseMenu;
    public AudioSource soundEffects;
    public AudioClip buttonClick;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (soundEffects)
            {

                soundEffects.PlayOneShot(buttonClick);
            }
            if (pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
            }

            else if (pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
            }
        }
    }
}

