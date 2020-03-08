using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class ECSManager : MonoBehaviour
{
    public NbOfCubesDisplayer m_nbOfCubeDisplayer;
    
    public GameObject m_prefab;

    public int m_nbOfInstancesToSpawn;

    public float m_width;
    public float m_length;

    // Materials à appliquer au mesh, plus joli
    public Material[] m_materials;
    // Start is called before the first frame update
    void Start()
    {
        m_world = World.DefaultGameObjectInjectionWorld;
        m_manager = m_world.EntityManager;
        
        //Permet de convertir une prefab Mono en entité
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(m_world,null);
        m_entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(m_prefab, settings);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEntitiesFromPrefab();
            m_nbOfCubeDisplayer.AddToCount(m_nbOfInstancesToSpawn);
        }
    }

    private void SpawnEntitiesFromPrefab()
    {
        //Liste optimisée pour ECS
        var entitiesNativeArray = new NativeArray<Entity>(m_nbOfInstancesToSpawn,Allocator.Temp);
        m_manager.Instantiate(m_entityPrefab, entitiesNativeArray);

        foreach (var entity in entitiesNativeArray)
        {
            var randomX = Random.Range(0, m_length);
            var randomZ = Random.Range(0, m_width);
            
            m_manager.SetComponentData(entity, new Translation
            {
                Value = new float3(randomX,0,randomZ)
            });

            //SharedComponentData >< ComponentData
            var meshRenderer = m_manager.GetSharedComponentData<RenderMesh>(entity);

            meshRenderer.material = m_materials[Random.Range(0, m_materials.Length)];
            m_manager.SetSharedComponentData(entity, meshRenderer);
            
            var component = m_manager.GetComponentData<JumpingComponent>(entity);
            component.m_jumpForce = Random.value*50;
            component.m_currentSpeed = component.m_jumpForce;
            m_manager.SetComponentData(entity,component);
        }
    }

    private EntityManager m_manager;
    private World m_world;
    private Entity m_entityPrefab;
    
}
/// <summary>
/// ------------------------------ ECS -------------------------------------
/// </summary>

public class JumpSystem : ComponentSystem
 {
     protected override void OnUpdate()
     {
         Entities.WithAll<JumpingComponent>().ForEach((Entity _entity, ref Translation _translation, ref
             JumpingComponent _jumpingComponent) =>
         {
             _translation.Value += new float3(0,_jumpingComponent.m_currentSpeed*Time.DeltaTime,0);
             _jumpingComponent.m_currentSpeed -= _jumpingComponent.m_gravity * Time.DeltaTime;
         });
     }
 }
 
public class GroundDetector : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<JumpingComponent>().ForEach((Entity _entity, ref Translation _translation, ref
            JumpingComponent _jumpingComponent) =>
        {
            if (_translation.Value.y < 0)
            {
                _jumpingComponent.m_currentSpeed = _jumpingComponent.m_jumpForce;
            }
        });
    }
}

/// <summary>
/// --------------------------------- Job System ------------------------------------------
/// </summary>
public class JumpJobSystem : JobComponentSystem
 {
     [BurstCompile]
     protected override JobHandle OnUpdate(JobHandle _inputDeps)
     {
         float deltaTime = Time.DeltaTime;
         return Entities.ForEach((ref Translation _translation, ref JumpingComponent _jumpingComponent) =>
             {
                 _translation.Value += new float3(0,_jumpingComponent.m_currentSpeed*deltaTime,0);
                 _jumpingComponent.m_currentSpeed -= _jumpingComponent.m_gravity * deltaTime;
             })
             .Schedule(_inputDeps);
     }
 }
 

public class GroundDetectorJobSystem : JobComponentSystem
{
    [BurstCompile]
    protected override JobHandle OnUpdate(JobHandle _inputDeps)
    {
        return Entities.ForEach((ref Translation _translation, ref JumpingComponent _jumpingComponent) =>
            {
                if (_translation.Value.y < 0)
                {
                    _jumpingComponent.m_currentSpeed = _jumpingComponent.m_jumpForce;
                }
            })
            .Schedule(_inputDeps);
    }
}