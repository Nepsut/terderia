INCLUDE global_vars.ink

VAR considered_frying_bonk = false
VAR considered_evil_bottle = false


The path you've been walking along seems to have taken you to a swamp. #narrator

Now though, you're a bit unsure of how to proceed, as there's a suspicious lump in the puddle ahead of you. #narrator

All sorts of wild theories start running in your head - many dangerous beasts call swamps their home. #narrator

It could be a giant frog, a troll, or even something like a young hydra! #narrator

As you stand there, lost in your thought trying to figure out what it is, the lump rises from the puddle! #narrator #showtargets:kappa1 #setsprite:kappa1>kappa1_default

"You alright, {kappa_gender_word()}? This isn't the best place to just stand around you know." #speaker:A Swamp Creature?

You jump a little as you're pulled back to reality. The creature spoke to you? #narrator

"I uh. I'm alright, I think? Unless you're about to change that." #speaker:You

"No no, I'm not the type to bully {g_player_gender == "hat": little hats|humans} who happen by." #speaker:A Swamp Creature?

Phew. This... creature? Seems friendly at least. Although... #narrator

{
    - g_kappa_seen:
    Right, it's a kappa. You recall having seen one of these before, maybe even this one! #narrator
    "Say, have we met before?" #speaker:You
    "Mmm, not that I remember at least. Might have been the previous occupant of this swamp?" #speaker:The Kappa
    "We do winter in the mountains, and settling back down is a kind of 'first-come-first-serve' thing" #speaker:The Kappa
    "Anyhow, since ye've met our kind before, ya know how this goes!" #speaker:The Kappa
    Right... #narrator
    - else:
    "So uh. Might be a rude question but... what exactly are you? I can't recall having seen one of your kind before." #speaker:You
    "I'm a kappa!" #speaker:A Kappa
    A kappa, huh. You wait a few moments for it to elaborate, yet it never does. Alright then. #narrator
    "Now, since yer here..." #speaker:The Kappa
}

"Ya got any goodies for me? I'll return the favor if ya do!" #speaker:The Kappa

You wonder what it considers "goodies", and if there might be any other way to gain its favor... #narrator

-> KAPPA_CARD_OPTIONS

=== KAPPA_CARD_OPTIONS ===

