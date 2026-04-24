using Configs;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform spriteParent;

    private ConfigProvider configProvider;
    private InputProvider inputProvider;
    private Rigidbody2D rb;

    private PlayerConfig config;
    private int level = 0;
    private float moveInput;

    public bool IsFacingRight { get; private set; } = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        configProvider = ServiceLocator.Get<ConfigProvider>();
        if (configProvider)
        {
            UpdateConfig();
            configProvider.ConfigUpdated += UpdateConfig;
        }

        inputProvider = ServiceLocator.Get<InputProvider>();
        if(inputProvider)
            inputProvider.OnMove += HandleMoveInput;
    }
    private void OnEnable()
    {
        if(configProvider)
            configProvider.ConfigUpdated += UpdateConfig;

        if (inputProvider) 
            inputProvider.OnMove += HandleMoveInput;
    }  
    void OnDisable() 
    {
        if (configProvider)
            configProvider.ConfigUpdated -= UpdateConfig;

        if (inputProvider)
            inputProvider.OnMove -= HandleMoveInput;
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateFacing();
    }

    private void UpdateConfig()
    {
        if (configProvider)
        {
            config = configProvider.GetConfigs<PlayerConfig>()[level];
        }
    }

    private void HandleMoveInput(float value) => moveInput = value;

    private void ApplyMovement()
    {
        float targetVelocity = moveInput * config.MaxSpeed;
        float currentVelocity = rb.linearVelocity.x;

        bool isAccelerating = Mathf.Abs(moveInput) > 0.01f;
        float rate = isAccelerating ? config.Acceleration : config.Deceleration;

        float newVelocity = Mathf.MoveTowards(currentVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newVelocity, rb.linearVelocity.y);
    }

    private void UpdateFacing()
    {
        if (moveInput > 0.01f && !IsFacingRight)
            Flip();
        else if (moveInput < -0.01f && IsFacingRight)
            Flip();
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 newLocalScale = new Vector3(
            spriteParent.localScale.x * -1,
            spriteParent.localScale.y,
            spriteParent.localScale.z);
        spriteParent.localScale = newLocalScale;
    }


}
