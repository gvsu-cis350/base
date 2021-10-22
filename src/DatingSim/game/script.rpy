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
    $ rebelPoints = 0  #defult starting amount of favor points for rebel
    $ prepPoints = 0 #defult starting amount of favor points for prep
    $ artistPoints = 0 #default starting amount of favor points for artist
    $ TusnPoints = 0 #defult starting amount of favor points for Tunsundre

    # Show a background. This uses a placeholder by default, but you can
    # add a file (named either "bg room.png" or "bg room.jpg") to the
    # images directory to show it.

    scene bg room

    # This shows a character sprite. A placeholder is used, but you can
    # replace it by adding a file named "eileen happy.png" to the images
    # directory.

    show eileen happy

    # These display lines of dialogue.

    e "DATING SIM DEMO"

    # ALEXIS: Name input
    python:
        name = renpy.input("What's your name?")
        name = name.strip() or "Default"
    e "So your name is [name]... interesting."

    # ANDREA: Pronoun Selection
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

    # AMELA: Character appearance selection

    e "What do you look like?"

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

    #start of quiz
    e "Would you rather"
    menu:
            "Read a book at home":
                $ artistPoints += 1
                $ prepPoints += 1
            "Go out to a raging party":
                $ rebelPoints += 1
                $ TusnPoints += 1

    e "Would you rather have"
    menu:
        "A close group of friends":
            $ artistPoints += 1
            $ TusnPoints += 1
        "A large number of acquaintances":
            $ rebelPoints += 1
            $ prepPoints += 1

    e "One a first date you would like to go to "
    menu:
        "To a movie theater":
            $ artistPoints += 1
            $ rebelPoints += 1
        "On a picnic":
            $ prepPoints += 1
            $ TusnPoints += 1

    # This ends the game.

    return
