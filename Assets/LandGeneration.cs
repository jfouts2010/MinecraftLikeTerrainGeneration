using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LandGeneration : MonoBehaviour
{
    public Dictionary<Vector3, BrickInformation> WorldInformation = new Dictionary<Vector3, BrickInformation>();
    public Dictionary<Vector3, BrickInformation> SurfaceBricks = new Dictionary<Vector3, BrickInformation>();
    public List<BrickInformation> FlowingWater = new List<BrickInformation>();
    // Start is called before the first frame update
    public Material terrainMat;
    public Material treeMat;
    public Material grassMat;
    GameObject TreeMeshs;
    GameObject GrassMeshs;
    Dictionary<GameObject, List<Vector3>> MeshChunks = new Dictionary<GameObject, List<Vector3>>();
    float lastUpdate = 0;
    int mapXSize = 100;
    int mapYSize = 50;
    int mapZSize = 100;
    public GameEngine ge;
    FastNoiseLite fnl;

    public bool IsAir(Vector3 pos)
    {
        return WorldInformation.ContainsKey(pos) ? WorldInformation[pos].bt == BrickType.air && WorldInformation[pos].waterLevel == 0 : true;
    }
    void Start()
    {
        TreeMeshs = Instantiate(new GameObject());
        TreeMeshs.transform.parent = transform;
        GrassMeshs = Instantiate(new GameObject());
        GrassMeshs.transform.parent = transform;
        ge = GameObject.Find("Engine").GetComponent<GameEngine>();
        int rand = 69;//Random.Range(0, 100);
        UnityEngine.Random.InitState(rand);
        if (ge.TerrainType == Terrain.Hill)
            fnl = GenHillGeneration(rand);
        else if (ge.TerrainType == Terrain.Mountain)
            fnl = GenMountainGeneration(rand);
        else if (ge.TerrainType == Terrain.Plains)
            fnl = GenHillGeneration(rand);
        for (int x = -mapXSize / 2; x < mapXSize / 2; x++)
            for (int z = -mapZSize / 2; z < mapZSize / 2; z++)
                for (int y = -mapYSize / 2; y < mapYSize / 2; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    float height = fnl.GetNoise(x + 1000, z);
                    height = Mathf.Clamp(height, 0f, 1f);
                    //float height = StackedPerlinNoise(pos, octaves, zoom, persistance, lac);
                    if ((y + mapYSize / 4f) / (float)mapYSize < height)
                    {
                        WorldInformation.Add(pos, new BrickInformation() { bt = BrickType.dirt, pos = pos, waterLevel = 0 });
                    }
                    else
                        WorldInformation.Add(pos, new BrickInformation() { bt = BrickType.air, pos = pos, waterLevel = 0 });
                }
        GenerateInitialMeshs();
        foreach (var go in MeshChunks)
            GenerateMesh(go.Key);

        AddWaterBlock(new Vector3(14, 0, 1));
        AddWaterBlock(new Vector3(13, 0, 1));
        AddWaterBlock(new Vector3(14, 0, 0));
        AddWaterBlock(new Vector3(14, 0, 1));
        AddBrick(new Vector3(15, -2, -1));
        AddBrick(new Vector3(14, -2, 0));
        AddBrick(new Vector3(13, -2, 1));
        AddBrick(new Vector3(14, -2, 2));
        AddBrick(new Vector3(15, -2, 3));
        AddBrick(new Vector3(16, -2, 0));
        AddBrick(new Vector3(16, -2, 2));
        AddBrick(new Vector3(17, -2, 2));

        InitialGrassAndTreeSpawn();
    }
    public void InitialGrassAndTreeSpawn()
    {
        MeshRenderer mrTree = TreeMeshs.AddComponent<MeshRenderer>();
        mrTree.material = treeMat;
        MeshFilter mfTree = TreeMeshs.AddComponent<MeshFilter>();
        Mesh treeMesh = new Mesh();
        mfTree.mesh = treeMesh;

        MeshRenderer mrGrass = GrassMeshs.AddComponent<MeshRenderer>();
        mrGrass.material = grassMat;
        MeshFilter mfGrass = GrassMeshs.AddComponent<MeshFilter>();
        Mesh grassMesh = new Mesh();
        mfGrass.mesh = grassMesh;

        List<int> treeTriangles = new List<int>();
        List<Vector3> treeVerts = new List<Vector3>();
        List<Vector2> treeUVs = new List<Vector2>();

        List<int> grassTriangles = new List<int>();
        List<Vector3> grassVerts = new List<Vector3>();
        List<Vector2> grassUVs = new List<Vector2>();

        float seasonGrassLimit = ge.cd.seasonData[ge.gt.season].GrowthValue * 1.5f;
        float seasonTreeLimit = ge.cd.seasonData[ge.gt.season].GrowthValue;
        List<Vector3> surface = SurfaceBricks.Where(p => p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();//lg.WorldInformation.Where(p => p.Value.SurfaceBrick && p.Value.bt == BrickType.dirt).Select(p => p.Key).ToList();

        FastNoiseLite fnlGRASS = ge.GrassGeneration(ge.seed);
        FastNoiseLite fnlTREE = ge.TreeGeneration(ge.seed + 10);
        var temp = GetFNLMin(fnlGRASS);
        float grassMin = temp[0];
        float grassMax = temp[1];
        temp = GetFNLMin(fnlTREE);
        float treeMin = temp[0];
        float treeMax = temp[1];
        foreach (Vector3 pos in surface.OrderBy(a => Guid.NewGuid()))
        {
            BrickInformation bi = WorldInformation[pos];
            float heightGrass = (fnlGRASS.GetNoise(pos.x, pos.z) - grassMin) / (grassMax - grassMin);
            float heightTree = (fnlTREE.GetNoise(pos.x, pos.z) - treeMin) / (treeMax - treeMin);
            if (heightGrass < seasonGrassLimit) //grow grass
            {
                if (!bi.grass)
                {
                    bi.grass = true;
                    for (int i = 0; i < 3; i++)
                    {
                        grassTriangles.Add(treeVerts.Count());
                        grassTriangles.Add(treeVerts.Count() + 3);
                        grassTriangles.Add(treeVerts.Count() + 2);

                        grassTriangles.Add(treeVerts.Count());
                        grassTriangles.Add(treeVerts.Count() + 1);
                        grassTriangles.Add(treeVerts.Count() + 3);

                        grassTriangles.Add(treeVerts.Count() + 4);
                        grassTriangles.Add(treeVerts.Count() + 7);
                        grassTriangles.Add(treeVerts.Count() + 6);

                        grassTriangles.Add(treeVerts.Count() + 4);
                        grassTriangles.Add(treeVerts.Count() + 5);
                        grassTriangles.Add(treeVerts.Count() + 7);

                        Vector3 diplacement = new Vector3(UnityEngine.Random.Range(-0.3f, .3f), UnityEngine.Random.Range(-0.05f, 0), UnityEngine.Random.Range(-0.3f, .3f));
                        grassVerts.Add(pos + new Vector3(0.1f, .7f, 0) + diplacement);
                        grassVerts.Add(pos + new Vector3(0.1f, .5f, 0) + diplacement);
                        grassVerts.Add(pos + new Vector3(-0.1f, .7f, 0) + diplacement);
                        grassVerts.Add(pos + new Vector3(-0.1f, .5f, 0) + diplacement);
                        grassVerts.Add(pos + new Vector3(0, .7f, .1f) + diplacement);
                        grassVerts.Add(pos + new Vector3(0, .5f, .1f) + diplacement);
                        grassVerts.Add(pos + new Vector3(0, .7f, -.1f) + diplacement);
                        grassVerts.Add(pos + new Vector3(0, .5f, -.1f) + diplacement);

                        grassUVs.Add(new Vector2(1, 1));
                        grassUVs.Add(new Vector2(1, 0));
                        grassUVs.Add(new Vector2(0, 1));
                        grassUVs.Add(new Vector2(0, 0));
                        grassUVs.Add(new Vector2(1, 1));
                        grassUVs.Add(new Vector2(1, 0));
                        grassUVs.Add(new Vector2(0, 1));
                        grassUVs.Add(new Vector2(0, 0));
                    }
                }
            }
            if (heightTree < seasonTreeLimit) //grow grass
            {
                if (!bi.tree)
                {
                    bi.tree = true;

                    treeTriangles.Add(treeVerts.Count());
                    treeTriangles.Add(treeVerts.Count() + 3);
                    treeTriangles.Add(treeVerts.Count() + 2);

                    treeTriangles.Add(treeVerts.Count());
                    treeTriangles.Add(treeVerts.Count() + 1);
                    treeTriangles.Add(treeVerts.Count() + 3);

                    treeTriangles.Add(treeVerts.Count() + 4);
                    treeTriangles.Add(treeVerts.Count() + 7);
                    treeTriangles.Add(treeVerts.Count() + 6);

                    treeTriangles.Add(treeVerts.Count() + 4);
                    treeTriangles.Add(treeVerts.Count() + 5);
                    treeTriangles.Add(treeVerts.Count() + 7);

                    Vector3 diplacement = new Vector3(UnityEngine.Random.Range(-0.3f, .3f), UnityEngine.Random.Range(-0.3f, 0), UnityEngine.Random.Range(-0.3f, .3f));
                    treeVerts.Add(pos + new Vector3(0.5f, 2.5f, 0) + diplacement);
                    treeVerts.Add(pos + new Vector3(0.5f, .5f, 0) + diplacement);
                    treeVerts.Add(pos + new Vector3(-0.5f, 2.5f, 0) + diplacement);
                    treeVerts.Add(pos + new Vector3(-0.5f, .5f, 0) + diplacement);
                    treeVerts.Add(pos + new Vector3(0, 2.5f, .5f) + diplacement);
                    treeVerts.Add(pos + new Vector3(0, .5f, .5f) + diplacement);
                    treeVerts.Add(pos + new Vector3(0, 2.5f, -.5f) + diplacement);
                    treeVerts.Add(pos + new Vector3(0, .5f, -.5f) + diplacement);

                    treeUVs.Add(new Vector2(1, 1));
                    treeUVs.Add(new Vector2(1, 0));
                    treeUVs.Add(new Vector2(0, 1));
                    treeUVs.Add(new Vector2(0, 0));
                    treeUVs.Add(new Vector2(1, 1));
                    treeUVs.Add(new Vector2(1, 0));
                    treeUVs.Add(new Vector2(0, 1));
                    treeUVs.Add(new Vector2(0, 0));
                }
            }
        }
        treeMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        treeMesh.Clear();
        treeMesh.vertices = treeVerts.ToArray();
        treeMesh.triangles = treeTriangles.ToArray();
        treeMesh.uv = treeUVs.ToArray();
        treeMesh.Optimize();
        treeMesh.RecalculateBounds();
        treeMesh.RecalculateTangents();

        grassMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        grassMesh.Clear();
        grassMesh.vertices = grassVerts.ToArray();
        grassMesh.triangles = grassTriangles.ToArray();
        grassMesh.uv = grassUVs.ToArray();
        grassMesh.Optimize();
        grassMesh.RecalculateBounds();
        grassMesh.RecalculateTangents();
    }
    public new List<float> GetFNLMin(FastNoiseLite fnl)
    {
        float min = 0;
        float max = 0;
        for (int x = -1000; x < 1000; x++)
            for (int y = -1000; y < 1000; y++)
            {
                float val = fnl.GetNoise(x, y);
                if (val < min)
                    min = val;
                if (val > max)
                    max = val;
            }
        return new List<float>() { min, max };
    }
    public void GenerateInitialMeshs()
    {
        for (int x = -mapXSize / 2; x < mapXSize / 2; x += 10)
            for (int z = -mapZSize / 2; z < mapZSize / 2; z += 10)
            {
                GameObject go = Instantiate(new GameObject());
                go.AddComponent<MeshRenderer>();
                go.GetComponent<MeshRenderer>().material = terrainMat;
                go.AddComponent<MeshFilter>();
                go.transform.parent = transform;
                Mesh m = new Mesh();

                List<Vector3> meshPositions = new List<Vector3>();
                foreach (var pair in WorldInformation)
                {
                    if (pair.Key.x >= x && pair.Key.x < x + 10 && pair.Key.z >= z && pair.Key.z < z + 10)
                    {
                        pair.Value.mesh = go;
                        meshPositions.Add(pair.Key);
                    }
                }
                MeshChunks.Add(go, meshPositions);
            }
    }
    public void GenerateMesh(GameObject meshGO)
    {
        Mesh m = new Mesh();
        //m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        MeshFilter mf = meshGO.GetComponent<MeshFilter>();
        MeshRenderer mr = meshGO.GetComponent<MeshRenderer>();
        mr.material = terrainMat;
        mf.mesh = m;
        //List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> Verts = new List<Vector3>();
        List<Vector3> allNormals = new List<Vector3>();
        List<Vector2> allUVs = new List<Vector2>();
        foreach (Vector3 pos in MeshChunks[meshGO])
        {
            BrickInformation bi = WorldInformation[pos];
            if (bi.bt == BrickType.air && bi.waterLevel == 0)
                continue;
            //Vector3 pos = pair.Key;
            List<Vector3> airNeighbors = GetAirNeighbors(pos);
            foreach (Vector3 airNeighbor in airNeighbors)
            {
                Vector3 v = airNeighbor - pos;
                if (WorldInformation.ContainsKey(airNeighbor))
                {
                    BrickInformation biNeighbor = WorldInformation[airNeighbor];
                    if (v == Vector3.up && !biNeighbor.water)
                    {
                        bi.SurfaceBrick = true;
                        if (!SurfaceBricks.ContainsKey(pos))
                            SurfaceBricks.Add(pos, bi);
                    }
                }
                List<Vector3> newVertexs = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                //messy but I dont wanna calc perpendiculars
                if (bi.bt == BrickType.air && bi.waterLevel > 0)
                {
                    if (v.x != 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(v.x/2f, (bi.waterLevel * .1f - .5f),.5f),
                            pos+ new Vector3(v.x/2f, -.5f,.5f),
                            pos+ new Vector3(v.x/2f, (bi.waterLevel * .1f - .5f),-.5f),
                            pos+ new Vector3(v.x/2f, -.5f,-.5f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                    else if (v.y < 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(.5f, v.y/2f,.5f),
                            pos+ new Vector3(.5f, v.y/2f,-.5f),
                            pos+ new Vector3(-.5f, v.y/2f,.5f),
                            pos+ new Vector3(-.5f, v.y/2f,-.5f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                    else if (v.y > 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(.5f, (bi.waterLevel * .1f - .5f),.5f),
                            pos+ new Vector3(.5f, (bi.waterLevel * .1f - .5f),-.5f),
                            pos+ new Vector3(-.5f, (bi.waterLevel * .1f - .5f),.5f),
                            pos+ new Vector3(-.5f, (bi.waterLevel * .1f - .5f),-.5f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                    else if (v.z != 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(.5f, (bi.waterLevel * .1f - .5f), v.z/2f),
                            pos+ new Vector3(-.5f, (bi.waterLevel * .1f - .5f), v.z/2f),
                            pos+ new Vector3(.5f,-.5f, v.z/2f),
                            pos+ new Vector3(-.5f, -.5f, v.z/2f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                }
                else
                {
                    if (v.x != 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(v.x/2f, .5f,.5f),
                            pos+ new Vector3(v.x/2f, -.5f,.5f),
                            pos+ new Vector3(v.x/2f, .5f,-.5f),
                            pos+ new Vector3(v.x/2f, -.5f,-.5f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                    else if (v.y != 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(.5f, v.y/2f,.5f),
                            pos+ new Vector3(.5f, v.y/2f,-.5f),
                            pos+ new Vector3(-.5f, v.y/2f,.5f),
                            pos+ new Vector3(-.5f, v.y/2f,-.5f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                    else if (v.z != 0)
                    {
                        newVertexs = new List<Vector3>()
                        {
                            pos+ new Vector3(.5f, .5f, v.z/2f),
                            pos+ new Vector3(-.5f, .5f, v.z/2f),
                            pos+ new Vector3(.5f,-.5f, v.z/2f),
                            pos+ new Vector3(-.5f, -.5f, v.z/2f),
                        };
                        for (int i = 0; i < 4; i++)
                            normals.Add(v);
                    }
                }
                if (v.x > 0 || v.y > 0 || v.z > 0)
                {
                    triangles.Add(Verts.Count());
                    triangles.Add(Verts.Count() + 3);
                    triangles.Add(Verts.Count() + 2);

                    triangles.Add(Verts.Count());
                    triangles.Add(Verts.Count() + 1);
                    triangles.Add(Verts.Count() + 3);
                }
                else
                {
                    triangles.Add(Verts.Count() + 0);
                    triangles.Add(Verts.Count() + 2);
                    triangles.Add(Verts.Count() + 3);

                    triangles.Add(Verts.Count() + 0);
                    triangles.Add(Verts.Count() + 3);
                    triangles.Add(Verts.Count() + 1);
                }
                Verts.AddRange(newVertexs);
                allNormals.AddRange(normals);
                List<Vector2> UVs = new List<Vector2>();
                if (bi.bt == BrickType.dirt)
                {
                    if (v == Vector3.up)
                    {
                        UVs.Add(new Vector2(64f / 1280f, 127f / 1280f));
                        UVs.Add(new Vector2(64f / 1280f, 64f / 1280f));
                        UVs.Add(new Vector2(0f / 1280f, 127f / 1280f));
                        UVs.Add(new Vector2(0f / 1280f, 64f / 1280f));
                    }
                    else
                    {
                        UVs.Add(new Vector2(63f / 1280f, 63f / 1280f));
                        UVs.Add(new Vector2(63f / 1280f, 0));
                        UVs.Add(new Vector2(0, 63f / 1280f));
                        UVs.Add(new Vector2(0, 0));
                    }
                }
                else if (bi.bt == BrickType.rock)
                {
                    UVs.Add(new Vector2(127f / 1280f, 64f / 1280f));
                    UVs.Add(new Vector2(127f / 1280f, 0f / 1280f));
                    UVs.Add(new Vector2(64f / 1280f, 64f / 1280f));
                    UVs.Add(new Vector2(64f / 1280f, 0f / 1280f));
                }
                else if (bi.bt == BrickType.air)
                {
                    UVs.Add(new Vector2(191f / 1280f, 64f / 1280f));
                    UVs.Add(new Vector2(191f / 1280f, 0f / 1280f));
                    UVs.Add(new Vector2(191f / 1280f, 64f / 1280f));
                    UVs.Add(new Vector2(191f / 1280f, 0f / 1280f));
                }
                allUVs.AddRange(UVs);

            }
        }
        m.Clear();
        m.vertices = Verts.ToArray();
        m.triangles = triangles.ToArray();
        m.normals = allNormals.ToArray();
        m.uv = allUVs.ToArray();
        m.Optimize();
        m.RecalculateBounds();
        m.RecalculateTangents();
        // m.RecalculateNormals();
    }
    // Update is called once per frame
    void Update()
    {
        /* if (lastUpdate + 1 < Time.time)
         {
             lastUpdate = Time.time;
             UpdatePerlinNoise();
         }*/
        //remove random block
        /*int removeRand = Random.Range(0, WorldInformation.Count);
        Vector3 block = WorldInformation.Select(p => p.Key).ToList()[removeRand];
        RemoveBlock(block);*/
    }
    public void RemoveBlock(Vector3 pos)
    {
        /*GameObject mesh = WorldInformation[pos].mesh;
        MeshChunks[mesh].Remove(pos);
        WorldInformation.Remove(pos);
        GenerateMesh(mesh);
        var neighbors = GetImmediateNeighbors(pos);
        foreach (var neighbor in neighbors)
            if (neighbor.bt == BrickType.air && neighbor.waterLevel > 0 && !FlowingWater.Contains(neighbor))
                FlowingWater.Add(neighbor);*/
        UpdateBricks(pos);
    }
    public void AddBrick(Vector3 pos)
    {
        BrickInformation bi = WorldInformation[pos];
        GameObject mesh = bi.mesh;
        bi.bt = BrickType.rock;
        GenerateMesh(mesh);
        UpdateBricks(pos);
    }
    public void UpdateBricks(Vector3 pos)
    {
        List<Vector3> Neighbors = GetNeighbors(pos, 1, true);
        foreach (Vector3 neighbor in Neighbors)
        {
            BrickInformation neighBrick = WorldInformation[neighbor];
            //recheck if you are a surface brick
            if (neighBrick.bt != BrickType.air)
            {
                BrickInformation aboveNeighBrick = WorldInformation[neighbor + Vector3.up];
                if (aboveNeighBrick.bt == BrickType.air && !aboveNeighBrick.water)
                {
                    neighBrick.SurfaceBrick = true;
                    if (!SurfaceBricks.ContainsKey(neighbor))
                        SurfaceBricks.Add(neighbor, neighBrick);
                }
                else
                {
                    neighBrick.SurfaceBrick = false;
                    if (SurfaceBricks.ContainsKey(neighbor))
                        SurfaceBricks.Remove(neighbor);
                }
            }

            if (!neighBrick.SurfaceBrick && neighBrick.grassObjects.Count > 0)
            {
                foreach (GameObject go in neighBrick.grassObjects)
                    Destroy(go);
                neighBrick.grassObjects.Clear();
            }
        }
    }
    public void AddWaterBlock(Vector3 pos)
    {
        BrickInformation bi = WorldInformation[pos];
        GameObject mesh = bi.mesh;
        bi.bt = BrickType.air;
        bi.waterLevel = 9;
        GenerateMesh(mesh);
        var neighbors = GetImmediateNeighbors(pos);
        FlowingWater.Add(bi);
    }
    public void UpdatePerlinNoise()
    {
        /* fnl.SetFrequency(freq);
         fnl.SetFractalType(FastNoiseLite.FractalType.Ridged);
         fnl.SetFractalOctaves(octaves);
         fnl.SetFractalLacunarity(lac);
         fnl.SetFractalGain(persistance);
         fnl.SetFractalWeightedStrength(weightedStrength);
         Texture2D tx = new Texture2D(100, 100);
         for (int x = -50; x < 50; x++)
             for (int z = -50; z < 50; z++)
                 for (int y = -50; y < 50; y++)
                 {
                     Vector3 pos = new Vector3(x, y, z);

                     float height = fnl.GetNoise(x + 1000, z);
                     //float height = StackedPerlinNoise(pos, octaves, zoom, persistance, lac);
                     tx.SetPixel(x + 50, z + 50, new Color(height, height, height));
                 }
         tx.Apply();
         PerlinNoisePreview.GetComponent<RawImage>().texture = tx;*/

    }
    public float StackedPerlinNoise(Vector3 pos, int octaves, float zoom, float pers, float lac)
    {
        float amp = 1;
        float freq = 1;
        float noiseHeight = 0;
        for (int i = 0; i < octaves; i++)
        {
            var per = Mathf.PerlinNoise((pos.x + 60) / 50f / zoom * freq, (pos.z + 60) / 50f / zoom * freq) * 2.5f - 1;
            noiseHeight += per * amp;
            amp *= pers;
            freq *= lac;
        }
        if (noiseHeight > 1)
            noiseHeight = 1;
        if (noiseHeight < 0)
            noiseHeight = 0;
        return noiseHeight;
    }
    public List<Vector3> GetNeighbors(Vector3 pos, int dist, bool checkSelf = false)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int x = -dist + (int)pos.x; x < dist + pos.x + 1; x++)
            for (int y = -dist + (int)pos.y; y < dist + pos.y + 1; y++)
                for (int z = -dist + (int)pos.z; z < dist + (int)pos.z + 1; z++)
                {
                    Vector3 newpos = new Vector3(x, y, z);
                    if (pos == newpos && !checkSelf)
                        continue;
                    if (WorldInformation.ContainsKey(newpos))
                    {
                        positions.Add(newpos);
                    }
                }
        return positions;
    }
    public List<Vector3> GetAirNeighbors(Vector3 pos)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 newPos = pos + new Vector3(1, 0, 0);
        List<Vector3> positionsToCheck = new List<Vector3>()
        {
            pos + new Vector3(1, 0, 0),
            pos + new Vector3(-1, 0, 0),
            pos + new Vector3(0, 1, 0),
            pos + new Vector3(0, -1, 0),
            pos + new Vector3(0, 0, 1),
            pos + new Vector3(0, 0, -1)
        };
        foreach (Vector3 check in positionsToCheck)
        {
            if (!WorldInformation.ContainsKey(check))
                positions.Add(check);
            else if (WorldInformation[check].bt == BrickType.air)
                positions.Add(check);
        }
        return positions;
    }
    public List<BrickInformation> GetImmediateNeighbors(Vector3 pos)
    {
        List<BrickInformation> positions = new List<BrickInformation>();
        Vector3 newPos = pos + new Vector3(1, 0, 0);
        List<Vector3> positionsToCheck = new List<Vector3>()
        {
            pos + new Vector3(1, 0, 0),
            pos + new Vector3(-1, 0, 0),
            pos + new Vector3(0, 1, 0),
            pos + new Vector3(0, -1, 0),
            pos + new Vector3(0, 0, 1),
            pos + new Vector3(0, 0, -1)
        };
        foreach (Vector3 check in positionsToCheck)
        {
            if (WorldInformation.ContainsKey(check))
                positions.Add(WorldInformation[check]);
        }
        return positions;
    }
    public FastNoiseLite GenMountainGeneration(int seed)
    {
        FastNoiseLite fnl2 = new FastNoiseLite(seed);
        fnl2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fnl2.SetFrequency(.0025f);
        fnl2.SetFractalType(FastNoiseLite.FractalType.Ridged);
        fnl2.SetFractalOctaves(4);
        fnl2.SetFractalLacunarity(2.2f);
        fnl2.SetFractalGain(.7f);
        fnl2.SetFractalWeightedStrength(.3f);
        mapYSize = 50;
        return fnl2;
    }
    public FastNoiseLite GenHillGeneration(int seed)
    {
        FastNoiseLite fnl2 = new FastNoiseLite(seed);
        fnl2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        fnl2.SetFrequency(.0025f);
        fnl2.SetFractalType(FastNoiseLite.FractalType.FBm);
        fnl2.SetFractalOctaves(4);
        fnl2.SetFractalLacunarity(2.2f);
        fnl2.SetFractalGain(.8f);
        fnl2.SetFractalWeightedStrength(1.45f);
        mapYSize = 20;
        return fnl2;
    }
}
public class BrickInformation
{
    public GameObject mesh;
    public BrickType bt;
    public int waterLevel = 0;
    public Vector3 pos;
    public bool SurfaceBrick = false;
    public bool grass = false;
    public bool tree = false;
    public bool water { get { return bt == BrickType.air && waterLevel > 0; } }
    public List<GameObject> grassObjects = new List<GameObject>();
    public GameObject TreeObjects = new GameObject();
}
public enum BrickType
{
    air,
    dirt,
    rock
}
public enum Terrain
{
    Mountain,
    Hill,
    Plains
}
