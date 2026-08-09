using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class UVFlashlightLogic : MonoBehaviour
{
    [Header("Dependable Game Objects")]
    [SerializeField]
    private GameObject uvFlashlightObject;
    [SerializeField]
    private GameObject uvLightObject;
   

    [Header("Input Action Rerferences")]
    [SerializeField]
    private InputActionReference lightActionRefrence;

    // Unassignable variables
    private bool uvLightShown = false;

    void Start()
    {
        uvLightShown = false;
    }

    private void OnEnable()
    {
        uvFlashlightObject.SetActive(true);
        uvLightShown = true;
    }

    private void OnDisable()
    {
        uvFlashlightObject.SetActive(false);
    }

    void Update()
    {
        if(!GameStateManager.Instance.IsGamePaused())
        {
            HandleInput();

            uvLightObject.SetActive(uvLightShown);
        }
    }

    private void HandleInput()
    {
        if(lightActionRefrence.action.triggered)
        {
            uvLightShown = !uvLightShown;
        }
    }

    public bool UVActive()
    { 
        return uvLightShown;
    }
}
