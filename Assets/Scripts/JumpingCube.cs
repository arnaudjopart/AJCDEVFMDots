using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingCube : MonoBehaviour
{
    [SerializeField]
    private float m_currentSpeed;
    [SerializeField]
    private float m_jumpSpeed;
    [SerializeField]
    private float m_gravity;

    [SerializeField] private MeshRenderer m_meshRenderer;
    
    [SerializeField]
    private Material[] m_materials;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            
            m_currentSpeed = m_jumpSpeed;
        }

        m_currentSpeed -= m_gravity * Time.deltaTime;
        transform.position+=new Vector3(0,m_currentSpeed,0);
    }

    public void SetParamRandom()
    {
        m_gravity = Random.value;
        m_jumpSpeed = Random.value*.8f;
        var materialIndex = Random.Range(0, m_materials.Length);
        m_meshRenderer.material = m_materials[materialIndex];
    }
}
