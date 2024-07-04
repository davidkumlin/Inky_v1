using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class P_Stats : MonoBehaviour
{
    public static P_Stats Instance { get; private set; }

    [SerializeField] public GameObject inkyObj;
    [SerializeField] private P_Inky pinky;
    [SerializeField] private inky_animation inkyAni;
    private Vector2 inkyPos;
    public bool inkyActive;

    public Vector2 aimPos;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 cameraOffset;

    public static float hp = 100f;
    public float maxHp = 100f;

    public bool OnWall { get; private set; } = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pinky = FindObjectOfType<P_Inky>();
        inkyAni = FindObjectOfType<inky_animation>();
        mainCamera = Camera.main;

        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }
        if (inkyAni == null)
        {
            Debug.LogError("inky_ani not found");
        }
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found");
        }
        
    }

    void Start()
    {
        GameManager.OnWallChanged += OnWallStatus;
        
    }

    private void OnWallStatus(bool OnWall)
    {
        this.OnWall = OnWall;
    }

    void Update()
    {
        aimPos = pinky.CurrentAim;
        inkyPos = pinky.whereIsInky;

        ActiveUnit();
    }

    public void Damage(float Ammount)
    {
        hp -= Ammount;
        if (inkyAni.takinDamage == false)
        {
            inkyAni.takinDamage = true;

            if (hp <= 0)
            {
                hp = 0;
                inkyAni.dying = true;
                pinky.moveSpeed = 0;
            }
        }
        else
        {
            Debug.Log("cannot take damage");
        }
    }

    public void ResetLevel()
    {
        hp = maxHp; // Reset HP to maxHp
        //SceneStateManager.Instance.SaveSceneState(SceneManager.GetActiveScene().name, new SceneState { playerState = CaptureState() });
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ActiveUnit()
    {
        if (!OnWall)
        {
            inkyActive = true;
        }
        else
        {
            inkyActive = true;
        }
    }

    private void LateUpdate()
    {
        if (pinky == null)
        {
            pinky = FindObjectOfType<P_Inky>();
            if (pinky == null)
            {
                Debug.LogError("P_Inky not found");
                return;
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found");
                return;
            }
        }

        Transform activeCharacter = null;
        if (pinky.inkyActive)
        {
            activeCharacter = pinky.transform;
        }

        if (activeCharacter != null)
        {
            Vector3 targetPosition = activeCharacter.position + cameraOffset;
            targetPosition.z = mainCamera.transform.position.z;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    public PlayerState CaptureState()
    {
        return new PlayerState
        {
            inkyPos = this.inkyPos,
            //hp = this.hp,
        };
    }

    public void ApplyState(PlayerState state)
    {
        this.inkyPos = state.inkyPos;
       // this.hp = state.hp;
    }

    public void LoadScene(string sceneName)
    {
        SceneStateManager.Instance.SaveSceneState(SceneManager.GetActiveScene().name, new SceneState { playerState = CaptureState() });
        SceneManager.LoadScene(sceneName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*SceneState state = SceneStateManager.Instance.LoadSceneState(scene.name);
        if (state != null)
        {
            ApplyState(state.playerState);
        }
        else
        {
            // If there is no saved state, reset HP to maxHp
            hp = maxHp;
        }
        */

       pinky = FindObjectOfType<P_Inky>();
        mainCamera = Camera.main;

        if (pinky == null)
        {
            Debug.LogError("P_Inky not found");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found");
        }
    }

}

[System.Serializable]
public class PlayerState
{
    public Vector2 inkyPos;
    public float hp;
}
