using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

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

    public void UpdatePos()
    {
        defPos = gameObject.transform.position+ Vector3.left;
        startPosition = defPos ;
    }

    public void GetItem(Cell c)
    {
        c.IsMoved = false;
        Item view = c.Item; 
        listCell.Add(c);
        startPosition = defPos + (listCell.Count - 1) * 0.5f * Vector3.right;
        view.AnimationMoveToBottom(startPosition);
        c.transform.DOMove(startPosition, 0.2f).OnComplete(() =>
        {
            if (c != null && c.gameObject != null) 
            {
                CheckMatches();
            }
        });
        
    }

    public void GetItemAuto(Cell c,bool isWinMode)
    {
        c.IsMoved = false;
        Item view = c.Item; 
        listCell.Add(c);
        startPosition = defPos + (listCell.Count - 1) * 0.5f * Vector3.right;
        view.AnimationMoveToBottom(startPosition);
        c.transform.DOMove(startPosition, 0.2f).OnComplete(() =>
        {
            if (isWinMode && listCell.Count == 3)
                    {
                        List<Cell> cellsToRemove = new List<Cell>(listCell);
                        
                        foreach (var x in cellsToRemove)
                        {
                            listCell.Remove(x);
                            x.ExplodeItem();
                            x.transform.DOKill();
                            if (x.Item != null && x.Item.View != null)
                            {
                                x.Item.View.DOKill();
                            }
                            GameObject.Destroy(x.gameObject);
                        }
                        if (listCell.Count == 0) startPosition = defPos;
                        else startPosition = defPos + (listCell.Count - 1) * 0.5f * Vector3.right;
                    }
        });
        
    }

    private void Rearrage()
    {
        for (int i = 0; i < listCell.Count; i++)
        {
            if (listCell[i] == null) continue;
            Vector3 offset = defPos + i * 0.5f * Vector3.right;
            listCell[i].Item.AnimationMoveToBottom(offset);
            listCell[i].transform.DOMove(offset, 0.2f);
        }
    }
    
    public void CheckMatches()
    {
        List<Cell> match = new List<Cell>();
        for (int i = 0; i < listCell.Count; i++)
        {
            if (listCell[i] == null) continue;

            for (int j = i + 1; j < listCell.Count; j++)
            {
                if (listCell[j] == null) continue;

                if (listCell[i].IsSameType(listCell[j]))
                {
                    if (!match.Contains(listCell[i])) match.Add(listCell[i]);
                    if (!match.Contains(listCell[j])) match.Add(listCell[j]);

                    if (match.Count == 3)
                    {
                        foreach (var x in match)
                        {
                            listCell.Remove(x);
                            x.ExplodeItem();
                            x.transform.DOKill();
                            if (x.Item != null && x.Item.View != null)
                            {
                                x.Item.View.DOKill();
                            }
                            GameObject.Destroy(x.gameObject);
                        }
                        if (listCell.Count == 0) startPosition = defPos;
                        else startPosition = defPos + (listCell.Count - 1) * 0.5f * Vector3.right;
                        match.Clear();
                        Rearrage();
                        return; 
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

    public void ClearBottom()
    {
        listCell.Clear();
        UpdatePos();
    }
}
