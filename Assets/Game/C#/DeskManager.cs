using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskManager : MonoBehaviour
{
    public Desk ByCheckFindDesk(Check check)
    {
        Level level = GameManager.instance.GetLevel();
        Desk deskTarget = null;
        foreach (Desk desk in level.desks)
        {
            if (desk.checks.Contains(check))
            {
                deskTarget= desk;
                return deskTarget;
            }
        }
        return null;
    }

    public Check GetCheck()
    {
        Level level = GameManager.instance.GetLevel();
        Check checkTarget = null;
        foreach (Desk desk in level.desks)
        {
            foreach (Check check in desk.checks)
            {
                if (!check.isUSe)
                {
                    checkTarget= check;
                    return checkTarget;
                }
            }
        }

        return null;

    }
}
