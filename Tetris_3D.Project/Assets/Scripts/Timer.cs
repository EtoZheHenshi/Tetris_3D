using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private UnityEvent onTick;
    [SerializeField] private float speed;
    [SerializeField] private float speedUp;
    private float currentTime;

    private void Start()
    {
        enabled = false;
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

    public void UpSpeed()
    {
        this.speed += speedUp;
    }
}
