using System;
using System.Collections.Generic;

[Serializable]
public class ShopData
{
    public List<Plant> plants;
    public List<NonPlantItemData> nonPlantItems;
}

[Serializable]
public class NonPlantItemData
{
    public string name;
    public int clickCount;
}