using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    [SerializeField] Transform spriteParent;
    [SerializeField] PlayerConfig config;
    
    private Rigidbody2D rb;
    private float moveInput;

    public bool IsFacingRight { get; private set; } = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable() => inputReader.OnMove += HandleMoveInput;
    private void OnDisable() => inputReader.OnMove -= HandleMoveInput;

    private void HandleMoveInput(float value) => moveInput = value;
    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateFacing();
    }

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
