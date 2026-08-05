using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{

    public static InventoryManger Instance;


    [Header("Inventory")]
    public int MaxSlots = 8;
    public ItemData[] Slots;


    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Slots = new ItemData[MaxSlots];
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public bool Add(ItemData item)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i] == null)
            {
                Slots[i] = item;

                Debug.Log(item.ItemName + " Added");

                return true;
            }
        }

        Debug.Log("Inventory Full");

        return false;
    }


    }
