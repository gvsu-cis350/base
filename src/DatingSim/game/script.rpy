# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define e = Character("Eileen")

# The game starts here.

# Pronoun data setup
default subj_pron = ""
default obj_pron = ""
default posses_adj = ""
default posses_pron = ""
default reflex_pron = ""

label start:
    $ portrait_number = 0 # default

    # Show a background. This uses a placeholder by default, but you can
    # add a file (named either "bg room.png" or "bg room.jpg") to the
    # images directory to show it.

    scene bg room

    # This shows a character sprite. A placeholder is used, but you can
    # replace it by adding a file named "eileen happy.png" to the images
    # directory.

    show eileen happy
    
    # Pronoun Selection
    e "What are your pronouns?"
    menu:
		"They/Them":
			$ subj_pron = "they"
			$ obj_pron = "them"
			$ posses_adj = "their"
			$ posses_pron = "theirs"
			$ reflex_pron = "themselves"
		"She/Her":
	    	$ subj_pron = "she"
	        $ obj_pron = "her"
	        $ posses_adj = "her"
	        $ posses_pron = "hers"
	        $ reflex_pron = "herself"
	    "He/Him":
	        $ subj_pron = "he"
	        $ obj_pron = "him"
	        $ posses_adj = "his"
	        $ posses_pron= "his"
	        $ reflex_pron = "himself"

	e "Ah! So you're a [subj_pron] / [obj_pron]"

    # These display lines of dialogue.

    e "You've created a new Ren'Py game."

    e "Once you add a story, pictures, and music, you can release it to the world!"

    # AMELA: Character appearance selection

    screen portrait_selection():
        # Screen that displays 4 image buttons in a 2x2 grid.
        # The player clicks on an image to set their "appearance".

        vpgrid:
            cols 2
            spacing 20

            xalign 0.5
            yalign 0.5

            imagebutton auto "portrait1_%s.png" action Return(1)
            imagebutton auto "portrait2_%s.png" action Return(2)
            imagebutton auto "portrait3_%s.png" action Return(3)
            imagebutton auto "portrait4_%s.png" action Return(4)

    call screen portrait_selection

    # player appearance set by portrait selected
    $ portrait_number = _return

    # FIXME: delete this code later; for testing purposes now
    if portrait_number == 1:
        e "Your appearance is Red"
    elif portrait_number == 2:
        e "Your appearance is Green"
    elif portrait_number == 3:
        e "Your appearance is Blue"
    elif portrait_number == 4:
        e "Your appearance is Purple"
    else:
        e "Something went wrong"

    # This ends the game.

    return
