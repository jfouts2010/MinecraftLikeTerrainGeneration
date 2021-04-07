using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime
{
    public int minute = 0;
    public int hour = 0;
    public int day = 1;
    public Season season = Season.Spring;
    public int year = 0;

    public void Next()
    {
        minute++;
        day++;
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
        if (day >= 60)
        {
            day = 0;
            season++;
        }
        if ((int)season >= 4)
        {
            season = Season.Spring;
            year++;
        }
    }
}

public enum Season
{
    Spring = 0,
    Summer = 1,
    Autumn = 2,
    Winter = 3
}
