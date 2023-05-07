using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset = new Vector3(0, 20, -5);
    private int distance = 0;
    
    public static float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public static float shakeTimer = 0f;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (shakeTimer > 0)
        {
            initialPosition = player.transform.position - player.transform.forward * distance + offset;
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.position = player.transform.position - player.transform.forward * distance + offset;
        }
    }
    
    public static void Shake()
    {
        shakeTimer = shakeDuration;
    }

}
