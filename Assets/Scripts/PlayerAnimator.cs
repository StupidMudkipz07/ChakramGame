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
        player.AnimatorEventRising.AddListener(StartRising);
    }

    void StartWalk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("LookUp", false);
    }

    void StartIdle()
    {
        animator.SetBool("Walk", false);
    }

    void StartLookUp()
    {
        animator.SetBool("LookUp", true);
    }
    
    void StartRising()
    {
        animator.SetBool("Rising", true);
    }

    void Update()
    {
        
    }
}