using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawnerMono : MonoBehaviour
{
    public GameObject m_cubePrefab;
    public int width;
    public int length;
    public int m_nbOfPrefabToSpawn;

    public NbOfCubesDisplayer m_nbOfCubesDisplayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCubes()
    {
        for (int i = 0; i < m_nbOfPrefabToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3((Random.value*2-1)*width,0,(Random.value*2-1)*length);
            GameObject instance = Instantiate(m_cubePrefab, spawnPosition, quaternion.identity);
            instance.GetComponent<JumpingCube>().SetParamRandom();
        }
        
        m_nbOfCubesDisplayer.AddToCount(m_nbOfPrefabToSpawn);
    }
}
