﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseLooker : MonoBehaviour {

    public GameObject pauseMenu;
    public AudioSource soundEffects;
    public AudioClip buttonClick;
    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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

