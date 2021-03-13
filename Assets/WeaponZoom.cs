using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    Camera camera;
    [SerializeField] float ZoomInFOV = 30f;
    [SerializeField] float ZoomOutFOV = 60f;
    bool isZoomToggle=false;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
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
        camera.fieldOfView = ZoomInFOV;
    }

    public void ZoomOut() {
        camera.fieldOfView = ZoomOutFOV;
    }
}
