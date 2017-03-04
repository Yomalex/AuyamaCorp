using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    private float Elapsed;
	// Use this for initialization
	void Start () {
		if(!isValidPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-Grid.size, 0, 0);

            // See if valid
            if (isValidPos())
                // Its valid. Update grid.
                updateGrid();
            else
                // Its not valid. revert.
                transform.position += new Vector3(Grid.size, 0, 0);
        }else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(Grid.size, 0, 0);

            // See if valid
            if (isValidPos())
                // Its valid. Update grid.
                updateGrid();
            else
                // Its not valid. revert.
                transform.position += new Vector3(-Grid.size, 0, 0);
        }else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                transform.Rotate(0, 0, 90);
        }else if(Input.GetKeyDown(KeyCode.DownArrow) ||
             Time.time - Elapsed >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -Grid.size, 0);

            // See if valid
            if (isValidPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, Grid.size, 0);
                foreach (Transform child in transform)
                {
                    int x = (int)Mathf.Round(child.position.x / Grid.size);
                    int y = (int)Mathf.Round(child.position.y / Grid.size);
                    Grid.Connect(x, y);
                }

                // Clear filled horizontal lines
                Grid.FindCompleteRows();

                // Spawn next Group
                FindObjectOfType<NewPiece>().Add();

                // Disable script
                enabled = false;
            }
            Elapsed = Time.time;
        }
	}

    bool isValidPos()
    {
        foreach(Transform child in transform)
        {
            int x = (int)Mathf.Round(child.position.x / Grid.size);
            int y = (int)Mathf.Round(child.position.y / Grid.size);

            if (
                !Grid.PointIn(x, y) ||
                (!Grid.isFree(x, y) && Grid.Get(x, y).parent != transform)
                )
            {
                Debug.Log("Invalid pos At X:"+x+" Y:"+y);
                return false;
            }
        }
        return true;
    }
    void updateGrid()
    {
        Grid.RemoveChildOf(transform);
        foreach(Transform child in transform)
        {
            int x = (int)Mathf.Round(child.position.x / Grid.size);
            int y = (int)Mathf.Round(child.position.y / Grid.size);

            Grid.Attach(child, x, y);
        }
    }
}
