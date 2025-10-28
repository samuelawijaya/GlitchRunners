using UnityEngine;
using System.Collections;

public class Player1Effects : MonoBehaviour
{
    [SerializeField] private Player1 player;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speedUp;
    [SerializeField] private float jumpBoost;
    [SerializeField] private float slowDown;

    private float originalSpeed;
    private float originalJump;
    private float originalGravity;
    private bool controlsReversed = false;

    private void Awake()
    {
        originalSpeed = player.GetMoveSpeed1();
        originalJump = player.GetJumpSpeed1();
        originalGravity = rb.gravityScale;
    }

    public void ApplyEffect(Buffs.EffectType effect, float duration)
    {
        StopAllCoroutines(); // clear old buffs
        ResetEffects();
        StartCoroutine(EffectRoutine(effect, duration));
    }

    private IEnumerator EffectRoutine(Buffs.EffectType effect, float duration)
    {
        Debug.Log($"Applying {effect} for {duration} seconds");

        switch (effect)
        {
            case Buffs.EffectType.SpeedUp:
                player.SetMoveSpeed1(originalSpeed * speedUp);
                break;

            case Buffs.EffectType.SuperJump:
                player.SetJumpSpeed1(originalJump * jumpBoost);
                break;

            case Buffs.EffectType.LowGravity:
                rb.gravityScale = 1f;
                break;

            case Buffs.EffectType.ReverseControls:
                controlsReversed = true;
                break;

            case Buffs.EffectType.Slimed:
                player.SetMoveSpeed1(originalSpeed * slowDown);
                break;

            case Buffs.EffectType.NoGravity:
                rb.gravityScale = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                break;
            case Buffs.EffectType.Stun:
                player.SetMoveSpeed1(0f);
                player.SetJumpSpeed1(0f);
                rb.linearVelocity = Vector2.zero;
                break;
        }

        yield return new WaitForSeconds(duration);

        // Reset all modified properties
        ResetEffects();
    }

    private void ResetEffects()
    {
        player.SetMoveSpeed1(originalSpeed);
        player.SetJumpSpeed1(originalJump);
        rb.gravityScale = originalGravity;
        transform.localScale = Vector3.one;
        controlsReversed = false;

        Debug.Log("Effects reset");
    }


    public Vector2 ProcessInput(Vector2 input)
    {
        return controlsReversed ? -input : input;
    }
}
