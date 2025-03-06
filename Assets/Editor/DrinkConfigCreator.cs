using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DrinkConfigCreator : EditorWindow
{
    [MenuItem("Tools/Generate Drink Configs")]
    private static void GenerateDrinkConfigs()
    {
        // 确保目标目录存在
        string folderPath = "Assets/Game/C#/Bag/Drink";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 获取所有枚举值
        System.Array drinkNames = System.Enum.GetValues(typeof(Drink.DrinkName));

        foreach (Drink.DrinkName drinkName in drinkNames)
        {
            // 创建新的Drink实例
            Drink newDrink = ScriptableObject.CreateInstance<Drink>();
            
            // 设置基础属性（可根据需要初始化compose列表）
            newDrink.compose = new List<ItemBase>(); // 初始化空列表

            // 生成资源路径
            string fileName = drinkName.ToString();
            string assetPath = Path.Combine(folderPath, $"{fileName}.asset");

            // 创建并保存资源
            AssetDatabase.CreateAsset(newDrink, assetPath);
            EditorUtility.SetDirty(newDrink);
        }

        // 刷新资源数据库
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"已成功生成 {drinkNames.Length} 个饮品配置文件");
    }
}
