using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    private static SceneStateManager _instance;
    public static SceneStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SceneStateManager");
                _instance = go.AddComponent<SceneStateManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Dictionary<string, SceneState> sceneStates = new Dictionary<string, SceneState>();

    public void SaveSceneState(string sceneName, SceneState state)
    {
        sceneStates[sceneName] = state;
    }

    public SceneState LoadSceneState(string sceneName)
    {
        sceneStates.TryGetValue(sceneName, out SceneState state);
        return state;
    }
}

[System.Serializable]
public class SceneState
{
    public PlayerState playerState;
}