////// SELF OPTIONS START //////
 + Just for a moment you consider that this may come off as teasing, but you eat your apple in front of the kappa. #card #@self #!apple
    The kappa stares you as you munch on the apple, its tongue plopping out ever-so-slightly. It's drooling. #narrator
    You finish the apple after a few moments, and the kappa seems to shake off its food-induced trance. #narrator
    {
        - try_heal_player(1) == -1:
        You were already feeling pretty healthy, so the apple didn't heal you this time. #narrator
        - else:
        You feel the apple heal you slightly. Nice! #narrator
    }
    The kappa looks like it's almost pouting slightly. You almost feel a little bad now. #narrator
    -> KAPPA_CARD_OPTIONS
 + You consider casting a bonfire on yourself, but the environment seems too moist. #card #@self #!bonfire
    You give up this time, determined to light yourself on fire once you have a better opportunity. #narrator #refundcard:bonfire
    -> KAPPA_CARD_OPTIONS
 + You talk to yourself for a moment, verbally pointing out details of the surrounding swamp. #card #@self #!banter
    The kappa seems very proud of itself. Apparently your rambling sounded like compliments to it? #narrator
    -> KAPPA_CARD_OPTIONS
 + You consider your lockpick for a moment. Surely some day you'll get an opportunity to use it? #card #@self #!lockpick #refundcard:lockpick
    -> KAPPA_CARD_OPTIONS
 + The staff feels comforting in your hand. Perhaps you should just start walking with it in hand? #card #@self #!staff
    Something seems to disagree, as the staff returns to being a card due to not being used. Damn. #narrator #refundcard:staff
    -> KAPPA_CARD_OPTIONS
 + You give your dagger a twirl. Sadly there's no desk around, so instead of five-finger-fillet, you just spin it around a little. #card #@self #!dagger
    The kappa looks highly uninterested in your antics, even as you flick the dagger in the air and catch it. #narrator
    Tough crowd. #narrator
    -> KAPPA_CARD_OPTIONS
 + You spin the frying pan in your hand once, then twice. What if you..? #card #@self #!frying-pan
    {
        - considered_frying_bonk:
        You've thought this through, and you're going to do it. #narrator
        You give it one more courtesy spin, and ready yourself. #narrator
        \*WHAM*
        You feel dizzy, and {g_player_gender == "hat":lint flies|blood drips} onto the ground below. Whooow... #narrator
        There's just no feeling quite like this... #narrator
        ~ considered_frying_bonk = false
        - else:
        No, not now. No matter how nice it would feel to strike its cool cast-iron form against your skull, you can't. #narrator #refundcard:frying-pan
        ~ considered_frying_bonk = true
    }
    -> KAPPA_CARD_OPTIONS
 + You quickly chug the potion and wait for the effects to kick in. #card #@self #!glass-bottle
    {
        - g_player_health < g_player_max_health - 1:
        You don't have to wait long, and you quickly regain a good amount of health. #narrator
        ~ g_player_health += 2
        - g_player_health == g_player_max_health:
        You wait for a moment before figuring that you're already in perfect health. Eh, whatever. #narrator
        - else:
        You don't have to wait long, and you quickly regain some health. #narrator
        ~ g_player_health++
    }
    The kappa stares at you blankly. You wonder what it's thinking. #narrator
    -> KAPPA_CARD_OPTIONS
 + "I'm just such a failure, I can't even call myself a wizard without a proper spellbook anymore..." #card #@self #!insult #speaker:You
    Disinterest written all over the kappa's face as it stares at you. #narrator
    ...At least you think it's staring at you. Hard to say when you can't see its eyes. #narrator
    A few moments pass, and the awkwardness builds. You cough to break the silence. #narrator
    "Right, where was I?" #speaker:You
    -> KAPPA_CARD_OPTIONS
 + You consider tying yourself up for a moment, but that'd hardly do you any good right now. #card #@self #!rope
    The kappa seems interested in the rope. Could this rope perhaps pass for a "goodie"? #narrator #refundcard:rope
    -> KAPPA_CARD_OPTIONS
 + Punching yourself just feels so... Uninnovative. So basic. So you decide to pass. #card #@self #!punch #refundcard:punch
    -> KAPPA_CARD_OPTIONS
 + Though a wet environment swamp seems like a good place to solve problems with electricity, you're unable to shock yourself. #card #@self #!static-shock
    The kappa seems mildly amused at your attempts, at least. #refundcard:static-shock
    -> KAPPA_CARD_OPTIONS
 + You consider the evil bottle in your hand. Not exactly suitable for drinking. #card #@self #!evil-bottle
    Not that something being a stupid idea has stopped you before. Maybe..? #narrator
    {
        - g_player_gender == "hat":
        Maybe not. Host bodies are precious in this economy after all. #narrator #refundcard:evil-bottle
        - considered_evil_bottle:
        You've thought it through, and the answer is yes. Somehow. #narrator
        You take a swig from the bottle, and your guts instantly start to churn. #narrator
        You keel over, as the poison burns through your innards, the pain making you curl up. #narrator
        ~ g_player_health--
        "Uh, ya ok there {kappa_gender_word()}?" #speaker:The Kappa
        You can't speak like this, so you just groan in response. #narrator
        "I uh. I'll just give ya a moment, 'kay?" #speaker:The Kappa
        The kappa withdraws back below the surface. #narrator #setsprite:kappa1>kappa1_hiding
        Perhaps it'll keep away any threats while you recover at least. Right? #narrator
        ... #narrator
        After what feels like days, you finally feel fine enough to continue dealing with the kappa. #narrator
        "Hey uh, I think I'm good enough to continue our... whatever this is." #speaker:You
        The kappa quickly pops back up from under the water and looks at you expectantly. #narrator #setsprite:kappa1>kappa1_default
        What to do now... #narrator
        - else:
        Yeah, maybe not. This stuff would burn through your guts like a hot knife through butter. #narrator #refundcard:evil-bottle
    }
    -> KAPPA_CARD_OPTIONS
 + You think about having some soup, but before you can crack the can open, the kappa speaks up! #card @self #!can-of-beans
    "Is that fer me? Looks like a proper goodie!" #speaker:The Kappa
    ...Right. The kappa might take it as a grave offense, if you dared to eat the soup yourself. #narrator
    "Uh, I'm thinking about it!" #speaker:You #refundcard:can-of-beans
    -> KAPPA_CARD_OPTIONS
 + You manifest a snowball in your hand. But you have no idea what to do with it. So you chomp down on it. #card #@self #!snowball
    Tastes like... cold. And not much else. Eh. #narrator
    -> KAPPA_CARD_OPTIONS
 + A cloud of smoke rapidly expands around you, and you bolt away from the swamp! #card #@self #!smokescreen
    Screw all that, you're not giving the kappa anything! #narrator
    The kappa is quickly left behind, coughing as you make your exit. #narrator
    -> KAPPA_EVENT_EPILOGUE("player_left")
 + Not wanting to deal with any of this, you start running off! #card #@self #!run
    The kappa seems uninterested. Apparently leaving was a valid option all along? #narrator
    -> KAPPA_EVENT_EPILOGUE("player_left")
 + You take a look at the stick. You don't have any ideas on how to use it on yourself though. #card #@self #!stick #refundcard:stick
    -> KAPPA_CARD_OPTIONS
 + Lying to yourself seems a tad impossible, so you give up on that for now. #card #@self #!deceive
    -> KAPPA_CARD_OPTIONS
 + You recite a healing spell, and you're quickly surrounded by little wisps of energy! #card #@self #!soothsay
    {
        - g_player_health < g_player_max_health - 1:
        As the healing wisps make their way around your body, you feel the full healing effects kick in! #narrator
        ~ g_player_health += 2
        - g_player_health == g_player_max_health:
        As the healing wisps make their way around your body, you realize that you're already in perfect health. Eh, whatever. #narrator
        - else:
        As the healing wisps make their way around your body, you quickly regain some health. #narrator
        ~ g_player_health++
    }
    -> KAPPA_CARD_OPTIONS
 + You consider the longsword in your hands. You can't really think of a way to use it on yourself. #card #@self #!longsword #refundcard:longsword
    -> KAPPA_CARD_OPTIONS
 + "You know, all things considered, I'm doing pretty great! I'm capable of anything, and I'm proud of myself. #card #@self #!praise #speaker:You
    The kappa gives you a look. Hard to say what kind of a look. #narrator
    "Good fer ya, {kappa_gender_word()}! It's important to know yer worth. Now, got any goodies fer me?" #speaker:The Kappa
    Well, at least it's being <i>somewhat</i> supportive. #narrative
    -> KAPPA_CARD_OPTIONS
 + You consider hypnotizing yourself, but that could lead to trouble in this environment. #card #@self #!hypnotize #refundcard:hypnotize
    -> KAPPA_CARD_OPTIONS
 + <b>"RRRRRRRHHHHAAAAA!!!"</b> #card #@self #@kappa #!yell
    Phew. That felt really cathartic. Although the kappa seems incredibly confused by your spontaneous outburst. #narrator
    "...Is everything ok, {kappa_gender_word()}?" #speaker:The Kappa
    "Yeah, perfectly ok! Just had to let that out. Apologies." #speaker:You
    "Gotcha. Could ya warn me next time?" #speaker:The Kappa
    You reassure the kappa that you'll give it a warning before yelling out again. #narrator
    -> KAPPA_CARD_OPTIONS
 + You know better than to try to use telekinesis on yourself. You've heard the horror-stories after all. #card #@self #!telekinesis #refundcard:telekinesis
    -> KAPPA_CARD_OPTIONS
 + You take a long look at the rusty spear in your hand. You can't really think of a way to use it on yourself. #card #@self #!spear #refundcard:spear
    -> KAPPA_CARD_OPTIONS

