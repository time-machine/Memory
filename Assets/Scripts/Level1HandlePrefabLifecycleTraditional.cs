using System.Collections.Generic;
using UnityEngine;

public class Level1HandlePrefabLifecycleTraditional : MonoBehaviour
{
    [SerializeField] private Transform spawnAnchor = null;
    [SerializeField] private float separation = 1.1f;
    [SerializeField] private int instanceCount = 10;
    [SerializeField] private GameObject prefabReference = null;

    List<GameObject> _instances = new List<GameObject>();

    public void HandleLifecycle()
    {
        var hasSpawnedInstances = _instances.Count > 0;
        if (hasSpawnedInstances)
        {
            Despawn();
        }
        else
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        for (var i = 0; i < instanceCount; i++)
        {
            var newGameObject = Instantiate(prefabReference, spawnAnchor.position + i * separation * Vector3.right, spawnAnchor.rotation);
            _instances.Add(newGameObject);
        }
    }

    private void Despawn()
    {
        foreach (var instance in _instances)
        {
            Destroy(instance);
        }
        _instances.Clear();
    }
}