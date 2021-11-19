using UnityEngine;

namespace Project.Infrastructure
{
    [CreateAssetMenu(menuName = "Infrastructure/Configuration", fileName = "Configuration)")]
    public sealed class Configuration : ScriptableObject
    {
        public float CameraMoveSpeed = 5f;
        public float CameraZoomSpeed = .5f;
        public float ZoomInMin = 5f;
        public float ZoomOutMax = 12f;
        
        public Level[] Levels;
    }
}