////// TARGET OPTIONS START //////
 + You toss the kappa an apple. Can't go wrong with an apple, right? #card #@kappa #!apple
    It attempts to catch the apple, but its reflexes are far too slow, and the apple plops into the water. #narrator #setsprite:kappa1>kappa1_happy
    The kappa fetches the apple from the water, and "cleans" it against its messy... fur? #narrator #setsprite:kappa1>kappa1_default
    It tosses the entire apple in its mouth, and the crunch almost makes you jealous for a swamp apple. Almost. #narrator
    "That's a goodie alright! Ya pass!" #speaker:The Kappa #setsprite:kappa1>kappa1_happy
    Nice. #setsprite:kappa1>kappa1_default
    -> KAPPA_EVENT_EPILOGUE("goodie_accepted")
 + You don't feel like playing along with this thing, so you've chosen to roast it instead. #card #@kappa !bonfire
    You summon the bonfire right where the kappa is, but even before the first sparks form, it dives in its puddle! #narrator #setsprite:kappa1>kappa1_fried
    ...Seems like fire doesn't do much to creatures submerged in water. #narrator
    You wait a few moments for the kappa the re-emerge, then a few more moments. #narrator
    ... #narrator
    It's been a while now. Seems like the kappa is done dealing with you. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + "So, how's life in the swamp? Oh, and what do kappas eat{g_kappa_seen: again}?" #card #@kappa #!banter #speaker:You
    "Swamp-life's pretty chill for the most part! None of the more dangerous creatures don't bother me either." #speaker:The Kappa
    "As fer what we eat... My favorites are apples, <size=60%>souls</size>, and cucumbers of course. I also eat fish sometimes." #speaker:The Kappa
    Wait, what was that second favorite food again? Perhaps it's better not to ask? #narrator
    "Sounds pretty nice." #speaker:You
    Mooooving on. #narrator
    -> KAPPA_CARD_OPTIONS
 + You hold the lockpick toward the kappa. Perhaps it'll count as a "goodie"? #card #@kappa #!lockpick
    "Would this be counted as a goodie? It's a lockpick." #speaker:You
    "Uh, grateful fer the offer, {kappa_gender_word()}, but I don't think I can use that here in the swamp, especially with these clumsy paws." #speaker:The Kappa
    Unused, the lockpick returns to card form. Worth a shot, you suppose. #narrator #refundcard:lockpick
    -> KAPPA_CARD_OPTIONS
 + You heft the staff in your hand. Time to show this thing what for! #card #@kappa #!staff
    -> KAPPA_BONK_PARAGRAPH
 + You manifest the dagger into your hand, and throw it at the kappa with all you've got! #card #@kappa #!dagger
    Before the dagger is half-way toward the kappa, it's already below the water! #narrator #setsprite:kappa1>kappa1_hiding
    The dagger flies past the only part of the kappa that's still on the surface, then sinks into the swamp. Damnit. #narrator
    You wait a few moments for the kappa to re-emerge, then a few more moments. After a while it becomes clear that it's staying down there. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + You give the frying pan a few spins. Time to show this thing the might of your dear frying pan! #card #@kappa #!frying-pan
    -> KAPPA_BONK_PARAGRAPH
 + You hold the healing potion toward the kappa. Surely something like this will be a suitable "goodie"? #card #@kappa #!glass-bottle
    "Would a healing potion be a suitable goodie?" #speaker:You
    "Sounds like a goodie alright! Don't toss it at me though, I'm terrible at catching anything." #speaker:The Kappa
    Good timing, since you were just about to toss the bottle to the kappa. #narrator
    You walk over, and hand the potion to the kappa. #narrator
    "Yup-yup, ya pass!" #speaker:The Kappa #setsprite:kappa1>kappa1_happy
    Nice. #narrator #setsprite:kappa1>kappa1_default
    -> KAPPA_EVENT_EPILOGUE("goodie_accepted")
 + "Are you really so broke that you have to sit here and ask for handouts? That's <i>really</i> embarrassing, could NOT be me." #card #@kappa #!insult
    ...Perhaps that was a tad too far. You <i>are</i> pretty broke yourself... #narrator
    "That's real rude ya know. And I'm trying to trade with ya, not just get yer stuff fer free!" #speaker:The Kappa
    "Either way, deal's off, {kappa_gender_word()}. Hope the trolls get ya." #speaker:The Kappa
    You're about to cuss out the creature, but it quickly dips back below the surface. #narrator #setsprite:kappa1>kappa1_hiding
    Damnit. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + You hold the rope towards the kappa. You wonder if this'll count as a "goodie". #card #@kappa #!rope
    "Would you consider a rope to be a goodie? Could come in handy out here!" #speaker:You
    "Hmm. Hmmmmmm... I suppose yer right, it could be useful. I'll accept it. #speaker:The Kappa
    Nice! #narrator
    -> KAPPA_EVENT_EPILOGUE("decent_goodie")
 + You form a fist with your hand. Much easier than forming a plan, you think. #card #@kappa #!punch
    -> KAPPA_BONK_PARAGRAPH
 + You start building static electricity, and slowly moving towards the kappa. Time to give this thing what it deserves! #card #@kappa #!static-shock
    Once you're charged up and close enough, you leap the rest of the way at the kappa, and give it a devastating shock! #narrator #setsprite:kappa1>kappa1_shocked
    After taking a tiny moment to recover, it quickly dips below the surface, hiding from you. #narrator #setsprite:kappa1>kappa1_hiding
    You wait a good few moments for it to reappear, but it seems to be done with you. #narrator
    Damn. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + You hold the bottle of poison towards the kappa. Now, should you lie, or tell the truth... #card #@kappa #!evil-bottle
    "Whatcha got there, {kappa_gender_word()}? Seems mighty dangerous by the look of the bottle!" #speaker:The Kappa
    Good thing you didn't immediately go with a lie. #narrator
    "It's a bottle of poison, figured it might come in handy out here. Do you think it's a good enough goodie?" #speaker:You
    "Hmm. Hmmmmmm... Don't think I'll have use fer it, but I could maybe trade it fer something else. I'll accept it." #speaker:The Kappa
    Nice! #narrator
    -> KAPPA_EVENT_EPILOGUE("decent_goodie")
 + <i>Surely</i> a can of soup will count as a "goodie"! You gently toss the can towards the kappa. #card #@kappa #!can-of-beans
    The kappa attempts to catch the can, but its reflexes are far too slow. The can plops into the nearby water, and slowly floats back up. #narrator #setsprite:kappa1>kappa1_happy
    The kappa fetches the can, and studies it for a moment. #narrator #setsprite:kappa1>kappa1_default
    "Soup, huh? Might be a bit hard on my beak to open, but it seems like a right proper goodie to me! Ya pass!" #speaker:The Kappa #setsprite:kappa1>kappa1_happy
    Nice. #narrator #setsprite:kappa1>kappa1_default
    -> KAPPA_EVENT_EPILOGUE("goodie_accepted")
 + You feel hesitant to even try this, but you eventually manifest a snowball and gently toss it at the kappa. Maybe it'll count..? #card #@kappa #!snowball
    The kappa attempts to catch the snowball, but its reflexes are far too slow. The snowball plops into the water and quickly dissolves. #narrator #setsprite:kappa1>kappa1_happy
    "Uh, yer goodie seems to have melted. Sorry about that, {kappa_gender_word()}. I feel bad fer ya, so I'll give ya a consolation gift." #speaker:The Kappa #setsprite:kappa1>kappa1_default
    That's... a win, you suppose? #narrator
    -> KAPPA_EVENT_EPILOGUE("decent_goodie")
 + You quickly pop a cloud of smoke at the kappa, and start running off! #card #@kappa #!smokescreen
    You hear the kappa withdraw back in the puddle, and you easily make your exit. Nice! #narrator
    -> KAPPA_EVENT_EPILOGUE("player_left")
 + You quickly take off, and run past the kappa! You're not dealing with this thing! #card #@kappa #!run
    The kappa seems uninterested. Apparently leaving was a valid option all along? #narrator
    -> KAPPA_EVENT_EPILOGUE("player_left")
 + You look at the stick in your hands. Maybe the kappa can appreciate a cool stick? #card #@kappa #!stick
    "Would you perhaps count this one-of-a-kind cool stick as a goodie? It's even shaped like a wizard's staff!" #speaker:You
    "Listen, {kappa_gender_word()}. I'm sure ya consider sticks cool, but I can find those anywhere 'round here. No deal, sorry. #speaker:The Kappa
    Damnit. #narrator
    -> KAPPA_CARD_OPTIONS
 + "I don't really have any goodies on me right now, but I promise to bring you some superb goodies really soon, if you give me something useful! Promise!" #card #@kappa #!deceive #speaker:You
    "Hmm. Hmmmmmm... No, no deal. I only deal in the here-and-now kind of goodies. Potential future goodies are not good enough." #speaker:The Kappa
    Damnit. #narrator
    -> KAPPA_CARD_OPTIONS
 + You hope the kappa needs some healing, and you especially hope that it counts healing as a "goodie". Time to give it a shot! #card #@kappa #!soothsay
    "Say, you wouldn't happen to need any healing? I don't really have better goodies to offer you." #speaker:You
    "Well, now that ya mention it... I've been toughing it out, but I've got a splinter in my paw. If ya help me out with it, I'll give ya something in return. Deal?" #speaker:The Kappa
    ...Poor thing, of course you'll help! #narrator
    You walk over and tell it to show you the splinter. It's pretty obvious, and you could easily pull it out with just your fingers. #narrator
    "I'll cast a healing spell before I yank it out so it doesn't hurt, okay?" #speaker:You
    It gives you a nod, and you quickly recite the spell, healing wisps surrounding the kappa. #narrator
    You quickly yank out the splinter as the wisps circle the kappa, and the tiny wound closes up almost immediately. Nice! #narrator
    "Thank ya kindly, {kappa_gender_word()}! Totally worth some goodies from me!" #speaker:The Kappa
    Nice!
    -> KAPPA_EVENT_EPILOGUE("goodie_accepted")
 + You manifest the longsword in your hand. Time to show this thing your might! #card #@kappa #!longsword
    You bolt at the creature, and it immediately withdraws back into its puddle! #narrator #setsprite:kappa1>kappa1_hiding
    You slice at it with all you've got! #narrator
    ... #narrator
    It doesn't react. #narrator
    You wait a few moments for the kappa to re-emerge, then a few more moments. After a while it becomes clear that it's staying down there. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + "You've got such a cozy looking home here! And your fur looks really soft and glossy!" #card #@kappa #!praise #speaker:You
    "Why thank ya, {kappa_gender_word()}, I put great care into my home, and even more into my fur!" #speaker:The Kappa
    "I placed all the plants myself, and I think it really adds to the charm!" #speaker:The Kappa
    You wonder if it's talking about the plants on its fur, or the ones around its puddle. Perhaps both? #narrator
    Anyway, what to do now... #narrator
    -> KAPPA_CARD_OPTIONS
 + You decide to hypnotize this thing, and start with casting the spell. Surely it'll just give you whatever it has if you have it hypnotized, right? #card #@kappa #!hypnotize
    "{kappa_capital_gender_word()}? What're ya..." #speaker:The Kappa
    As the hypnosis finishes taking effect, the kappa gets a blank look on its face. Success! #narrator
    "Dear kappa, could you give me your treasures?" #speaker:You
    "Why of course, {kappa_gender_word()}! Give me just a moment..." #speaker:The Kappa
    -> KAPPA_EVENT_EPILOGUE("goodie_accepted")
