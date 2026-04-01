INCLUDE global_vars.ink

VAR frog_health = 2
VAR frog_hostile = false
VAR considered_frying_bonk = false
VAR considered_evil_bottle = false

While travelling, come across a small cave. You could swear you heard some strange noises too, but they seem to have stopped. #narrator

"Hello? Anybody in here?" #speaker:You

...#narrator

"Ribbit." #speaker:Suspicious Bush

A frog? But something's off. Frogs ribbit, yes, but they don't <i>say</i> "ribbit". Is this thing pretending to be a frog? #narrator

"Come out! I know you're not a frog!" #speaker:You

... #speaker:Suspicious Bush

"How did you figure out I'm in this bush?" #speaker:Suspicious Bush

What a croaky voice. #narrator

"Saying "ribbit" kind of gave you away." #speaker:You

"Fair play, I suppose. I'll hop out." #speaker:Suspicious Bush

It does indeed hop out, launching some mushrooms out of the bush right alongside it. It drops a sword it was holding. #narrator #showtargets:fanaticfrog1

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
        ~ considered_frying_bonk = false
        Ohh yes. Ohhhh yesss... Blood starts streaming from your {g_player_gender == "hat": host body's} nose. Perrrrfect. #narrator
        The pain is dull and hot, and your {g_player_gender == "hat":control over your host body feels a bit weaker|head starts spinning}, causing you to sway slightly. #narrator
        "Wow. You're out of your mind." #speaker:Rudibert
        He looks shellshocked. Like he just saw someone crack {player_subject_pronoun()} skull open with a cast-iron pan. #narrator
        Wonder what that's about. #narrator
        -> FROG_CARD_OPTIONS
        - else:
        ...not. Maybe you should not. As tempting as it is, you might need this for fighting. Or hell, maybe even cooking! #narrator #refundcard:frying-pan
        ~ considered_frying_bonk = true
        {
            - frog_hostile:
            -> FROG_OPPORTUNITY_ATTACK
        }
        -> FROG_CARD_OPTIONS
    }
 + You quickly swig down your healing potion, not stopping to think if you need it or not. #card #@self #!glass-bottle #function:PlayCardSfx(glass-bottle)
    {
        - g_player_health < g_player_max_health - 1:
        ~ g_player_health += 2
        You feel its healing effects kick in, restoring you considerably. #narrator
        - g_player_health < g_player_max_health:
        ~ g_player_health++
        You feel its healing effects kick in, restoring you slightly. #narrator
        - else:
        And you don't. At least the potion itself is somewhat sweet. #narrator
    }
    {
        - frog_hostile:
        Thanks to your speed, Rudibert doesn't have an opportunity to strike you! #narrator
        - else:
        As you finish your potion, Rudibert gives you an approving look. #narrator
        "Ah, {frog_gender_word()} prefers the drink instead. Fine choice. #speaker:Rudibert
    }
    -> FROG_CARD_OPTIONS
 + "Why did I ever think I could be a wizard... I don't even have a proper spellbook anymore... I'm just such a loser..." #card #@self #!insult #speaker:You
    Rudibert looks on as you're wallowing in self-loathing, seemingly considering what to do. #narrator
    {
        - frog_hostile:
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        He shifts awkwardly. Seems like he isn't great with comforting people. #narrator
        -> FROG_CARD_OPTIONS
    }
 + You give the rope some thought. Maybe not the time or place to be tying yourself up. #card #@self #!rope #refundcard:rope
    -> FROG_CARD_OPTIONS
 + You examine your fists for a moment. Punching yourself isn't really your style. #card #@self #!punch #refundcard:punch
    -> FROG_CARD_OPTIONS
 + Try as you might, giving yourself a static shock proves to be impossible. Darn it. #card #@self #!static-shock #refundcard:static_shock
    -> FROG_CARD_OPTIONS
 + You peer into the bottle of poison. The liquid inside swirls menacingly. You... shouldn't drink this, right? #card #@self #!evil-bottle
    {
        - considered_evil_bottle:
        Well... maybe just a sip. #narrator
        ~ considered_evil_bottle = false
        It tastes like bittersweet metal. Mostly bitter. And it <i>burns</i>. #narrator
        ~ g_player_health--
        You try to tough it out, but your face contorts, somewhat unsettlingly. #narrator
        {
            - frog_hostile:
            -> FROG_OPPORTUNITY_ATTACK
            - else:
            "What's that stuff? Seems gnarly, if your face is anything to go by!" #speaker:Rudibert
            You only manage a few weak coughs in response. #narrator
            This only seems to pique his curiosity further, and he slowly approaches you. #narrator
            "You mind if I have a taste?" #speaker:Rudibert
            Try as you might, you can't get the words out, and he seems to take this as a sign of approval. #narrator
            He takes the poison from your weak grip and downs the rest of the bottle! #narrator
            "Whuh, what's in this stuff?" he manages to say between coughs. #speaker:Rudibert
            He stumbles backwards, clutching his stomach. And then he keels over. #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
            Victory? You suppose? Definitely what you intended to happen! #narrator
            -> FROG_EVENT_EPILOGUE("won_fight")
        }
        - else:
        Right. You brewed this thing yourself, and you know it would burn the whole way down, destroying tissue as it goes. #narrator
        Not really your <i>poison</i>. Heh. #narrator
        ~ considered_evil_bottle = true
        -> FROG_CARD_OPTIONS
    }
 + You take out your can of soup. Time for a little meal! #card #@self #!can-of-beans
    {
        - frog_hostile:
        Rudibert is stunned at your audacity for a few seconds, then he regains his composure. #narrator
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "Want some?" #speaker:You
        "Well I... I suppose? Could we maybe add some mus-" #speaker:Rudibert
        "No." #speaker:You
        "Ah. Well fair's fair I suppose." #speaker:Rudibert
        ...
        You and Rudibert eventually finish the soup. No firewood nearby meant that you had to have it cold, but Rudibert didn't seem to mind. #narrator
        {
            - g_player_health < g_player_max_health:
            ~ g_player_health++
        }
        "That was pretty good!" #speaker:Rudibert
        What to do now, you ponder. #narrator
        -> FROG_CARD_OPTIONS
    }
 + You form a snowball in your hands. And then... Hmm. You decide to give it a bite. #card #@self #!snowball
    Eh. Tastes like nothing. Kind of cold. #narrator
    {
        - frog_hostile:
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        Even Rudibert seems disinterested in the snowball. #narrator
        -> FROG_CARD_OPTIONS
    }
 + You decide that it's best to make a quick exit, blasting smoke all around yourself! #card #@self #!smokescreen
    -> FROG_EVENT_EPILOGUE("player_left")
 + You decide to leg it! Rudibert clearly wasn't expecting you to run, so he's left confused as you head towards the cave entrance. #card #@self #@frog #!run
    -> FROG_EVENT_EPILOGUE("player_left")
 + You consider the stick in your hands. What a cool shape... #card #@self #!stick #refundcard:stick
    -> FROG_CARD_OPTIONS
 + You think for a moment, pondering how you could deceive yourself... #card #@self #!deceive
    Try as you might, you can't come up with anything helpful. #narrator #refundcard:deceive
    -> FROG_CARD_OPTIONS
 + You recite some healing words, and wait for the effect to kick in. #card #@self #!soothsay
    {
        - g_player_health < g_player_max_health - 1:
        Wisps of healing energy coil around you, closing your wounds as they make their way from bottom to top. #narrator
        ~ g_player_health += 2
        - g_player_health < g_player_max_health:
        Wisps of healing energy coil around you, closing your wounds as they make their way from bottom to top. #narrator
        ~ g_player_health++
        - else:
        Wisps of healing energy coil around you, but they can't find anything to heal. #narrator
    }
    {
        - frog_hostile:
        Though you're supposed to be fighting, Rudibert seems too mesmerized by the healing wisps to exploit the opportunity! #narrator
        He quickly shakes off his amazement as the spell's visual effects wane. #narrator
        - else:
        Rudibert seems thoroughly mesmerized by the healing wisps, his gaze following the energy as it moves. #narrator
        "Whoa... That's so pretty. Super impressive, {frog_gender_word()}! #speaker:Rudibert
    }
    -> FROG_CARD_OPTIONS
 + You consider the spear in your hand. You can't really think of a way to use it on yourself. #card #@self #!spear #refundcard:spear
    -> FROG_CARD_OPTIONS
 + You know better than to try to use telekinesis on yourself. You've heard the horror-stories after all. #card #@self #!telekinesis #refundcard:telekinesis
    ->FROG_CARD_OPTIONS
    
