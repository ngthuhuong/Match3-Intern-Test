using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomController : MonoBehaviour
{
    private Vector3 startPosition;
    private List<Cell> listCell;
    
    
    
    void Start()
    {
        startPosition = new Vector3(this.transform.position.x-2.5f, this.transform.position.y -2.5f, this.transform.position.z); 
        listCell = new List<Cell>();
    }

    public void GetItem(Cell c)
    {
        GameObject item = c.gameObject;
        Vector3 pos = item.transform.position;
        item.transform.position = Vector3.Lerp(pos,startPosition, 0.5f);
        listCell.Add(c);
        CheckMatches();
        startPosition += new Vector3(0.5f, 0f, 0f);
    }

    public void CheckMatches()
    {
        List<Cell> match = new List<Cell>();
        for (int i = 0; i < listCell.Count-1; i++)
        {
            for (int j = 1; j < listCell.Count; j++)
            {
                if (listCell[i].IsSameType(listCell[j]))
                {
                    if(!match.Contains(listCell[i])) match.Add(listCell[i]);
                    if (!match.Contains(listCell[j])) match.Add(listCell[j]);
                    if (match.Count == 3)
                    {
                        foreach (var x in listCell)
                        {
                            x.ExplodeItem();
                        }
                    }
                    
                }
            }
            match.Clear();
        }
    }
}
