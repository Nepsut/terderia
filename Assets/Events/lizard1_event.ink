INCLUDE global_vars.ink

/////// Asset variables here :) ///////
VAR considered_evil_bottle = false
VAR did_first_dagger_trial = false
VAR lizard_health = 2

As you're walking, the forest around you starts turning greener and quieter with every step. #narrator

You don't get much time to appreciate the nature as your travels are cut short due to a fallen tree on the road. #narrator #showtargets:lizard1 #setsprite:lizard1>lizard1_default

Is that someone underneath? #narrator

"Hey! Are you alraight?" #speaker:You

"Oh thank the heavens you're here! I've been yelling for hours!" #speaker:Unknown Lizard

"...I didn't hear a thing when I entered." #speaker:Your

"Oh? Must be the leaves. I wouldn't be too surprised if they absorbed sound as well." #speaker:Unknown Lizard

"They are good rain covers! I tried to pick some up when this happened." #speaker:Unknown Lizard

"Now that I think about it, I didn't hear the tree fall..." #speaker:Unknown Lizard

"Could you perhaps help me out?" #speaker:Unknown Lizard

He seems to be badly stuck. Maybe you should do something about this? #narrator

-> LIZARD_CARD_OPTIONS

=== LIZARD_CARD_OPTIONS ===

/////// SELF OPTIONS START ///////

//TODO: ADD HOSTILE VERSIONS?

+ The apple tastes as good as you imagined. #card #@self #!apple
 {
    - g_player_health < g_player_max_health:
    ~ g_player_health++
 }
    -> LIZARD_CARD_OPTIONS

+ You consider lighting yourself on fire but your clothes are too moist to be lit due to the humidity... #card #@self #!bonfire #refundcard:bonfire
    -> LIZARD_CARD_OPTIONS
    
+ You start having a discussion with yourself on what you could use the wood for, if you chopped it into pieces right now. #card #@self #!banter
    "Ah talking to ourselves are we? I do that sometimes too." #speaker:Unknown Lizard
    The lizard appreciates your quite pointless pondering. #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You ponder your lockpick for a moment. Good to have one of these, you think. #card #@self #!lockpick #refundcard:lockpick
    -> LIZARD_CARD_OPTIONS
    
+ You grasp your trusty staff with your dominant hand. It feels reassuring to hold. #card #@self #!staff
    Can't really do much with it right now, though. #narrator #refundcard:staff
    -> LIZARD_CARD_OPTIONS
    
+ You spin the dagger in your hand, skillfully twirling it around, and around again. Finally you flip it in the air and catch it. #card #@self #!dagger
    The lizard seems amazed by your tricks. Maybe he likes you now. #narrator
    -> LIZARD_CARD_OPTIONS
 
+ Now is not the time to start making a meal. Someone is struggling! #card #@self #!frying-pan
    -> LIZARD_CARD_OPTIONS
    
+ You quickly take a swig of the potion. #card #@self #!glass-bottle
    {
        - g_player_health >= g_player_max_health:
        You were already at full health though. At least the potion tastes somewhat sweet. #narrator
        - g_player_health < g_player_max_health - 1:
        The healing properties quickly kick in, and you regain some health.
        ~ g_player_health += 2
        - else:
        The healing properties quickly kick in, and you regain some health.
        ~ g_player_health++
    }
  -> LIZARD_CARD_OPTIONS

+ "I'm such a failure, I cant even help a stranger without my spellbook!" #card #@self #!insult #speaker:You
    "Hey don't be so hard on yourself... It's alright!" #speaker: Unknown Lizard
    "If you can't help me could you at least deliver a message to the mountain city that I'm stuck here?" #speaker: Unknown Lizard
    "That is enough help. You don't have to be or do any more than that!" #speaker: Unknown Lizard
    The lizards kind words make you feel better. Perhaps are not a failure. #narrator
    "I'll do that as fast as I can. Thank you." #speaker:You
    You leave the waving lizard behind. What an inspirational speaker. #hidetargets:lizard1
    ~ g_got_lizard_message = true
    -> DONE
    
