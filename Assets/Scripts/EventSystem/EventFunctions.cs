using System;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;

//Command pattern solution for calling methods with parameters from ink files
//This is the base class for all event functions

public abstract class EventFunction
{
    public abstract bool TryExecute(object[] args = null);
}

public class DisplayCutscene : EventFunction
{
    public override bool TryExecute(object[] args = null)
    {
        //If we didn't get enough args to figure out what cutscene to play
        //and at what frame, log error and return
        if (args == null || args.Length != 2 || args[0] is not string || args[1] is not int)
        {
            Debug.LogError($"DisplayCutscene call did not have proper args!");
            return false;
        }
        UIController.Instance.RunCutscene((string)args[0], (int)args[1]);
        return true;
    }
}

public class EndCutscene : EventFunction
{
    public override bool TryExecute(object[] args = null)
    {
        if (args != null)
        {
            Debug.LogWarning($"EndCutscene should not be called with parameters!");
        }
        UIController.Instance.EndCutscene();
        return true;
    }
}

//UnlockCards expects all parameters to be strings
public class RewardCards : EventFunction
{
    /// <summary>
    /// Offers player a menu to choose some cards to unlock.
    /// First arg should be an int that specifies how many cards the player can choose.
    /// </summary>
    /// <param name="args">UnlockCard expects first arg to be int and the rest to be strings</param>
    public override bool TryExecute(object[] args = null)
    {
        //If we didn't get enough args to figure out how many cards can be chosen
        //and at least one card string, return without doing anything
        if (args == null || args.Length < 2 || args[0] is not int)
        {
            Debug.LogError($"RewardCards call did not have sufficient args!");
            return false;
        }
        
        int cardPickAmount = (int)args[0];
        Array cardIdArray = Array.CreateInstance(typeof(string), args.Length - 1);
        Array.Copy(args[1..], cardIdArray, args.Length - 1);
        List<string> cardRewardIds = new();

        foreach (string id in cardIdArray)
        {
            if (id[0] == '/')
            {
                if (!CardManager.SubclassRewardDictionary.ContainsKey(id[1..]))
                {
                    string error = string.Concat("Error while trying to add subclass-specific card rewards! ",
                    $"Tried to use non-existent key {id[1..]}");
                    Debug.LogError(error);
                    continue;

                }

                SubclassSpecificRewards subclassReward = CardManager.SubclassRewardDictionary[id[1..]];

                switch (GameManager.Instance.playerData.subclass)
                {
                    case PlayerData.Subclass.trickster:
                        cardRewardIds.Add(subclassReward.TricksterReward);
                        break;
                    case PlayerData.Subclass.elementalist:
                        cardRewardIds.Add(subclassReward.ElementalistReward);
                        break;
                    case PlayerData.Subclass.chef:
                        cardRewardIds.Add(subclassReward.ChefReward);
                        break;
                    case PlayerData.Subclass.alchemist:
                        cardRewardIds.Add(subclassReward.AlchemistReward);
                        break;
                }
            }
            else cardRewardIds.Add(id);
        }

        if (GameManager.Instance.DebugModeOn)
        {
            foreach (string cardId in cardIdArray)
            {
                Debug.Log($"Choosable card id found {cardId}");
            }
            Debug.Log($"Choosable cards amount {cardPickAmount}");
            Debug.Log($"RewardCards function was called during event");
        }

        UIController.Instance.HandleCardRewards(cardPickAmount, cardRewardIds.ToArray());

        return true;
    }
}

public class LoadScene : EventFunction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">LoadScene expects one string that can be parsed to enum</param>
    /// <returns></returns>
    public override bool TryExecute(object[] args = null)
    {
        if (!Enum.TryParse<SceneTransitionManager.Scene>(args[0].ToString(), ignoreCase: true, out var scene))
        {
            Debug.LogError($"Could not parse LoadScene param to valid scene!");
            return false;
        }
        SceneTransitionManager.Instance.StartTransitionAfterDelay(scene);
        return true;
    }
}