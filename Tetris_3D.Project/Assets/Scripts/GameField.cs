using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private Tetris_Logic logic;
    [SerializeField] private GameObject cube;
    private GameObject[,] cubesGameField;
    void Start()
    {
    }

    void Update()
    {   
    }

    public void SetField()
    {
        cubesGameField = new GameObject[logic.Height, logic.Width];
        for (int i = 0; i < logic.Height; i++)
        {
            for (int j = 0; j < logic.Width; j++)
            {
                cubesGameField[i, j] = Instantiate(cube, new Vector3(j, logic.Height - i), new Quaternion());
                cubesGameField[i, j].transform.SetParent(transform);
            }
        }
    }
    public void ChangeSetActive()
    {
        for (int i = 0; i < logic.Height; i++)
        {
            for (int j = 0; j < logic.Width; j++)
            {
                cubesGameField[i, j].SetActive(logic.GameField[i, j]);
            }
        }
    }
}
