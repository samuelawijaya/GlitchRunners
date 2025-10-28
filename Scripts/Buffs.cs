using UnityEngine;

public class Buffs : MonoBehaviour
{
    public enum EffectType
    {
        SpeedUp, SuperJump, LowGravity, ReverseControls, Slimed, NoGravity, Stun
    }

    [SerializeField] private float effectDuration = 5f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private int testIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play pickup sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Pick a random effect
            EffectType randomEffect = (EffectType)Random.Range(0, System.Enum.GetValues(typeof(EffectType)).Length);

            // Apply effect
            Player1Effects player1Effects = other.GetComponent<Player1Effects>();
            Player2Effects player2Effects = other.GetComponent<Player2Effects>();
            if (player1Effects != null)
            {
                player1Effects.ApplyEffect(randomEffect, effectDuration);
            }
            else if (player2Effects != null) 
            {
                player2Effects.ApplyEffect(randomEffect, effectDuration);
            }

                // Remove pickup from scene
                gameObject.SetActive(false);
        }
    }
}
