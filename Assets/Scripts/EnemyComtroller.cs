using UnityEngine;

public class EnemyComtroller : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float gravity = 10;

    [Header("References")]
    [SerializeField] Transform Target;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CollisionChecker _groundCheck;

    public Vector2 velocity;

    void FixedUpdate()
    {
        StateCheck();

        ChangeVelocity();
    }

    void Grounded()
    {
        velocity = new(moveSpeed, 0);
    }

    void Airborne()
    {
        velocity.x *= 0.5f * Time.fixedDeltaTime;
        velocity.y -= gravity * Time.fixedDeltaTime;
    }


    void StateCheck()
    {
        //grounded
        if (_groundCheck.IsTouching)
        {
            Grounded();
        }
        //airborne
        else
        {
            Airborne();
        }
    }

    void ChangeVelocity()
    {
        _rb.linearVelocity = velocity;
    }
}