+ You tie the rope around yourself and give the other end to the lizard. #card #@self #!rope
    "Don't blame me if this doesn't work..." #speaker:You
    You start pulling the lizard as they hold on with their dear life on the rope. #narrator
    ~ lizard_health--
    {
        - lizard_health == 0:
        They seem to get knocked out by this sudden force. That wasn't supposed to happen! #narrator
        -> LIZARD_EVENT_EPILOGUE("escaped")
        - else:
        Slowly but surely there is more lizard on your side than on the other! #narrator
        -> LIZARD_EVENT_EPILOGUE("lizard_saved")
    }
    
    
+ You punch yourself on your face. You feel refreshed yet you still have no solutions to the problem. #card #@self #!punch
    The lizards seems very confused by this action. #narrator
    -> LIZARD_CARD_OPTIONS

+ Try as you might, giving yourself a static shock proves to be impossible in this humidity. Damn it. #card #@self #!static-shock
    You'd think it would be the other way around. #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You consider the evil potion for a moment. Surely you shouldn't drink it? #card #@self #!evil-bottle
    {
     - considered_evil_bottle:
        No. You're doing this. #narrator
        You take a big swig from the bottle... it's horrible. You only manage to finish it thanks to using all of your willpower. #narrator
        You feel a burn spreading through your body as the foul liquid makes its way down. This can't be good for you. #narrator
        ~ g_player_health--
        And it isn't. You keel over, trying to make yourself smaller to somehow minimize the pain in your chest and stomach. It doesn't do much. #narrator
        The lizard follows your action with sadness and horror. Why would you do that #narrator #setsprite:lizard1>lizard1_crying
        {g_player_gender == "hat": Hats | Humans} are so horrible! But this is not the time to cry about that. #narrator #setsprite:lizard1>lizard1_default
        ~ considered_evil_bottle = false
        
        - else:
        Yeah maybe not. You brewed this yourself, and you know that it'd cause significant damage to your esophagus and stomach if ingested. #narrator #refundcard:evil-bottle
        ~ considered_evil_bottle = true
    }
    -> LIZARD_CARD_OPTIONS
    
+ Now is not the time to start making a meal. Someone is struggling! #card #@self #!can-of-beans
    -> LIZARD_CARD_OPTIONS
    
+ You pull out the snowball and start eating it. It really helps you cool down in the hot humid air. #card #@self #!snowball
    "Can I have some? I'm getting thirsty." #speaker:Unknown Lizard
    You give the lizard some of the snowball. He looks happy. #narrator #setsprite:lizard1>lizard1_thumb
    -> LIZARD_CARD_OPTIONS
    
+ You throw smokescreen to the ground and it quickly covers everything near you. #card #@self #!smokescreen
    Not exactly sure of the direction, you start running through the leaves until you find yourself somewhere on the road again. #narrator
    Behind you you see a tail poking underneath a tree making surprised moves. It seems you made it to the other side of the fallen tree. #narrator
    You can't really hear the lizard anymore as you continue down the road. #narrator
    ~ g_tried_help_lizard_soldier = false
    -> DONE

+ You start suddenly running towards the tree. #card #@self #!run
    "H- Hey! What are you planning on?!" #speaker:Unknown Lizard
    You successfully run to the tree, jump over it, and continue your journey. #narrator
    You can't hear the lizard anymore as you continue down the road. #narrator
    ~ g_tried_help_lizard_soldier = false
    -> DONE

+ You pull out a cool stick you've found and swing it around. It's not helpfull this time unfortunately. #card #@self #!stick
    -> LIZARD_CARD_OPTIONS

+ You've heard that making yourself believe in something helps it make true." #card #@self #!deceive
    You start repeating "this tree is not here" in your head and proceed to walk towards- and over the tree. #narrator
    The lizard thinks you might be insane. But atleast you got to continue your travels. #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You start mumbling some words that sound positive. #card #@self #!glass-bottle
    {
        - g_player_health >= g_player_max_health:
        You were already at full health, but least the lizard thinks your mumbling was cool. #narrator
        - g_player_health < g_player_max_health - 1:
        The healing properties quickly kick in, and you regain some health.
        ~ g_player_health += 2
        - else:
        The healing properties quickly kick in, and you regain some health.
        ~ g_player_health++
    }
  -> LIZARD_CARD_OPTIONS
    
