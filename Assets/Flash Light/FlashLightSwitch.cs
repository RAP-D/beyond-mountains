using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightSwitch : MonoBehaviour
{
    bool isTurnOn;
    [SerializeField] GameObject flashLight;
    // Start is called before the first frame update
    void Start()
    {
        isTurnOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessKeyInput();
        if (isTurnOn)
        {
            SetFlashLightActive(true);
        }
        else {
            SetFlashLightActive(false);
        }
    }

    private void SetFlashLightActive(bool isActive )
    {
        flashLight.SetActive(isActive);
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isTurnOn = !isTurnOn;
        }
    }
}
