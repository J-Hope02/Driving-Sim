using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // DESIGN PATTERN: SINGLETON
    public static GameManager Instance { get; private set; }
    public static InputManager InputManager { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Removed below line of code as shouldn't be able to instantiate a MonoBehaviour with 'new' at runtime.
        //InputManager = new InputManager();
        InputManager = this.GetOrAddComponent<InputManager>();
    }
}