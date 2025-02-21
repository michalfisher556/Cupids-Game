using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    public QuestionData question;

    [SerializeField]
    private TMP_InputField[] inputFields; // Array of input fields

    [SerializeField]
    private GameObject letterContainer;

    [SerializeField]
    private Image questionImageUI;

    [SerializeField]
    private Text questionTextUI;

    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private AudioClip correctSound;
    
    [SerializeField]
    private AudioClip wrongSound;

    private AudioSource audioSource;
    private int hintIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (question != null)
        {
            if (!string.IsNullOrEmpty(question.questionText))
            {
                questionTextUI.text = question.questionText;
            }

            if (question.questionImage != null)
            {
                questionImageUI.sprite = question.questionImage;
                questionImageUI.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("question.questionImage is null");
                questionImageUI.gameObject.SetActive(false);
            }
        }
    }

    public void CheckAnswer()
{
    Debug.Log("CheckAnswer function called!");

    // Collect all input field values and combine them into one string
    string userInput = "";
    foreach (TMP_InputField inputField in inputFields)
    {
        userInput += inputField.text; // Append each input field's text
    }
    

    userInput = userInput.ToUpper().Trim(); // Convert to uppercase and trim spaces
    Debug.Log("User input (processed): " + userInput);

    // Process the correct answer: Remove spaces and convert to uppercase
    string correctAnswer = question.answer.Replace(" ", "").ToUpper();
    Debug.Log("Correct answer (processed): " + correctAnswer);

    // Compare processed user input with processed correct answer
    if (userInput == correctAnswer)
    {
        PlaySound(correctSound);
        DisplayMessage("CORRECT! 🎉", Color.green);
        ProceedToNextQuestion();
    }
    else
    {
        HighlightLetters(userInput, correctAnswer);
        PlaySound(wrongSound);
    }

    // Clear all input fields after checking
    foreach (TMP_InputField inputField in inputFields)
    {
        inputField.text = "";
    }
}


    private void HighlightLetters(string userInput, string correctAnswer)
    {
        for (int i = 0; i < letterContainer.transform.childCount; i++)
        {
            GameObject letter = letterContainer.transform.GetChild(i).gameObject;
            TMP_Text letterText = letter.GetComponentInChildren<TMP_Text>();
            Image letterBackground = letter.GetComponent<Image>();

            if (i < userInput.Length && i < correctAnswer.Length && userInput[i] == correctAnswer[i])
            {
                letterBackground.color = Color.green;
            }
            else
            {
                letterBackground.color = Color.red;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void DisplayMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
            StartCoroutine(FadeMessage());
        }
    }

    private IEnumerator FadeMessage()
    {
        float duration = 2f;
        float elapsedTime = 0f;

        Color startColor = messageText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (elapsedTime < duration)
        {
            messageText.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        messageText.color = endColor;
    }

    private void ProceedToNextQuestion()
    {
        hintIndex = 0;
        LoadNextQuestion();
    }

    private void LoadNextQuestion()
    {
        Debug.Log("Loading the next question...");
    }
}

