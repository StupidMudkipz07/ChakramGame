using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] PlayerController player;
    //jag vill inte behöva coupla spelaren med andra objekt, kan man skicka med sådan data i unityevent?


    Vector2 lastPlayerVelocity = Vector2.zero;

    void Start()
    {
        player.AnimatorEventWalk.AddListener(StartWalk);
        player.AnimatorEventIdle.AddListener(StartIdle);
    }

    void StartWalk()
    {
        animator.SetTrigger("Walk");
    }

    void StartIdle()
    {
        animator.SetTrigger("Idle");
    }

    void Update()
    {
        animator.SetInteger("xSpeed", (int)player.velocity.x);
        animator.SetInteger("ySpeed", (int)player.velocity.y);


    }
}