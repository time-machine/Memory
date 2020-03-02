using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Scripts
{
    public class Level1HandlePrefabLifecycleTraditional : MonoBehaviour
    {
        [SerializeField] private Transform spawnAnchor = null;
        [SerializeField] private float separation = 1.1f;
        [SerializeField] private int instanceCount = 10;
        [SerializeField] private AssetReference prefabReference = null;

        private AsyncOperationHandle<GameObject> _asyncOperationHandle;
        private readonly List<GameObject> _instances = new List<GameObject>();

        public void HandleLifecycle()
        {
            var hasSpawnedInstances = _asyncOperationHandle.IsValid();
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
            _asyncOperationHandle = prefabReference.LoadAssetAsync<GameObject>();
            _asyncOperationHandle.Completed += handle =>
            {
                var prefab = handle.Result;
                SpawnPrefabs(prefab);
            };
        }

        private void SpawnPrefabs(GameObject prefab)
        {
            for (var i = 0; i < instanceCount; i++)
            {
                var newGameObject = Instantiate(prefab, spawnAnchor.position + i * separation * Vector3.right,
                    spawnAnchor.rotation);
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
            Addressables.Release(_asyncOperationHandle);
        }
    }
}