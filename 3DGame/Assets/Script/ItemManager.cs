using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct itemObjInfo
{
    public GameObject item;
    public string itemName;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    [SerializeField]
    List<itemObjInfo> iteminfo = new List<itemObjInfo>();
    public Dictionary<string,GameObject> itemList = new Dictionary<string, GameObject>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        for (int i = 0; i < iteminfo.Count; i++) 
        {
            itemList.Add(iteminfo[i].itemName, iteminfo[i].item);
        }
    }
    public void SpawnItem(string name,Vector3 postion,Quaternion rotation) 
    {
        if (!itemList.ContainsKey(name))
            return;
        
        Instantiate(itemList[name], postion, rotation);
    }

}
