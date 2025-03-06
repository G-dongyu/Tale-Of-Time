using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ItemConfigGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Item Configs")]
    private static void GenerateItemConfigs()
    {
        // 配置保存路径
        string folderPath = "Assets/Game/C#/Bag/Item";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 获取所有枚举值
        System.Array itemNames = System.Enum.GetValues(typeof(ItemBase.ItemName));

        foreach (ItemBase.ItemName itemName in itemNames)
        {
            // 创建新的ItemBase实例
            ItemBase newItem = ScriptableObject.CreateInstance<ItemBase>();

            // 设置基础属性
            newItem.itemName = itemName; // 关键：设置对应的枚举值
            newItem.money = 0; // 默认值
            newItem.icon = null; // 默认值

            // 生成合法文件名
            string fileName = itemName.ToString().Trim();
            string assetPath = Path.Combine(folderPath, $"{fileName}.asset");

            // 创建并保存资源
            AssetDatabase.CreateAsset(newItem, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"已生成 {itemNames.Length} 个物品配置文件，路径：{folderPath}");
    }
}