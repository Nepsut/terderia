VAR g_player_health = 5
VAR g_player_max_health = 5

VAR g_player_class = "default"
VAR g_player_gender = "default"

//Variables for tracking if player has seen an event for the first time or not.
VAR g_first_cabin_seen = false

//Variables for tracking if player has experienced non-standard card behaviors before
VAR g_bonfire_marshmallows_seen = false

//Variables for tracking event outcomes
VAR g_helped_lizard_soldier = false
VAR g_tried_help_lizard_soldier = false
VAR g_got_lizard_message = false


//Global functions
=== function player_pronoun ===
{ g_player_gender:
    - "female":
    ~ return "she"
    - "male":
    ~ return "he"
    - else:
    ~ return "they"
}

=== function player_subject_pronoun ===
{ g_player_gender:
    - "female":
    ~ return "her"
    - "male":
    ~ return "him"
    - else:
    ~ return "them"
}

=== function player_possessive_pronoun ===
{ g_player_gender:
    - "female":
    ~ return "her"
    - "male":
    ~ return "his"
    - else:
    ~ return "their"
}

=== function player_possessive_end_pronoun ===
{ g_player_gender:
    - "female":
    ~ return "hers"
    - "male":
    ~ return "his"
    - else:
    ~ return "theirs"
}