using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BottomController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 defPos;
    private List<Cell> listCell;
    
    
    
    void Start()
    {
        defPos = gameObject.transform.position+ Vector3.left;
        startPosition = defPos ;
        listCell = new List<Cell>();
    }

    public void GetItem(Cell c)
    {
        c.IsMoved = false;
        Item view = c.Item; 
        listCell.Add(c);
        view.AnimationMoveToBottom(startPosition);
        c.transform.DOMove(startPosition, 0.2f).OnComplete(() =>
        {
            CheckMatches();
            Rearrage();
            startPosition += new Vector3(0.5f, 0f, 0f);
        });
        
    }

    private void Rearrage()
    {
        for (int i = 0; i < listCell.Count; i++)
        {
            Vector3 offset = defPos + i * 0.5f * Vector3.right;
            listCell[i].Item.AnimationMoveToBottom(offset);
            listCell[i].transform.DOMove(offset, 0.2f);
        }
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
                    if(j>i+1) Swap(listCell[j],listCell[i+1]);
                    if(!match.Contains(listCell[i])) match.Add(listCell[i]);
                    if (!match.Contains(listCell[j])) match.Add(listCell[j]);
                    if (match.Count == 3)
                    {
                        foreach (var x in match)
                        {
                            x.ExplodeItem();
                            GameObject.Destroy(x.gameObject);
                            listCell.Remove(x);
                            startPosition-=  new Vector3(0.5f, 0f, 0f);
                        }
                    }
                    
                }
            }
            match.Clear();
        }
    }
    public void Swap(Cell cell1, Cell cell2)
    {
        Item item = cell1.Item;
        cell1.Free();
        Item item2 = cell2.Item;
        cell1.Assign(item2);
        cell2.Free();
        cell2.Assign(item);

        item.View.DOMove(cell2.transform.position, 0.3f);
        item2.View.DOMove(cell1.transform.position, 0.3f);
    }
    public bool isFull()
    {
        return listCell.Count == 5 ? true: false;
    }
}
