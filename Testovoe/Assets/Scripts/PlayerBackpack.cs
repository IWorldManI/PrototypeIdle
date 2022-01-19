using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public int _woodCount;
    [SerializeField]List<GameObject> ItemsInBackpack = new List<GameObject>();


    public void CleanInventory()
    {
        for (int i = 0; i < ItemsInBackpack.Count; i++)  //disable all items 
        {
            ItemsInBackpack[i].SetActive(false);
        }
        //ItemsInBackpack[dataIndex].SetActive(true); 
    }
    public void EnableVisualWood()
    {
        if (_woodCount <= 3)
        {
            for (int i = 0; i < _woodCount; i++)  //disable all items 
            {
                ItemsInBackpack[i].SetActive(true);
            }
        }
    }
}
