﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour // TODO: Implement command pattern in place of this class's simple booleans
{
    private static InputHandler inputHdlerInstance;

    public static InputHandler Instance
    {
        get
        {
            if (inputHdlerInstance == null)
            {
                GameObject inputGO = new GameObject();
                inputHdlerInstance = inputGO.AddComponent<InputHandler>();
                inputGO.name = "InputHandler (Singleton)";
            }

            return inputHdlerInstance;
        }
    }

    [HideInInspector]
    public bool UpKeyHeld, UpKeyDown,
                DownKeyHeld, DownKeyDown,
                LeftKeyHeld, LeftKeyDown,
                RightKeyHeld, RightKeyDown,
                ChoiceKeyDown;

    // Start is called before the first frame update
    void Start()
    {
        UpKeyHeld = UpKeyDown =
                DownKeyHeld = DownKeyDown =
                LeftKeyHeld = LeftKeyDown =
                RightKeyHeld = RightKeyDown =
                ChoiceKeyDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateReflectInput();
    }

    // TODO: Implement command pattern in place of this class's simple booleans
    private void UpdateReflectInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) UpKeyDown = true;
        else UpKeyDown = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) UpKeyHeld = true;
        else UpKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) DownKeyDown = true;
        else DownKeyDown = false;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) DownKeyHeld = true;
        else DownKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) LeftKeyDown = true;
        else LeftKeyDown = false;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) LeftKeyHeld = true;
        else LeftKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) RightKeyDown = true;
        else RightKeyDown = false;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) RightKeyHeld = true;
        else RightKeyHeld = false;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) ChoiceKeyDown = true;
        else ChoiceKeyDown = false;
    }
}