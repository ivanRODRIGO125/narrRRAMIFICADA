using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class DialogoManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;
    public GameObject restartButton;

    [Header("Nodos de diálogo")]
    public List<DialogoNode> nodes;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoBase;
    public Vector2 rangoPitch = new Vector2(0.9f, 1.2f);
    public float intervaloSonido = 0.05f;

    private DialogoNode currentNode;
    private Coroutine typingCoroutine;

    void Start()
    {
        restartButton.SetActive(false);
        StartDialogue("1");
    }

    public void StartDialogue(string startNodeId)
    {
        currentNode = nodes.Find(n => n.id == startNodeId);
        DisplayCurrentNode();
    }

    void DisplayCurrentNode()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        HideAllOptions();
        typingCoroutine = StartCoroutine(TypeSentence(currentNode.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (char.IsLetterOrDigit(letter) && sonidoBase != null)
            {
                audioSource.pitch = Random.Range(rangoPitch.x, rangoPitch.y);
                audioSource.PlayOneShot(sonidoBase);
            }

            yield return new WaitForSeconds(0.03f);
        }

        ShowOptionsOrEnd();
    }

    void ShowOptionsOrEnd()
    {
        if (currentNode.options.Length == 0)
        {
            // Nodo final, activar reinicio
            restartButton.SetActive(true);
        }
        else
        {
            ShowOptions();
        }
    }

    void ShowOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentNode.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.options[i];

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void HideAllOptions()
    {
        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    void SelectOption(int optionIndex)
    {
        string nextId = currentNode.nextNodeIds[optionIndex];
        currentNode = nodes.Find(n => n.id == nextId);
        DisplayCurrentNode();
    }

  
    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}