using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple pool to reuse obstacle instances and reduce GC.
/// </summary>
public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int initialSize = 10;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObstacle()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(obstaclePrefab);
    }

    public void ReturnObstacle(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}