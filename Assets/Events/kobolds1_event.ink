INCLUDE global_vars.ink

VAR kobolds_revealed = false
VAR kobolds_hostile = false
VAR considered_frying_bonk = false
VAR considered_evil_bottle = false

You're walking down a familiar path, enjoying the sounds of birds chirping and the trees rustling slightly in the wind. #narrator

After a short while of walking, you spot a strange figure in the distance. #narrator

Despite it being a good bit taller than you, it doesn't seem very imposing due to the way it moves. #narrator

As you get even closer, you start to understand why this figure seems to struggle with moving. #narrator #showtargets:kobolds1

"Keep moving, chip." #speaker:Shady Figure

...You're unsure if you should see their pretentiousness as insulting or endearing. But you want to do <i>something</i> about this. #narrator

-> KOBOLDS_CARD_OPTIONS

=== KOBOLDS_CARD_OPTIONS ===

/////// SELF OPTIONS START ///////
 + The apple crunches pleasantly as you bite into it. #card #@self #!apple
  {
    - g_player_health < g_player_max_health:
    ~ g_player_health++
  }
  {
    - kobolds_hostile:
    The kobolds seem insulted that you think you have time to snack, and they take the opportunity to attack you! #narrator
    The red kobold springs at you, its teeth sinking into {g_player_gender == "hat": your felted form! | your flesh!} Pain spreads through you as you tear the creature off of you. #narrator
    ~ g_player_health--
  }
    -> KOBOLDS_CARD_OPTIONS
 + This might not be the time to light yourself on fire, but when has that ever stopped you? #card #@self #!bonfire
    The {kobolds_revealed: kobolds look | figure looks} shocked as you manifest a bonfire directly at your feet! #narrator
    The pain is immense, and the smell of burnt flesh doesn't make the experience any more pleasant. #narrator
        ~ g_player_health--
    The {kobolds_revealed: kobolds share a brief look among each other, and bolt away, seemingly now terrified of you! | figure stumbles for a moment, before apparently deciding to take its own advice, as it starts walking away.} #narrator #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
 + You decide to consult the smartest person you know, and you make some good points on how to handle this situation. #card #@self #!banter
    The {kobolds_revealed: kobolds look | figure looks} at you as if you were insane, seemingly too stunned to react. #narrator
    -> KOBOLDS_CARD_OPTIONS
 + You ponder your lockpick for a moment. Good to have one of these, you think. #card #@self #!lockpick #refundcard:lockpick
    ->KOBOLDS_CARD_OPTIONS
 + You spin the dagger in your hand, skillfully twirling it around, and around again. Finally you flip it in the air and catch it. #card #@self #!dagger
    The {kobolds_revealed: kobolds look | figure looks} flabbergasted. {kobolds_revealed: They | It} seemingly can't believe you just hit an emote on {kobolds_revealed: them. | it. } #narrator
    -> KOBOLDS_CARD_OPTIONS
 + You give the frying pan a spin. Cracking your head open right here would <i>for sure</i> alter the course of this {kobolds_revealed: group's | figure's} life. #card #@self #!frying-pan
    But... perhaps it's better to not do that? #narrator
    {
        - considered_frying_bonk:
        No. You've thought this through. Time to give in to the temptation. #narrator
        \*CRACK* #narrator
        ~ g_player_health--
        Blood drips from your {g_player_gender == "hat": host body's} nose. {g_player_gender == "hat": Its | Your} head feels mushy now. Yess... #narrator
        ~ considered_frying_bonk = false
        The {kobolds_revealed: kobolds look | figure looks} horrified! #narrator
        {
            - kobolds_revealed:
            "What the hell!? Why did you do that? Are you literally insane?" #speaker:Red Kobold
            The kobolds frantically mutter between themselves, and seem to have decided something. #narrator
            "You're crazy! We're not sticking around to find out <i>how</i> crazy! Bye, freak!" #speaker:Red Kobold
            - else:
            "What the hell, chip! Are you literally insane!?" #speaker:Shady Figure
            The figure shifts strangely, then starts rapidly walking off. #narrator
            You hear the figure's final words to you as it's making its exit: "let's not meet again." #narrator
        }
        All according to your plan. Heh. #narrator
        -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
        - else:
        Right. Not this time. Not yet, anyway. #narrator #refundcard:frying-pan
        ~ considered_frying_bonk = true
        -> KOBOLDS_CARD_OPTIONS
    }
 + You grasp your trusty staff with your dominant hand. It feels reassuring to hold. #card #@self #!staff
    Can't really do much with it right now, though. #narrator #refundcard:staff
    -> KOBOLDS_CARD_OPTIONS
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
    You acted so fast that the {kobolds_hostile: kobolds couldn't react in time to attack you! | {kobolds_revealed: kobolds seem suspicious of whatever you just drank. | figure couldn't really react, and it just looks slightly confused.}} #narrator
    -> KOBOLDS_CARD_OPTIONS
 + Punching yourself would be a bit strange, even for you. #card #@self #!punch #refundcard:punch
    -> KOBOLDS_CARD_OPTIONS
 + "I'm such a failure, I don't even have a proper spellbook anymore. What kind of wizard even am I? Curse my stupid flumph life..." #card #@self #!insult #speaker:You
    {
        - kobolds_revealed:
        The kobolds look at you with disdain and pity. You seem to have lost any respect they may have had of you. #narrator
        "Um. We're just... gonna go. Good luck with all that?" #speaker:Red Kobold
        "Go then, everyone just leaves me anyway..." #speaker:You
        The kobolds are already walking away, not looking back. Victory. Of sorts, anyway. #narrator
        - kobolds_hostile:
        The kobolds look at you with disdain and pity. You seem to have lost any respect they may have had of you. #narrator
        "Great, now fighting would be stupid awkward! Stupid loser wizard!" #speaker:Red Kobold
        "C'mon, let's just go." #speaker:Red Kobold
        "Go then, everyone just leaves me anyway..." #speaker:You
        The kobolds are already walking away, not looking back. Victory. Of sorts, anyway. #narrator
        - else:
        The figure shifts strangely, its stoic expression failing, and turning to one of disdain. #narrator
        "Right. I'm very busy so I'll just go. Good luck with all that." #speaker:Shady Figure
        "Go then, everyone just leaves me anyway..." #speaker:You
        The figure is already walking away, not looking back. Victory. Of sorts, anyway. #narrator
    }
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
 + Try as you might, giving yourself a static shock proves to be impossible. Damn it. #card #@self #!static-shock
    -> KOBOLDS_CARD_OPTIONS
 + For a moment you consider tying yourself up but... Maybe this isn't the time. Or the place, frankly. #card #@self #!rope #refundcard:rope
    -> KOBOLDS_CARD_OPTIONS
 + You drop a smokescreen at your feet, and bolt away! You're not dealing with this {kobolds_revealed: anymore! | for even a second!} #card #@self #!smokescreen
    -> KOBOLD_EVENT_EPILOGUE("player_left")
 + You consider the bottle of poison for a moment. Surely you shouldn't drink it? #card #@self #!evil-bottle
    {
        - considered_evil_bottle:
        No. You're doing this. #narrator
        You take a big swig from the bottle, and almost retch immediately. You only manage to finish it thanks to using all of your willpower. #narrator
        You feel a burn spreading through your body as the foul liquid makes its way down. This can't be good for you. #narrator
        And it isn't. You keel over, trying to make yourself smaller to somehow minimize the pain in your chest and stomach. It doesn't do much. #narrator
        ~ g_player_health--
        ~ considered_evil_bottle = false
        {
            - kobolds_hostile:
            A pang of empathy seems to hit the previously hostile kobolds as they share a worried look between each other. #narrator #setsprite:kobolds1>kobolds1_revealed
            "Uh, you don't look so good. Maybe let's finish this fight another time." #speaker:Red Kobold
            You try to reply, but speaking feels impossible. You give a quick, pained nod, and the kobolds start making their exit. #narrator
            This was not a good idea. #narrator
            You whimper on the ground for several hours, but eventually the worst of the pain passes, and you feel like you could move on. #narrator
            -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
            - kobolds_revealed:
            The kobolds share a worried look between each other, before moving slightly closer. #narrator
            "Uh, you good? No, right?" #speaker:Red Kobold
            You barely manage a "nuh uh". #narrator
            "You want us to go? We should probably go right?" #speaker:Red Kobold
            A weak "mh" is all you manage. You'd rather suffer in private. #narrator
            "Right, well. We'll go then. Good meeting you, {g_player_gender == "hat": wizard-hat | wizard} #narrator
            The kobolds quickly make their exit.
            You whimper on the ground for several hours, but eventually the worst of the pain passes, and you feel like you could move on. #narrator
            -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
            - else:
            The figure shifts strangely. #narrator
            "You good, chip?" #speaker:Shady Figure
            Speaking seems like it'd hurt a lot, so you just shake your head. #narrator
            "Thought so. I'll just keep walking, chip. Good luck with recovering from that." #speaker:Shady Figure
            The figure does as it says, and makes its exit.
            You whimper on the ground for several hours, but eventually the worst of the pain passes, and you feel like you could move on. #narrator
            -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
        }
        - else:
        Yeah maybe not. You brewed this yourself, and you know that it'd cause significant damage to your esophagus and stomach if ingested. #narrator #refundcard:evil-bottle
        ~ considered_evil_bottle = true
        -> KOBOLDS_CARD_OPTIONS
    }
 + You take out the can of soup and crack it open. Time for a little meal! #card #@self #!can-of-beans
  {
    - g_player_health < g_player_max_health:
    ~ g_player_health++
  }
  {
    - kobolds_hostile:
    The kobolds seem insulted that you think you have time to have dinner, and they take the opportunity to attack you!
    The red kobold springs at you, its teeth sinking into {g_player_gender == "hat": your felted form! | your flesh!} Pain spreads through you as you tear the creature off of you.
    ~ g_player_health--
    - else:
    The {kobolds_revealed: kobolds look | figure looks} a bit confused, but {kobolds_revealed:they let|it lets} you finish eating.
  }
    -> KOBOLDS_CARD_OPTIONS
 + You give the snowball a few tosses in your hand, but you can't really figure out what to do with it like this. So you give it a chomp. #card #@self #!snowball
    Cold. Doesn't taste like much. Eh. #narrator
    The {kobolds_revealed: kobolds look slightly jealous, but they quickly hide their feelings. | figure looks on as you chomp the snowball. You wonder what its thinking.}
    -> KOBOLDS_CARD_OPTIONS
