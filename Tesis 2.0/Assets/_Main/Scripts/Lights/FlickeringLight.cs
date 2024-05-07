using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    [Header("Intensity Settings")] 
    [SerializeField] private float minIntensity = 0.8f;
    [SerializeField] private float maxIntensity = 1.2f; 
    [SerializeField] private float intensityFlickerSpeed = 0.1f;
    
    [Header("Movement Settings")]
    [SerializeField] private float movementRadius = 0.1f;
    [SerializeField] private float movementSpeed = 0.1f;
    
    private Light2D m_light2D;
    private Vector3 m_initialPosition;
    private float m_randomSeed;

    void Start()
    {
        m_light2D = GetComponent<Light2D>();
        m_initialPosition = transform.localPosition;
        m_randomSeed = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Intensity Flicker
        float intensityNoise = Mathf.PerlinNoise(m_randomSeed, Time.time * intensityFlickerSpeed);
        m_light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityNoise);

        // Random Movement
        float movementX = Mathf.PerlinNoise(m_randomSeed, Time.time * movementSpeed) * 2f - 1f;
        float movementY = Mathf.PerlinNoise(m_randomSeed + 1, Time.time * movementSpeed) * 2f - 1f;
        Vector3 offset = new Vector3(movementX, movementY, 0) * movementRadius;
        transform.localPosition = m_initialPosition + offset;
    }

}
