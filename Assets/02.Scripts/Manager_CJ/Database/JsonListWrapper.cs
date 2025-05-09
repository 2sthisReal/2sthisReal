using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonListWrapper<T>
{
    [System.Serializable]
    private class Wrapper
    {
        public List<T> items;
    }

    public static List<T> FromJson(string json)
    {
        return JsonUtility.FromJson<Wrapper>($"{{\"items\":{json}}}")?.items ?? new List<T>();
    }
}