+ You start swinging the comically large sword around appreciating its beauty. It shines nicely in the faint light. #card #@self #!longsword
    -> LIZARD_CARD_OPTIONS

+ "I'm such a good {g_player_gender == "hat": hat | person} and I deserve all love this world has to give for me." #card #@self #!praise #speaker:You
    "I'm glad you think that. It's important thing to knowledge." #speker:Unknown Lizard
    He liked your kind words towards yourself, even if it kind of came out of nowhere. #narrator
    -> LIZARD_CARD_OPTIONS

+ You try performing self hypnotism but it just made you dizzy. Great job. #card #@self #!hypnotize
    -> LIZARD_CARD_OPTIONS
    
    
    
    
    
    
    
    
    
    
    /////// SELF OPTIONS END ///////

+ "Would you like an apple?" #card #@lizard #!apple #speaker:You
    "Well why not? I was getting little hungry here." #speaker:Unknown Lizard #setsprite:lizard1>lizard1_default
    ~ lizard_health++
    -> LIZARD_EVENT_EPILOGUE("tried_helping")

+ "I cast bonfire!" #card #@lizard #!bonfire #speaker:You 
    Big flames start appearing underneath the lizard. It seems the air is too humid to burn anything. #narrator #setsprite:lizard1>lizard1_fried
    You and the lizard stare each other awkwardly. #narrator
    That didn't go as you planned so you deside to quietly leave. #narrator #hidetargets:lizard1 
    You continue the road with regrets. #narrator
    ~ g_tried_help_lizard_soldier = false
    -> DONE
    
+ "Lovely day isn't it?" #card #@lizard #!apple #speaker:You
    "It really is" #speaker:Unknown Lizard
    Akward silence takes place. Maybe you should try something else? #narrator
    -> LIZARD_CARD_OPTIONS
    
+ Unfortunately the tree has nothing to use this on. Not even a secret compartment. #card #@lizard #!lockpick
    -> LIZARD_CARD_OPTIONS
    
+ You bash the tree trunk with your trusty staff and it breaks apart! #card #@lizard #!staff
    -> LIZARD_EVENT_EPILOGUE("lizard_saved")
    
    
+ You look at the dagger you have. Would it be strong enough? #card #@self #!dagger
    {
     - did_first_dagger_trial:
        You try channel as much power as you can through your body to get done this time #narrator
        \*THUMP* #narrator
        With a big, powerful swing, the tree trunk splits into pieces. #narrator
         ~ did_first_dagger_trial = false
         -> LIZARD_EVENT_EPILOGUE("lizard_saved")
        
        - else:
        You try stabbing the tree trunk with your dagger but it doesn't break. However, it looks like one more hit could do the trick. #narrator #refundcard:dagger
        ~ did_first_dagger_trial = true
        -> LIZARD_CARD_OPTIONS
    }
    
+ You bash the tree trunk with your trusty frying pan and it easily breaks apart! #card #@lizard #!frying-pan
    -> LIZARD_EVENT_EPILOGUE("lizard_saved_wfryingpan")
    
+ You give the lizard a potion in hopes it would at least help with the pain. #card #@lizard #!glass-bottle
    "Thank you {g_player_gender == "hat": dear hat! | stranger!} This makes the pain bearable." #speaker: Unknown Lizard
    ~ g_tried_help_lizard_soldier = true
    -> LIZARD_CARD_OPTIONS
    
