using System;
using Unity.VisualScripting;
using UnityEngine;

public class BatMovementScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float y_koefficient;
    [SerializeField] float wave_length;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var y = MathF.Sin(transform.position.x*wave_length)*y_koefficient;
        transform.position -=  new Vector3(speed,y,0)* Time.deltaTime;
    }
}
