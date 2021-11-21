using UnityEngine;

namespace Project.Infrastructure
{
    [CreateAssetMenu(menuName = "Infrastructure/Configuration", fileName = "Configuration)")]
    public sealed class Configuration : ScriptableObject
    {
        public float CameraMoveSpeed = 5f;
        public float CameraZoomSpeed = .5f;
        public float MinCameraSize = 5f;
        public float ZoomOutMax = 12f;
        public float CameraBoundOffset = 3f;
        
        public Level[] Levels;
    }
}