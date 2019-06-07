﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public Button botaoStart;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clicouBotaoStart()
    {
        SceneManager.LoadScene("Main");
    }

 

    public void clicouBotaoExit()
    {
        Application.Quit();
    }
}