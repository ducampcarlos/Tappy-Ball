using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool for power-up items supporting multiple prefab types (e.g., speed, magnet)
/// with equal spawn probability.
/// </summary>
public class PowerUpPool : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private int initialSizePerType = 3;

    private Dictionary<int, Queue<GameObject>> poolDict = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        // Initialize a queue for each prefab type and pre-instantiate
        for (int i = 0; i < powerUpPrefabs.Length; i++)
        {
            var queue = new Queue<GameObject>();
            for (int j = 0; j < initialSizePerType; j++)
            {
                var obj = Instantiate(powerUpPrefabs[i]);
                obj.SetActive(false);
                var pooledComp = obj.GetComponent<PooledObject>() ?? obj.AddComponent<PooledObject>();
                pooledComp.PoolID = i;
                queue.Enqueue(obj);
            }
            poolDict[i] = queue;
        }
    }

    /// <summary>
    /// Returns a pooled power-up GameObject of a random type.
    /// </summary>
    public GameObject GetPowerUp()
    {
        int id = Random.Range(0, powerUpPrefabs.Length);
        var queue = poolDict[id];

        if (queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // No available instance, instantiate a new one
            var obj = Instantiate(powerUpPrefabs[id]);
            var pooledComp = obj.GetComponent<PooledObject>() ?? obj.AddComponent<PooledObject>();
            pooledComp.PoolID = id;
            return obj;
        }
    }

    /// <summary>
    /// Returns a power-up GameObject back to its corresponding pool.
    /// </summary>
    public void ReturnPowerUp(GameObject obj)
    {
        var pooledComp = obj.GetComponent<PooledObject>();
        if (pooledComp == null)
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        int id = pooledComp.PoolID;
        if (!poolDict.ContainsKey(id))
            poolDict[id] = new Queue<GameObject>();
        poolDict[id].Enqueue(obj);
    }
}
