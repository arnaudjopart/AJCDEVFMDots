using Unity.Entities;

[GenerateAuthoringComponent]
public struct JumpingComponent : IComponentData
{
    public float m_jumpForce;
    public float m_currentSpeed;
    public float m_gravity;
}