//Yell is shared with self-section
 + You don't know how well it'll go, but you attempt to lift the kappa out of its puddle with your mind! #card #@kappa #!telekinesis
    "{kappa_capital_gender_word()}? What're ya..." #speaker:The Kappa
    You manage to get the kappa mostly out of the water, but the further up it gets, the harder it becomes as the lift from the water is lessened. #narrator
    After it's a bit more than half way up, it quickly splashes back down, as your mind buckles under the heavy load! #narrator
    After the splash has settled, the kappa is already fully withdrawn back to its hiding position. Damn. #narrator #setsprite:kappa1>kappa1_hiding
    You wait a moment, then another moment, as it becomes clearer and clearer that it plans to stay down there. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")
 + The spear pops into your hand, and you ready your stance for a few moments, before dashing right at the creature! #card #@kappa #!spear
    Quickly after you start running, it withdraws back into its puddle! #narrator #setsprite:kappa1>kappa1_hiding
    Not wanting to waste the spear, you drive it right at the part left on the surface! #narrator
    The spear \*cracks* in two, negating most of the damage! The kappa doesn't react. #narrator
    You wait a few moments for the kappa to re-emerge, then a few more moments. After a while it becomes clear that it's staying down there. #narrator
    -> KAPPA_EVENT_EPILOGUE("kappa_hid")

