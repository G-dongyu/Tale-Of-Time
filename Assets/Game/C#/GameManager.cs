using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        _currentLevel= levels[0];
    }

    public List<Level> levels;
    private Level _currentLevel;

    public Level GetLevel()
    {
        return _currentLevel;
    }
}

[Serializable]
public class Level
{
    public List<Desk> desks;
    public int npcMax;
}

[Serializable]
public class Desk
{
    public Transform desk;
    public List<Check> checks;
}

[Serializable]
public class Check
{
    public Transform check;
    public bool isUSe = false;
}