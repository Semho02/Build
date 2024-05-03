using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBoneQuestStep : QuestStep
{
    private int boneCollected = 0;
    private int boneToComplete = 5;
    private InventoryManager inventoryManager;

    void OnEnable()
    {
        // Найдем объект InventoryManager в сцене
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        // Проверяем есть ли уже подобные предметы
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            if (itemSlot.itemName == "Bone")
            {
                boneCollected += itemSlot.quantity;
            }
        }
        // Подписываемся на событие подбора предмета
        Item.ItemPickedUp += OnItemPickedUp;
        // Подписываемся на событие выбрасывания предмета
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            itemSlot.ItemThrownAway += OnItemThrownAway;
        }
    }

    void OnDisable()
    {
        // Отписываемся от события при отключении компонента
        Item.ItemPickedUp -= OnItemPickedUp;
        // Отписываемся от события выбрасывания предмета
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            itemSlot.ItemThrownAway -= OnItemThrownAway;
        }
    }

    void OnItemPickedUp(string itemName, int quantity)
    {
        // Проверяем, если подобранная кость, то увеличиваем счетчик
        if (itemName == "Bone")
        {
            boneCollected += quantity;
            // Проверяем, достигли ли мы необходимого количества костей для завершения квеста
            if (boneCollected >= boneToComplete)
            {
                // Завершаем этап квеста
                FinishQuestStep();
            }
        }
    }
    void OnItemThrownAway(string itemName, int quantity)
    {
        // Проверяем, если выброшенная кость, то уменьшаем счетчик
        if (itemName == "Bone")
        {
            boneCollected -= quantity;
        }
    }
    void Start()
    {
        // Проверяем есть ли на начало квеста нужно количество предметов, если есть, можем сразу выполнить квест.
        if (boneCollected >= boneToComplete)
        {
            FinishQuestStep();
        }
    }


    //void Start()
    //{
    //    inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    //}
    //void Update()
    //{
    //    // Проверяем количество костей в инвентаре при каждом обновлении
    //    CheckBoneCount();
    //}
    //void CheckBoneCount()
    //{
    //    // Проходим по всем слотам инвентаря, чтобы найти кости
    //    for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
    //    {
    //        // Проверяем, содержит ли этот слот кости
    //        if (inventoryManager.itemSlot[i].itemName == "Bone")
    //        {
    //            // Если содержит, увеличиваем счетчик костей
    //            boneCollected += inventoryManager.itemSlot[i].quantity;
    //        }
    //    }

    //    // Проверяем, достигли ли мы необходимого количества костей для завершения квеста
    //    if (boneCollected >= boneToComplete)
    //    {
    //        // Завершаем этап квеста
    //        FinishQuestStep();
    //    }
    //}
}
