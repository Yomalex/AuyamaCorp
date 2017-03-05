using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPiece : MonoBehaviour {
    public GameObject[] Pieces;
    public Color[] PieceColors;
    private List<GameObject> PiecesClone;
    private float Elapsed;
	// Use this for initialization
	void Start () {
        PiecesClone = new List<GameObject>();
        Add();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - Elapsed > 1)
        {
            List<GameObject> forDestroy = new List<GameObject>();
            foreach(GameObject prefab in PiecesClone)
            {
                if(prefab == null && !ReferenceEquals(prefab, null))
                {
                    PiecesClone.Remove(prefab);
                }
            }
            foreach (GameObject prefab in PiecesClone)
            {
                int c = 0;
                foreach (Transform child in prefab.transform)
                {
                    c++;
                }
                if (c == 0)
                {
                    forDestroy.Add(prefab);
                }
            }
            foreach (GameObject prefab in forDestroy)
            {
                PiecesClone.Remove(prefab);
                Destroy(prefab);
            }
                Elapsed = Time.time;
        }
    }

    public void Add()
    {
        int id = Random.Range(0, Pieces.Length);
        GameObject prefab = Instantiate(Pieces[id], transform.position, Quaternion.identity);
        foreach(SpriteRenderer sRenderer in prefab.GetComponentsInChildren<SpriteRenderer>())
        {
            PieceColors[id].a = 1.0f;
            sRenderer.color = PieceColors[id];
        }
        PiecesClone.Add(prefab.gameObject);
    }
}
