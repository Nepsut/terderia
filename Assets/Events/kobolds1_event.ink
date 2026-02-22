INCLUDE global_vars.ink

VAR kobolds_revealed = false
VAR kobolds_hostile = false

You're walking down a familiar path, enjoying the sounds of birds chirping and the trees rustling slightly in the wind. #narrator

After a short while of walking, you spot a strange figure in the distance. #narrator

Despite it being a good bit taller than you, it doesn't seem very imposing due to the way it moves. #narrator

As you get even closer, you start to understand why this figure seems to struggle with moving. #narrator #targets:kobolds1

"Keep moving, chip." #speaker:Shady Figure

...You're unsure if you should see their pretentiousness as insulting or endearing. But you want to do something about this. #narrator

-> KOBOLDS_CARD_OPTIONS

//TODO: ADD SELF OPTIONS, ADD AT LEAST THE 5 REWARD CARDS FROM CABIN

=== KOBOLDS_CARD_OPTIONS ===
 + Your generous offer of an apple seems to garner the respect of the {kobolds_revealed: kobolds! | figure!} #card #@kobolds #!apple
    -> HANDLE_APPLE_OUTCOME
 + You decide to roast {g_bonfire_marshmallows_seen: some marshmallows with the {kobolds_revealed: kobolds. | shady figure.} Better to have friends than enemies after all. | the {kobolds_revealed: kobolds. This has gone too far! | shady figure. No one calls you chip!}} #card #@kobolds #!bonfire
    -> HANDLE_BONFIRE_OUTCOME
 + You attempt to strike up a conversation with {kobolds_revealed: the kobolds. | the figure.} #card #@kobolds #!banter
    "Nice weather today, huh?" #speaker:You
    -> HANDLE_BANTER_OUTCOME
 + You hand a lockpick to the {kobolds_revealed: kobolds. | figure.} #card #@kobolds #!lockpick
    -> HANDLE_LOCKPICK_OUTCOME
 + You whip out your dagger. {kobolds_hostile: Time to finish this. | {kobolds_revealed: You've had enough of these three. | No one calls you chip! }} #card #@kobolds #!dagger
    -> HANDLE_DAGGER_OUTCOME
 + You firmly grip the frying pan and swing straight at {kobolds_revealed: the kobold stack! | shady figure!} #card #@kobolds #!frying-pan
    -> HANDLE_BONK_OUTCOME
 + You grip the staff with both hands, and swing straight at {kobolds_revealed: the kobold stack! | shady figure!} #card #@kobolds #!staff
    -> HANDLE_BONK_OUTCOME
 + You offer a healing potion to the {kobolds_revealed: kobolds, hoping to demonstrate that you mean no harm. | shady figure. Perhaps that'll gain you some respect.}! #card #@kobolds #!glass-bottle #type:utility #other:healing
    -> HANDLE_POTION_OUTCOME
 
-> DONE

=== HANDLE_APPLE_OUTCOME ===
{
    - !kobolds_revealed:
    "I accept your generous offer, chip. Good luck on your travels." #speaker:Shady Figure
    The figure resumes its laborous walking, eventually fading into the distance #narrator
    -> KOBOLD_EVENT_EPILOGUE("kobolds_left")
    
    - else: "Whoa, thanks a bunch! We'll totally try to remember this!" #speaker:Red Kobold
    "Bye now, {g_player_gender == "hat": hat | human}!" #speaker:Red Kobold
    The three kobolds hop off of each other, before continuing on their way. Seems they forgot their coat. #narrator
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
    "Good talk", you say while already walking away to escape the awkwardness. #narrator
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
        You swing at the stack of kobolds, landing a few strikes on the middle one, who yelps out in pain! #narrator
        The stack quickly dismantles, and the other two rapidly carry off their wounded stack-mate. #narrator
        As the group is escaping, the red one yells out: "We'll definitely finish you off next time!" #narrator
        You'd like to see them try. #narrator
        -> KOBOLD_EVENT_EPILOGUE("won_fight")
    - kobolds_revealed:
        You swing at the stack of kobolds, landing a few strikes on the middle one, who yelps out in pain! #narrator
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
        The stack quickly dismantles, and the other two rapidly carry off their wounded stack-mate. #narrator
        As the group is escaping, the red one yells out: "We'll definitely mess you up next time!" #narrator
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
    The figure quickly falls apart, revealing three kobolds! #narrator #setsprite:kobolds1>kobolds1_revealed
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
    The two kobolds still standing sprint away from you while carrying the third. #narrator
    As they make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    
    - !kobolds_hostile:
    The top kobold falls from atop the stack, unconscious. You can already see a bump forming on his head. #narrator
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
    As the kobolds start to make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
    
    - else:
    The top kobold falls from atop the stack, unconscious. You can already see a bump forming on his head. #narrator
    The remaining two kobolds quickly pick up their wounded stack-mate, preparing to run off. #narrator
    As the kobolds start to make their escape, the dark grey one yells at you: "We'll definitely mess you up next time!" #narrator
    You'd like to see them try. #narrator
    -> KOBOLD_EVENT_EPILOGUE("won_fight")
}

=== HANDLE_POTION_OUTCOME ===
{
    - !kobolds_revealed:
    "I accept your generous offer, chip", the figure says while taking the offered potion. #speaker:Shady Figure
    "I will remember this. Now keep walking, chip" #speaker:Shady Figure
    You seem to have garnered the respect of the figure, who is now laborously walking away from you. #narrator #hidesprite:kobolds1
    
    - kobolds_revealed:
    "Wow, are you sure we can take this? Thanks a lot!" #speaker:Red Kobold #setsprite:kobolds1>kobolds1_revealed
    "You're alright for a {g_player_gender == "hat": hat! | human!} We'll totally try to remember this!" #speaker:Red Kobold
    The red kobold whispers something to the others, and they nod in unison. #narrator
    "Thanks for the potion!" #speaker:The Kobold Stack
    "Bye now!" #speaker:Red Kobold
    The three kobolds hop off of each other, before continuing on their way. Seems they forgot their coat. #narrator
}
-> KOBOLD_EVENT_EPILOGUE("kobolds_left")

=== KOBOLD_EVENT_EPILOGUE(exit_method) ===
{
    - exit_method == "marshmallows":
    {
        - g_player_gender == "hat":
        "Thanks again for the marshmallows, hat-thing! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator
        - g_player_gender == "female":
        "Thanks again for the marshmallows, kind lady! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator
        - g_player_gender == "male":
        "Thanks again for the marshmallows, mister! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator
        - g_player_gender == "non-binary":
        "Thanks again for the marshmallows, wizard-person! Good luck with the adventuring!" The group yells, their voices growing distant. #narrator
    }
    What a surprisingly nice group of kobolds. Even though the red one called you chip. #narrator
    You suppose there's nothing left to do here, and resume traveling. #narrator #function:LoadMap1Scene
    
    - exit_method == "kobolds_left":
    You suppose there's nothing left to do here, and resume traveling. #narrator #function:LoadMap1Scene
}

-> DONE