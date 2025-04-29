using UnityEngine;

/// <summary>
/// Marks a pooled power-up object with its prefab index for correct pooling.
/// </summary>
public class PooledObject : MonoBehaviour
{
    /// <summary>Index of the prefab type in the pool.</summary>
    public int PoolID;
}