using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Figure : MonoBehaviour
{
    private bool[,] size;
    private bool[,] backupArray;
    private Random rand;

    public bool[,] Size { get { return size; } }

    void Start()
    {
        size = new bool[4, 4];
        backupArray = new bool[0,0];
        rand = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFigure()
    {
        switch (rand.Next(3))
        {
            case 0:
                size = new bool[,] 
                {
                    { true, true, true, true}
                };
                break;
            case 1:
                size = new bool[,]
                {
                    { true, true },
                    { true, true }
                };
                break;
            case 2:
                size = new bool[,]
                {
                    { false, true, true },
                    { true, true, false }
                };
                break;
            default:
                break;
        }
    }

    public void Rotate()
    {
        backupArray = size;
        bool[,] rotateArray = new bool[size.GetLength(1), size.GetLength(0)];
        for (int i = 0; i < rotateArray.GetLength(0); i++)
        {
            int k = rotateArray.GetLength(1) - 1;
            for (int j = 0; j < rotateArray.GetLength(1); j++)
            {
                rotateArray[i, j] = size[k, i];
                k--;
            }
        }
        size = rotateArray;
    }

    public void CancelRotate()
    {
        size = backupArray;
    }
}
