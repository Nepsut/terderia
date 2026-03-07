INCLUDE global_vars.ink

VAR frog_health = 2

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

This isn't going anywhere. You'll have to do something about this.

-> FROG_CARD_OPTIONS

=== FROG_CARD_OPTIONS ===

 + You ready yourself for a moment, then hurl a punch directly at Rudibert! #card #@frog #!punch
    ~frog_health--
    {frog_health == 0: He collapses to the ground, motionless. -> FROG_EVENT_EPILOGUE("won_fight")|He shakes his head slightly, seemingly taking some damage. -> FROG_COUNTER_ATTACK}
 + Your fingers spark with electricity as you ready a static shock. #card #@frog #!static-shock
    You inch closer to Rudibert, and unleash a devastatic static shock directly on his snout!
    ~frog_health--
    {frog_health == 0: He collapses to the ground, motionless. -> FROG_EVENT_EPILOGUE("won_fight")|He flinches back violently, clearly surprised by your devious attack.-> FROG_COUNTER_ATTACK}
 + "Your hat doesn't suit you and you smell weird!" #card #speaker:You #@frog #!insult
    A single tear forms below Rudibert's eye. #narrator
    "But... it's my favorite hat..." #speaker:Rudibert
    He seems genuinely heartbroken. Inconsolable. Beyond any comforting. #narrator
    You monster. #narrator
    He starts hopping away with tears streaming from his eyes! #narrator
    "Rudibert I-", you start, but he just hops away faster. #speaker:You
    Your gut wrenches. What have you done? #narrator
    -> FROG_EVENT_EPILOGUE("frog_fled")
 + You swing the rope in your hands a few times, then skillfully fling it around Rudibert! #card #@frog #!rope
    "Ha! Caught you!" #speaker:You
    He starts to slowly try to untangle himself, but you wrap the rest of the rope around him before he can get free.
    "Okay okay, I give up. Now what?" #speaker:Rudibert
    "Now uh. You're going to sit here and uh... I didn't really think things this far. I'll just go." #speaker:You
    "Eh. Sounds fair enough I guess. Could you toss me that mushroom on your way out?" #speaker:Rudibert
    Far be it for you to get between a frog and his mushroom-habit. #narrator
    You knock the mushroom towards him as you start to leave. #narrator
    "Hm. I don't think I can actually eat it with my hands tied. Could you..." His voice fades as you walk away. #narrator
 
 -> DONE
 
 === FROG_COUNTER_ATTACK ===
    "Ow! Jeez! What was that for!?" #speaker:Rudibert
    His expression shifts to anger, and he seems to be planning something. #narrator
    You have no time to predict his actions before he launches his tongue directly at you! #narrator
    It hurts worse than some punches you've taken, and you cough up some {g_player_gender == "hat": lint. | blood.} #narrator
    ~g_player_health--
    -> FROG_CARD_OPTIONS
 
 === FROG_EVENT_EPILOGUE(exit_method) ===
The event continues
-> DONE