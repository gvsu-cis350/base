# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define e = Character("Eileen")

# The game starts here.

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

    # These display lines of dialogue.

    e "You've created a new Ren'Py game."

    e "Once you add a story, pictures, and music, you can release it to the world!"

    # AMELA: Character appearance selection
    
    # menu:
    #     "{image=red_square.png}":
    #         $ appearance = "red_square"
    #         "You're red"
    #     "{image=green_square.png}":
    #         $ appearance = "green_square"
    #         "You're green"
    #     "No image":
    #         $ appearance = "none"
    #         "no image"

    # e "Cool image"

    # if appearance == "red_square":
    #     e "You are so red"
    # elif appearance == "green_square":
    #     e "You are so green"
    # else:
    #     e "Well you're boring..."

    screen grid_test():

        vpgrid:
            cols 2
            spacing 20

            xalign 0.5
            yalign 0.5

            imagebutton auto "red_square_%s.png" action Return(1)
            imagebutton auto "green_square_%s.png" action Return(2)

    call screen grid_test

    $ portrait_number = _return

    if portrait_number == 1:
        e "Your appearance is Red"
    elif portrait_number == 2:
        e "Your appearance is Green"
    else:
        e "Something went wrong"

    # This ends the game.

    return
