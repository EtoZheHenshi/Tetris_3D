using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private UnityEvent onTick;
    private float currentTime;
    private float speed;

    private void Start()
    {
        enabled = false;
        speed = 1f;
    }

    void Update()
    {
        currentTime = currentTime - Time.deltaTime * speed;
        if (currentTime <= 0f)
        {
            enabled = false;
            onTick.Invoke();
        }
    }

    public void Activate()
    {
        currentTime = maxTime;
        enabled = true;
    }
}
