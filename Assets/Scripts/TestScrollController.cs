using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KinoZero
{
    public enum Direction
    {
        Vertical = 0,
        Horizontal = 1,
    }
    public class TestScrollController : MonoBehaviour
    {
        [Header("Parameters"), SerializeField, Tooltip("Direction of scroll.")]
        private Direction direction = Direction.Vertical;
        [SerializeField] private GameObject BaseItem;
        [SerializeField] private int BaseItemCount;
        [SerializeField] private int TotalItemCount;
        [SerializeField] private int row;
        [SerializeField] private int collum;
        private float BaseItemHeight;
        [SerializeField] private float ItemSpacing;
        [SerializeField] private RectTransform Content;
        [SerializeField] private RectTransform Viewport;
        [SerializeField] private ScrollRect ScrollRect;
        [SerializeField] private Transform firstItem;
        [SerializeField] private Transform lastItem;
        private List<TestScrollListItem> _items = new List<TestScrollListItem>();
        private List<TestScrollListItem> sortedGameObjects;
        private List<TestScrollListItem> sortedIndex;
        private int t = -1;
        
        private void Start()
        {
            Init();
        }

        void Init()
        {
            if (BaseItemCount % 2 == 1)
            {
                BaseItemCount++;
            }
            if(BaseItemCount > TotalItemCount)
            {
                TotalItemCount = BaseItemCount;
            }
            
            BaseItemHeight = BaseItem.GetComponent<RectTransform>().rect.width;
            float temp = BaseItemHeight * TotalItemCount + ItemSpacing * (TotalItemCount + 1);
            
            
            if (direction == Direction.Vertical)
            {
                Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, temp);
                ScrollRect.vertical = true;
                ScrollRect.horizontal = false;
                for(int i = 0; i < (int)BaseItemCount/collum; i++)
                {
                    firstItem.position -= new Vector3(0,BaseItemHeight + ItemSpacing, 0);
                    for (int j = 0; j < collum ; j++)
                    {
                        GameObject go = Instantiate(BaseItem, transform.position, Quaternion.identity);
                        go.transform.SetParent(ScrollRect.content, false);
                        go.transform.position = firstItem.position + Vector3.right * (BaseItemHeight + ItemSpacing) * j;
                        //AddData();
                        _items.Add(go.GetComponent<TestScrollListItem>());
                        go.GetComponentInChildren<TextMeshProUGUI>().SetText((j+t+1).ToString());
                        go.GetComponentInChildren<TestScrollListItem>().index = (j+t+1);
                        if (j == collum - 1) 
                        {
                            t = j+t+1;
                        }
                    }
                    
                }
            }
            else
            {
                
                Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, temp);
                ScrollRect.vertical = false;
                ScrollRect.horizontal = true;
                firstItem.position -= new Vector3(0,BaseItemHeight/2 + ItemSpacing, 0);
                for(int i = 0; i < BaseItemCount; i++)
                {
                    GameObject go = Instantiate(BaseItem, transform.position, Quaternion.identity);
                    go.transform.SetParent(ScrollRect.content, false);
                    go.transform.position = firstItem.position + Vector3.right * (BaseItemHeight + ItemSpacing) * i;
                    //AddData();
                    _items.Add(go.GetComponent<TestScrollListItem>());
                    go.GetComponentInChildren<TextMeshProUGUI>().SetText(i.ToString());
                    go.GetComponentInChildren<TestScrollListItem>().index = i;
                    
                }
            }
            firstItem.gameObject.SetActive(false);
        }

        private void AddData()
        {   
            // todo: Add data to object
            
            throw new NotImplementedException();
        }

        private void Update()
        {
            if (direction == Direction.Vertical)
            {
                UpdateVertical();
            }
            else
            {
                UpdateHorizontal();
            }
        }

        private void UpdateVertical()
        {
            sortedGameObjects = _items.OrderBy(go => go.transform.position.y).ToList();
            sortedIndex = _items.OrderBy(go => go.index).ToList();
            foreach (var go in _items)
            {
                if(go.transform.position.y >= Viewport.position.y + BaseItemHeight/2)
                {
                    if (go.index <= TotalItemCount - BaseItemCount - 1  && go.index == sortedIndex[0].index)
                    {
                        go.index += BaseItemCount;
                        go.GetComponentInChildren<TextMeshProUGUI>().SetText(go.index.ToString());
                        //------------------------------------------------------------
                        // todo: Add data here
                        
                        //------------------------------------------------------------
                        go.transform.position = sortedGameObjects[0].transform.position + Vector3.down * (BaseItemHeight + ItemSpacing);
                    }
                }
                else if(go.transform.position.y <= 0 - BaseItemHeight * 1.3f)
                {
                    if (go.index >= BaseItemCount && go.index == sortedIndex[BaseItemCount-1].index)
                    {
                        go.index -= BaseItemCount;
                        go.GetComponentInChildren<TextMeshProUGUI>().SetText(go.index.ToString());
                        //------------------------------------------------------------
                        // todo: Add data here
                        //------------------------------------------------------------
                        go.transform.position = sortedGameObjects[BaseItemCount-1].transform.position + Vector3.up * (BaseItemHeight + ItemSpacing);
                    }
                }
            }
        }

        private void UpdateHorizontal()
        {
            sortedGameObjects = _items.OrderBy(go => go.transform.position.x).ToList();
            sortedIndex = _items.OrderBy(go => go.index).ToList();
            foreach (var go in _items)
            {
                if(go.transform.position.x >= Viewport.rect.width + BaseItemHeight * 4f)
                {
                    Debug.Log("someshit");
                    if (go.index >= BaseItemCount && go.index == sortedIndex[BaseItemCount-1].index)
                    {
                        go.index -= BaseItemCount;
                        go.GetComponentInChildren<TextMeshProUGUI>().SetText(go.index.ToString());
                        //------------------------------------------------------------
                        // todo: Add data here
                        //------------------------------------------------------------
                        go.transform.position = sortedGameObjects[0].transform.position + Vector3.left * (BaseItemHeight + ItemSpacing);
                    }
                }
                else if(go.transform.position.x <= 0 - BaseItemHeight * 1/2)
                {
                    
                    if (go.index <= TotalItemCount - BaseItemCount - 1  && go.index == sortedIndex[0].index)
                    {
                        go.index += BaseItemCount;
                        go.GetComponentInChildren<TextMeshProUGUI>().SetText(go.index.ToString());
                        //------------------------------------------------------------
                        // todo: Add data here
                        //------------------------------------------------------------
                        go.transform.position = sortedGameObjects[BaseItemCount-1].transform.position + Vector3.right * (BaseItemHeight + ItemSpacing);
                    } 
                }
            }
        }
    }
}