=== KAPPA_BONK_PARAGRAPH ===
You bolt at the creature, and it immediately withdraws back into its puddle! #narrator #setsprite:kappa1>kappa1_hiding
You give it a \*whack* with all you've got! #narrator
... #narrator
It doesn't react. #narrator
You wait a few moments for the kappa to re-emerge, then a few more moments. After a while it becomes clear that it's staying down there. #narrator
-> KAPPA_EVENT_EPILOGUE("kappa_hid")

=== KAPPA_EVENT_EPILOGUE(exit_method) ===

{
    - exit_method == "goodie_accepted":
    "Now, seems like ya can make use of these cards I found. I've found a few, and since I can't seem to do anything with 'em, ya can have 'em!" #speaker:The Kappa
    Now that definitely sounds worth your trouble! #narrator
    As you're mentally celebrating, the kappa dives down into its puddle. #narrator #setsprite:kappa1>kappa1_hiding
    You wait a few moments, and just as you start to think that you've been had, the kappa pops back up! #narrator #setsprite:kappa1>kappa1_default
    "Sorry fer taking a while, it's tough to grab these tiny things with my paws. Now, have a look, {kappa_gender_word()}! #speaker:The Kappa #function:RewardCards(4, dagger, staff, glass-bottle, frying-pan)
    After browsing through the cards, you thank the kappa. #narrator
    "Don't mention it, {kappa_gender_word()}! Thanks fer the trade!" #speaker:The Kappa
    After thanking you, the kappa dips back into its puddle. Seems your business is concluded? #narrator
    You dust yourself off, and ready yourself for more adventuring! #narrator
    - exit_method == "decent_goodie":
    "Now, seems like ya can make use of these cards I found. I'm gonna show ya a few, and ya can pick yer favorite out of 'em. Deal?" #speaker:The Kappa
    "Sounds fair to me." #speaker:You
    The kappa dives down into its puddle. #narrator #setsprite:kappa1>kappa1_hiding
    You wait a few moments, and just as you start to think that you've been had, the kappa pops back up! #narrator #setsprite:kappa1>kappa1_default
    "Sorry fer taking a while, it's tough to grab these tiny things with my paws. Now, take yer pick, {kappa_gender_word()}! #speaker:The Kappa #function:RewardCards(1, dagger, staff, glass-bottle, frying-pan)
    After making your choice, you thank the kappa.
    "Don't mention it, {kappa_gender_word()}! Thanks fer the trade!" #speaker:The Kappa
    After thanking you, the kappa dips back into its puddle. Seems your business is concluded? #narrator
    You dust yourself off, and ready yourself for more adventuring! #narrator
    - exit_method == "player_left":
    You've made your choice, so you keep running. #narrator
    You slow down after a while to spare your stamina, and perhaps because you feel like you missed out on something. #narrator
    - exit_method == "kappa_hid":
    Well. Nothing left to do here, since the kappa seems determined to stay withdrawn like that in its puddle. #narrator
    You toss a rock at its puddle as a parting shot, which makes a satisfying \*plop* sound. That'll show it. #narrator
    You decide to keep adventuring, dragging your feet slightly, since you feel like you missed out on something. #narrator
}

#function:LoadScene(Map1)

-> DONE
 
=== function kappa_capital_gender_word ===
{
    - g_player_gender == "female":
    ~ return "Little lady"
    - g_player_gender == "male":
    ~ return "Little guy"
    - g_player_gender == "non-binary":
    ~ return "Little wizard"
    - g_player_gender == "hat":
    ~ return "Tiny hat"
}
 
=== function kappa_gender_word ===
{
    - g_player_gender == "female":
    ~ return "little lady"
    - g_player_gender == "male":
    ~ return "little guy"
    - g_player_gender == "non-binary":
    ~ return "little wizard"
    - g_player_gender == "hat":
    ~ return "tiny hat"
}