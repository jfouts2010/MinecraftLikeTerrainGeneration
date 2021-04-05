using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClimateData
{
    public Climate clim { get; set; }
    public Dictionary<Season, ClimateSeasonData> seasonData = new Dictionary<Season, ClimateSeasonData>();
}
[Serializable]
public class ClimateSeasonData
{
    /// <summary>
    /// A value from 0 to 1 to know how much grass will grow in this season. Grass grows when the value of the perlin noise on the map is higher than this value
    /// Therefore, a value of 0 means the map will be covered in grass, while 1 means there will be no grass grown
    /// </summary>
    public float GrowthValue { get; set; }
    /// <summary>
    /// A value from 0 to 1 to know how much snow will cover the terrain, 0 is no snow, 1 is complete cover in snow
    /// Start is how much snow is at the start of the season
    /// </summary>
    public float SnowStartValue { get; set; }
    /// <summary>
    /// Mid is how much snow is at the mid season
    /// </summary>
    public float SnowMidValue { get; set; }
    /// <summary>
    /// End is how much snow is at the end of the season
    /// </summary>
    public float SnowEndValue { get; set; }
    /// <summary>
    /// How much rain there is this season
    /// </summary>
    public float RainValue { get; set; }
    public decimal Temperature { get; set; }
}
public enum Climate
{
    Grassland,

    TropicalRainforest,
    TropicalSavannah,
    Desert,
    HotSteppe,
    TropicalMonsoon,

    HumidContinental,
    SubarcticContinental,

    Mediterranean,
    HumidSubtropical,
    Oceanic,

    ColdDesert,
    ColdSteppe,

    Tundra,
    IceCap
}