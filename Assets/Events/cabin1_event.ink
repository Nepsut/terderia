INCLUDE global_vars.ink

// Example max line length. Example max line length. Example max line length. Example max line length. Example max line length.

//Story variables
VAR considered_frying_bonk = false

#targets:cabindoor1

You wake in a familiar cabin. Right back where you started. #narrator

//TESTING LINE, REMOVE LATER
    -> CABIN_CARD_OPTIONS

A nauseating cackle echoes in your head, sending shudders down your spine. Its mocking presence lingers in your mind. #narrator

You knew the risks, of course. But what self-respecting wizard would miss the chance to experiment with the essence of a god? #narrator

... #narrator

Just days ago you were chasing after a champion of the god of trickery that decided to mess with your home. #narrator

{g_player_class == "trickster": Your home is your playground after all. | You'd rather have peace in the region after all.} #narrator

Eventually you caught up to the troublemaker, and despite their divine backing you managed to eke out a victory. #narrator

{g_player_class == "trickster": Not wanting to miss the chance of learning the tricks of used by a champion, you got to experimenting. | Wanting to avoid incidents like this in the future, you decided to experiment with the now-dead champion's source of power.} #narrator

Perhaps unsurprisingly, the backer of said champion wasn't exactly a fan of your antics. #narrator

So, it played a trick on you. #narrator

"Have fun sorting this mess out!" it said amidst a cackling laugh, its {g_player_class == "trickster": somehow familiar} power tearing your spellbook apart. #narrator

"But worry not, your precious book can still be restored", it said, with a mischievous smile creeping in your mind. #narrator

"You see, I enjoy a good card game! Gather your deck and meet me back at the peak of this mountain." #narrator

"If you manage to be entertaining enough, I'll even consider putting your little spellbook back to how it was!" #narrator

"But for now, I think I'll help you out just a little, to make sure you don't go dying on the way back to your little cabin." #narrator

"Nighty-night, {g_player_gender == "hat": little hat! Though I'm not exactly sure how a hat sleeps. | little hero.}" #narrator

... #narrator

And so you ended up back here. #narrator

As you try to gather your bearings, you check your pockets to find a few... playing cards? #narrator

Your keys were nowhere to be found, however. #narrator

After looking around for {perhaps a bit too long, | a short while,} you decide to try out the cards you found. #narrator

-> CABIN_CARD_OPTIONS

=== CABIN_CARD_OPTIONS ===

 + You eat the apple. It has a nice crunch to it. #card #@self #!apple
 {
    - g_player_health < g_player_max_health:
    ~ g_player_health++
 }
    -> CABIN_CARD_OPTIONS
 + A roaring bonfire springs forth right beneath your feet! #card #@self #!bonfire
    You hop away just quickly enough to avoid being burned. That was close. #narrator
        -> CABIN_CARD_OPTIONS
 + You have a lovely conversation with yourself. {g_player_gender =="hat": You even add some spice, speaking half of your lines through the crease on your front-side, and half through the mouth on your host body.} #card #@self #!banter
    -> CABIN_CARD_OPTIONS
 + You ponder the lockpick. It doesn't seem to do anything when used on yourself. #card #@self #!lockpick #refundcard:lockpick
    -> CABIN_CARD_OPTIONS
 + You heft the frying pan in your hand. It has a comforting weight to it. #card #@self #!frying-pan
    {
        - !considered_frying_bonk:
        For a moment you consider smashing your head in, but refrain this time. #narrator #refundcard:frying-pan
        ~ considered_frying_bonk = true
        - else:
        The temptation to {g_player_gender =="hat": test the durability of your host body | crack your skull open} with the frying pan proves too much, and you smash it on your head with surprising force! #narrator
        ~ g_player_health--
        ~ considered_frying_bonk = false
    }
    -> CABIN_CARD_OPTIONS
 + You spin the dagger in your hand, and decide on a round of five-finger-fillet. #card #@self #!dagger
    "I have all my fingers..." #speaker:You
    {~ As your pace quickens, you eventually fumble and stab your finger. -> FILLET_RESULTS(true)| You quicken your pace as you go, but even after a good 30 seconds of intense finger-filleting, you haven't yet hit yourself. -> FILLET_RESULTS(false)} #narrator
 + The staff feels comfortable in your hand, but nothing useful happens. #card #@self #!staff
    -> CABIN_CARD_OPTIONS
 + You consider the potion in your hand. #card #@self #!glass-bottle
    {
        - g_player_health >= g_player_health:
        Using it now would be a waste, perhaps its better to save it for later. #narrator #refundcard:glass-bottle
        - else:
        You down the potion, and feel its healing properties take effect. #narrator
        {
            - g_player_health == g_player_max_health - 1:
            ~ g_player_health++
            - else
            ~ g_player_health += 2
        }
    }
    -> CABIN_CARD_OPTIONS
 + You adjust the apple in your hand for a better grip before flinging it wildly at the door! #card #@cabindoor1 #!apple
    It makes a loud splat as it hits the door, sending pieces of apple flying everywhere. Nice. #narrator
    -> CABIN_CARD_OPTIONS
 + A roaring bonfire springs forth next to the door, quickly setting it ablaze! #card #@cabindoor1 #!bonfire
    As the door burns, you begin to question your choice of setting fire to the only way out ot the cabin. #narrator
    Luckily for you, a hole burns in the door before the rest of the cabin can catch on fire. #narrator #setsprite:cabindoor1>cabindoor1_exploded
    You quietly think to yourself, that a more destructive fire spell could have been a catastrophic choice. #narrator
    -> CABIN_EPILOGUE("hole")
 + You attempt to chat with the door. It does not reply. Still, you've had worse conversations. #card #@cabindoor1 #!banter
    -> CABIN_CARD_OPTIONS
 + You take out a lockpick, and attempt to pick the door open. #card #@cabindoor1 #!lockpick
    {g_player_class == "trickster": Lucky for you, you've grown accustomed to this kind of work, and the lock pops open in no time. | It takes a few attempts, and as you're about to give up, the lock finally clicks open. } #narrator #setsprite:cabindoor1>cabindoor1_open
    -> CABIN_EPILOGUE("unlock")
 + You heft the frying pan in your hand, determined to pop the handle straight off the door, and the lock with it. #card #@cabindoor1 #!frying-pan
    You slam the frying pan on the handle, and it pops off exactly as planned, though the impact leaves your hand slightly sore. #narrator #setsprite:cabindoor1>cabindoor1_open
    -> CABIN_EPILOGUE("force_unlock")
 + You give your dagger a quick spin before unleashing a flurry of strikes at the door! #card #@cabindoor1 #!dagger
    "..." #speaker:Door
    Now decorated with 24 shallow "stab wounds", the door doesn't budge. Damnit. #narrator
    -> CABIN_CARD_OPTIONS
 + You heft the staff in your hand, determined to pop the handle straight off the door, and the lock with it. #card #@cabindoor1 #!staff
    { shuffle:
        - You slam the staff on the handle, and it pops off exactly as planned, though the impact reverberates through the staff into your hands. You try to shake off the nasty feeling. #narrator
            ->STAFF_BONK_RESULTS(true)
        - You slam the staff at the handle, but it glances off the side, redirecting your strike into the floor. The impact sends terrible reverberations through your entire arms, leaving you reeling. #narrator
        ->STAFF_BONK_RESULTS(false)
    }
 + You adjust the bottle for a better grip, and launch it at the door! #card #@cabindoor1 #!glass-bottle
    The bottle shatters against the door, sending glass flying everywhere! #narrator
    As the glass shards settle, the potion splatter on the door drips onto the floor. It was an attempt, for certain. #narrator
    -> CABIN_CARD_OPTIONS
 
 === FILLET_RESULTS(lost_game) ===
 {
    - lost_game:
        ~g_player_health--
        You wince as a small amount of blood spills from your finger. Time to call it quits for now. #narrator
        -> CABIN_CARD_OPTIONS
    - else:
        A satisfied smile settles onto {g_player_gender == "hat": the crease on your front-side, | your face,} and you feel reaffirmed of your {g_player_gender == "hat": control over this vessel | dexterity}. #narrator
        -> CABIN_CARD_OPTIONS
 }
 
 === STAFF_BONK_RESULTS(success) ===
 {
    - success:
    After your hands feel normal again, you poke the door open with the staff. #narrator #setsprite:cabindoor1>cabindoor1_open
    -> CABIN_EPILOGUE("force_unlock")
    - else:
    -> CABIN_CARD_OPTIONS
 }
 
 === CABIN_EPILOGUE(exit_method) ===
 
 {
    - !g_first_cabin_seen:
    As you give the cabin a hasty final once-over, you notice some cards on the table. #narrator
    You have no idea how you missed these before, and you quickly pocket them. #narrator #function:UnlockCabinCards
    You give the cabin another glance in hopes of finding something else you may have missed, but nothing turns up. #narrator
    - else:
    You quickly scour through the cabin again, hoping to find some cards again, but nothing turns up. #narrator
 }
 { exit_method:
    - "hole":
    You hop out of the cabin through the hole in the door, {g_player_gender == "hat": making sure to have your host body hold on to you on the way. | taking in the fresh air as you go.}. #narrator #function:LoadMap1Scene
    - "unlock":
    You step out of the cabin, taking in the beautiful nature view around you. Adventure awaits! #narrator #function:LoadMap1Scene
    - else:
    You exit the cabin, bracing yourself for the coming adventure. #narrator #function:LoadMap1Scene
 }
 
 -> DONE