# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define e = Character("Eileen")

#Gallery code
default galleryList = ["anime_street_art", "red_anime_character"]
default Lightbox_image = ""

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
    $ tsunPoints = 0 #defult starting amount of favor points for Tunsundre

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
                $ tsunPoints += 1

    e "Would you rather have"
    menu:
        "A close group of friends":
            $ artistPoints += 1
            $ tsunPoints += 1
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
            $ tsunPoints += 1

    # This ends the game.

    jump choose_club

label choose_club:
    "Choose a club."

    menu:
        "Graphic Design Club":
            jump meet_artist
        "Debate Team":
            jump meet_prep
        "No club":
            jump meet_tsun

label meet_artist:
    "MEET THE ARTIST"
    jump skip_class

label meet_tsun:
    "MEET TSUN"
    jump skip_class

label meet_prep:
    "MEET PREP"

    jump skip_class

label skip_class:
    "I'm not really feeling it today. Do I skip class?"

    # jump to meet_badboy, else go to class, then jump to free time
    menu:
        "Skip class":
            jump meet_badboy
        "Go to class":
            pass
    "I'm going to class."
    jump free_time_1

label meet_badboy:
    "MEET BAD BOY"

    jump free_time_1

label free_time_1:
    "I have some free time... what should I do?"

    menu:
        "Library":
            jump library_1
        "School store":
            jump schoolstore_1
        "Tennis courts":
            jump tenniscourts_1
        "Dorms":
            jump dorms_1

label library_1:
    "LIBRARY 1: PREP IS THERE"
    jump halloween_party

label schoolstore_1:
    "SCHOOL STORE 1: ARTIST IS THERE"
    jump halloween_party

label tenniscourts_1:
    "TENNIS COURTS 1: BADBOY IS THERE"
    jump halloween_party

label dorms_1:
    "DORMS 1: TSUN IS THERE"
    jump halloween_party

label halloween_party:
    "HALLOWEEN PARTY -- where should I go?"
    menu:
        "Haunted house":
            jump haunted_house
        "Pumpkin patch":
            jump pumpkin_patch

label haunted_house:
    "Haunted house event"

    menu:
        "Side with bad boy":
            pass
        "Side with tsundere":
            pass

    jump free_time_2

label pumpkin_patch:
    "Pumpkin patch event"

    menu:
        "Side with prep":
            pass
        "Side with artist":
            pass
    jump free_time_2

label free_time_2:
    "I have some free time... what should I do?"

    # TODO: probably nicer way to do this using "call" instead of "jump",
    # having two separate free_time labels is fine for now

    menu:
        "Library":
            jump library_2
        "School store":
            jump schoolstore_2
        "Tennis courts":
            jump tenniscourts_2
        "Dorms":
            jump dorms_2

    return

label library_2:
    "LIBRARY 2: TSUN IS THERE"
    jump route_determination

label schoolstore_2:
    "SCHOOL STORE: BADBOY IS THERE"

    jump route_determination

label tenniscourts_2:
    "TENNIS COURTS: PREP IS THERE"

    jump route_determination

label dorms_2:
    "DORMS: ARTIST IS THERE"

    jump route_determination

label route_determination:
    # check which character has the most points here --
    # whoever it is, trigger their route
    # set flag to current route

    "Route determined here"

    return

label BADBOY_START:
    # badboy route
    return

label ARTIST_START:
    # artist route
    return

label PREP_START:
    # prep route
    return

label TSUNDERE_START:
    # tsundere route
    return

label FINAL_PARTY:
    # new year's party event
    return

label BAD_END:
    # general bad ending
    return

# TODO: good endings for whichever route
# check flag variable for route
