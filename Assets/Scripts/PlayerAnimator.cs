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
        player.AnimatorEventLookUp.AddListener(StartLookUp);
        player.OnJump.AddListener(OnPlayerJump);
    }

    void StartWalk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("LookingUp", false);
    }

    void StartIdle()
    {
        animator.SetBool("Walk", false);
    }

    void StartLookUp()
    {
        animator.SetBool("LookingUp", true);
    }

    void OnPlayerJump()
    {
        animator.SetBool("Walk", false);
    }

    void Update()
    {
        animator.SetInteger("yspeed", (int)player.velocity.y);
    }
}