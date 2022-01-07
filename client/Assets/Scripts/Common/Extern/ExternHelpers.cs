using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class ExternHelpers
{
    public static bool HasKey(this JsonData jsonData, string key)
    {
        return ((IDictionary)jsonData).Contains(key);
    }
}
