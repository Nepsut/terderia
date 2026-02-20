INCLUDE global_vars.ink

// Example max line length. Example max line length. Example max line length. Example max line length. Example max line length.

You wake in a familiar cabin. Right back where you started. #narrator

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

"Nighty-night, {g_player_gender == "hat": little hat! Though I'm not exactly sure how a hat sleeps. | little hero.}"

... #narrator

And so you ended up back here. #narrator

As you try to gather your bearings, you check your pockets to find a few... playing cards? #narrator

Your keys were nowhere to be found, however. #narrator

After looking around for perhaps a bit too long, you decide to try out the cards you found. #narrator #targets:cabindoor1

 + The fireball scorches your enemy #card #@kobold1 #@kobold2 #!fireball #damage:fire #strength:medium #setsprite:kobolds1>kobolds1_burned
 + Your spell blasts your target and their surroundings #card #@kobold1 #@kobold2 #range:high #aoe:large
 + The potato connects with your target, blasting it to smithereens #card #@kobold1 #@kobold2 #!tater-launcher
 + Your attempts at reasonable discussion fall apart as you get stabbed #card #@kobold1 #@kobold2 #type:speech #setsprite:kobolds1>kobolds1_attackmode
    ~ g_player_health--
 + You manage to get away from your target #card #@self #@kobold1 #@kobold2 #other:disengage
 + The kobolds look slightly confused, though they quickly recover and stab you. #card #@kobold1 #@kobold2 #!lockpick
 + You bonk the life out of one of the kobolds, and the rest quickly scatter. #card #@kobold1 #@kobold2 #range:touch #strength:low
 + Your generous offer of a healing potion seems to garner the respect of the kobolds! #card #@kobold1 #@kobold2 #!glass-bottle
 + You roast marshmallows with the kobolds. Aww. #card #@kobold1 #@kobold2 #!bonfire
 + Your generous offer of an apple seems to garner the respect of the kobolds! #card #@kobold1 #@kobold2 #!apple
 
 === FIRST_CARD_USED ===
The event continues
-> DONE