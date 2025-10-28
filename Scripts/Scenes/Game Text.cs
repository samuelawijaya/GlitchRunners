using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameText : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;

    [Header("Players to Freeze")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private Player1 player1Script;
    private Player2 player2Script;

    private void Start()
    {
        player1Script = player1.GetComponent<Player1>();
        player2Script = player2.GetComponent<Player2>();
        SetPlayersFrozen(true);

        StartCoroutine(PlayIntroSequence());
        

    }

    private IEnumerator PlayIntroSequence()
    {
        // Messages you want to show
        string[] messages = {
            "Outrun your opponent !",
            "Collect Crystals to acquire special effects !",
            "Go !"
        };

        messageText.gameObject.SetActive(true);

        foreach (string msg in messages)
        {
            yield return StartCoroutine(ShowMessage(msg));
        }

        SetPlayersFrozen(false);
        messageText.gameObject.SetActive(false);
    }

    private IEnumerator ShowMessage(string text)
    {
        messageText.text = text;
        Color c = messageText.color;

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        c.a = 1f;
        messageText.color = c;

        // Wait while visible
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        c.a = 0f;
        messageText.color = c;
    }

    private void SetPlayersFrozen(bool frozen)
    {
        if (player1Script != null)
            player1Script.enabled = !frozen; // disable movement script
        if (player2Script != null)
            player2Script.enabled = !frozen;
    }
}
