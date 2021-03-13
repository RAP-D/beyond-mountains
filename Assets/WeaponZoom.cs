using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    Camera fpsCamera;
    RigidbodyFirstPersonController fpsController;
    [SerializeField] float ZoomInFOV = 30f;
    [SerializeField] float ZoomOutFOV = 60f;
    [SerializeField] float ZoomInMouseSens = 1f;
    [SerializeField] float ZoomOutMouseSens = 2f;
    bool isZoomToggle=false;
    // Start is called before the first frame update
    void Start()
    {
        //TODO consider to do this with inspector
        fpsCamera = GetComponent<Camera>();
        fpsController=GetComponentInParent<RigidbodyFirstPersonController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            isZoomToggle = !isZoomToggle;
        }
        if (isZoomToggle) { ZoomIn(); }
        else { ZoomOut(); }
    }

    public void ZoomIn()
    {
        fpsCamera.fieldOfView = ZoomInFOV;
        SetMouseSensitivity(ZoomInMouseSens);
    }

    private void SetMouseSensitivity(float sensitivity)
    {
        fpsController.mouseLook.XSensitivity = sensitivity;
        fpsController.mouseLook.YSensitivity = sensitivity;
    }

    public void ZoomOut() {
        fpsCamera.fieldOfView = ZoomOutFOV;
        SetMouseSensitivity(ZoomOutMouseSens);
    }
}
