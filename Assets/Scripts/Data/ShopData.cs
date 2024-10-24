using System;
using System.Collections.Generic;

[System.Serializable]
public class ShopData
{
    public List<Plant> plants;
    public List<NonPlantItemData> nonPlantItems;
}

[System.Serializable]
public class NonPlantItemData
{
    public string name;
    public int clickCount;
}