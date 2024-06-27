using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable] public struct Data
    {
        [TextArea(5, 5)]
        public string[] showingTexts;
    }

    public Data[] data = new Data[4];
}
