using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public int seed = 69;
    float lastUpdate = 0;
    public GameTime gt = new GameTime() { minute = 60, hour = 24, day = 60, season = Season.Winter };
    LandGeneration lg;
    public GameObject grassPrefab;
    public GameObject treePrefab;
    public Material terrainMaterial;
    public ClimateData cd;
    public Climate c;
    public Terrain TerrainType;
    // Start is called before the first frame update
    public void Awake()
    {
        lg = GameObject.Find("Generator").GetComponent<LandGeneration>();
        Dictionary<Climate, ClimateData> allData = JsonConvert.DeserializeObject<Dictionary<Climate, ClimateData>>(File.ReadAllText("ClimateData.json"));
        cd = allData[c];
    }

    // Update is called once per frame
    void Update()
    {
        if (lastUpdate + .1f < Time.time)
        {
            lastUpdate = Time.time;
            gt.Next();
            WaterUpdate();
           /* if (gt.minute == 1)
            {
                VegetationUpdate();
                TreeUpdate();
            }*/
            Snow();
        }
    }
    public void Snow()
    {
        float snowValue = 0;
        if (gt.day <= 30)
        {
            float hours = (float)gt.hour + (float)gt.day * 24f;
            float totalHours = 30f * 24f;
            float startValue = cd.seasonData[gt.season].SnowStartValue;
            float endValue = cd.seasonData[gt.season].SnowMidValue;
            snowValue = Mathf.Clamp(startValue + (hours / totalHours * (endValue - startValue)), 0, 1);
        }
        else
        {
            float hours = (float)gt.hour + (float)gt.day * 24f - 30f * 24f;
            float totalHours = 30f * 24f;
            float startValue = cd.seasonData[gt.season].SnowMidValue;
            float endValue = cd.seasonData[gt.season].SnowEndValue;
            snowValue = Mathf.Clamp(startValue + (hours / totalHours * (endValue - startValue)), 0, 1);
        }
        terrainMaterial.SetFloat("Vector1_47F2660D", snowValue);

    }
    public void TreeUpdate()
    {
        //NEEDS TO BE REDONE
        float seasonGrassLimit = cd.seasonData[gt.season].GrowthValue / 5f;
        int growthRate = 1;
        if (gt.season == Season.Spring)
            growthRate = 3;
        else if (gt.season == Season.Summer)
            growthRate = 1;
        else if (gt.season == Season.Autumn)
            growthRate = 2;
        else if (gt.season == Season.Winter)
            growthRate = 4;

        List<Vector3> surface = lg.SurfaceBricks.Where(p => p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();//lg.WorldInformation.Where(p => p.Value.SurfaceBrick && p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();
        FastNoiseLite fnl = TreeGeneration(seed+10);
        int limitPerUpdate = growthRate;
        foreach (Vector3 pos in surface.OrderBy(a => Guid.NewGuid()))
        {
            BrickInformation bi = lg.WorldInformation[pos];
            float height = Mathf.Clamp(fnl.GetNoise(pos.x, pos.z), 0, 1);
            if (height < seasonGrassLimit) //grow grass
            {
                if (!bi.tree)
                {
                    GameObject go = Instantiate(treePrefab);
                    go.transform.position = pos + new Vector3(0, 0.5f, 0);
                    bi.TreeObjects = go;
                    bi.tree = true;

                    limitPerUpdate--;
                    if (limitPerUpdate <= 0)
                        break;
                }
            }
        }
    }
    public void VegetationUpdate()
    {
        // if (gt.season == Season.Spring)
        {
            float seasonGrassLimit = cd.seasonData[gt.season].GrowthValue;
            int growthRate = 1;
            if (gt.season == Season.Spring)
                growthRate = 3;
            else if (gt.season == Season.Summer)
                growthRate = 1;
            else if (gt.season == Season.Autumn)
                growthRate = 2;
            else if (gt.season == Season.Winter)
                growthRate = 4;

            List<Vector3> surface = lg.SurfaceBricks.Where(p=>p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();//lg.WorldInformation.Where(p => p.Value.SurfaceBrick && p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();
            FastNoiseLite fnl = GrassGeneration(seed);
            int limitPerUpdate = growthRate;
            foreach (Vector3 pos in surface.OrderBy(a => Guid.NewGuid()))
            {
                BrickInformation bi = lg.WorldInformation[pos];
                float height = Mathf.Clamp(fnl.GetNoise(pos.x, pos.z), 0, 1);
                if (height < seasonGrassLimit) //grow grass
                {
                    if (!bi.grass)
                    {
                        GameObject go = Instantiate(grassPrefab);
                        go.transform.position = pos + new Vector3(UnityEngine.Random.Range(-.35f, .35f), 0.5f, UnityEngine.Random.Range(-.35f, .35f));
                        bi.grassObjects.Add(go);
                        bi.grass = true;

                        limitPerUpdate--;
                        if (limitPerUpdate <= 0)
                            break;
                    }
                }
                else // kill grass
                {
                    if (bi.grass)
                    {
                        foreach (GameObject go in bi.grassObjects)
                        {
                            Destroy(go);
                        }
                        bi.grassObjects.Clear();
                        bi.grass = false;

                        limitPerUpdate--;
                        if (limitPerUpdate <= 0)
                            break;
                    }
                }
            }
        }
    }
    public FastNoiseLite GrassGeneration(int seed)
    {
        FastNoiseLite fnl2 = new FastNoiseLite(seed);
        fnl2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fnl2.SetFrequency(0.118f);
        fnl2.SetFractalType(FastNoiseLite.FractalType.None);
        fnl2.SetFractalOctaves(4);
        fnl2.SetFractalLacunarity(2.2f);
        fnl2.SetFractalGain(.8f);
        fnl2.SetFractalWeightedStrength(1.45f);
        return fnl2;
    }
    public FastNoiseLite TreeGeneration(int seed)
    {
        FastNoiseLite fnl2 = new FastNoiseLite(seed);
        fnl2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fnl2.SetFrequency(0.118f);
        fnl2.SetFractalType(FastNoiseLite.FractalType.None);
        fnl2.SetFractalOctaves(4);
        fnl2.SetFractalLacunarity(2.2f);
        fnl2.SetFractalGain(.8f);
        fnl2.SetFractalWeightedStrength(1.45f);
        return fnl2;
    }
    public void WaterUpdate()
    {
        List<BrickInformation> unflowingWater = new List<BrickInformation>();
        List<BrickInformation> newFlowingWater = new List<BrickInformation>();
        foreach (BrickInformation bi in lg.FlowingWater)
        {
            //first check if can flow directly down
            //if no information, flow everything down 1
            if (lg.IsAir(bi.pos + new Vector3(0, -1, 0)))
            {
                int newBIWater = 0;
                int oldBIWater = bi.waterLevel;
                if (bi.waterLevel <= 9)
                {
                    newBIWater = bi.waterLevel; oldBIWater = 0;
                    unflowingWater.Add(bi);
                }
                else if (bi.waterLevel <= 18)
                {
                    newBIWater = 9; oldBIWater = bi.waterLevel - 9;
                }
                else
                {
                    newBIWater = bi.waterLevel / 2; oldBIWater = bi.waterLevel / 2;
                }
                BrickInformation newBI = lg.WorldInformation[bi.pos + new Vector3(0, -1, 0)];
                newBI.waterLevel = newBIWater;
                bi.waterLevel = oldBIWater;
                newFlowingWater.Add(newBI);
            }
            else
            {
                //first check for downflow
                BrickInformation MinusYBI = lg.WorldInformation[bi.pos + new Vector3(0, -1, 0)];
                BrickInformation MinusXBI = lg.WorldInformation[bi.pos + new Vector3(-1, 0, 0)];
                BrickInformation PlusXBI = lg.WorldInformation[bi.pos + new Vector3(1, 0, 0)];
                BrickInformation MinusZBI = lg.WorldInformation[bi.pos + new Vector3(0, 0, -1)];
                BrickInformation PlusZBI = lg.WorldInformation[bi.pos + new Vector3(0, 0, 1)];
                List<BrickInformation> sideBricks = new List<BrickInformation>();
                sideBricks.Add(MinusXBI); sideBricks.Add(PlusXBI); sideBricks.Add(MinusZBI); sideBricks.Add(PlusZBI);
                //priority minus y
                bool DoneFlowing = true;
                if (MinusYBI.bt == BrickType.air && MinusYBI.waterLevel < 9)
                {
                    DoneFlowing = false;
                    int waterMov = Mathf.Clamp(9 - MinusYBI.waterLevel, 0, bi.waterLevel);
                    MinusYBI.waterLevel += waterMov;
                    bi.waterLevel -= waterMov;
                    if (bi.waterLevel == 0)
                    {
                        bi.bt = BrickType.air;
                        unflowingWater.Add(bi);
                    }
                    newFlowingWater.Add(MinusYBI);
                }
                sideBricks = sideBricks.Where(p => p.bt == BrickType.air).ToList();
                if (sideBricks.Any(p => p.waterLevel != bi.waterLevel - 1))
                {
                    //a check to make sure at least 1 side brick is not -1 water level, if all -1 water level we stop flowing to prevent "Ping ponging"
                    foreach (BrickInformation sideBrick in sideBricks.OrderBy(p => p.waterLevel))
                    {
                        if (bi.waterLevel == 0)
                            break;
                        if (bi.waterLevel == 1 && !sideBricks.Any(p => p.waterLevel > 1))
                            break;
                        if (sideBrick.waterLevel < bi.waterLevel)
                        {
                            DoneFlowing = false;
                            bi.waterLevel--;
                            sideBrick.waterLevel++;
                            newFlowingWater.Add(sideBrick);
                        }
                    }
                }
                if (DoneFlowing)
                    unflowingWater.Add(bi);
            }
        }
        //if (gt.minute % 5 == 0)
        {
            foreach (GameObject go in lg.FlowingWater.Select(p => p.mesh).Distinct())
                lg.GenerateMesh(go);
        }
        foreach (BrickInformation delete in unflowingWater)
            lg.FlowingWater.Remove(delete);
        foreach (BrickInformation add in newFlowingWater)
            if (!lg.FlowingWater.Contains(add))
                lg.FlowingWater.Add(add);
    }
}