/////// SELF OPTIONS END ///////

////// KOBOLD OPTIONS START //////
 + Your generous offer of an apple seems to garner the respect of the {kobolds_revealed: kobolds! | figure!} #card #@kobolds #!apple
    -> HANDLE_APPLE_OUTCOME
 + You decide to roast {g_bonfire_marshmallows_seen: some marshmallows with the {kobolds_revealed: kobolds. | shady figure.} Better to have friends than enemies after all. | the {kobolds_revealed: kobolds. This has gone too far! | shady figure. No one calls you chip!}} #card #@kobolds #!bonfire
    -> HANDLE_BONFIRE_OUTCOME
 + You attempt to strike up a conversation with {kobolds_revealed: the kobolds. | the figure.} #card #@kobolds #!banter
    "Nice weather today, huh?" #speaker:You
    -> HANDLE_BANTER_OUTCOME
 + You hand a lockpick to the {kobolds_revealed: kobolds. | figure.} #card #@kobolds #!lockpick
    -> HANDLE_LOCKPICK_OUTCOME
 + You whip out your dagger. {kobolds_hostile: Time to finish this. | {kobolds_revealed: You've had enough of these three. | No one calls you chip!}} #card #@kobolds #!dagger
    -> HANDLE_DAGGER_OUTCOME
 + You firmly grip the frying pan and swing straight at {kobolds_revealed: the kobold stack! | shady figure!} #card #@kobolds #!frying-pan
    -> HANDLE_BONK_OUTCOME
 + You grip the staff with both hands, and swing straight at the {kobolds_revealed: kobold stack! | shady figure!} #card #@kobolds #!staff
    -> HANDLE_BONK_OUTCOME
 + You offer a healing potion to the {kobolds_revealed: kobolds, hoping to demonstrate that you mean no harm.|shady figure. Perhaps that'll gain you some respect?} #card #@kobolds #!glass-bottle #type:utility #other:healing
    -> HANDLE_POTION_OUTCOME
 + You form a fist with your hand, and swing at the {kobolds_revealed: kobold stack! | shady figure!} #card #@kobolds #!punch
    -> HANDLE_BONK_OUTCOME
 + "{kobolds_revealed:Are you three stacked because you're insecure about your height? Because you're all <i>really</i> short." |Your sunglasses don't fit you and the coat makes you look like a total creep!"} #card #@kobolds #!insult #speaker:You
    -> HANDLE_INSULT_OUTCOME
 + You rub your hands together, tiny sparks forming as you do. #card #@kobolds #!static-shock
    You spring for the {kobolds_revealed: stack, | figure,} managing to land a shock directly on {kobolds_revealed: the red one's snout! | figure's face!} #narrator
    -> HANDLE_SHOCK_OUTCOME
 + You quickly fling several loops of rope around the {kobolds_revealed: kobolds|figure}! #card #@kobolds #!rope
    -> HANDLE_ROPE_OUTCOME
 + You cast a cloud of smoke around the {kobolds_revealed: kobolds | figure}, and immediately bolt away! #card #@kobolds #!smokescreen
    As you're dashing, you hear {kobolds_revealed: the kobolds all coughing loudly, sounding very confused| a lot more coughing than what one person should be able to let out}. #narrator
    Whatever, that's not your problem anymore! #narrator
    -> KOBOLD_EVENT_EPILOGUE("player_left")
 + You mould up a snowball in your hands for a few second before launching it at the {kobolds_revealed:kobolds|figure}! #card #@kobolds #!snowball
    -> HANDLE_SNOWBALL_OUTCOME
 + "Soup for your family"... {kobolds_hostile:Since you're currently in a fight, it may be time to use this for self-defense.|Perhaps this will gain you the favor of the {kobolds_revealed:kobolds|figure}?} #card #@kobolds #!can-of-beans
    -> HANDLE_SOUP_OUTCOME
 + You lob the bottle of poison at the {kobolds_revealed:kobolds|figure}! #card #@kobolds #!evil-bottle
    -> HANDLE_EVIL_BOTTLE_OUTCOME
 
-> DONE

=== HANDLE_APPLE_OUTCOME ===
{
    - !kobolds_revealed:
    "I accept your generous offer, chip. Good luck on your travels." #speaker:Shady Figure
    The figure resumes its laborous walking, eventually fading into the distance #narrator #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
    
    - kobolds_hostile:
    "Wait, I thought we were fighting! Why're you giving us gifts now!" His tail is wagging. #speaker:Red Kobold
    "I didn't mean to start a fight, hopefully the apple is enough to repay you." #speaker:You
    The kobolds chatter among themselves for a short while. #narrator
    "We've reached a decision! You're off the hook this time, {g_player_gender == "hat": hat-thing!|wizard!}" #speaker:Red Kobold
    The kobolds start preparing to leave. #narrator
    "Bye, {g_player_gender == "hat": hat-thing|wizard}. Try not to start fights with everyone you meet!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_coat
    You wave them off as they're leaving. #narrator
    Hm. Seems they forgot their coat. #narrator
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
    
    - else: "Whoa, thanks a bunch! We'll totally try to remember this!" #speaker:Red Kobold
    "Bye now, {g_player_gender == "hat": hat | human}!" #speaker:Red Kobold
    The three kobolds hop off of each other, before continuing on their way. Seems they forgot their coat. #narrator #setsprite:kobolds1>kobolds1_coat
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
}
 
 === HANDLE_BONFIRE_OUTCOME ===
{
    - !kobolds_revealed && !g_bonfire_marshmallows_seen:
    As you call forth a bonfire, the shady figure stumbles back a few steps. #narrator
    Seeing its uncertainty, you take out some marshmallows to hopefully pique its interest. No, wait. #narrator
    You were meant to roast the figure, not some marshmallows! #narrator
    As you ponder what exactly went wrong, the shady figure shifts around strangely. #narrator
    ~ g_bonfire_marshmallows_seen = true
    You hear strange bickering from inside the shady figure, which quickly ends as... #narrator
    The coat covering the figure falls off, revealing three kobolds stacked on top of each other. #narrator #setsprite:kobolds1>kobolds1_revealed
    ~ kobolds_revealed = true
    Ah. #speaker:A Stack of Kobolds
    Well now that our great secret is revealed, nothing's stopping us from having the marshmallows, right? #speaker:White Kobold
    The red one lets out an exasperated sigh. #narrator
    ... #narrator
    -> KOBOLD_EVENT_EPILOGUE("marshmallows")
    
    - !kobolds_revealed && g_bonfire_marshmallows_seen:
    You call forth a bonfire between you and the figure, taking out some marshmallows. #narrator
    "Want some?" #speaker:You
    You hear strange bickering from within the figure, as it shifts around strangely. #narrator
    The bickering quickly stops, as the coat covering the figure falls off, revealing a stack of three kobolds. #narrator #setsprite:kobolds1>kobolds1_revealed
    ~ kobolds_revealed = true
    Ah. #speaker:A Stack of Kobolds
    Well now that our great secret is revealed, nothing's stopping us from having the marshmallows, right? #speaker:White Kobold
    The red one lets out an exasperated sigh. #narrator
    ... #narrator
    -> KOBOLD_EVENT_EPILOGUE("marshmallows")
    
    - kobolds_revealed && !g_bonfire_marshmallows_seen:
    As you call forth a bonfire, the kobold stack stumbles back a few steps. #narrator
    Seeing their uncertainty, you take out some marshmallows to hopefully pique its interest. No, wait. #narrator
    You were meant to roast the kobolds, not some marshmallows! #narrator
    As you ponder what exactly went wrong, the white kobold voices their opinion. #narrator
    ~ g_bonfire_marshmallows_seen = true
    {
        - g_player_gender == "hat":
        "The hat already knows our great secret, taking the marshmallows it offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "female":
        "The wizard lady already knows our great secret, taking the marshmallows she offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "male":
        "This wizard guy already knows our great secret, taking the marshmallows he offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "non-binary":
        "The wizard-person already knows our great secret, taking the marshmallows they offer wouldn't change anything!" #speaker:White Kobold
    }
    The red one lets out an exasperated sigh. #narrator #setsprite:kobolds1>kobolds1_revealed
    ... #narrator
    -> KOBOLD_EVENT_EPILOGUE("marshmallows")
    
    - else:
    You call forth a bonfire between you and the kobolds, taking out some marshmallows. #narrator
    "You three some?" #speaker:You
    The three bicker quietly for a moment, before the white kobold voices their opinion. #narrator
    {
        - g_player_gender == "hat":
        "The hat already knows our great secret, taking the marshmallows it offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "female":
        "The wizard lady already knows our great secret, taking the marshmallows she offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "male":
        "This wizard guy already knows our great secret, taking the marshmallows he offers wouldn't change anything!" #speaker:White Kobold
        - g_player_gender == "non-binary":
        "The wizard-person already knows our great secret, taking the marshmallows they offer wouldn't change anything!" #speaker:White Kobold
    }
    The red one lets out an exasperated sigh. #narrator
    ... #narrator
    -> KOBOLD_EVENT_EPILOGUE("marshmallows")
}

=== HANDLE_BANTER_OUTCOME ===
{
    - !kobolds_revealed:
    "I suppose. I'm far too busy to stay for a chat, however. Keep walking, chip", the figure says while lumbering away. #speaker:Shady Figure
    "What a weirdo", you think to yourself while watching the figure grow distant. #narrator
    
    - else:
    "Uhm. I guess it is?" #speaker:Red Kobold
    You and the kobold stack both shuffle awkwardly. #narrator
    "Good talk", you say while already walking away to escape the awkwardness. #narrator #hidetargets:kobolds1
    "You too", a kobold replies before you hear a slap behind you. #narrator
    What a horribly awkward encounter. Hopefully they'll also try to forget about it. #narrator
}
-> KOBOLD_EVENT_EPILOGUE("kobolds_left")

=== HANDLE_LOCKPICK_OUTCOME ===
{
    - kobolds_hostile:
        The lockpick is swatted away as the red kobold bites your hand! #narrator
    ~ g_player_health--
        What a rude bunch of lizards! #narrator
    
    - kobolds_revealed: 
        The red kobold gives it a confused look. #narrator
        "Uh, no thanks." #speaker:Red Kobold
    
    - else:
        The figure bends strangely to inspect it, before straightening back up. #narrator
        "No thanks, chip. Keep moving." #speaker:Shady Figure
        This guy... #narrator
}
-> KOBOLDS_CARD_OPTIONS

=== HANDLE_DAGGER_OUTCOME ===
{
    - kobolds_hostile:
        You swing at the stack of kobolds, landing a few strikes on the top one, who yelps out in pain! #narrator
        The stack quickly dismantles, and the other two rapidly carry off their wounded stack-mate. #narrator #setsprite:kobolds1>kobolds1_defeat
        As the group is escaping, the dark grey one yells out: "We'll definitely finish you off next time!" #narrator #hidetargets:kobolds1
        You'd like to see them try. #narrator
        -> KOBOLD_EVENT_EPILOGUE("won_fight")
    - kobolds_revealed:
        You swing at the stack of kobolds, landing a few strikes on the top one, who yelps out in pain! #narrator
        ~ kobolds_hostile = true
        {
            - g_player_gender == "female":
            "What the hell, lady!? What was that for?" #speaker:Red Kobold
            - g_player_gender == "male":
            "What the hell, mister!? What was that for?" #speaker:Red Kobold
            - g_player_gender == "non-binary":
            "What the hell was that for!?" #speaker:Red Kobold
            - g_player_gender == "hat":
            "What the hell, hat!? What was that for?" #speaker:Red Kobold
        }
        The stack quickly dismantles, and the other two prepare carry off their wounded stack-mate. #narrator #setsprite:kobolds1>kobolds1_defeat
        As the group begins their escape, the dark grey one yells out: "We'll definitely mess you up next time!" #narrator #hidetargets:kobolds1
        You'd like to see them try. #narrator
        -> KOBOLD_EVENT_EPILOGUE("won_fight")
    - else:
        You swing at the figure, and while you do land some strikes, the figure seems more scared than hurt. #narrator
        You quickly find out the reason, as the figure's coat slips off, revealing a very pissed off looking stack of kobolds! #narrator #setsprite:kobolds1>kobolds1_attackmode
        ~ kobolds_revealed = true
        ~ kobolds_hostile = true
        {
            - g_player_gender == "female":
            "What the hell, lady!? What was that for?" #speaker:Red Kobold
            - g_player_gender == "male":
            "What the hell, mister!? What was that for?" #speaker:Red Kobold
            - g_player_gender == "non-binary":
            "What the hell was that for!?" #speaker:Red Kobold
            - g_player_gender == "hat":
            "What the hell, hat!? What was that for?" #speaker:Red Kobold
        }
        "No one calls me chip!" #speaker:You
        The kobolds growl at you. You seem to have a fight on your hands. #narrator
        -> KOBOLDS_CARD_OPTIONS
}

=== HANDLE_BONK_OUTCOME ===
Your strike connects with the head of {kobolds_revealed: the top kobold, | shady figure,} sounding like it hurt. A lot. #narrator
{
    - !kobolds_revealed:
    A fitting punishment for calling you chip, you think. #narrator
    The figure quickly falls apart, revealing three kobolds! #narrator #setsprite:kobolds1>kobolds1_defeat
    ~ kobolds_revealed = true
    {
        - g_player_gender == "female":
        "What the hell, lady!? What was that for?" #speaker:Dark Grey Kobold
        - g_player_gender == "male":
        "What the hell, mister!? What was that for?" #speaker:Dark Grey Kobold
        - g_player_gender == "non-binary":
        "What the hell was that for!?" #speaker:Dark Grey Kobold
        - g_player_gender == "hat":
        "What the hell, hat!? What was that for?" #speaker:Dark Grey Kobold
    }
    "No one calls me chip!" #speaker:You
    The remaining two kobolds quickly pick up their wounded stack-mate, preparing to run off. #narrator
    "Point taken, now get the hell out of the way!" #speaker:Dark Grey Kobold
    The two kobolds still standing sprint away from you while carrying the third. #narrator #hidetargets:kobolds1
    As they make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    
    - !kobolds_hostile:
    The top kobold falls from atop the stack, unconscious. You can already see a bump forming on his head. #narrator #setsprite:kobolds1>kobolds1_defeat
    ~ kobolds_hostile = true
    {
        - g_player_gender == "female":
        "What the hell, lady!? What was that for?" #speaker:Dark Grey Kobold
        - g_player_gender == "male":
        "What the hell, mister!? What was that for?" #speaker:Dark Grey Kobold
        - g_player_gender == "non-binary":
        "What the hell was that for!?" #speaker:Dark Grey Kobold
        - g_player_gender == "hat":
        "What the hell, hat!? What was that for?" #speaker:Dark Grey Kobold
    }
    The remaining two kobolds quickly pick up their wounded stack-mate, preparing to run off. #narrator
    As the kobolds start to make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator hidetargets:kobolds1
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    
    - else:
    The top kobold falls from atop the stack, unconscious. You can already see a bump forming on his head. #narrator #setsprite:kobolds1>kobolds1_defeat
    The remaining two kobolds quickly pick up their wounded stack-mate, preparing to run off. #narrator
    As the kobolds start to make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator #hidetargets:kobolds1
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
}

=== HANDLE_POTION_OUTCOME ===
{
    - kobolds_hostile:
    "Wait, I thought we were fighting! Why are you giving us gifts now!?" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_revealed
    "I didn't mean to start a fight, hopefully a healing potion is enough to repay you." #speaker:You
    The kobolds chatter among themselves for a short while. #narrator
    "We've reached a decision! You're off the hook this time, {kobolds_gender_word()}." #speaker:Red Kobold
    The kobolds start preparing to leave. #narrator
    "Bye, {kobolds_gender_word()}. Try not to start fights with everyone you meet!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_coat
    You wave them off as they're leaving. #narrator
    Hm. Seems they forgot their coat. #narrator
    
    - kobolds_revealed:
    "Wow, are you sure we can take this? Thanks a lot!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_revealed
    "You're alright for a {g_player_gender == "hat": hat! | human!} We'll totally try to remember this!" #speaker:Red Kobold
    The red kobold whispers something to the others, and they nod in unison. #narrator
    "Thanks for the potion!" #speaker:The Kobold Stack
    "Bye now!" #speaker:Red Kobold
    The three kobolds hop off of each other, before continuing on their way. Seems they forgot their coat. #narrator #setsprite:kobolds1>kobolds1_coat
    
    - else:
    "I accept your generous offer, chip", the figure says while taking the offered potion. #speaker:Shady Figure
    "I will remember this. Now keep walking, chip" #speaker:Shady Figure
    You seem to have garnered the respect of the figure, who is now laborously walking away from you. #narrator #hidestargets:kobolds1
}
-> KOBOLD_EVENT_EPILOGUE("kobolds_left")

=== HANDLE_INSULT_OUTCOME ===
{
    - kobolds_hostile:
    "We're not short!" #speaker:The Kobold Stack
    "<i>So</i> insecure. #speaker:You
    "That's it, now you're gonna get it!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_attackmode
    The red kobold springs at you, its teeth digging right into {g_player_gender == "hat": your felted edge! | shoulder!} #narrator
    ~ g_player_health--
    You manage to tear the creature off of you, but the pain remains. Time to fight back! #narrator
    - kobolds_revealed:
    "We're not short!" #speaker:The Kobold Stack
    "<i>So</i> insecure. #speaker:You
    "That's it, now you're gonna get it!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_attackmode
    The kobolds look pretty pissed off now. It seems your insult has gotten you into a fight! #narrator
    ~ kobolds_hostile = true
    - else:
    The figure flings its coat off, revealing a stack of very pissed off kobolds! #narrator #setsprite:kobolds1>kobolds1_attackmode
    ~ kobolds_revealed = true
    ~ kobolds_hostile = true
    "We're not creeps! And the sunglasses are cool!" #speaker:A Stack of Kobolds
    "Cope more." #speaker:You
    The stack starts growling at you. Seems you've gotten yourself into a fight! #narrator
}
    -> KOBOLDS_CARD_OPTIONS
    
=== HANDLE_SHOCK_OUTCOME ===
{
    - kobolds_revealed:
    The red kobold yelps loudly, falling dramatically off from on top of the stack! #narrator #setsprite:kobolds1>kobolds1_defeat
    The other two quickly de-stack as well, huddling around their injured friend. That should show them! #narrator
    - else:
    The figure quickly falls apart, revealing three kobolds, one of whom seems to already be knocked out! #narrator #setsprite:kobolds1>kobolds1_defeat
    The other two quickly huddle around their injured friend. That should show them? #narrator
}
{
    - !kobolds_hostile:
    The dark grey one gives you a seriously hostile look. #narrator
    {
        - g_player_gender == "female":
        "What the hell, lady!? What was that for!" #speaker:Dark Grey Kobold
        - g_player_gender == "male":
        "What the hell, mister!? What was that for!" #speaker:Dark Grey Kobold
        - g_player_gender == "non-binary":
        "What the hell was that for!?" #speaker:Dark Grey Kobold
        - g_player_gender == "hat":
        "What the hell, hat!? What was that for!" #speaker:Dark Grey Kobold
    }
    It seems to have more pressing matters than revenge right now though, as the red kobold groans on the ground. #narrator
}

"Stay with us! Just focus on breathing!" #speaker:Dark Grey Kobold
"I... I don't think I'll make it... Please... Take care of yourselves." #speaker:Red Kobold
...Huh? That was a static shock, not anything seriously damaging! Is he pretending? #narrator
Whatever the case, seems like you've won the day! #narrator
The dark grey and white kobolds hoist their injured stackmate above their heads and start making an exit! #narrator
"We'll definitely get you next time, you evil wizard! Definitely!" #speaker:Dark Grey Kobold
The kobolds skitter away, leaving you to ponder what to do next. #narrator #hidetargets:kobolds1

-> KOBOLD_EVENT_EPILOGUE("kobolds_left")

=== HANDLE_ROPE_OUTCOME ===
{
    - kobolds_revealed:
    The kobolds try to quickly de-stack, but they're too slow! #narrator
    You quickly finish tying them up, dusting off your {g_player_gender == "hat": host body's} hands once you're done. #narrator #setsprite:kobolds1>kobolds1_roped
    The kobolds mutter among each other, clearly peeved that they've been tied up so easily. #narrator
    "Ha, caught you! And so easily too!" #speaker:You
    "Well. Once you're done mocking us, could you untie us? This is <i>really</i> uncomfortable, especially for those two." #speaker:Red Kobold
    "Mmm, no. You'll can repent for your actions right there." #speaker:You
    "What actions!? {kobolds_hostile: You started it!|We weren't even doing anything!}" #speaker:Red Kobold
    "No one calls me chip!" #speaker:You
    They groan loudly, probably realizing they can't really talk back in this position. #narrator
    "Well then, I'll be going. Have fun getting free!" #speaker:You #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("player_left")
    
    - else:
    As you try to wrap the rope around the figure, its body twists alarmingly, before falling apart entirely! #narrator
    From the coat emerge three kobolds that quickly hop back onto each other, growling at you all the while! #narrator #setsprite:kobolds1>kobolds1_attackmode
    ~ kobolds_revealed = true
    ~ kobolds_hostile = true
    "What the hell was that for!? We're totally gonna mess you up for that!" #speaker:Red Kobold
    Seems you've landed yourself in a fight! #narrator
    -> KOBOLDS_CARD_OPTIONS
}

=== HANDLE_SNOWBALL_OUTCOME ===
{
    - kobolds_hostile:
    The snowball lands squarely on the the top kobold's snout, knocking him down! The other two quickly surround him. #narrator #setsprite:kobolds1>kobolds1_defeat
    "Hang in there! We'll get you to safety!" #speaker:Dark Grey Kobold
    The red kobold slowly reaches for the other two, then his arms fall limp. #narrator
    The other two panic for a few seconds, then hoist their fallen stackmate above their heads and sprint away! #narrator #hidetargets:kobolds1
    "We'll totally get you next time, evil wizard! You'll see!" #speaker:Dark Grey Kobold
    Parting words of a sore loser. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    
    - else:
    {
        - kobolds_revealed:
        The snowball hits the middle kobold in the stomach, and the stack seems to have taken this personally! #narrator
        - else:
        The snowball hits the figure in the stomach, causing it to quickly collapse! #narrator
        Three kobolds emerge from the pile, reassuming their stacked arrangement. #narrator #setsprite:kobolds1>kobolds1_attackmode
        ~ kobolds_revealed = true
    }
    
    "That's it, now you're gonna get it!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_attackmode
    ... #narrator
    After a good half an hour of the most intense snowball fight in your life, you and the kobolds are both too exhausted to continue. #narrator #setsprite:kobolds1>kobolds1_revealed
    "Too... cold..." #speaker:White Kobold
    Ah, right, these guys are exothermic! This must mean you've won! #narrator
    "Looks like... *huff*... I win!" #speaker:You
    Still, this wasn't a serious fight, so you should probably make sure this doesn't cause them lasting damage. #narrator
    ... #narrator
    After a while of sitting with the kobolds by a small campfire you built, you eventually help them back into their coat. #narrator #setsprite:kobolds1>kobolds1_default
    {
        - g_player_gender == "hat":
        "Thanks for helping us warm back up, hat-thing. We haven't met hats before, but you seem alright!" #speaker:Red Kobold (disguised)
        - else:
        "Thanks for helping us warm back up, {kobolds_kind_gender_word()}. You're alright for a human!" #speaker:Red Kobold (disguised)
    }
    The re-disguised stack starts to make its leave, slowly hobbling away. #narrator #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
}

=== HANDLE_SOUP_OUTCOME ===
{
    - kobolds_hostile:
    You swing your makeshift can-in-a-bag weapon at the kobolds! #narrator
        -> HANDLE_BONK_OUTCOME
    
    - kobolds_revealed:
    "...You three want some soup?" #speaker:You
    The white kobold seems to be about to say something, but the red one is faster: "hold on just a moment, we need to think." #narrator
    After a short deliberation, the kobolds seem to have reached a conclusion! #narrator
    "We would very much like some soup, {kobolds_kind_gender_word()}!" #speaker:Red Kobold
    ... #narrator
    After a while of sitting with the kobolds by a small campfire you built, the soup is finished, and the kobolds have re-stacked themselves. #narrator
    {
        - g_player_health < g_player_max_health:
        ~ g_player_health++
    }
    "Thanks for the soup, {kobolds_gender_word()}! We'll totally remember this!" #speaker:Red Kobold
    The group makes a quick exit, the red one waving at you as they go. #narrator #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
    
    - else:
    "...You want some soup?" #speaker:You
    The figure shifts slightly, seemingly muttering something to itself. #narrator
    "I'll take it. Thanks chip." #speaker:Shady Figure
    "I thought we could sha-" #speaker:You
    The figure takes the can from your hand, and immediately starts walking away. #narrator
    "I won't forget about this, chip," it says while leaving. #speaker:Shady Figure #hidetargets:kobolds1
    You're a bit stunned by the boldness of this guy, but you don't care enough to chase it down just for some soup. #narrator
    Oh well. #narrator
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
}

=== HANDLE_EVIL_BOTTLE_OUTCOME ===
{
    - kobolds_revealed:
    As the bottle is about to hit the stack, the red kobold makes a heroic dive towards it, taking the hit all by itself! #narrator
    The red kobold yelps loudly as it takes the hit, falling to the ground dramatically. #narrator #setsprite:kobolds1>kobolds1_defeat
    The other two are already huddling around their injured friend, trying not to panic. That should show them! #narrator
    The injured kobold tenses up as the poison takes effect, and the other two can't keep it together any longer. #narrator
    "We're gonna get you out of here! Don't worry!" #speaker:Dark Grey Kobold
    "And you! We're totally gonna mess you up next time, you evil wizard!" #speaker:Dark Grey Kobold
    The other two kobolds hoist the injured one above their heads, and quickly run off! #narrator #hidetargets:kobolds1
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    - else:
    The bottle hits the figure, poison spreading all over its coat! #narrator
    The figure quickly throws off its coat, revealing a stack of incredibly pissed off kobolds! #narrator #setsprite:kobolds1>kobolds1_attackmode
    ~ kobolds_revealed = true
    ~kobolds_hostile = true
    "What the hell, {kobolds_mean_gender_word()}!? We're totally going to mess you up for that!" #speaker:Red Kobold
    Seems you've landed yourself in a fight! #narrator
    -> KOBOLDS_CARD_OPTIONS
}

////// FUNCTIONS //////
=== function kobolds_gender_word ===
{ g_player_gender:
    - "female":
        ~ return "lady"
    - "male":
        ~ return "mister"
    - "non-binary":
        ~ return "wizard-person"
    - "hat":
        ~ return "hat-thing"
}

=== function kobolds_kind_gender_word ===
{ g_player_gender:
    - "female":
        ~ return "kind lady"
    - "male":
        ~ return "mister"
    - "non-binary":
        ~ return "wizard-person"
    - "hat":
        ~ return "hat-thing"
}

=== function kobolds_mean_gender_word ===
{ g_player_gender:
    - "female":
        ~ return "lady"
    - "male":
        ~ return "mister"
    - "non-binary":
        ~ return "wizard"
    - "hat":
        ~ return "hat"
}

////// EPILOGUE //////
=== KOBOLD_EVENT_EPILOGUE(exit_method) ===
{
    - exit_method == "marshmallows":
    {
        - g_player_gender == "hat":
        "Thanks again for the marshmallows, hat-thing! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator #hidetargets:kobolds1
        - g_player_gender == "female":
        "Thanks again for the marshmallows, kind lady! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator #hidetargets:kobolds1
        - g_player_gender == "male":
        "Thanks again for the marshmallows, mister! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator #hidetargets:kobolds1
        - g_player_gender == "non-binary":
        "Thanks again for the marshmallows, wizard-person! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator #hidetargets:kobolds1
    }
    What a surprisingly nice group of kobolds. Even though the red one called you chip. #narrator
    As you prepare to leave as well, you notice a few cards where the kobolds were sitting! Did they leave these for you? #narrator
    You figure they're safer in your binder than on the ground, so you gather up the intact ones. #narrator #function:RewardCards(2, stick, deceive, soothsay, run)
    - exit_method == "kobolds_left": 
    As you prepare to leave as well, you notice a few cards on the ground! #narrator
    You figure they're safer in your binder than on the ground, so you gather up the intact ones. #narrator #function:RewardCards(2, stick, deceive, soothsay, run)
    - exit_method == "won_fight": 
    As you're relishing in your triumph, you notice a few cards on the ground! #narrator
    You figure they're safer in your binder than on the ground, so you gather up the intact ones. #narrator #function:RewardCards(2, stick, deceive, soothsay, run)
    - exit_method == "player_left": 
    As you keep walking away, you notice a few cards on the ground! Did the {kobolds_revealed: kobolds | figure} drop these? #narrator
    In any case, they're safer in your binder than on the ground, so you gather up the intact ones. #narrator #function:RewardCards(2, stick, deceive, soothsay, run)
    - else: 
    As you start to leave, you notice a few cards on the ground! #narrator
    You figure they're safer in your binder than on the ground, so you gather up the intact ones. #narrator #function:RewardCards(2, stick, deceive, soothsay, run)
}

You suppose there's nothing left to do here, and resume traveling. #narrator #hidetargets:kobolds1

#function:LoadScene(Map1)

-> DONE