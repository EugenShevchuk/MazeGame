using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Infrastructure
{
    [CreateAssetMenu(menuName = "Infrastructure/AssetReferences", fileName = "AssetReferences")]
    public sealed class AssetReferences : ScriptableObject
    {
        public AssetReference CellViewRef;
        public AssetReference ProcessorViewRef;
        public AssetReference ProcessorSurroundingViewRef;
        public AssetReference SpawnerViewRef;
        public AssetReference BugViewRef;
    }
}