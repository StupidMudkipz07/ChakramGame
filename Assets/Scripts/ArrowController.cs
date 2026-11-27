using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class ArrowController : MonoBehaviour
{
    PlayerInput _input;

    [Header("References")] [SerializeField]
    PlayerController player;

    [SerializeField] InputHandler _inputHandler;

    void Update()
    {
        _input = _inputHandler.GetPlayerInput();
    }

    void FixedUpdate()
    {
        Vector2 direction;
        if (_input.AimDirection == Vector2.zero)
        {
            direction = player.FacingDirection;
        }
        else
        {
            direction = _input.AimDirection;
        }

        direction.Normalize(); 
        // vector2 to degrees                                                      matte sloppet
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //float angle = Mathf.Atan2(player.velocity.y, player.velocity.x) * Mathf.Rad2Deg;

        // make the angle into quaternion
        var rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;
    }
}