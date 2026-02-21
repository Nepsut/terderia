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
    private const string eventAssetPath = "EventSystem/EventSprites";

    [Header("Engine Variables")]
    [SerializeField] private InputReader inputReader;
    // [SerializeField] private AudioSource voiceSource;

    [Header("Event UI")]
    [SerializeField] private TextAsset globalInkVariables;
    private Story inkVariablesStory;
    private List<string> functionsToCall = new();
    private string cardToRefund = "";
    public EventVariables EventVariables { get; private set; }
    private EventFunctions eventFunctions;
    // [SerializeField] private AudioClip fallbackVoice;
    [SerializeField] private CardHolder cardHolder;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject speakerPanel;
    [SerializeField] private TextMeshProUGUI dialogueSpeaker;
    [SerializeField] private GameObject selfTargetObject;
    [SerializeField] private Transform eventTargetGroupHolder;
    [SerializeField] private float typeSpeed = 20f;

    //Story variables
    public Story CurrentStory { get; private set; }
    private Dictionary<string, Sprite> eventSpriteAssets;
    private List<GameObject> activeEventTargets;
    private bool isTyping = false;
    private bool choiceWasMade = false;
    private bool pendingCardUse = false;
    private bool stopTyping = false;
    private bool disableInput = false;

    //Const values related to typing. Do not change if unsure.
    private const string alphaCode = "<color=#00000000>";
    private const float maxTypeTime = 0.1f;
    private const float timeBeforeChoices = 0.2f;

    //Events
    public static event Action<Card> OnCardUsed;

    //DEBUG VARIABLES. DO NOT TOUCH IF UNSURE.
    private string activeStoryName;

    private void Awake()
    {
        inkVariablesStory = new Story(globalInkVariables.text);
        EventVariables = new(inkVariablesStory);

        eventFunctions = GetComponent<EventFunctions>();

        //fetch event sprite assets
        Sprite[] tempEventSprites = Resources.LoadAll<Sprite>(eventAssetPath);
        eventSpriteAssets = new();

        foreach(Sprite sprite in tempEventSprites)
        {
            eventSpriteAssets.Add(sprite.name, sprite);
            if (GameManager.Instance.DebugModeOn) Debug.Log(sprite.name);
        }

        // CheckForDuplicateNPCIds();
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        DontDestroyOnLoad(selfTargetObject);
        selfTargetObject.SetActive(false);
        Card.OnCardDragEnd += CheckForCardUse;
    }

    //this enters dialogue with the inkJSON file assigned to the npc
    public void EnterEvent(TextAsset _inkJSON)
    {
        //first this sets the ink story as the active dialogue and activates dialogue panel
        CurrentStory = new Story(_inkJSON.text);
        EventVariables.StartListening(CurrentStory);
        inputReader.OnSubmitEvent += HandleSubmit;
        inputReader.OnClickEvent += HandleClickWithDelay;
        dialoguePanel.SetActive(true);

        activeStoryName = _inkJSON.name;

        //continue story prints dialogue so it's called here
        ContinueStory();
    }

    //dialogue printer
    private void ContinueStory()
    {
        if (pendingCardUse) return;

        if (choiceWasMade && CurrentStory.canContinue)
        {
            choiceWasMade = false;
            if (CurrentStory.currentChoices.Count != 0)
            {
                pendingCardUse = true;
                selfTargetObject.SetActive(true);
                selfTargetObject.GetComponent<Collider2D>().enabled = true;
                cardHolder.ActivateHolder();
            }
            else StartCoroutine(TypeDialogue());
        }
        else if (CurrentStory.canContinue)
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
        EventVariables.StopListening(CurrentStory);

        dialoguePanel.SetActive(false);
        dialogueText.text = "Dialogue Text";
        dialogueSpeaker.text = "Speaker";
        // activeEventTargets?.ForEach(target => target.SetActive(false));
        // activeEventTargets = null;
        inputReader.OnSubmitEvent -= HandleSubmit;
        inputReader.OnClickEvent += HandleClickWithDelay;
    }

    //this is called when choice is made to advance ink story based on made choice
    public void MakeChoice(int choiceNumber)
    {
        CurrentStory.ChooseChoiceIndex(choiceNumber);
        pendingCardUse = false;
        choiceWasMade = true;
        activeEventTargets.ForEach(target =>
        {
            foreach (Transform child in target.transform)
            {
                child.GetComponent<Collider2D>().enabled = false;
            }
        });
        selfTargetObject.GetComponent<Collider2D>().enabled = false;
        ContinueStory();
    }

    private void ParseDialogueOptionFromCard(CardData usedCard, string targetString)
    {
        if (GameManager.Instance.DebugModeOn)
            Debug.Log($"Card {usedCard.name} was used on {targetString}");

        if (CurrentStory.currentChoices.Count == 0) return;

        int[] matchingTagCounts = new int[CurrentStory.currentChoices.Count];
        float[] matchingTagPercentage = new float[CurrentStory.currentChoices.Count];
        int bestMatchIndex = -1;
        
        //This looping if-else hell determines which dialogue option has the most matching
        //tags with the used cards, and if a targetTag-uniqueTag match is found, exits with that
        for (int i = 0; i < CurrentStory.currentChoices.Count && bestMatchIndex == -1; i++)
        {
            bool targetTagMatch = false;
            bool uniqueTagMatch = false;
            matchingTagCounts[i] = 0;

            for (int j = 0; j < CurrentStory.currentChoices[i].tags.Count && bestMatchIndex == -1; j++)
            {
                string choiceTag = CurrentStory.currentChoices[i].tags[j];

                if (choiceTag.Contains('@'))
                {
                    if (targetString.Equals(choiceTag.Replace("@", null)))
                    {
                        targetTagMatch = true;
                    }
                }
                else if (choiceTag.Contains('!'))
                {
                    if (usedCard.id.Equals(choiceTag.Replace("!", null)))
                    {
                        uniqueTagMatch = true;
                    }
                }
                else if (choiceTag.Contains("type:"))
                {
                    choiceTag = choiceTag.Replace("type:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.CardType type))
                    {
                        string warning = string.Concat($"Error in \'type\' tag: event {CurrentStory.debugMetadata.fileName} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.cardType == type) matchingTagCounts[i]++;
                }
                else if (choiceTag.Contains("school:"))
                {
                    choiceTag = choiceTag.Replace("school:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.SpellSchool school))
                    {
                        string warning = string.Concat($"Error in \'school\' tag: event {CurrentStory} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.spellSchool == school) matchingTagCounts[i]++;
                }
                else if (choiceTag.Contains("damage:"))
                {
                    choiceTag = choiceTag.Replace("damage:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.DamageType damageType))
                    {
                        string warning = string.Concat($"Error in \'damage\' tag: event {CurrentStory} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.damageType == damageType) matchingTagCounts[i]++;
                }
                else if (choiceTag.Contains("range:"))
                {
                    choiceTag = choiceTag.Replace("range:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.Range range))
                    {
                        string warning = string.Concat($"Error in \'range\' tag: event {CurrentStory} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.range == range) matchingTagCounts[i]++;
                    else if ((int)usedCard.range < (int)range) matchingTagCounts[i]--;
                }
                else if (choiceTag.Contains("strength:"))
                {
                    choiceTag = choiceTag.Replace("strength:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.Strength strength))
                    {
                        string warning = string.Concat($"Error in \'strength\' tag: event {CurrentStory} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.strength == strength) matchingTagCounts[i]++;
                    else if ((int)usedCard.strength < (int)strength) matchingTagCounts[i]--;
                }
                else if (choiceTag.Contains("aoe:"))
                {
                    choiceTag = choiceTag.Replace("aoe:", null);
                    if (!Enum.TryParse(choiceTag, ignoreCase: true, out CardData.AreaOfEffect aoe))
                    {
                        string warning = string.Concat($"Error in \'aoe\' tag: event {CurrentStory} ",
                        $"on choice {CurrentStory.currentChoices[i].text}");
                        Debug.LogWarning(warning);
                        continue;
                    }
                    if (usedCard.areaOfEffect == aoe) matchingTagCounts[i]++;
                    else if ((int)usedCard.areaOfEffect < (int)aoe) matchingTagCounts[i]--;
                }
                else if (choiceTag.Contains("other:"))
                {
                    if (usedCard.otherTags == null || usedCard.otherTags.Count == 0) continue;
                    choiceTag = choiceTag.Replace("other:", null);
                    if (usedCard.otherTags.Contains(choiceTag)) matchingTagCounts[i]++;
                }

                if (targetTagMatch && uniqueTagMatch)
                {
                    bestMatchIndex = i;
                    break;
                }
            }
            //if target didn't match, invalidate choice
            if (!targetTagMatch)
            {
                matchingTagCounts[i] -= 100;
                matchingTagPercentage[i] = -1f;
            }
            else
            {
                matchingTagPercentage[i] = matchingTagCounts[i] / (float)CurrentStory.currentChoices[i].tags.Count;
            }
        }

        //code goes here if both target and unique tag matched on any dialogue option
        //it then progresses the dialogue with the best choice and exits
        if (bestMatchIndex != -1)
        {
            MakeChoice(bestMatchIndex);
            return;
        }

        bestMatchIndex = 0;
        //this thing finds the index of the option with the highest % of matching tags,
        //using the highest count of matching tags if two or more options share a %
        for (int i = 1; i < matchingTagCounts.Length; i++)
        {
            if (matchingTagPercentage[bestMatchIndex] < matchingTagPercentage[i] ||
                (matchingTagPercentage[bestMatchIndex] == matchingTagPercentage[i] &&
                matchingTagCounts[bestMatchIndex] < matchingTagCounts[i]))
            {
                bestMatchIndex = i;
            }
        }

        MakeChoice(bestMatchIndex);
        return;
    }

    private void CheckForCardUse(Card usedCard)
    {
        Vector2 mousePos = UIController.MousePosition;
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out CardTarget target))
            {
                OnCardUsed?.Invoke(usedCard);
                ParseDialogueOptionFromCard(usedCard.CardData, target.TargetName);
                Destroy(usedCard.gameObject);
                break;
            }
        }
    }

    //this coroutine types the dialogue one letter at a time
    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";
        string originalText = CurrentStory.Continue();
        if (originalText == null || originalText == "")
        {
            dialoguePanel.SetActive(false);
        }
        else dialoguePanel.SetActive(true);
        string displayedText;
        int alphaIndex = 0;
        WaitForSeconds realTypeTime = new(maxTypeTime / typeSpeed);
        functionsToCall = new();

        HandleDialogueTags();
        TryCallFunctionsFromTags();
        if (cardToRefund != "")
        {
            CardManager.TryAddCardToHand(cardToRefund);
            cardToRefund = "";
        }
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
        disableInput = false;

        if (CurrentStory.currentChoices.Count != 0)
        {
            selfTargetObject.SetActive(true);
            selfTargetObject.GetComponent<Collider2D>().enabled = true;
            cardHolder.ActivateHolder();
            pendingCardUse = true;
        }
        else
        {
            if (cardHolder.IsActive) cardHolder.DeactivateHolder();
            selfTargetObject.SetActive(false);
            // activeEventTargets?.ForEach(target => target.SetActive(false));
        }
    }

    private void HandleDialogueTags()
    {
        //if no tags, set fallbacks and return
        if (!CurrentStory.currentTags.Any())
        {
            speakerPanel.SetActive(false);
            dialogueSpeaker.text = "";
            // voiceSource.clip = null;
            Debug.Log($"You have given me a dialogue line without tags, how dare you.");
            return;
        }

        Dictionary<string, string> spritesToSet = new();
        bool foundSpeakerTag = false;
        //if tags, handle each tag appropriately
        foreach (string tag in CurrentStory.currentTags)
        {
            if (tag.Contains("narrator"))
            {
                if (GameManager.Instance.DebugModeOn) Debug.Log("Found narrator tag");
                speakerPanel.SetActive(false);
            }
            else if (tag.Contains("speaker:"))
            {
                string speakerId = tag.Replace("speaker:", null);
                if (GameManager.Instance.DebugModeOn) Debug.Log($"Found speaker tag: \"{speakerId}\"");
                foundSpeakerTag = true;
                speakerPanel.SetActive(true);
                dialogueSpeaker.text = speakerId.FirstCharacterToUpper();
            }
            else if (tag.Contains("function:"))
            {
                string functionName = tag.Replace("function:", null);
                functionsToCall.Add(functionName);
                if (GameManager.Instance.DebugModeOn) Debug.Log($"Found function tag: \"{functionName}\"");
            }
            else if (tag.Contains("targets:"))
            {
                activeEventTargets ??= new();
                string targetGroupName = tag.Replace("targets:", null);
                GameObject eventTarget = eventTargetGroupHolder.Find(targetGroupName).gameObject;
                activeEventTargets.Add(eventTarget);
                if (GameManager.Instance.DebugModeOn) Debug.Log($"Found targets tag: \"{targetGroupName}\"");
            }
            else if (tag.Contains("refundcard:"))
            {
                string cardName = tag.Replace("refundcard:", null);
                if (cardToRefund == "")
                {
                    cardToRefund = cardName;
                    if (GameManager.Instance.DebugModeOn)
                        Debug.Log($"Found refundcard tag: trying to give player {cardName} card");
                }
                else if (GameManager.Instance.DebugModeOn)
                {
                    string warning = string.Concat($"One dialogue line can't have more than one card refund!",
                    $"File {activeStoryName} line {CurrentStory.currentText} tries to give multiple cards!");
                    Debug.LogWarning(warning);
                }

            }
            else if (tag.Contains("setsprite:"))
            {
                string setSpriteInfo = tag.Replace("setsprite:", null);
                int splitIndex = setSpriteInfo.IndexOf('>');
                string gameObjectName = setSpriteInfo[..splitIndex];
                splitIndex++;
                string spriteName = setSpriteInfo[splitIndex..];
                if (!spritesToSet.TryAdd(gameObjectName, spriteName))
                {
                    string warning = string.Concat($"Dialogue line {CurrentStory.currentText}\n",
                        $"tries to set game object {gameObjectName}'s sprite multiple times!");
                    Debug.LogWarning(warning);
                }
                else if (GameManager.Instance.DebugModeOn)
                    Debug.Log($"Found setsprite tag: setting \"{gameObjectName}\" image to sprite \"{spriteName}\"");
            }
        }

        if (!foundSpeakerTag)
        {
            dialogueSpeaker.text = "";
            speakerPanel.SetActive(false);
        }

        if (spritesToSet.Count != 0) HandleSpriteChanges(spritesToSet);

        activeEventTargets?.ForEach(target =>
        {
            target.SetActive(true);
            foreach (Transform child in target.transform)
            {
                child.GetComponent<Collider2D>().enabled = true;
            }
        });
    }

    private void TryCallFunctionsFromTags()
    {
        if (functionsToCall == null || functionsToCall.Count == 0) return;

        //Invoke is relatively safe and doesn't produce errors even if func is misspelled
        //So miraculously there's no need for error handling
        functionsToCall.ForEach(func => eventFunctions.Invoke(func, 0f));
    }

    public object GetVariableState(string _variableName)
    {
        EventVariables.DialogueVariables.TryGetValue(_variableName, out Ink.Runtime.Value _variableValue);
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
        if (!disableInput && !pendingCardUse) ContinueStory();
    }

    private void HandleClickWithDelay()
    {
        Invoke(nameof(HandleClick), 0f);
    }

    private void HandleClick()
    {
        if (DraggableObject.DraggingOn) return;

        if (isTyping)
        {
            stopTyping = true;
            return;;
        }
        if (!disableInput && !pendingCardUse) ContinueStory();
    }

    private void HandleSpriteChanges(Dictionary<string, string> objectNameSpriteNameDict)
    {
        foreach (var kvp in objectNameSpriteNameDict)
        {
            if (!eventSpriteAssets.ContainsKey(kvp.Value))
            {
                string warning = string.Concat("Tried to set an event sprite via dialogue setsprite with erronous sprite name ",
                $"{kvp.Value}. Check for tag typos in ink event file \"{activeStoryName}\" on line \"{CurrentStory.currentText}\".");
                Debug.LogWarning(warning);
                continue;
            }
            Transform targetTransform = eventTargetGroupHolder.Find(kvp.Key);
            if (targetTransform == null)
            {
                string warning = string.Concat("Tried to set an event sprite via dialogue setsprite on non-existent object ",
                $"{kvp.Key}. Check for tag typos in ink event file \"{activeStoryName}\" on line \"{CurrentStory.currentText}\".");
                Debug.LogWarning(warning);
                continue;
            }
            if (!targetTransform.TryGetComponent(out SpriteRenderer renderer))
            {
                Debug.LogWarning($"Could not find SpriteRenderer on event sprite object {kvp.Key}");
                continue;
            }
            renderer.sprite = eventSpriteAssets[kvp.Value];
        }
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
    public Dictionary<string, Ink.Runtime.Value> DialogueVariables { get; private set; }
    private int Health => GameManager.playerHealth;
    private Story currentStory;

    // Variable change events
    public static event Action<int> OnHealthGained;
    public static event Action<int> OnHealthLost;

    // Variable keys
    public const string healthKey = "g_player_health";
    public const string genderKey = "g_player_gender";
    public const string subclassKey = "g_player_class";

    public EventVariables(Story _globalVariableStory)
    {
        DialogueVariables = new();

        foreach (string name in _globalVariableStory.variablesState)
        {
            Ink.Runtime.Value value = (Ink.Runtime.Value)_globalVariableStory.variablesState.GetVariableWithName(name);
            DialogueVariables.Add(name, value);
            if (GameManager.Instance.DebugModeOn)
                Debug.Log($"Variable global dialogue initialized: {name} = {value}");
        }
    }

    public void StartListening(Story _story)
    {
        VariablesToStory(ref _story);
        _story.variablesState.variableChangedEvent += HandleVariableChange;
        currentStory = _story;
    }

    public void StopListening(Story _story)
    {
        _story.variablesState.variableChangedEvent -= HandleVariableChange;
        currentStory = null;
    }

    private void HandleVariableChange(string _varName, Ink.Runtime.Object _value)
    {
        if (!DialogueVariables.ContainsKey(_varName) || _value is not Ink.Runtime.Value) return;

        Ink.Runtime.Value valueToChange = _value as Ink.Runtime.Value;
        if (GameManager.Instance.DebugModeOn) Debug.Log($"Variable changed: {_varName} = {_value}");
        DialogueVariables[_varName] = valueToChange;
        if (valueToChange.valueType == Ink.Runtime.ValueType.Int)
        {
            Ink.Runtime.IntValue valueAsInt = (Ink.Runtime.IntValue)valueToChange;
            HandleIntegerUpdate(_varName, valueAsInt.value);
        }
    }

    public void VariablesToStory(ref Story _story)
    {
        foreach (var kvp in DialogueVariables)
        {
            Debug.Log($"Setting ink story variable {kvp.Key} to {kvp.Value}");
            _story.variablesState.SetGlobal(kvp.Key, kvp.Value);
        }
    }

    public bool TryChangeGlobalInkVariable(string _varName, object _value)
    {
        if (DialogueVariables == null || !DialogueVariables.ContainsKey(_varName))
        {
            if (GameManager.Instance.DebugModeOn)
            {
                string log = "Failed to update ink story variable due to ";
                log += DialogueVariables == null ? $"DialogueVariables being null" : $"variable with name {_varName} not existing";
                Debug.Log(log);
            }
            return false;
        }
        DialogueVariables[_varName] = Ink.Runtime.Value.Create(_value);
        currentStory?.variablesState.SetGlobal(_varName, (Ink.Runtime.Object)_value);
        if (GameManager.Instance.DebugModeOn)
            Debug.Log($"Updated ink variable \"{_varName}\". New value: {_value}");
        return true;
    }

    public bool TryChangeGlobalInkVariables(Dictionary<string, object> _variableDict)
    {
        if (_variableDict == null || _variableDict.Count == 0)
        {
            if (GameManager.Instance.DebugModeOn)
            {
                Debug.Log("Failed to update ink story variables due to method being called with empty or null Dictionary");
            }
            return false;
        }
        bool success = true;
        foreach (var kvp in _variableDict)
        {
            if (!TryChangeGlobalInkVariable(kvp.Key, kvp.Value)) success = false;
        }
        return success;
    }

    private void HandleIntegerUpdate(string varName, int value)
    {
        switch (varName)
        {
            case healthKey:
                if (value > Health) OnHealthGained?.Invoke(Health + value);
                else if (value < Health) OnHealthLost?.Invoke(Health - value);
                else
                {
                    string warning = string.Concat("Health was changed in ink variables but value stayed same. ",
                        $"Check ink files altering variable {varName}");
                    Debug.LogWarning(warning);
                }
                break;
        }
    }
}