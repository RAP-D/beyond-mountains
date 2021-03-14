using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField]Camera fpsCamera;
    [SerializeField]RigidbodyFirstPersonController fpsController;
    [SerializeField] float ZoomInFOV = 30f;
    [SerializeField] float ZoomOutFOV = 60f;
    [SerializeField] float ZoomInMouseSens = 1f;
    [SerializeField] float ZoomOutMouseSens = 2f;
    bool isZoomToggle=false;

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
