using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataCreation
{
    public static void GenerateClimateData()
    {
        Dictionary<Climate, ClimateData> AllClimateData = new Dictionary<Climate, ClimateData>();
        ClimateData cd = new ClimateData();
        cd.clim = Climate.TropicalRainforest;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.95f,
            RainValue = 0.9f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.95f,
            RainValue = 0.9f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.95f,
            RainValue = 0.9f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.95f,
            RainValue = 0.9f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        AllClimateData.Add(Climate.TropicalRainforest, cd);

        cd = new ClimateData();
        cd.clim = Climate.TropicalMonsoon;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.98f,
            RainValue = 1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 29
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.15f,
            RainValue = 0.1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 22
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.15f,
            RainValue = 0.1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 22
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.98f,
            RainValue = 1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 29
        });
        AllClimateData.Add(Climate.TropicalMonsoon, cd);

        cd = new ClimateData();
        cd.clim = Climate.TropicalSavannah;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.98f,
            RainValue = 1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 29
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.98f,
            RainValue = 1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 29
        });
        AllClimateData.Add(Climate.TropicalSavannah, cd);

        cd = new ClimateData();
        cd.clim = Climate.Desert;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.00f,
            RainValue = 0.00f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 37
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.00f,
            RainValue = 0.00f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 37
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 27
        });
        AllClimateData.Add(Climate.Desert, cd);

        cd = new ClimateData();
        cd.clim = Climate.HotSteppe;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 18
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 25
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.3f,
            RainValue = 0.3f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 22
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 15
        });
        AllClimateData.Add(Climate.HotSteppe, cd);

        cd = new ClimateData();
        cd.clim = Climate.ColdDesert;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 5
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 12
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 0
        });
        AllClimateData.Add(Climate.ColdDesert, cd);

        cd = new ClimateData();
        cd.clim = Climate.ColdSteppe;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 5
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.15f,
            RainValue = 0.15f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 10
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 0
        });
        AllClimateData.Add(Climate.ColdSteppe, cd);

        cd = new ClimateData();
        cd.clim = Climate.Mediterranean;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.3f,
            RainValue = .15f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 12
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.5f,
            RainValue = 0.05f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 25
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.3f,
            RainValue = 0.2f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 18
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.15f,
            RainValue = .2f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 9
        });
        AllClimateData.Add(Climate.Mediterranean, cd);

        cd = new ClimateData();
        cd.clim = Climate.HumidSubtropical;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.4f,
            RainValue = .3f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 15
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.7f,
            RainValue = 0.3f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 25
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.4f,
            RainValue = 0.3f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.2f,
            RainValue = .2f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 12
        });
        AllClimateData.Add(Climate.HumidSubtropical, cd);

        cd = new ClimateData();
        cd.clim = Climate.Oceanic;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.4f,
            RainValue = .1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 7
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.7f,
            RainValue = 0.1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 18
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.4f,
            RainValue = 0.1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 12
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.2f,
            RainValue = .1f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 3
        });
        AllClimateData.Add(Climate.Oceanic, cd);

        cd = new ClimateData();
        cd.clim = Climate.HumidContinental;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.2f,
            RainValue = .05f,
            SnowStartValue = 1,
            SnowMidValue = .3f,
            SnowEndValue = 0f,
            Temperature = 5
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.35f,
            RainValue = 0.2f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 20
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.2f,
            RainValue = 0.15f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = .2f,
            Temperature = 10
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.1f,
            RainValue = .075f,
            SnowStartValue = .2f,
            SnowMidValue = .5f,
            SnowEndValue = 1,
            Temperature = -3
        });
        AllClimateData.Add(Climate.HumidContinental, cd);

        cd = new ClimateData();
        cd.clim = Climate.SubarcticContinental;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.1f,
            RainValue = .05f,
            SnowStartValue = .7f,
            SnowMidValue = .3f,
            SnowEndValue = 0f,
            Temperature = -3
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.2f,
            RainValue = 0.2f,
            SnowStartValue = 0f,
            SnowMidValue = 0f,
            SnowEndValue = 0f,
            Temperature = 15
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.1f,
            RainValue = 0.15f,
            SnowStartValue = 0f,
            SnowMidValue = .1f,
            SnowEndValue = .3f,
            Temperature = 8
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .075f,
            SnowStartValue = .3f,
            SnowMidValue = .5f,
            SnowEndValue = .7f,
            Temperature = -8
        });
        AllClimateData.Add(Climate.SubarcticContinental, cd);

        cd = new ClimateData();
        cd.clim = Climate.Tundra;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = .02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = .2f,
            Temperature = -30
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0.1f,
            RainValue = 0.05f,
            SnowStartValue = .2f,
            SnowMidValue = 0f,
            SnowEndValue = .2f,
            Temperature = 5
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0.05f,
            RainValue = 0.05f,
            SnowStartValue = .2f,
            SnowMidValue = .5f,
            SnowEndValue = 1,
            Temperature = -15
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0,
            RainValue = .02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = 1,
            Temperature = -30
        });
        AllClimateData.Add(Climate.Tundra, cd);

        cd = new ClimateData();
        cd.clim = Climate.IceCap;
        cd.seasonData.Add(Season.Spring, new ClimateSeasonData()
        {
            GrowthValue = 0,
            RainValue = .02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = 1,
            Temperature = -40
        });
        cd.seasonData.Add(Season.Summer, new ClimateSeasonData()
        {
            GrowthValue = 0,
            RainValue = 0.02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = 1,
            Temperature = -20
        });
        cd.seasonData.Add(Season.Autumn, new ClimateSeasonData()
        {
            GrowthValue = 0,
            RainValue = .02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = 1,
            Temperature = -30
        });
        cd.seasonData.Add(Season.Winter, new ClimateSeasonData()
        {
            GrowthValue = 0,
            RainValue = .02f,
            SnowStartValue = 1,
            SnowMidValue = 1,
            SnowEndValue = 1,
            Temperature = -40
        });
        AllClimateData.Add(Climate.IceCap, cd);

        string json = JsonConvert.SerializeObject(AllClimateData);
        File.WriteAllText("ClimateData.json", json);
    }
}
