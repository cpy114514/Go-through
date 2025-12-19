using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public Text dialogueText;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.04f;

    private string[] sentences;
    private int index;

    private bool isTalking = false;
    private bool isTyping = false;

    // ★ 防止退出对话后立刻再次触发
    private bool inputLocked = false;

    private Coroutine typingCoroutine;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // 不在对话 or 输入被锁 → 直接返回
        if (!isTalking || inputLocked) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // 正在打字 → 立刻显示整句
                StopCoroutine(typingCoroutine);
                dialogueText.text = sentences[index];
                isTyping = false;
            }
            else
            {
                // 已显示完整 → 下一句
                NextSentence();
            }
        }
    }

    // -------------------------
    // 对外调用：开始对话
    // -------------------------
    public void StartDialogue(string[] lines)
    {
        // 正在对话 or 键被锁 → 不允许开始
        if (isTalking || inputLocked) return;

        sentences = lines;
        index = 0;
        isTalking = true;

        dialoguePanel.SetActive(true);
        StartTyping(sentences[index]);
    }

    void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextSentence()
    {
        index++;

        if (index >= sentences.Length)
        {
            EndDialogue();
        }
        else
        {
            StartTyping(sentences[index]);
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        isTyping = false;

        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // ★ 锁住 E 键一帧，防止立刻重进
        inputLocked = true;
        StartCoroutine(UnlockInputNextFrame());
    }

    IEnumerator UnlockInputNextFrame()
    {
        yield return null; // 等一帧
        inputLocked = false;
    }

    // -------------------------
    // 给 Player / NPC 使用
    // -------------------------
    public bool IsTalking()
    {
        return isTalking;
    }
}
