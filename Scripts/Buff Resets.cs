using UnityEngine;

public class BuffResets : MonoBehaviour
{
    [SerializeField] GameObject[] buffs;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            

            // Find all inactive buff pickups and re-enable them
            foreach (GameObject buff in buffs)
            {
                buff.SetActive(true);
            }
        }
    }
}
