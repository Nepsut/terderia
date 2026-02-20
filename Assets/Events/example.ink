INCLUDE global_vars.ink

Money or life, buddy. #speaker:kobold #targets:kobolds1

 + The fireball scorches your enemy #card #@kobold1 #@kobold2 #!fireball #damage:fire #strength:medium #setsprite:kobolds1>kobolds1_burned
 + Your spell blasts your target and their surroundings #card #@kobold1 #@kobold2 #range:high #aoe:large
 + The potato connects with your target, blasting it to smithereens #card #@kobold1 #@kobold2 #!tater-launcher
 + Your attempts at reasonable discussion fall apart as you get stabbed #card #@kobold1 #@kobold2 #type:speech #setsprite:kobolds1>kobolds1_attackmode
    ~ g_health -= 1
 + You manage to get away from your target #card #@self #@kobold1 #@kobold2 #other:disengage
 + The kobolds look slightly confused, though they quickly recover and stab you. #card #@kobold1 #@kobold2 #!lockpick
 + You bonk the life out of one of the kobolds, and the rest quickly scatter. #card #@kobold1 #@kobold2 #range:touch #strength:low
 + Your generous offer of a healing potion seems to garner the respect of the kobolds! #card #@kobold1 #@kobold2 #!glass-bottle
 + You roast marshmallows with the kobolds. Aww. #card #@kobold1 #@kobold2 #!bonfire
 + Your generous offer of an apple seems to garner the respect of the kobolds! #card #@kobold1 #@kobold2 #!apple
 
 === FIRST_CARD_USED ===
The event continues
-> DONE