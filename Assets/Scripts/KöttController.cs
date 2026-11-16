using UnityEngine;
using UnityEngine.Events;

public class KöttController : MonoBehaviour
{

    [SerializeField] UnityEvent OnKöttigKontaktEvent;

    [SerializeField] Collider2D collider;

    void Start()
    {

    }


    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnKöttigKontaktEvent.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }
}
