﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Clearing
{
    public float delay;
    public Transform Obj;
}

public class Grid : MonoBehaviour {
    public static float size = 1.28f;
    public static int iGridW = 10;
    public static int iGridH = 20;
    private static Transform[,] tGrid;
    public Sprite sConnected;
    public static Sprite Connected;
    public GameObject Score;
    private static List<Clearing> tClear;

    private static int iScore;
    private int iShowScore;
    private float fTime;

	// Use this for initialization
	void Start () {
        tGrid = new Transform[iGridW, iGridH];
        tClear = new List<Clearing>();
        Connected = sConnected;
        iShowScore = 0;
        iScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var Elapsed = Time.time - fTime;
        if (Elapsed > 0.01)
        {
            if (iScore != iShowScore)
            {
                var dif = (iScore - iShowScore) / 4.0f;
                if (dif < 1)
                {
                    iShowScore = iScore;
                }
                else
                {
                    iShowScore += (int)dif;
                }

                Score.GetComponent<Text>().text = "Score: " + iShowScore;
            }

            List<Clearing> ttClear = new List<Clearing>();
            
            //foreach (Clearing child in tClear)
            for(int i = 0; i< tClear.Count; i++)
            {
                if(tClear[i].delay > Elapsed) {
                    Clearing child = tClear[i];
                    child.delay -= Elapsed;
                    tClear[i] = child;
                    continue;
                }
                var dist = (new Vector3(14,22,0)) - tClear[i].Obj.position;
                if (dist.magnitude < 1)
                {
                    ttClear.Add(tClear[i]);
                    iScore += 10;
                }
                else
                {
                    tClear[i].Obj.position += dist * 0.1f;
                }
            }
            foreach (Clearing child in ttClear)
            {

                tClear.Remove(child);
                Destroy(child.Obj.gameObject);
            }

            fTime = Time.time;
        }
    }
    
    public static bool PointIn(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < iGridW && y < iGridH);
    }
    public static bool isFree(int Col, int Row)
    {
        return tGrid[Col, Row] == null;
    }
    public static void Attach(Transform tObject, int Col, int Row)
    {
        tGrid[Col, Row] = tObject;
    }
    public static void Remove(int Col, int Row)
    {
        tGrid[Col, Row] = null;
    }
    public static void RemoveChildOf(Transform tParent)
    {
        for(int j = 0; j < iGridH; j++)
        {
            for (int i = 0; i < iGridW; i++)
            {
                if (tGrid[i, j] != null && tGrid[i, j].parent == tParent)
                    tGrid[i, j] = null;
            }
        }
    }
    public static Transform Get(int Col, int Row)
    {
        if (Col < iGridW && Row < iGridH) return tGrid[Col, Row];
        throw new System.ArgumentException("parameters out of range", "col or row");
        //return null;
    }
    public static void FindCompleteRows()
    {
        int c = 0;
        for (int j = 0; j < iGridH; )
        {
            if (!RowCompleted(j)) { j++; continue; }
            ClearRow(j, c);
            c++;
        }
    }
    public static void Connect(int Col, int Row)
    {
        Get(Col, Row).GetComponent<SpriteRenderer>().sprite = Connected;
    }

    private static void ClearRow(int Row, int cleared)
    {
        for (int i = 0; i < iGridW; i++)
        {
            //Destroy(tGrid[i, Row].gameObject);
            Clearing child;
            if (cleared % 2 == 0)
            {
                child.delay = 0.1f * i + cleared;
            }
            else
            {
                child.delay = 0.1f * (9 - i) + cleared;
            }
            child.Obj = tGrid[i, Row];
            tClear.Add(child);
            tGrid[i, Row] = null;
        }

        for (int j = Row + 1; j < iGridH; j++)
            for (int i = 0; i < iGridW; i++)
            {
                if(tGrid[i,j] != null)
                {
                    tGrid[i, j].position -= new Vector3(0, size, 0);
                    tGrid[i, j - 1] = tGrid[i, j];
                    tGrid[i, j] = null;
                }
            }
    }
    private static bool RowCompleted(int Row)
    {
        for (int i = 0; i < iGridW; i++)
            if (tGrid[i, Row] == null) return false;
        return true;
    }
}
