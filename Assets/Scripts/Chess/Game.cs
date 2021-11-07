﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8];

    // Cantidad de enemigos --> seguramente cambiara en cada nivel
    // O puede ser la cantidad de piezas en pantalla --> siendo pieces[0] la pieza del jugador ?¿
    public GameObject playerKnight;
    private GameObject Goal;
    public int GoalPosX = 5;
    public int GoalPosY = 6;
    public GameObject[] pieces = new GameObject[8]; 

    //Game Ending
    private bool gameOver = false;
    public bool win = false;

    /*
        There are 2 enemy pieces: rook and bishop:

        The rook can move Bottom-Top, Top-Bottom, Left-Right, Rigth-Left:
            - Bottom-Top: "rookVertical_BottomToTop"
            - Top-Bottom: "rookVertical_TopToBottom"
            - Left-Right: "rookHorizontal_LeftToRight"
            - Right-Left: "rookHorizotanl_RightToLeft"

        The bishop has a similar movement but diagonally:
            - Left-Right-Top: "bishopLeft_Top"
            - Left-Right-Bottom: "bishopLeft_Bottom"
            - Right-Left-Top: "bishopRight_Top"
            - Right-Left-Bottom: "bishopRight_Bottom"
    */

    public void Start()
    {
        // Creamos las piezas en una posicion del tablero
        pieces = new GameObject[] { Create("rookVertical_BottomToTop", 3, 0, true), /*Create("rookVertical_TopToBottom", 6, 7, false),*/
                                    /*Create("rookHorizontal_LeftToRight", 0, 5, true),*/ /*Create("rookHorizotanl_RightToLeft", 7, 4, false),*/

                                    /*Create("bishopLeft_Top", 2, 7, false),*/ /*Create("bishopLeft_Bottom", 7, 0, true),
                                    Create("bishopRight_Top", 5, 7, false),*/ Create("bishopRight_Bottom", 0, 0, true)};

        playerKnight = Create("player", 4, 4);
        Goal = Create("goal", GoalPosX, GoalPosY);

        //Set all piece positions on the positions board
        for (int i = 0; i < pieces.Length; i++)
        {
            SetPosition(pieces[i]);
        }
        SetPosition(playerKnight);
        SetPosition(Goal);
    }

    public GameObject Create(string name, int x, int y, bool forward)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetForward(forward);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void MoveEnemies()
    {
        
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                GameObject obj = pieces[i];
                Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script

                int boardX = cm.GetXBoard();
                int boardY = cm.GetYBoard();

                //Set the Chesspiece's original location to be empty
                SetPositionEmpty(boardX, boardY);

                if (cm.name == "rookVertical_BottomToTop" || cm.name == "rookVertical_TopToBottom")
                {
                    MoveRook(obj, true);
                }
                else if (cm.name == "rookHorizontal_LeftToRight" || cm.name == "rookHorizotanl_RightToLeft")
                {
                    MoveRook(obj, false);
                }
                else if (cm.name == "bishopLeft_Top" || cm.name == "bishopLeft_Bottom")
                {
                    MoveBishop(obj, true);
                }
                else
                {
                    MoveBishop(obj, false);
                }
              }
          }
    }

    void MoveBishop(GameObject obj, bool left)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        int aux;

        if (cm.GetForward() == true)
        {
            if (left)
            {
                aux = cm.GetXBoard();
                for (int i = cm.GetYBoard() + 1; i <= aux; i++)
                {
                    cm.SetXBoard(cm.GetXBoard() - 1);
                    cm.SetYBoard(i);
                    if (CheckCollision(cm.GetXBoard(), i)) break;

                }
                cm.SetBishopCoordsCoroutine();
            }
            else
            {
                aux = 7 - cm.GetXBoard();
                for (int i = cm.GetYBoard() + 1; i <= aux; i++)
                {
                    cm.SetXBoard(cm.GetXBoard() + 1);
                    cm.SetYBoard(i);
                    if (CheckCollision(cm.GetXBoard(), i)) break;
                }
                cm.SetBishopCoordsCoroutine();
            }

            cm.SetForward(false);
        }
        else // !cm.GetForward()
        {
            if (left)
            {
                aux = cm.GetXBoard();
                for (int i = cm.GetYBoard() - 1; i >= aux; i--)
                {
                    cm.SetXBoard(cm.GetXBoard() + 1);
                    cm.SetYBoard(i);
                    if (CheckCollision(cm.GetXBoard(), i)) break;
                }
                cm.SetBishopCoordsCoroutine();
            }
            else
            {
                aux = 7 - cm.GetXBoard();
                for (int i = cm.GetYBoard() - 1; i >= aux; i--)
                {
                    cm.SetXBoard(cm.GetXBoard() - 1);
                    cm.SetYBoard(i);
                    if (CheckCollision(cm.GetXBoard(), i)) break;
                }
                cm.SetBishopCoordsCoroutine();
            }

            cm.SetForward(true);
        }
        SetPosition(obj);
    }

    void MoveRook(GameObject obj, bool vertical)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        if (cm.GetForward() == true)
        {
            if (vertical)
            {
                for (int i = cm.GetYBoard() + 1; i <= 7; i++)
                {
                    cm.SetXBoard(cm.GetXBoard());
                    cm.SetYBoard(i);
                    
                    if (CheckCollision(cm.GetXBoard(), i)) break;

                }
                cm.SetRookCoordsCoroutine();
            }
            else
            {
                for (int i = cm.GetXBoard() + 1; i <= 7; i++)
                {
                    cm.SetXBoard(i);
                    cm.SetYBoard(cm.GetYBoard());
                    if (CheckCollision(i, cm.GetYBoard())) break;
                }
                cm.SetRookCoordsCoroutine();
            }

            cm.SetForward(false);
        }
        else // !cm.GetForward()
        {
            if (vertical)
            {
                for (int i = cm.GetYBoard() - 1; i >= 0; i--)
                {
                    cm.SetXBoard(cm.GetXBoard());
                    cm.SetYBoard(i);
                    if (CheckCollision(cm.GetXBoard(), i)) break;
                }
                cm.SetRookCoordsCoroutine();
            }
            else
            {
                for (int i = cm.GetXBoard() - 1; i >= 0; i--)
                {
                    cm.SetXBoard(i);
                    cm.SetYBoard(cm.GetYBoard());
                    if (CheckCollision(i, cm.GetYBoard())) break;
                }
                cm.SetRookCoordsCoroutine();
            }

            cm.SetForward(true);
        }
        SetPosition(obj);
    }

    public bool CheckCollision(int x, int y)
    {
        Chessman player = playerKnight.GetComponent<Chessman>();
        int playerX = player.GetXBoard();
        int playerY = player.GetYBoard();

        if (x == playerX && y == playerY )
        {
            // Temporal
            //SceneManager.LoadScene("Victor");
            // Temporal
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            if (win)
            {
                // next level
            }
            else
            {
                // repeat level
            }
        }
        else if (win)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                Destroy(pieces[i]);
                // Temporal
                SceneManager.LoadScene("Victor");
                // Temporal
            }
        }
    }
    
    public void Winner(string text)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("NextLevelTextChess").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("NextLevelTextChess").GetComponent<Text>().text = "Click for the next level";
    }
}
