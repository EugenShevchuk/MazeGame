using System.Threading.Tasks;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Infrastructure;
using Project.UnityComponents;
using Project.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Systems
{
    internal sealed class AssetsPreLoadingSystem :  IEcsInitSystem
    {
        [EcsShared] private readonly SharedData _data = default;

        public void Init(EcsSystems systems)
        {
            Addressables.InitializeAsync().Completed += LoadAssets;
        }

        private async void LoadAssets(AsyncOperationHandle<IResourceLocator> obj)
        {
#if UNITY_EDITOR || DEBUG
            var timer = new CountUpTimer(TimerType.UnscaledTick);
            timer.Start();
#endif
            var cellViewTask = _data.References.CellViewRef.LoadAssetAsync<CellView>().Task;
            var processorViewTask = _data.References.ProcessorViewRef.LoadAssetAsync<ProcessorView>().Task;
            var spawnerViewTask = _data.References.SpawnerViewRef.LoadAssetAsync<SpawnerView>().Task;
            var bugViewTask = _data.References.BugViewRef.LoadAssetAsync<BugView>().Task;
            var processorSurroundingViewTask = _data.References.ProcessorSurroundingViewRef
                .LoadAssetAsync<ProcessorSurroundingView>().Task;
            
            await Task.WhenAll(cellViewTask, processorViewTask, processorSurroundingViewTask, spawnerViewTask, bugViewTask);

            _data.Assets = new AssetsData
            {
                CellPrefab = cellViewTask.Result,
                ProcessorPrefab = processorViewTask.Result,
                SpawnerPrefab = spawnerViewTask.Result,
                ProcessorSurroundingPrefab = processorSurroundingViewTask.Result,
                BugPrefab = bugViewTask.Result,
            };

#if UNITY_EDITOR || DEBUG
            Debug.Log($"Assets loaded, it took {timer.Time:0.000000}");
            timer.Stop();
#endif
        }
    }
}