using UnityEngine;

public class MapLogic : MonoBehaviour
{
    [Header("Dependable Game Objects")]
    [SerializeField]
    private GameObject initialMapObject;

    [Header("Script Refrences")]
    [SerializeField]
    private WalkingController walkingControllerRef;
    [SerializeField]
    private MouseLook mouseLookRef;

    void Start()
    {
        initialMapObject.SetActive(false);
    }

    public void InteractWithMap()
    {
        if(initialMapObject == null) return;

        if (initialMapObject.activeSelf)
        {
            initialMapObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            initialMapObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        mouseLookRef.enabled = !mouseLookRef.enabled;
        walkingControllerRef.enabled = !walkingControllerRef.enabled;
    }
}
