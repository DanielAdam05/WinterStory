using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField]
    private InputActionReference mapInteractActionReference;
    [SerializeField]
    private InputActionReference showUVFlashlightActionRefrence;

    [Header("Script References")]
    [SerializeField]
    private UVFlashlightLogic UVFlashlightLogicRef;
    [SerializeField]
    private MapLogic mapLogicRef;

    // Unassignable variables

    void Start()
    {
    }
    
    void Update()
    {
        if (!GameStateManager.Instance.IsGamePaused())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (showUVFlashlightActionRefrence.action.triggered)
        {
            UVFlashlightLogicRef.enabled = !UVFlashlightLogicRef.enabled;
        }

        if (mapInteractActionReference.action.triggered)
        {
            mapLogicRef.InteractWithMap();
        }
    }
}
