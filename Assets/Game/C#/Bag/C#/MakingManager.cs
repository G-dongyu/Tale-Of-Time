using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MakingManager : MonoBehaviour
{
    [Header("基础元素")] public GameObject drinkMakingPage;
    public Button makingButton;
    public Button closeButton;
    private Drink _currentShowDrink;

    [Header("制作菜单")] public List<Drink> drinks;
    public GameObject drinksButtonPrefab;
    public Transform drinksButtonParent;

    [Header("购买菜单")] public Button buyButton;

    [Header("组成成分的菜单")] public GameObject itemButtonPrefab;
    public Transform itemButtonParent;

    [Header("主页")] public Text mainNameText;
    public Image mainIconImage;

    [Header("材料提示")] public GameObject noticePage;

    [Header("背包")] private InventoryManager _inventoryManager;
    private List<ItemBase> needItem = new List<ItemBase>();
    private DialogManager _dialogManager;


    [Header("制作显示")] 
    public List<CupPoint> cupPoints;
    private List<Drink> _havenMakingDrink=new List<Drink>();
    public GameObject cupPrefab;
    
    [Header("提杯子")]
    public GameObject cupParent;
    private Cup _currnetCup;


    private void Start()
    {
        //AddAction.instance.DelayPlay(OpenPage,2f);
        _inventoryManager = GetComponent<InventoryManager>();
        closeButton.onClick.AddListener(() => { AddAction.instance.ClosePageBySize(drinkMakingPage); });
        UpdateUIDrinkMenu();
        buyButton.onClick.AddListener(BuyDrink);
        makingButton.onClick.AddListener(Making);
        _dialogManager = GameManager.instance.GetComponent<DialogManager>();
    }

    public void OpenPage()
    {
        AddAction.instance.OpenPageBySize(drinkMakingPage);
        _currentShowDrink = drinks[0];
        UpdateUIItemMenu();
        UpdateMainMenu();
    }

    private void UpdateUIDrinkMenu()
    {
        foreach (var drink in drinks)
        {
            var button = Instantiate(drinksButtonPrefab, drinksButtonParent);
            button.transform.GetChild(0).GetComponent<Image>().sprite = drink.icon;
            button.GetComponentInChildren<Text>().text = drink.drinkName.ToString();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                _currentShowDrink = drink;
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

        needItem.Clear();
        foreach (var item in _currentShowDrink.compose)
        {
            if (!needItem.Contains(item))
            {
                needItem.Add(item);
            }
        }

        foreach (var item in needItem)
        {
            var button = Instantiate(itemButtonPrefab, itemButtonParent);
            button.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
        }
    }

    private void UpdateMainMenu()
    {
        mainNameText.text = _currentShowDrink.drinkName.ToString();
        mainIconImage.sprite = _currentShowDrink.icon;
    }

    private void BuyDrink()
    {
        MoneyManager money = GetComponent<MoneyManager>();
        int needMoney = 0;
        foreach (var item in needItem)
        {
            needMoney += item.money;
        }

        if (money.ReMoveMoney(needMoney))
        {
            foreach (var item in needItem)
            {
                _inventoryManager.AddItem(item);
            }

            _dialogManager.Show(DialogType.BuySuccessful);
        }
        else
        {
            _dialogManager.Show(DialogType.BuyFail);
        }
    }

    public void Making()
    {
        List<ItemBase> notHaveItem = new List<ItemBase>();
        List<Drink> canDoItem = GameManager.instance.GetComponent<LevelManager>().GetItemCanDo();
        foreach (var item in needItem)
        {
            ItemPlate itemPlate = _inventoryManager.InspectIsHave(item);
            if (itemPlate == null)
            {
                notHaveItem.Add(item);
            }
        }

        if (canDoItem.Contains(_currentShowDrink))
        {
            if (notHaveItem.Count == 0&&_havenMakingDrink.Count<2)
            {
                _dialogManager.Show(DialogType.MakingSuccessful);
                _havenMakingDrink.Add(_currentShowDrink);
                UpdateMakingDrinkShow(_currentShowDrink);
            }
            else
            {
                //缺乏材料
                _dialogManager.Show(DialogType.MakingFail);
            }     
        }
        else
        {
            _dialogManager.Show(DialogType.MakingDrinkNoDo);
        }

       
    }

    private void UpdateMakingDrinkShow(Drink drink)
    {
        GameObject cup = Instantiate(cupPrefab);
        GameObject selectDrink = Instantiate(drink.drinkPrefab);
        selectDrink.transform.position = cup.transform.GetChild(0).position;
        selectDrink.transform.SetParent(cup.transform);
        CupPoint selectPoint = GetPoint();
        cup.GetComponent<Cup>().Init(drink, selectPoint);
    }

    private CupPoint GetPoint()
    {
        CupPoint selectPoint = null;
        foreach (var cupPoint in cupPoints)
        {
            if (!cupPoint.isUse)
            {
                selectPoint= cupPoint;
            }
        }
        return selectPoint;
    }

    public void ReMoveCupPoint(Drink drink, CupPoint cupPoint)
    {
        _havenMakingDrink.Remove(drink);
        cupPoint.SetUse(false);
        
    }
    
    public void PutUpCup(Cup cup)
    {
        if (_currnetCup != null)
        {
            _currnetCup.transform.position = GetPoint().point.position;
            _currnetCup.transform.SetParent(null);
            _currnetCup.GetComponent<Cup>().ControlInspect(true);
        }
        PlayerControl playerControl = GetComponent<PlayerControl>();
        playerControl.PlayerAni("PutUpCup", true);
        cup.transform.position= cupParent.transform.position;
        cup.transform.SetParent(cupParent.transform);
        _currnetCup= cup;
    }

    public void GiveCup(NpcController npc)
    {
        bool isActive = _currnetCup.GetDrink() == npc.GetNeedDrink();
        if (isActive)
        {
            PlayerControl playerControl = GetComponent<PlayerControl>();
            playerControl.PlayerAni("PutUpCup", false);
            npc.OnSentDrinkTrue(_currnetCup.GetDrink());
            Destroy(_currnetCup.gameObject);
            _currnetCup = null;
        }
        else
        {
            npc.OnSentDrinkFalse();
        }
        npc.ShowTrueOfFalse(isActive);
    }


    // public void ShowNotice()
    // {
    //     AddAction.instance.OpenPageBySize(noticePage);
    // }
    //
    // public void UpdateNoticePagePos(Vector3 pos)
    // {
    //     noticePage.GetComponent<RectTransform>().position= pos;
    // }
    //
    // public void CloseNotice()
    // {
    //     AddAction.instance.ClosePageBySize(noticePage);
    // }
}

[Serializable]
public class CupPoint
{
    public bool isUse;
    public Transform point;

    public void SetUse(bool isUse)
    {
        this.isUse = isUse;
    }
}