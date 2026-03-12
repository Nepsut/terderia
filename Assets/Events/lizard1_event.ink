INCLUDE global_vars.ink

/////// Asset variables here :) ///////
VAR considered_evil_bottle = false

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
    Slowly but surely there is more lizard on your side than on the other! #narrator
    -> LIZARD_EVENT_EPILOGUE("lizard_saved")
    
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
    -> DONE

+ You start suddenly running towards the tree. #card #@self #!run
    "H- Hey! What are you planning on?!" #speaker:Unknown Lizard
    You successfully run to the tree, jump over it, and continue your journey. #narrator
    You can't hear the lizard anymore as you continue down the road. #narrator
    -> DONE
//stick
//deceive
//soothsay
//longsword
//praise
//hypnotize
    
    /////// SELF OPTIONS END ///////
    
+ Unfortunately the tree has nothing to use this on. Not even a secret compartment. #card #@lizard #!lockpick
    -> LIZARD_CARD_OPTIONS
    
+ You give the lizard a potion in hopes it would at least help with the pain. #card #@lizard #!glass-bottle
    "Thank you {g_player_gender == "hat": dear hat! | stranger!} This makes the pain bearable." #speaker: Unknown Lizard
    ~ g_tried_help_lizard_soldier = true
    -> LIZARD_CARD_OPTIONS
    
+ "What kind of a lizard gets stuck underneath a tree? Have you no awareness of surroundings?" #card #speaker:You #@lizard #!insult
    "Hey! I- I didn't hear it... I told you that!" #speaker:Unknown Lizard #setsprite:lizard1>lizard1_crying
    They seem clearly very embarrassed and horrible about the situation. #narrator
    How dare you rub salt into the wound. #narrator
    -> LIZARD_CARD_OPTIONS

    
=== LIZARD_EVENT_EPILOGUE(exit_method) ===

"Thank you so much! I can't believe that actually worked." #speaker:Unknown Lizard
"No worries, I'm glad I could help." #speaker:You
"I don't have much to give you right now but if you ever come across the mountain town I'll be sure to reward you properly!" #speaker:Unknown Lizard
"Take these for now, they don't carry any value to me but you seem to know what they are." #speaker:Unknown Lizard
They hand you some cards. #narrator
"I hope we meet again {g_player_gender == "hat": dear hat! | stranger!}
The lizard continues into the leaves, leaving you behind, and you resume your travels. #narrator #hidetargets:lizard1
 ~ g_helped_lizard_soldier = true


-> DONE
 