+ "What kind of a lizard gets stuck underneath a tree? Have you no awareness of surroundings?" #card #speaker:You #@lizard #!insult
    "Hey! I- I didn't hear it... I told you that!" #speaker:Unknown Lizard #setsprite:lizard1>lizard1_crying
    They seem clearly very embarrassed and horrible about the situation. #narrator
    How dare you rub salt into the wound. #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You tie the rope around the lizard and take the other end. #card #@lizard #!rope
    "Don't blame me if this doesn't work..." #speaker:You
    You start pulling the lizard as they hold on with their dear life on the rope. #narrator
    ~ lizard_health--
    {
        - lizard_health == 0:
        They seem to get knocked out by this sudden force. That wasn't supposed to happen! #narrator
        -> LIZARD_EVENT_EPILOGUE("escaped")
        - else:
        Slowly but surely there is more lizard on your side than on the other! #narrator
        -> LIZARD_EVENT_EPILOGUE("lizard_saved")
    }
    
+ You throw a fist at this shady guy. Clearly he's just pretending to be stuck! #card #@lizard #!punch
    "Wait what-" #speaker:Unknown Lizard
    \*THUMP* #narrator  #setsprite:lizard1>lizard1_defeated
    Liars don't deserve to end sentences! #narrator
    You feel great after knocking out the poor creature and deside to continue down the road. #narrator
    You should be ashamed.
    ~ g_tried_help_lizard_soldier = false
    -> DONE

+ You cast static shock in hopes of it breaking the tree trunk. #card #@lizard #!static-shock
    Electricity crackles on the moist tree trunk only scrounching it a little. #narrator #setsprite:lizard1>lizard1_fried
    The lizard however looks very burned... #narrator
    ~ lizard_health--
    ...Maybe you should try again... or not #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You throw vile poison at the shady lizard. Clearly he's just pretending to be stuck! #card #@lizard #!evil-bottle
    "Wait what-" #speaker:Unknown Lizard
    \*THUMP* #narrator #setsprite:lizard1>lizard1_defeated
    Liars don't deserve to end sentences! #narrator
    The poison did nothing to this lizard. You should be ashamed. #setsprite:lizard1>lizard1_sad
    Well you can't stay after that so you quickly leave and continue your journey. #narrator
    ~ g_tried_help_lizard_soldier = false
    -> DONE

+ "Would you like some soup?" #card #@lizard #!can-of-beans #speaker:You
    "I keep it close by... for my family?" #speaker:You
    "What kind of soup?" #speaker:Unknown Lizard #setsprite:lizard1>lizard1_default
    "I'm not sure actually. I never checked." #speaker:You
    They look slightly confused by this statement. #narrator
    "You know what. I don't like soup anyways" #speaker:Unknown Lizard
    "Your loss" #speaker:You
    -> LIZARD_EVENT_EPILOGUE("tried_helping")
    
+ Throwing that would be too cruel. They are a lizard after all. #card #@lizard #!static-shock #refundcard:snowball
    -> LIZARD_CARD_OPTIONS
    
+ You throw smokescreen near the lizard and it quickly covers everything arond. #card #@lizard #!smokescreen
    Leaving isn't an option anymore. You can't see anything. #narrator
    "What was that??" #speaker:Unknown Lizard
    "Don't worry about it." #speaker:You
    -> LIZARD_CARD_OPTIONS

+ You start suddenly running towards the tree. #card #@lizard #!run
    "H- Hey! What are you planning on?!" #speaker:Unknown Lizard
    You successfully run to the tree, but can't jump over it and fall to the ground. #narrator
    Great job. #narrator
    -> LIZARD_CARD_OPTIONS
    
+ You take out the cool stick you found earlier and place it in the hole you found on the tree. #card #@lizard #!stick
    Unfortunately it doesn't do anything but look cool #narrator
    -> LIZARD_CARD_OPTIONS

+ "You know you're actually not stuck. You're just imagining it." #card #@lizard #!deceive #speaker:You
    "..." #speaker:Unknown Lizard
    -> LIZARD_EVENT_EPILOGUE("tried_helping")

+ You start mumbling some words that sound soothing. #card #@self #!soothsay
    {
        - lizard_health >= 2:
        They were already at full health, but least your mumbling was cool. #narrator
         ~ lizard_health++
        - lizard_health < 2:
        The healing properties quickly kick in, and they gain some health.
         ~ lizard_health++
    }
  -> LIZARD_CARD_OPTIONS


