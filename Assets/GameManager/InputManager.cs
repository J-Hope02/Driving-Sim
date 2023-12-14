using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public ProjectName inputActions;
    public event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new ProjectName();
        Debug.Log("AWAKE : ");
    }

    void Start()
    {
        ToggleActionMap(inputActions.Player);
    }

    public void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
        {
            return;
        }

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}