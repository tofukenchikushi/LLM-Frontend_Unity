using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement _funcTranslateContainer;

    private Button _funcSelectionButton1;

    private Button _funcCloseButton1;

    private VisualElement _translateUI;

    private VisualElement _centerLogo;

    private Button _submitButton1;
    
    private Label _userPrompt1;

    private Label _llmPrompt1;

    private TextField _inputField1;




    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _funcTranslateContainer = root.Q<VisualElement>("FuncTranslateContainer");
        _funcSelectionButton1 = root.Q<Button>("FuncSelectionButton1");
        _funcCloseButton1 = root.Q<Button>("FuncCloseButton1");
        _translateUI = root.Q<VisualElement>("TranslateUI");
        _centerLogo = root.Q<VisualElement>("CenterLogo");
        _submitButton1 = root.Q<Button>("SubmitButton1");
        _userPrompt1 = root.Q<Label>("UserPrompt1");
        _llmPrompt1 = root.Q<Label>("LlmPrompt1");
        _inputField1 = root.Q<TextField>("InputField1");
        

        _funcTranslateContainer.style.display = DisplayStyle.None;

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
        _funcTranslateContainer.style.display = DisplayStyle.Flex;
        _translateUI.AddToClassList("translateUI--visible");
    }

    private void OnCloseButton1Clicked(ClickEvent evt)
    {
        _funcTranslateContainer.style.display = DisplayStyle.None;
        _translateUI.RemoveFromClassList("translateUI--visible");
    }

    private void OnSubmitButton1Clicked(ClickEvent evt)
    {
        string inputText = _inputField1.text;

        _userPrompt1.text = inputText;
        _llmPrompt1.text = inputText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
