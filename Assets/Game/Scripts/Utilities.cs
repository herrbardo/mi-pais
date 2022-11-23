using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utilities
{
    public static string ConvertDateToDisplayMonthDate(DateTime date)
    {
        string chota = date.ToString("MMMM yyy");
        return char.ToUpper(chota[0]) + chota.Substring(1);
    }
}
