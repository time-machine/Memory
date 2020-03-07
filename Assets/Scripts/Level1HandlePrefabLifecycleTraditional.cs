using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    public class Level1HandlePrefabLifecycleTraditional : MonoBehaviour
    {
        [SerializeField] private Transform spawnAnchor = null;
        [SerializeField] private float separation = 1.1f;
        [SerializeField] private int instanceCount = 10;
        [SerializeField] private AssetReference prefabReference = null;

        private readonly List<GameObject> _instances = new List<GameObject>();

        public void HandleLifecycle()
        {
            var hasSpawnedInstances = _instances.Any();
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
                var pos = spawnAnchor.position + i * separation * Vector3.right;
                var rotation = spawnAnchor.rotation;
                var asyncOperationHandle = prefabReference.InstantiateAsync(pos, rotation);
                asyncOperationHandle.Completed += handle => _instances.Add(handle.Result);
            }
        }

        private void Despawn()
        {
            foreach (var instance in _instances)
            {
                Addressables.ReleaseInstance(instance);
            }
            _instances.Clear();
        }
    }
}