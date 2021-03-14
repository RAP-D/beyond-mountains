using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] float lightDecay = 0.1f;
    [SerializeField] float angleDecay = 1f;
    [SerializeField] float minimumAngle = 40f;
    float angle;
    Light flashLight;
    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
        angle = flashLight.spotAngle;
    }

    // Update is called once per frame
    void Update()
    {
        DecreaselightAngle();
        DecreaseLightIntensity();
    }
    private void DecreaseLightIntensity()
    {
        flashLight.intensity -= lightDecay * Time.deltaTime;
    }

    private void DecreaselightAngle()
    {
        if (flashLight.spotAngle >= minimumAngle)
        {
            flashLight.spotAngle -= angleDecay * Time.deltaTime;
        }
    }

    public void RestorelightAngle()
    {
            flashLight.spotAngle = angle;
    }

    public void IncreaseLightIntensity(float addIntensity)
    {
        flashLight.intensity += addIntensity;
    }
}
