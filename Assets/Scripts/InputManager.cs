﻿using System.Collections.Generic;
using UnityEngine;

public enum KeyInputMode
{
    KeyPressed,
    KeyUp,
    KeyDown
}

public class KeyAction
{
    public delegate void Callback();
    public KeyInputMode keyMode;
    public KeyCode code;
    public Callback callback;

    public KeyAction(KeyInputMode keyMode, KeyCode code, Callback callback)
    {
        this.keyMode = keyMode;
        this.code = code;
        this.callback = callback;
    }
}

public class InputManager : Manager<InputManager>
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";

    public const KeyCode Jump = KeyCode.Space;

    private List<KeyAction> actions;

    private void Awake()
    {
        actions = new List<KeyAction>();
    }

    public float GetWheel()
    {
        return Input.mouseScrollDelta.y;
    }

    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public void AddKeyAction(KeyAction item)
    {
        actions.Add(item);
    }

    public void RemoveKeyActions(KeyAction item)
    {
        actions.Remove(item);
    }

    private void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            switch (actions[i].keyMode)
            {
                case KeyInputMode.KeyPressed:
                    {
                        if (Input.GetKey(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
                case KeyInputMode.KeyUp:
                    {
                        if (Input.GetKeyUp(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
                case KeyInputMode.KeyDown:
                    {
                        if (Input.GetKeyDown(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
            }
        }
    }
}