using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool for collectable items (coins, stars).
/// </summary>
public class CollectablePool : MonoBehaviour
{
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private int initialSize = 5;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(collectablePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetCollectable()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(collectablePrefab);
    }

    public void ReturnCollectable(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}