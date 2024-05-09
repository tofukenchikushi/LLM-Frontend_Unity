using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class UIController : MonoBehaviour
{
    private VisualElement _func1Container;

    private Button _funcSelectionButton1;

    private Button _funcCloseButton1;

    private VisualElement _func1UI;

    private VisualElement _centerLogo;

    private Button _submitButton1;
    
    private Label _userPrompt1;

    private Label _llmPrompt1;

    private TextField _inputField1;


    




    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _func1Container = root.Q<VisualElement>("Func1Container");
        _funcSelectionButton1 = root.Q<Button>("FuncSelectionButton1");
        _funcCloseButton1 = root.Q<Button>("FuncCloseButton1");
        _func1UI = root.Q<VisualElement>("Func1UI");
        _centerLogo = root.Q<VisualElement>("CenterLogo");
        _submitButton1 = root.Q<Button>("SubmitButton1");
        _userPrompt1 = root.Q<Label>("UserPrompt1");
        _llmPrompt1 = root.Q<Label>("LlmPrompt1");
        _inputField1 = root.Q<TextField>("InputField1");
        

        _func1Container.style.display = DisplayStyle.None;

        _funcSelectionButton1.RegisterCallback<ClickEvent>(OnSelectionButton1Clicked);
        _funcCloseButton1.RegisterCallback<ClickEvent>(OnCloseButton1Clicked);
        _submitButton1.RegisterCallback<ClickEvent>(OnSubmitButton1Clicked);

        Invoke("AnimateLogo", 0.1f);
    }

    private void AnimateLogo()
    {
        _centerLogo.ToggleInClassList("centerLogo--down");
        _centerLogo.RegisterCallback<TransitionEndEvent>(AnimateBackLogo);
    }

    private void AnimateBackLogo(TransitionEndEvent evt)
    {
        _centerLogo.ToggleInClassList("centerLogo--down");
    }

    private void OnSelectionButton1Clicked(ClickEvent evt)
    {
        _func1Container.style.display = DisplayStyle.Flex;
        _func1UI.AddToClassList("func1UI--visible");
    }

    private void OnCloseButton1Clicked(ClickEvent evt)
    {
        _func1Container.style.display = DisplayStyle.None;
        _func1UI.RemoveFromClassList("func1UI--visible");
    }

    private void OnSubmitButton1Clicked(ClickEvent evt)
    {
        string inputText = _inputField1.text;
        _userPrompt1.text = inputText;
        StartCoroutine(SendToOpenAI(inputText));
    }

    IEnumerator SendToOpenAI(string userInput)
    {
        string jsonData = "{\"model\":\"HF://mlc-ai/Llama-3-8B-Instruct-q4f16_1-MLC\",\"messages\":[{\"role\":\"system\",\"content\":\"You are a helpful assistant.\"},{\"role\":\"user\",\"content\":\"" + userInput + "\"}]}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest("http://127.0.0.1:8000/v1/chat/completions", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // request.SetRequestHeader("Authorization", "Bearer YOUR_OPENAI_API_KEY");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            ProcessResponse(request.downloadHandler.text);
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        var responseObj = JsonUtility.FromJson<ResponseRoot>(jsonResponse);
        string content = responseObj.choices[0].message.content;
        _llmPrompt1.text = content;
    }

    [System.Serializable]
    private class ResponseRoot
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class Message
    {
        public string content;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
