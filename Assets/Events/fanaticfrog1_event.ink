INCLUDE global_vars.ink

VAR frog_health = 2
VAR frog_hostile = false
VAR considered_frying_bonk = false

While travelling, come across a small cave. You could swear you heard some strange noises too, but they seem to have stopped. #narrator

"Hello? Anybody in here?" #speaker:You

...#narrator

"Ribbit." #speaker:Suspicious Bush

A frog? But something's off. Frogs ribbit, yes, but they don't SAY "ribbit". Is this thing pretending to be a frog? #narrator

"Come out! I know you're not a frog!" #speaker:You

... #speaker:Suspicious Bush

"How did you figure out I'm in this bush?" #speaker:Suspicious Bush

What a croaky voice. #narrator

"Saying "ribbit" kind of gave you away." #speaker:You

"Fair play, I suppose. I'll hop out." #speaker:Suspicious Bush

It does indeed hop out, launching some mushrooms out of the bush right alongside it. It drops a sword it was holding. #narrator #targets:fanaticfrog1

"Whoops. Sorry about jumping out with a sword. Didn't think... how that might look." #speaker:A Frog?

"You want a mushroom too? They make you feel reeeeeeeeeally nice." #speaker:A Frog?

"I think those are poisonous for humans.{g_player_gender =="hat": Having to look for a new host would be troublesome.}" #speaker:You

"Don't be boring, take one!" #speaker:A Frog?

"What are you anyway?" #speaker:You

"My name is Rudibert, if that's what you mean." #speaker:A Frog?

"I meant the whole thing about being a huge purple frog." #speaker:You

"Oh that. I see. Hmm." #speaker:Rudibert

"Want a mushroom?" #speaker:Rudibert

This isn't going anywhere. You'll have to do something about this. #narrator

-> FROG_CARD_OPTIONS

=== FROG_CARD_OPTIONS ===

