using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Logic : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Figure currentFigure;
    private int height;
    private int width;
    private bool[,] gameField;
    private int currentFigurePositionY;
    private int currentFigurePositionX;

    void Start()
    {
        height = 15;
        width = 8;
        gameField = new bool[height, width];
        currentFigurePositionX = gameField.GetLength(1) / 2 - 2;
        timer.Activate();
    }

    void Update()
    {
        Debug.Log(currentFigurePositionY);
    }

    public void FigureFalling()
    {
        bool isStop = false;
        bool[,] tempArray = gameField.Clone() as bool[,];
        int figureHeight = currentFigure.Size.GetLength(0);
        int figureWidth;
        for (int i = currentFigurePositionY; i > currentFigurePositionY - currentFigure.Size.GetLength(0); i--)
        {
            figureWidth = 0;
            for (int j = currentFigurePositionX; j < currentFigurePositionX + currentFigure.Size.GetLength(1); j++)
            {
                if (i == gameField.GetLength(0))
                {
                    i--;
                    figureHeight--;
                }
                if (i == gameField.GetLength(0) + 1)
                {
                    i -= 2;
                    figureHeight -= 2;
                }
                if (gameField[i, j] && !currentFigure.Size[figureHeight - 1, figureWidth])
                {
                    continue;
                }
                if ((gameField[i, j] && currentFigure.Size[figureHeight - 1, figureWidth]) || i == gameField.GetLength(0) + 2)
                {
                    gameField = tempArray;
                    isStop = true;
                    break;
                }
                else
                {
                    gameField[i, j] = currentFigure.Size[figureHeight - 1, figureWidth++];
                    if (!(i == 0))
                    {
                        gameField[i - 1, j] = false;
                    }
                }
            }
            if (isStop) 
                break;
            figureHeight--;
            if (i == 0)
            {
                break;
            }
        }
        if (isStop)
            StopFalling();
        else
            currentFigurePositionY++;
        timer.Activate();
    }

    private void StopFalling()
    {
        LineDistractionCheck();
        GameOverCheck();
        currentFigurePositionY = 0;
        currentFigurePositionX = gameField.GetLength(1) / 2 - 2;
        currentFigure.CreateFigure();
        timer.Activate();
    }

    private void LineDistractionCheck()
    {

    }

    private void GameOverCheck()
    {
    }
}