+ You bash the tree trunk with the longsword and it breaks apart! #card #@lizard #!longsword
    -> LIZARD_EVENT_EPILOGUE("lizard_saved")
    
+ "You are a good lizard!" #card #@lizard #!praise #speaker:You
    "Thanks!" #speaker:Unknown Lizard #setsprite:lizard1>lizard1_thumb
    Now the lizard feels little better. How nice. #narrator
    -> LIZARD_CARD_OPTIONS

//hypnotize
+ You start hypnotizing the creature. #card #@self #!hypnotize
    Somehow you're successfull and decide to use that moment of confusion to escape. #narrator
    Quietly as you can you sneak past the tree leaving them behind. #narrator
    Hopefully someone else finds them soon #narrator
    ~ g_tried_help_lizard_soldier = false
    -> DONE

    
=== LIZARD_EVENT_EPILOGUE(exit_method) ===
{
    - exit_method == "escaped":
    You don't know what to do with the lizard but you can't stay here forever waiting for them to wake up. #narrator
    It seems obvious to you that you should leave quickly and quietly so that you do. #narrator 
    Towards new experiences! #narrator #hidetargets:lizard1
    How cruel #narrator
    ~ g_tried_help_lizard_soldier = false
    
    - exit_method == "tried_helping":
    "You know, if you can't get me out it's alraight-" #speaker:Unknown Lizard
    "No! I'm gonna keep trying till the very end!" #speaker:You
    "...I'd much prefere if you could just a deliver a message for me." #speaker:Unknown Lizard
    "There is a town in the mountains where I live. If you could just go there and tell them that I'm stuck that would be enough." #speaker:Unknown Lizard
    "...Fine. I can do that..." #speaker:You
    You get say goodbyes to the lizard and continue your journey towards the mountain city. #narrator #hidetargets:lizard1
    ~ g_tried_help_lizard_soldier = true
    
    - exit_method == "lizard_saved":
    "Thank you so much! I can't believe that actually worked." #speaker:Unknown Lizard #setsprite:lizard1>lizard1_saved
    "No worries, I'm glad I could help." #speaker:You
    "I don't have much to give you right now but if you ever come across the mountain town I'll be sure to reward you properly!" #speaker:Unknown Lizard
    "Take these for now, they don't carry any value to me but you seem to know what they are." #speaker:Unknown Lizard
    They hand you some cards. #narrator #function:RewardCards(1, yell, telekinesis, spear)
    "I hope we meet again {g_player_gender == "hat": dear hat! | stranger!}
    The lizard continues into the leaves, leaving you behind, and you resume your travels. #narrator #hidetargets:lizard1 
    ~ g_helped_lizard_soldier = true
    
    - exit_method == "lizard_saved_wfryingpan":
    "Thank you so much! I can't believe that actually worked." #speaker:Unknown Lizard #setsprite:lizard1>lizard1_saved
    "What a great frying pan that is." #speaker:Unknown Lizard
    "No worries, me and this thing are always ready to help" #speaker:You
    You hit different poses with the frying pan letting the faint light reflect on its dark color variations. #narrator
    The lizard seems slightly weirded out by that... #narrator
    "Where did you get it?" #speaker:Unknown Lizard
    You take a minute to think but cannot remember. #narrator
    "Well anyways...I don't have much to give you, but if you ever come across the mountain town I'll be sure to reward you better!" #speaker:Unknown Lizard
    "Take these for now, they don't carry any value to me but you seem to know what they are." #speaker:Unknown Lizard
    They hand you some cards. #narrator #function:RewardCards(1, yell, telekinesis, spear)
    "I hope we meet again {g_player_gender == "hat": dear hat! | stranger!}
    The lizard continues into the leaves, leaving you behind, and you resume your travels. #narrator #hidetargets:lizard1 
    ~ g_helped_lizard_soldier = true
}

-> DONE
 