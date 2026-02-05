using CardSystem;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EventFunctions))]
public class EventManager : MonoSingleton<EventManager>
{
    [Header("Engine Variables")]
    [SerializeField] private InputReader inputReader;
    // [SerializeField] private AudioSource voiceSource;

    [Header("Event UI")]
    [SerializeField] private TextAsset globalInkVariables;
    private Story inkVariablesStory;
    private List<string> functionsToCall = new();
    public EventVariables eventVariables { get; private set; }
    private EventFunctions eventFunctions;
    // [SerializeField] private AudioClip fallbackVoice;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject speakerPanel;
    [SerializeField] private TextMeshProUGUI dialogueSpeaker;
    [SerializeField] private GameObject cardTargetObject;
    public Story currentStory { get; private set; }
    [SerializeField] private float typeSpeed = 20f;
    private bool isTyping = false;
    private bool stopTyping = false;
    private bool disableInput = false;

    //Const values related to typing. Do not change.
    private const string alphaCode = "<color=#00000000>";
    private const float maxTypeTime = 0.1f;
    private const float timeBeforeChoices = 0.2f;

    public static event Action<Card> OnCardUsed;

    private void Awake()
    {
        inkVariablesStory = new Story(globalInkVariables.text);
        eventVariables = new(inkVariablesStory);

        eventFunctions = GetComponent<EventFunctions>();

        // CheckForDuplicateNPCIds();
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        Card.OnCardDragEnd += CheckForCardUse;
    }

    //this enters dialogue with the inkJSON file assigned to the npc
    public void EnterEvent(TextAsset _inkJSON)
    {
        //first this sets the ink story as the active dialogue and activates dialogue panel
        currentStory = new Story(_inkJSON.text);
        eventVariables.StartListening(currentStory);
        inputReader.OnSubmitEvent += HandleSubmit;
        dialoguePanel.SetActive(true);

        //continue story prints dialogue so it's called here
        ContinueStory();
    }

