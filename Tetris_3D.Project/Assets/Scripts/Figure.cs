using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Figure : MonoBehaviour
{
    private bool[,] size;
    private Random rand;

    public bool[,] Size { get { return size; } }

    void Start()
    {
        size = new bool[4, 4];
        rand = new Random();
        CreateFigure();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFigure()
    {
        switch (rand.Next(2))
        {
            case 0:
                size = new bool[,] 
                {
                    { false, false, false, false},
                    { false, false, false, false},
                    { true, true, true, true},
                    { false, false, false, false}
                };
                break;
            case 1:
                size = new bool[,]
                {
                    { false, false, false, false},
                    { false, true, true, false},
                    { false, true, true, false},
                    { false, false, false, false}
                };
                break;
            default:
                break;
        }
    }
}
