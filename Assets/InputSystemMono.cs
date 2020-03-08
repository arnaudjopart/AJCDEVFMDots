using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemMono : MonoBehaviour
{
    public CubeSpawnerMono m_spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_spawner.SpawnCubes();
        }
    }
    
    
}
