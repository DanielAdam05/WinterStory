using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ShowUVText : MonoBehaviour
{
    [SerializeField]
    private Camera firstPersonCamera;

    [SerializeField]
    private Light uvFlashLight;

    [Header("Script References")]
    [SerializeField]
    private UVFlashlightLogic uVFlashlightLogicRef;

    // Non-assignable variables
    private float uvFlashRange;
    private RaycastHit currentHit;

    private GameObject lastUVHitObject = null;

    void Start()
    {
        uvFlashRange = uvFlashLight.range;
    }

    
    void Update()
    {
        if (!(LookingAtUVText(ref currentHit) && uVFlashlightLogicRef.UVActive()))
        {
            if(lastUVHitObject != null && lastUVHitObject.GetComponent<TextMeshPro>().IsActive())
            {
                lastUVHitObject.GetComponent<TextMeshPro>().enabled = false;
                lastUVHitObject = null;
                Debug.Log("Reset last UV object");
            }
        }
        else
        {
            if (!currentHit.collider.gameObject.GetComponent<TextMeshPro>().IsActive())
            {
                currentHit.collider.gameObject.GetComponent<TextMeshPro>().enabled = true;
                Debug.Log("Enabled UV object");
            }
        }

        Debug.DrawLine(firstPersonCamera.transform.position, 
            firstPersonCamera.transform.position + firstPersonCamera.transform.forward * uvFlashRange, 
            Color.rebeccaPurple);
    }

    bool LookingAtUVText(ref RaycastHit hit)
    {
        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit, uvFlashRange))
        {
            //Debug.Log($"Layer: {hit.collider.gameObject.layer}");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UVText"))
            {
                lastUVHitObject = hit.collider.gameObject;
                Debug.Log("looking at UV");
                return true;
            }
        }

        return false;
    }
}
