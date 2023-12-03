// Scrmizu C# reference source
// Copyright (c) 2016-2020 COMCREATE. All rights reserved.

using System;
using System.Linq;
using Scrmizu.Sample;
using UnityEngine;
using UnityEngine.UI;

namespace Scrmizu.Sample
{
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalScrollItem : MonoBehaviour, IInfiniteScrollItem
    {
        private ScrollItemData _data;

        private Text _title;
        private Text _count;
        private Text _value;


        private Text Title => _title != null
            ? _title
            : _title = GetComponentsInChildren<Text>().FirstOrDefault(text => text.name == "Title");

        private Text Count => _count != null
            ? _count
            : _count = GetComponentsInChildren<Text>().FirstOrDefault(text => text.name == "Count");

        private Text Value => _value != null
            ? _value
            : _value = GetComponentsInChildren<Text>().FirstOrDefault(text => text.name == "Value");

        public void UpdateItemData(object data)
        {
            Debug.Log($"UpdateItemData {data}");
            if (!(data is ScrollItemData scrollingItemData)) return;
            gameObject.SetActive(true);
            if (_data == scrollingItemData) return;
            _data = scrollingItemData;
            Title.text = _data.title;
            Count.text = $"Count {_data.count:00}";
            Value.text = "横幅の最低サイズはこのぐらいです。　　　　\n" + _data.value.Replace("\n", "");

            // todo: Set text value and color

            if (data is CustomDAta scrollingItemData2)
            {
                Title.color = scrollingItemData2.titleColor;
                Count.color = scrollingItemData2.countColor;
                Value.color = scrollingItemData2.valueColor;

                // to changed data when scrolling
            }
            else
            {
                Debug.LogError("data is not CustomData");
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class CustomDAta : ScrollItemData
{
    public Color titleColor;
    public Color countColor;
    public Color valueColor;

    public CustomDAta(string title, int count, string value, Color titleColor, Color countColor, Color valueColor)
    {
        this.titleColor = titleColor;
        this.countColor = countColor;
        this.valueColor = valueColor;
    }
}