////// FROG TARGET OPTIONS START //////

 + You offer an apple to Rudibert.{frog_hostile: Perhaps this will smooth things over?} #card #@frog #!apple
    {
        - frog_hostile:
        He seems suspicious of your offer, and swats it away! #narrator
        "I'm not falling for your tricks anymore!" #speaker:Rudibert
        At least he didn't cut you up with the sword! #narrator
        - else:
        He seems hesitant for a moment, but he does accept your offer! #narrator
        "I suppose an apple couldn't hurt?" #speaker:Rudibert
        He tosses the apple in his mouth and swallows it whole. #narrator
        "Didn't taste like much honestly" #speaker:Rudibert
        Well maybe if he <i>chewed his food</i>... #narrator
    }
    -> FROG_CARD_OPTIONS
 + You decide to roast {g_bonfire_marshmallows_seen:some marshmallows with} Rudibert. {g_bonfire_marshmallows_seen:Who doesn't love marshmallows after all?|He's thoroughly ticked you off.} #card #@frog #!bonfire
    {
        - g_bonfire_marshmallows_seen:
        "You wanna have some marshmallows? They're super delicious!" #speaker:You
        A roaring bonfire appears before you as you cast the spell. #narrator
        "Marshmallows?" #speaker:Rudibert
        ... #narrator
        You eventually finish the marshmallows with Rudibert. Though you did have to explain "marshmallows" to him first. #narrator
        "Those were... Really, really delicious. Almost as good as my mushrooms!" #speaker:Rudibert
        You've successfully taught this frog the wonders of marshmallows! A roaring success! #narrator
        "That took a while though. I should probably hop home." #speaker:Rudibert
        You may or may not have thought that he lives in the cave. #narrator
        "Sounds good. I should keep going as well." #speaker:You
        That's right. Play it cool. #narrator
        Good bye, {frog_gender_word()}. 'Twas a pleasure meeting you! #speaker:Rudibert
        He hops off after that, leaving you by yourself.
        -> FROG_EVENT_EPILOGUE("frog_left")
        - else:
        You cast the spell, and a bonfire appears before Rudibert! He flinches for a second, before recovering. #narrator
        Wait, it was supposed to appear <i>on</i> him! What's going on? #narrator
        You take out some marshmallows to ease his mind, hoping to prove that you didn't plan to roast him. #narrator
        Wait. You <i>did</i> plan to roast him! What the hell? #narrator
        "You wanna have some marshmallows? They're super delicious!" #speaker:You
        You'll have to figure this out later, it seems. #narrator
        "Marshmallows?" #speaker:Rudibert
        ... #narrator
        You eventually finish the marshmallows with Rudibert. Though you did have to explain "marshmallows" to him first. #narrator
        "Those were... Really, really delicious. Almost as good as my mushrooms!" #speaker:Rudibert
        You've successfully taught this frog the wonders of marshmallows! A roaring... success? #narrator
        "That took a while though. I should probably hop home." #speaker:Rudibert
        You may or may not have thought that he lives in the cave. #narrator
        "Sounds good. I should keep going as well." #speaker:You
        That's right. Play it cool. #narrator
        Good bye, {frog_gender_word()}. 'Twas a pleasure meeting you! #speaker:Rudibert
        He hops off after that, leaving you wondering what went wrong. Or right? You can't tell anymore.
        -> FROG_EVENT_EPILOGUE("frog_left")
    }
 + "Nice microclimate in this cave, huh?" #card #@frog #!banter #speaker:You
    {
        - frog_hostile:
        He doesn't seem to approve of small-talk in the middle of a fight. #narrator
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "Very perceptive of you! That's one of the reasons I like this place so much!" #speaker:Rudibert
        He seems genuinely happy that you brought up the subject. Nice one! #narrator
        -> FROG_CARD_OPTIONS
    }
 + You offer a lockpick to Rudibert. #card #@frog #!lockpick
    "What... am I meant to do with this? Thanks I guess?" #speaker:Rudibert
    Nice. #narrator
    -> FROG_CARD_OPTIONS
 + Time to show this frog the might of your trusty staff! #card #@frog #!staff
    You swing at Rudibert, landing a blow directly on his forehead! #narrator
    ~frog_health--
    {
        - frog_health == 0:
        He collapses to the ground, motionless. You've won this fight! #narrator
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        He shakes his head as he stumbles backwards. Seems your strike did some damage! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + You give your dagger a few courtesy spins, and lunge at Rudibert! #card #@frog #!dagger
    You land some solid strikes, each leaving a painful-looking wound! #narrator
    ~frog_health--
    {
        - frog_health == 0:
        He seems to succumb to his wounds, and collapses to the ground. You've won. #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        He squeaks loudly, hopping backwards as he does. Seems you've dealt him a serious blow! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + You heft your dear cast-iron friend in your hand, and prepare to attack! #card #@frog #!frying-pan
    You lunge at Rudibert, and land a solid blow directly on the top of his head! #narrator
    ~ frog_health = 0
    He collapses almost instantly, being knocked out by the force of your attack! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
    You've won, all thanks to your trusty frying pan! You give it a few pats to show appreciation. #narrator
    -> FROG_EVENT_EPILOGUE("won_fight")
 + You take out a healing potion and offer it to Rudibert. {frog_hostile:Hopefully this smooths things over.} #card #@frog #!glass-bottle
    {
        - frog_hostile:
        "Hah, I'm not falling for such an obvious trick!" #speaker:Rudibert
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "What's this stuff? A healing potion?" #speaker:Rudibert
        You nod. #narrator
        "I'm not really injured right now, but thanks, I guess?" #speaker:Rudibert
        Now what?
        -> FROG_CARD_OPTIONS
    }

 + You ready yourself for a moment, then hurl a punch directly at Rudibert! #card #@frog #!punch
    ~frog_health--
    {
        - frog_health == 0:
        He collapses to the ground, motionless. Seems you take the win! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        He shakes his head slightly, seemingly taking some damage! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + Your fingers spark with electricity as you ready a static shock. #card #@frog #!static-shock
    You inch closer to Rudibert, and unleash a devastating static shock directly on his snout!
    ~frog_health--
    {
        - frog_health == 0:
        He collapses to the ground, motionless. Seems you've won! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        He flinches back violently, clearly surprised by your devious attack! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + "Your hat doesn't suit you and you smell weird!" #card #speaker:You #@frog #!insult
    A single tear forms below Rudibert's eye. #narrator
    "But... it's my favorite hat..." #speaker:Rudibert #setsprite:fanaticfrog1>fanaticfrog1_crying
    He seems genuinely heartbroken. Inconsolable. Beyond any comforting. #narrator
    You monster. #narrator
    He starts hopping away with tears streaming from his eyes! #narrator
    "Rudibert I-", you start, but he just hops away faster. #speaker:You #hidetargets:fanaticfrog1
    Your gut wrenches. What have you done? #narrator
    -> FROG_EVENT_EPILOGUE("frog_left")
 + You swing the rope in your hands a few times, then skillfully fling it around Rudibert! #card #@frog #!rope
    "Ha! Caught you!" #speaker:You
    He starts to slowly try to untangle himself, but you wrap the rest of the rope around him before he can get free. #narrator #setsprite:fanaticfrog1>fanaticfrog1_roped
    "Okay okay, I give up. Now what?" #speaker:Rudibert
    "Now uh. You're going to sit here and uh... I didn't really think things this far. I'll just go." #speaker:You
    "Eh. Sounds fair enough I guess. Could you toss me that mushroom on your way out?" #speaker:Rudibert
    Far be it for you to get between a frog and his mushroom-habit. #narrator
    You knock the mushroom towards him as you start to leave. #narrator
    "Hm. I don't think I can actually eat it with my hands tied. Could you..." His voice fades as you walk away. #narrator #hidetargets:fanaticfrog1
    -> FROG_EVENT_EPILOGUE("player_left")
 + A devious scheme brews in your mind, and you offer the bottle of poison to Rudibert. #card #@frog #!evil-bottle
    {
        - frog_hostile:
        "Nice try, but I'm not about to fall for such a cheap trick!" #speaker:Rudibert
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "What's this stuff? Seems super gnarly by the looks of the bottle!" #speaker:Rudibert
        "Oh it is! Way better than the mushrooms in here too. Gets you totally gone!" #speaker:You
        "Sounds totally crazy... Maybe I'll give it a shot." #speaker:Rudibert
        He takes the offered bottle, and downs the whole thing! #narrator
        "Whuh, what's in this stuff?" he manages to say between coughs. #speaker:Rudibert
        He stumbles backwards, clutching his stomach. And then he keels over. #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        Victory. All according to plan. #narrator
        -> FROG_EVENT_EPILOGUE("won_fight")
    }
 + You consider your can of soup. {frog_hostile:Seeing as you're in a fight, you could use it for self-defense.|Perhaps Rudibert would like some soup for a change?} #card #@frog #!can-of-beans
    {
        - frog_hostile:
        You place the can in your bag, and quickly swing it at Rudibert, landing a blow directly on his forehead! #narrator
        ~ frog_health--
        {
            - frog_health == 0:
            He collapses to the ground, motionless. Seems you take the win! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
            -> FROG_EVENT_EPILOGUE("won_fight")
            - else:
            He shakes his head slightly, seemingly taking some damage! #narrator
            -> FROG_COUNTER_ATTACK
        }
        - else:
        "Want some soup?" #speaker:You
        "Huh, why not I suppose." #speaker:Rudibert
        He grabs the can and stows it under his hat. Good deed done? #narrator
        -> FROG_CARD_OPTIONS
    }
 + You form a snowball in your hands and quickly fling it at Rudibert! #card #@frog #!snowball
    It lands on his stomach, sending snow flying! #narrator
    "Hey, that was cold! Rude!" #speaker:Rudibert
    He scowls at you for a moment before relaxing. Seems he didn't consider that to be aggressive, at least. #narrator
    -> FROG_CARD_OPTIONS
 + A cloud of smoke puffs around Rudibert, and you bolt for the cave entrance! #card #@frog #!smokescreen
    He's left in the dust, seemingly having lost his sense of direction in the cloud! Nice! #narrator
    -> FROG_EVENT_EPILOGUE("player_left")
 + Time to show Rudibert the might of this cool stick! #card #@frog #!stick
    You swing the stick at Rudibert's head, and land a blow! #narrator
    ~frog_health--
    {
        - frog_health == 0:
        He collapses to the ground, motionless. Seems you've won! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        He flinches back violently, probably surprised that such a cool stick is capable of such violence! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + "Hey did you know that there's like a whole forest of those mushrooms a short walk south of the cave?" #card #@frog #!deceive #speaker:You
    {
        - frog_hostile:
        "Hah, sounds too good to be true!" #speaker:Rudibert
        He says that, but his pupils got huge for a second there. How easy. #narrator
        "I'm being serious! I know we're fighting and all, but I wouldn't lie about this!" #speaker:You
        He relaxes for a moment. Suspicion slowly fades from his face, being replaced with excitement. One last push. #narrator
        "You're seriously not lying?" #speaker:Rudibert
        "I'm not, there were like at least a hundred of them too!" #speaker:You
        "Whoa... A hundred... Maybe... Maybe I'll go check it out after all..." #speaker:Rudibert
        "Thanks for the heads-up, {frog_gender_word()}. If there's really mushrooms there I might even forgive you!" #speaker:Rudibert
        - else:
        "Wait, really? Like really really?" #speaker:Rudibert
        "Uh huh, saw it myself on the way here!" #speaker:You
        "Whoa... I think I'll go check it out! I could use a few more for my stash." #speaker:Rudibert
        "Thanks for the heads-up, {frog_gender_word()}! See you around!" #speaker:Rudibert
    }
    He starts hopping away. Hook, line, and sinker. Nice. #narrator #hidetargets:fanaticfrog1
    -> FROG_EVENT_EPILOGUE("frog_left")
 + You recite some healing words, and wisps of energy start circling around Rudibert. #card #@frog #!soothsay
    {
        - frog_hostile:
        Rudibert braces for a moment, and keeps bracing, and... he's very surprised that the spell isn't hurting him.
        "Whoa, what's this? It's not hurting me?" #speaker:Rudibert
        "No, it's a healing spell. Sorry about attacking you." #speaker:You
        His wounds eventually finish closing up, and he seems to relax. #narrator #setsprite:fanaticfrog1>fanaticfrog1_default
        "Hm. Maybe you're not a bad person after all. Still, I'm gonna hop home now. I don't trust you just yet." #speaker:Rudibert
        He does as he says, and hops out of the cave. Hopefully you can mend this relationship some day. #narrator
        -> FROG_EVENT_EPILOGUE("frog_left")
        - else:
        "Whoa... that's really pretty..." #speaker:Rudibert
        He's mesmerized by the healing wisps encircling him. Though there doesn't seem to be anything to heal. #narrator
        "Pretty spell, {frog_gender_word()}!" #speaker:Rudibert
        Well at least he liked it. #narrator
        -> FROG_CARD_OPTIONS
    }
 + The spear pops into your hand, and you start dashing at Rudibert, determined to sink it into him! #card #@frog #!spear
    ~ frog_health--
    {
        - frog_health == 0:
        Rudibert collapses to the ground, motionless. Seems you've won! #narrator #setsprite:fanaticfrog1>fanaticfrog1_defeated
        -> FROG_EVENT_EPILOGUE("won_fight")
        - else:
        Rudibert flinches back violently, taking a few hops backwards as he does! #narrator
        -> FROG_COUNTER_ATTACK
    }
 + <b>"RRRRRRRHHHHAAAAA!!!"</b> #card #@self #@frog #!yell
    Rudibert flinches backwards, probably surprised that you started yelling out of nowhere. #narrator
    {
        - frog_hostile:
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "What's wrong with you?! Is everything okay?" #speaker:Rudibert
        Seems you've thoroughly confused him. #narrator
        "Yeah, perfectly fine! Just had to let that out." #speaker:You
        "Riiight. Well. Warn me next time, please?" #speaker:You
        "Sure thing!" #speaker:You
        What to do now... #narrator
        -> FROG_CARD_OPTIONS
    }
 + You cast the spell, targeting Rudibert's hat. This'll show him! #card #@frog #!telekinesis
    As the hat starts lifting off of his head, he quickly grabs hold of it! Seems like he's going for a ride then too! #narrator
    ...Or he would be, if he was lighter. At this load, your mind quickly buckles, as you're unable to lift such a heavy load! #narrator
    {
        - frog_hostile:
        "You fiend! Attacking me is one thing, but targeting my precious hat is unforgivable!" #speaker:Rudibert
        -> FROG_OPPORTUNITY_ATTACK
        - else:
        "You're lucky that failed, otherwise I'd have had to defend the honor of my precious hat and slay you!" #speaker:Rudibert
        "Now no more tricks, or I'll have to do something I'll regret!" #speaker:Rudibert
        Yeesh, way to take things too seriously! #narrator
        -> FROG_CARD_OPTIONS
    }
 
 === FROG_COUNTER_ATTACK ===
 ~ frog_hostile = true
"Ow! Jeez! What was that for!?" #speaker:Rudibert
His expression shifts to anger, and he seems to be planning something. #narrator #setsprite:fanaticfrog1>fanaticfrog1_attackmode
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

=== function frog_gender_word ===
{
    - g_player_gender == "female":
    ~ return "miss"
    - g_player_gender == "male":
    ~ return "sir"
    - g_player_gender == "non-binary":
    ~ return "your grace"
    - g_player_gender == "hat":
    ~ return "your hatness"
}
 
 === FROG_EVENT_EPILOGUE(exit_method) ===

{
    - exit_method == "player_left":
    As you're making your way out of the cave, you come across some cards! #narrator
    Since you're in a hurry, you quickly pick up what you can and keep going. #narrator #function:RewardCards(1, longsword, praise, hypnotize)
    - exit_method == "frog_left":
    You've solved the mystery of the cave, so you figure you're done here. Time to keep going. #narrator
    As you're making your leave, you spot some cards on the ground! #narrator
    You pick out the intact ones, not bothering with the rest. #narrator #function:RewardCards(1, longsword, praise, hypnotize)
    - exit_method == "won_fight":
    After you're done taking in your victory, you give the cave a quick once-over before leaving. #narrator
    Good thing you did, as you spot a few cards on the ground near Rudibert! #narrator
    You pick out the intact ones, not bothering with the rest. #narrator #function:RewardCards(1, longsword, praise, hypnotize)
}
You eventually make your way out of the cave, and need to take a moment to let your eyes adjust to the light. #narrator
Now then, time to keep adventuring! #narrator

#function:LoadScene(Map1)

-> DONE