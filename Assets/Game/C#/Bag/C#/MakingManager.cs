using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MakingManager : MonoBehaviour
{
    [Header("基础元素")] public GameObject drinkMakingPage;
    public Button makingButton;
    public Button closeButton;
    private Drink _currentShowDrink;

    [Header("制作菜单")]
    public List<Drink> drinks;
    public GameObject drinksButtonPrefab;
    public Transform drinksButtonParent;

    [Header("购买菜单")] public Button buyButton;
    
    [Header("组成成分的菜单")]
    public GameObject itemButtonPrefab;
    public Transform itemButtonParent;

    [Header("主页")] 
    public Text mainNameText;
    public Image mainIconImage;

    [Header("材料提示")] public GameObject noticePage;
    


    private void Start()
    {
        //AddAction.instance.DelayPlay(OpenPage,2f);
        closeButton.onClick.AddListener(() =>
        {
            AddAction.instance.ClosePageBySize(drinkMakingPage);
        });
        UpdateUIDrinkMenu();
    }

    public void OpenPage()
    {
       AddAction.instance.OpenPageBySize(drinkMakingPage);
       _currentShowDrink= drinks[0];
       UpdateUIItemMenu();
       UpdateMainMenu();
    }

    private void UpdateUIDrinkMenu()
    {
        foreach (var drink in drinks)
        {
            var button = Instantiate(drinksButtonPrefab, drinksButtonParent);
            button.transform.GetChild(0).GetComponent<Image>().sprite= drink.icon;
            button.GetComponentInChildren<Text>().text = drink.drinkName.ToString();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                _currentShowDrink= drink;
                UpdateUIItemMenu();
                UpdateMainMenu();
            });
        }
    }

    private void UpdateUIItemMenu()
    {
        int count = itemButtonParent.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(itemButtonParent.GetChild(i).gameObject);
        }
        
        List<ItemBase> items = new List<ItemBase>();
        foreach (var item in _currentShowDrink.compose)
        {
            items.Add(item);
        }
        foreach (var item in items)
        {
            var button = Instantiate(itemButtonPrefab, itemButtonParent);
            button.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
        }
    }

    private void UpdateMainMenu()
    {
        mainNameText.text= _currentShowDrink.drinkName.ToString();
        mainIconImage.sprite = _currentShowDrink.icon;
    }

    public void ShowNotice()
    {
        AddAction.instance.OpenPageBySize(noticePage);
    }

    public void UpdateNoticePagePos(Vector3 pos)
    {
        noticePage.GetComponent<RectTransform>().position= pos;
    }
    
    public void CloseNotice()
    {
        AddAction.instance.ClosePageBySize(noticePage);
    }
    
}