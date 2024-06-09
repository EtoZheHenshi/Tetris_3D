using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tetris_Logic : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Figure currentFigure;
    [SerializeField] private GameField cubesGameField;
    [SerializeField] private UI userInterface;
    [SerializeField] private int speedUpStep;
    private int height;
    private int width;
    private bool[,] gameField;
    private bool[,] backupGameField;
    private int currentCheckPositionY;
    private int currentCheckPositionX;
    private bool isMove;
    public int Height { get { return height; } }
    public int Width { get { return width; } }
    public bool[,] GameField { get { return gameField; } }

    void Start()
    {
        isMove = false;
        height = 14;
        width = 10;
        gameField = new bool[height, width];
        cubesGameField.SetField();
        CreateNewFigure();
        timer.Activate();
    }

    void Update()
    {
        if (!isMove && Time.timeScale == 1f)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isMove = true;
                FigureFalling(KeyCode.DownArrow);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isMove = true;
                if (currentCheckPositionX != 0)
                    FigureFalling(KeyCode.LeftArrow);
                else
                    isMove = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isMove = true;
                if ((currentCheckPositionX + currentFigure.Size.GetLength(1)) != gameField.GetLength(1))
                    FigureFalling(KeyCode.RightArrow);
                else
                    isMove = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isMove = true;
                RotateFigure();
            }
        }
    }

    public void FigureFallOnTick()
    {
        if (!isMove)
        {
            isMove = true;
            FigureFalling(KeyCode.DownArrow);
        }
        else
        {
            timer.Activate();
        }
    }

    private void FigureFalling(KeyCode key)
    {
        bool isStop = false;
        backupGameField = gameField.Clone() as bool[,];
        int figureIndexY = currentFigure.Size.GetLength(0) - 1;
        int figureIndexX = 0;
        int fieldIndexXForRightMove = 0;
        for (int i = currentCheckPositionY; i > currentCheckPositionY - currentFigure.Size.GetLength(0); i--)
        {
            if (key == KeyCode.RightArrow)
            {
                figureIndexX = currentFigure.Size.GetLength(1) - 1;
                fieldIndexXForRightMove = currentCheckPositionX + currentFigure.Size.GetLength(1);
            }
            else
                figureIndexX = 0;
            for (int j = currentCheckPositionX; j < currentCheckPositionX + currentFigure.Size.GetLength(1); j++)
            {
                if (!currentFigure.Size[figureIndexY, figureIndexX])
                {
                    if (key == KeyCode.RightArrow)
                    {
                        figureIndexX--;
                        fieldIndexXForRightMove--;
                    }
                    else
                        figureIndexX++;
                    continue;
                }
                
                switch (key)
                {
                    case KeyCode.DownArrow:
                        isStop = MoveDown(i, j, figureIndexY, figureIndexX);
                        break;
                    case KeyCode.LeftArrow:
                        isStop = MoveLeft(i, j, figureIndexY, figureIndexX);
                        break;
                    case KeyCode.RightArrow:
                        isStop = MoveRight(i, fieldIndexXForRightMove, figureIndexY, figureIndexX);
                        fieldIndexXForRightMove--;
                        break;
                    default:
                        break;
                }

                if (isStop)
                    break;
                if (key == KeyCode.RightArrow)
                    figureIndexX--;
                else
                    figureIndexX++;
            }
            if (isStop) 
                break;
            if (key == KeyCode.DownArrow)
            {
                if (i == 0)
                    break;
            }
            figureIndexY--;
        }

        EndMove(isStop, key);
        isMove = false;
    }

    private bool MoveDown(int fieldIndexY, int fieldIndexX, int figureIndexY, int figureIndexX)
    {
        if (fieldIndexY == gameField.GetLength(0) || (gameField[fieldIndexY, fieldIndexX] && currentFigure.Size[figureIndexY, figureIndexX]))
        {
            gameField = backupGameField;
            return true;
        }
        else
        {
            gameField[fieldIndexY, fieldIndexX] = true;
            if (!(fieldIndexY == 0))
            {
                gameField[fieldIndexY - 1, fieldIndexX] = false;
            }
        }
        return false;
    }

    private bool MoveLeft(int fieldIndexY, int fieldIndexX, int figureIndexY, int figureIndexX)
    {
        if (fieldIndexY > 0)
        {
            if (gameField[fieldIndexY - 1, fieldIndexX - 1] && currentFigure.Size[figureIndexY, figureIndexX])
            {
                gameField = backupGameField;
                return true;
            }
            else
            {
                gameField[fieldIndexY - 1, fieldIndexX - 1] = true;
                gameField[fieldIndexY - 1, fieldIndexX] = false;
            }
        }
        return false;
    }

    private bool MoveRight(int fieldIndexY, int fieldIndexX, int figureIndexY, int figureIndexX)
    {
        if (fieldIndexY > 0)
        {
            if (gameField[fieldIndexY - 1, fieldIndexX] && currentFigure.Size[figureIndexY, figureIndexX])
            {
                gameField = backupGameField;
                return true;
            }
            else
            {
                gameField[fieldIndexY - 1, fieldIndexX] = true;
                gameField[fieldIndexY - 1, fieldIndexX - 1] = false;
            }
        }
        return false;
    }

    private void EndMove(bool isStop, KeyCode key)
    {
        cubesGameField.ChangeSetActive();
        if (isStop && key == KeyCode.DownArrow)
            StopFalling();
        else if (key == KeyCode.DownArrow)
        {
            currentCheckPositionY++;
            timer.Activate();
        }
        else if (!isStop && key == KeyCode.LeftArrow)
            currentCheckPositionX--;
        else if (!isStop && key == KeyCode.RightArrow)
            currentCheckPositionX++;
    }

    private void StopFalling()
    {
        LineDestructionCheck();
        cubesGameField.ChangeSetActive();
        if (GameOverCheck())
            userInterface.GameOver();
        CreateNewFigure();
        timer.Activate();
    }

    private void RotateFigure()
    {
        backupGameField = gameField.Clone() as bool[,];
        currentCheckPositionY--;
        DeleteFigureInField();
        currentFigure.Rotate();
        bool isMistake = false;
        for (int i = 0; i < currentFigure.Size.GetLength(0); i++)
        {
            int fieldIndexY = currentCheckPositionY - currentFigure.Size.GetLength(0) + i;

            if (fieldIndexY >= 0)
            {
                for (int j = 0; j < currentFigure.Size.GetLength(1); j++)
                {
                    if (currentCheckPositionX + j >= gameField.GetLength(1) || 
                        (currentFigure.Size[i, j] && gameField[fieldIndexY, currentCheckPositionX + j]))
                    {
                        isMistake = true;
                        break;
                    }
                }
            }

            if (isMistake)
                break;
        }

        if (isMistake)
        {
            gameField = backupGameField;
            currentCheckPositionY++;
            currentFigure.CancelRotate();
            timer.Activate();
            isMove = false;
        }
        else
        {
            FigureFalling(KeyCode.DownArrow);
        }
    }

    private void DeleteFigureInField()
    {
        int figureIndexY = currentFigure.Size.GetLength(0) - 1;
        int figureIndexX = 0;
        for (int i = currentCheckPositionY; i > currentCheckPositionY - currentFigure.Size.GetLength(0); i--)
        {
            figureIndexX = 0;
            for (int j = currentCheckPositionX; j < currentCheckPositionX + currentFigure.Size.GetLength(1); j++)
            {
                if (!currentFigure.Size[figureIndexY, figureIndexX])
                {
                    figureIndexX++;
                    continue;
                }
                if (i >= 0)
                    gameField[i, j] = false;
                figureIndexX++;
            }
            figureIndexY--;
        }
    }

    private void LineDestructionCheck()
    {
        bool isDetruction;
        for (int i = 0; i < gameField.GetLength(0); i++)
        {
            isDetruction = true;
            for (int j = 0; j < gameField.GetLength(1); j++)
            {
                if (!gameField[i, j])
                {
                    isDetruction = false;
                    break;
                }
            }
            if (isDetruction)
            {
                LineDestruction(i);
                userInterface.UpdateScore(100);
                if (userInterface.Score % speedUpStep == 0)
                {
                    timer.UpSpeed();
                }
            }
        }
    }

    private void LineDestruction(int indexY)
    {
        for (int i = indexY; i >= 0; i--)
        {
            for (int j = 0; j < gameField.GetLength(1); j++)
            {
                if (i == 0)
                    gameField[i, j] = false;
                else
                    gameField[i, j] = gameField[i - 1, j];
            }
        }
    }

    private bool GameOverCheck()
    {
        for (int i = 0; i < gameField.GetLength(1); i++)
        {
            if (gameField[0, i])
            {
                return true;
            }
        }
        return false;
    }

    private void CreateNewFigure()
    {
        currentFigure.CreateFigure();
        currentCheckPositionY = 0;
        currentCheckPositionX = gameField.GetLength(1) / 2 - currentFigure.Size.GetLength(1) / 2;
    }
}
