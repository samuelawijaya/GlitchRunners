using UnityEngine;
using UnityEngine.UI; 

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private Text Player1Wins;
    [SerializeField] private Text Player2Wins;

    [SerializeField] private Button Restart;
    [SerializeField] private Button Exit;

    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0f; // 2D so volume doesn’t fall off
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.LogError($"{other.name} went out of bounds! Exiting game...");


            //AudioSource tempSource = new GameObject("TempAudio").AddComponent<AudioSource>();
            //tempSource.clip = deathSound;
            //tempSource.volume = 1f;           // Full volume
            //tempSource.spatialBlend = 0.3f;     // 0 = 2D, 1 = 3D
            //tempSource.Play();
            //Destroy(tempSource.gameObject, deathSound.length);

            if (deathSound != null) sfxSource.PlayOneShot(deathSound, 1f);

            //Application.Quit();

            if(other.gameObject == Player1)
            {
                if (Player2Wins != null)
                    Player2Wins.gameObject.SetActive(true);
            } else
            {
                if (Player1Wins != null)
                Player1Wins.gameObject.SetActive(true);
            }
            if(Restart != null)
            {
                Restart.gameObject.SetActive(true);
            }

            if (Exit != null)
            {
                Exit.gameObject.SetActive(true);
            }




            other.gameObject.SetActive(false);
        }

        
    }
}