////// SELF OPTIONS START //////

 + The apple crunches pleasantly as you bite into it. #card #@self #!apple
    {
        - g_player_health < g_player_max_health:
        ~ g_player_health++
    }
    {
        - frog_hostile:
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        Rudibert is eyeing you as you eat. Is he... jealous? #narrator
        -> FROG_CARD_OPTIONS
    }
 + While this <i>would</i> be a good opportunity to burn yourself to a crisp, you feel like you shouldn't mess around this time. #card #@self #!bonfire #refundcard:bonfire
    -> FROG_CARD_OPTIONS
 + You talk to yourself for a few moments, making observations on the fungus present in the cave. #card #@self #!banter
    {
        - frog_hostile:
        Despite having seemed mad at you just a few moments ago, Rudibert quickly gets too immersed in your speech to react. #narrate
        When you stop, he seems to realize that he should perhaps have taken the opportunity to attack you. Too late now! #narrator
        - else:
        Rudibert seems very interested in your little narration, listening intently. #narrator
    }
    -> FROG_CARD_OPTIONS
 + You ponder your lockpick for a moment. Good to have one of these, you think. #card #@self #!lockpick #refundcard:lockpick
    ->FROG_CARD_OPTIONS
 + Oh, your trusty staff. So dependable, yet you don't think using it at yourself would do anything. #card #@self #!staff #refundcard:staff
    -> FROG_CARD_OPTIONS
 + You consider the dagger in your hand, then you give it a few spins. Finally you flip it in the air and catch it. Nice. #card #@self #!dagger
    {
        - frog_hostile:
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        Rudibert looks on in awe, his eyes struggling to follow the movements. He eventually manages to shake off his amazement. #narrator
    }
    -> FROG_CARD_OPTIONS
 + You feel the allure of your cast iron friend calling. Oh how wonderful it could be to feel it crack your skull open... #card #@self #!frying-pan
    Maybe you should... #narrator
    {
        - considered_frying_bonk:
        ...do it. After all, you deserve some quality time with your favorite cooking instrument. #narrator
        You brush your finger over its textured surface, before firmly gripping the handle. #narrator
        \*CRACK*
        ~ g_player_health--
        Ohh yes. Ohhhh yesss... Blood starts streaming from your {g_player_gender == "hat": host body's} nose. Perrrrfect. #narrator
        The pain is dull and hot, and your {g_player_gender == "hat":control over your host body feels a bit weaker|head starts spinning}, causing you to sway slightly. #narrator
        - else:
        ...not. Maybe you should not. As tempting as it is, you might need this for fighting. Or hell, maybe even cooking! #narrator #refundcard:frying-pan
        ~ considered_frying_bonk = true
        {
            - frog_hostile:
            -> FROG_OPPORTUNITY_ATTACK
        }
    }
//glass-bottle,  
//insult,  
//rope,  
//punch,  
//static-shock,  
//evil-bottle,  
//can-of-beans,  
//snowball,  
//smokescreen,  
//run,  
//stick,  
//deceive,  
//soothsay,
    
////// FROG TARGET OPTIONS START //////

//apple, 
//bonfire,  
//banter,  
//lockpick,  
//staff,  
//dagger,  
//frying-pan,  
//glass-bottle,  

 + You ready yourself for a moment, then hurl a punch directly at Rudibert! #card #@frog #!punch
    ~frog_health--
    {frog_health == 0: He collapses to the ground, motionless. -> FROG_EVENT_EPILOGUE("won_fight")|He shakes his head slightly, seemingly taking some damage. -> FROG_COUNTER_ATTACK}
 + Your fingers spark with electricity as you ready a static shock. #card #@frog #!static-shock
    You inch closer to Rudibert, and unleash a devastatic static shock directly on his snout!
    ~frog_health--
    {frog_health == 0: He collapses to the ground, motionless. -> FROG_EVENT_EPILOGUE("won_fight")|He flinches back violently, clearly surprised by your devious attack.-> FROG_COUNTER_ATTACK}
 + "Your hat doesn't suit you and you smell weird!" #card #speaker:You #@frog #!insult
    A single tear forms below Rudibert's eye. #narrator
    "But... it's my favorite hat..." #speaker:Rudibert #setsprite:fanaticfrog1>fanaticfrog1_crying
    He seems genuinely heartbroken. Inconsolable. Beyond any comforting. #narrator
    You monster. #narrator
    He starts hopping away with tears streaming from his eyes! #narrator
    "Rudibert I-", you start, but he just hops away faster. #speaker:You #hidetargets:fanaticfrog1
    Your gut wrenches. What have you done? #narrator
    -> FROG_EVENT_EPILOGUE("frog_fled")
 + You swing the rope in your hands a few times, then skillfully fling it around Rudibert! #card #@frog #!rope
    "Ha! Caught you!" #speaker:You
    He starts to slowly try to untangle himself, but you wrap the rest of the rope around him before he can get free. #narrator #setsprite:fanaticfrog1>fanaticfrog1_roped
    "Okay okay, I give up. Now what?" #speaker:Rudibert
    "Now uh. You're going to sit here and uh... I didn't really think things this far. I'll just go." #speaker:You
    "Eh. Sounds fair enough I guess. Could you toss me that mushroom on your way out?" #speaker:Rudibert
    Far be it for you to get between a frog and his mushroom-habit. #narrator
    You knock the mushroom towards him as you start to leave. #narrator
    "Hm. I don't think I can actually eat it with my hands tied. Could you..." His voice fades as you walk away. #narrator #hidetargets:fanaticfrog1
    
//evil-bottle,  
//can-of-beans,  
//snowball,  
//smokescreen,  
//run,  
//stick,  
//deceive,  
//soothsay,
 
 -> DONE
 
 === FROG_COUNTER_ATTACK ===
 ~ frog_hostile = true
"Ow! Jeez! What was that for!?" #speaker:Rudibert
His expression shifts to anger, and he seems to be planning something. #narrator
You have no time to predict his actions before he launches his tongue directly at you! #narrator
It hurts worse than some punches you've taken, and you cough up some {g_player_gender == "hat": lint. | blood.} #narrator
~ g_player_health--
-> FROG_CARD_OPTIONS
 
 === FROG_OPPORTUNITY_ATTACK ===
Rudibert senses an opportunity to strike, and takes his chance! #narrator #setsprite:fanaticfrog1>fanaticfrog1_attackmode
He leaps forward, sword in hand, and strikes {g_player_gender == "hat": your pointy end|you across the chest}! #narrator
~ g_player_health--
It hurts, bad, and some {g_player_gender == "hat": lint drops to the ground. | blood drips onto the ground.} #narrator
-> FROG_CARD_OPTIONS
 
 === FROG_EVENT_EPILOGUE(exit_method) ===
The event continues
-> DONE

//Rewards: longsword, praise, hypnotize