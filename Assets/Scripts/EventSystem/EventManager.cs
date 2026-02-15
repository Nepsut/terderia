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

    public Story CurrentStory { get; private set; }
    private List<GameObject> activeEventTargets;
    private bool isTyping = false;
    private bool choiceWasMade = false;
    private bool stopTyping = false;
    private bool disableInput = false;

    //Const values related to typing. Do not change.
    private const string alphaCode = "<color=#00000000>";
    private const float maxTypeTime = 0.1f;
    private const float timeBeforeChoices = 0.2f;

    public static event Action<Card> OnCardUsed;

    public struct ChoiceTagHolder
    {
        public string TargetTag;
        public string UniqueTag;
        public List<string> OtherTags;

        public ChoiceTagHolder(string targetTag, string uniqueTag, List<string> otherTags = null)
        {
            TargetTag = targetTag;
            UniqueTag = uniqueTag;
            OtherTags = otherTags; 
        }
    }

    private void Awake()
    {
        inkVariablesStory = new Story(globalInkVariables.text);
        EventVariables = new(inkVariablesStory);

        eventFunctions = GetComponent<EventFunctions>();

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
        dialoguePanel.SetActive(true);

        //continue story prints dialogue so it's called here
        ContinueStory();
    }

    //dialogue printer
    private void ContinueStory()
    {
        if (choiceWasMade && CurrentStory.canContinue)
        {
            choiceWasMade = false;
            // CurrentStory.Continue();
            if (CurrentStory.currentChoices.Count != 0)
            {
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
        activeEventTargets.ForEach(target => target.SetActive(false));
        activeEventTargets = null;
        inputReader.OnSubmitEvent -= HandleSubmit;
    }

    //this is called when choice is made to advance ink story based on made choice
    public void MakeChoice(int choiceNumber)
    {
        CurrentStory.ChooseChoiceIndex(choiceNumber);
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
        string originalText = CurrentStory.Continue();
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

        if (CurrentStory.currentChoices.Count != 0)
        {
            selfTargetObject.SetActive(true);
            selfTargetObject.GetComponent<Collider2D>().enabled = true;
            cardHolder.ActivateHolder();
        }
        else
        {
            cardHolder.DeactivateHolder();
            selfTargetObject.SetActive(false);
            activeEventTargets?.ForEach(target => target.SetActive(false));
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

        bool foundSpeakerTag = false;
        //if tags, handle each tag appropriately
        foreach (string tag in CurrentStory.currentTags)
        {
            if (tag.Contains("narrator"))
            {
                Debug.Log("Found narrator tag");
                speakerPanel.SetActive(false);
            }
            else if (tag.Contains("speaker:"))
            {
                string speakerId = tag.Replace("speaker:", null);
                Debug.Log($"Found speaker tag: \"{speakerId}\"");
                foundSpeakerTag = true;
                speakerPanel.SetActive(true);
                dialogueSpeaker.text = speakerId.FirstCharacterToUpper();
            }
            else if (tag.Contains("function:"))
            {
                string functionName = tag.Replace("function:", null);
                functionsToCall.Add(functionName);
                Debug.Log($"Found function tag: \"{functionName}\"");
            }
            else if (tag.Contains("targets:"))
            {
                activeEventTargets ??= new();
                string targetGroupName = tag.Replace("targets:", null);
                activeEventTargets.Add(eventTargetGroupHolder.Find(targetGroupName).gameObject);
                Debug.Log($"Found targets tag: \"{targetGroupName}\"");
            }
        }
        if (!foundSpeakerTag)
        {
            dialogueSpeaker.text = "";
            speakerPanel.SetActive(false);
        }
        activeEventTargets?.ForEach(target => target.SetActive(true));
        activeEventTargets?.ForEach(target =>
        {
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

    public Ink.Runtime.Object GetVariableState(string _variableName)
    {
        EventVariables.dialogueVariables.TryGetValue(_variableName, out Ink.Runtime.Object _variableValue);
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