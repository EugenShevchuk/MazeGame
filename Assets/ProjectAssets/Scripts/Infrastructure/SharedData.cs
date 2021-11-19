using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Project.Infrastructure
{
    public sealed class SharedData
    {
        public EcsWorld CurrentWorld;
        public Grid Grid;
        public AssetsData Assets;
        public Level CurrentLevel;
        public Camera GameCamera;

        public List<SceneInstance> ActiveScenes = new List<SceneInstance>(8);
        
        public Configuration Configuration;
        public AssetReferences References;
    }
}