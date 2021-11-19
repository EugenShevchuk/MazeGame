using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Project.Events
{
    public struct UnloadSceneRequest
    {
        public SceneInstance Scene;
        public LoadSceneMode Mode;
    }
}