    //dialogue printer
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            currentStory.Continue();
            if (currentStory.currentChoices.Count != 0) cardTargetObject.SetActive(true);
            else StartCoroutine(TypeDialogue());
        }
        else if (currentStory.canContinue)
        {
            StartCoroutine(TypeDialogue());
        }
        else
        {
            //if no more story left, exit dialogue
            ExitDialogue();
        }
    }

    //this sets dialogue panel inactive, empties dialogue text and sets input scheme back to gameplay
    private void ExitDialogue()
    {
        eventVariables.StopListening(currentStory);

        dialoguePanel.SetActive(false);
        dialogueText.text = "Dialogue Text";
        dialogueSpeaker.text = "Speaker";
        inputReader.OnSubmitEvent -= HandleSubmit;
    }

    //this is called when choice is made to advance ink story based on made choice
    public void MakeChoice(int choiceNumber)
    {
        currentStory.ChooseChoiceIndex(choiceNumber);
        ContinueStory();
    }

    private void ParseDialogueOptionFromCard(CardData usedCard, string targetString)
    {
        Debug.Log($"Card {usedCard.name} was used on {targetString}");

        // if (currentStory.currentChoices.Count == 0) return;

        // List<List<string>> choiceTags = new();
        
        // for (int i = 0; i < currentStory.currentChoices.Count; i++)
        // {
            
        // }
    }

    private void CheckForCardUse(Card usedCard)
    {
        Vector2 mousePos = UIController.MousePosition;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out CardTarget target))
            {
                ParseDialogueOptionFromCard(usedCard.CardData, target.TargetName);
                OnCardUsed?.Invoke(usedCard);
                Destroy(usedCard.gameObject);
            }
        }
    }

    //this coroutine types the dialogue one letter at a time
    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";
        string originalText = currentStory.Continue();
        string displayedText;
        int alphaIndex = 0;
        WaitForSeconds realTypeTime = new(maxTypeTime / typeSpeed);
        functionsToCall = new();

        HandleDialogueTags();
        TryCallFunctionsFromTags();
        // voiceSource.Play();

        foreach (char c in originalText.ToCharArray())
        {
            if (stopTyping) break;
            alphaIndex++;
            dialogueText.text = originalText;
            displayedText = dialogueText.text.Insert(alphaIndex, alphaCode);
            dialogueText.text = displayedText;
            yield return realTypeTime;
        }

        if (stopTyping) dialogueText.text = originalText;
        stopTyping = false;
        isTyping = false;
        disableInput = true;
        // voiceSource.Stop();
        yield return new WaitForSeconds(timeBeforeChoices);

        if (currentStory.currentChoices.Count != 0) cardTargetObject.SetActive(true);
    }

    private void HandleDialogueTags()
    {
        //if no tags, set fallbacks and return
        if (!currentStory.currentTags.Any())
        {
            speakerPanel.SetActive(false);
            dialogueSpeaker.text = "";
            // voiceSource.clip = null;
            Debug.Log($"You have given me a dialogue line without tags, how dare you.");
            return;
        }

        //if tags, handle each tag appropriately
        foreach (string tag in currentStory.currentTags)
        {
            if (tag.Contains("narrator"))
            {
                Debug.Log("Found narrator tag");
                speakerPanel.SetActive(false);
            }
            else if (tag.Contains("speaker:"))
            {
                //parse out clean speaker id from tag
                string speakerId = tag.Replace("speaker:", null);
                speakerId = speakerId.Replace(" ", null);
                Debug.Log($"Found speaker tag: \"{speakerId}\"");
                speakerPanel.SetActive(true);
                dialogueSpeaker.text = speakerId.FirstCharacterToUpper();
            }
            else if (tag.Contains("function:"))
            {
                //parse out clean function name from tag
                string functionName = tag.Replace("function:", null);
                functionName = functionName.Replace(" ", null);
                functionsToCall.Add(functionName);
                Debug.Log($"Found function tag: \"{functionName}\"");
            }
        }
    }

    private void TryCallFunctionsFromTags()
    {
        if (functionsToCall == null || functionsToCall.Count == 0) return;

        //Invoke is relatively safe and doesn't produce errors even if func is misspelled
        //So miraculously there's no need for error handling
        functionsToCall.ForEach(func => eventFunctions.Invoke(func, 0f));
    }

    public Ink.Runtime.Object GetVariableState(string _variableName)
    {
        eventVariables.dialogueVariables.TryGetValue(_variableName, out Ink.Runtime.Object _variableValue);
        if (_variableValue == null)
        {
            Debug.LogWarning($"{_variableName} was null, did you mean to reference it?");
        }
        return _variableValue;
    }

    private void HandleSubmit()
    {
        if (isTyping)
        {
            stopTyping = true;
            return;
        }
        if (!disableInput) ContinueStory();
    }

    private const string dialogueVariablesKey = "dialogueVariables";

    // public override void Load(SaveData data)
    // {
    //     base.Load(data);
    //     inkVariablesStory.state.LoadJson(data.strings[dialogueVariablesKey]);
    //     dialogueVariables = new(inkVariablesStory);
    // }

    // public override SaveData Save()
    // {
    //     SaveData data = base.Save();

    //     dialogueVariables.VariablesToStory(ref inkVariablesStory);
    //     string varStoryToSave = inkVariablesStory.state.ToJson();
    //     data.strings.Add(dialogueVariablesKey, varStoryToSave);

    //     return data;
    // }
}

public class EventVariables
{
    public Dictionary<string, Ink.Runtime.Object> dialogueVariables { get; set; }

    public EventVariables(Story _globalVariableStory)
    {
        dialogueVariables = new();

        foreach (string name in _globalVariableStory.variablesState)
        {
            var value = _globalVariableStory.variablesState.GetVariableWithName(name);
            dialogueVariables.Add(name, value);
            Debug.Log($"Variable global dialogue initialized: {name} = {value}");
        }
    }

    public void StartListening(Story _story)
    {
        VariablesToStory(ref _story);
        _story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story _story)
    {
        _story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string _varName, Ink.Runtime.Object _value)
    {
        Debug.Log($"Variable changed: {_varName} = {_value}");

        if (dialogueVariables.ContainsKey(_varName))
        {
            dialogueVariables[_varName] = _value;
        }
    }

    public void VariablesToStory(ref Story _story)
    {
        foreach (var variable in dialogueVariables)
        {
            _story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}