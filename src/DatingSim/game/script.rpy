# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define player = DynamicCharacter('player_name')
define player_thinking = Character(what_italic = True)

define roomie = Character("Roommate")

# define romanceable characters
define dom = Character("Dominic")
define victoria = Character("Victoria")
define august = Character("August")
define finley = Character("Finley")

# Gallery code
default galleryList = ["anime_street_art", "red_anime_character"]
default Lightbox_image = ""

# Pronoun data setup
default subj_pron = ""
default obj_pron = ""
default posses_adj = ""
default posses_pron = ""
default reflex_pron = ""

# Flags
default club = "no club"
default skippedClass = "false"

label start:
    $ portrait_number = 0
    $ badboyPoints = 0  
    $ prepPoints = 0 
    $ artistPoints = 0 
    $ tsunPoints = 0 

    scene dorm room

    "DATING SIM DEMO"

    #####################################################################
    #
    #  FIRST SCENE
    #  > Roommate introduction
    #  > Name input
    #  > Pronoun choice
    #  > Portrait choice
    #  > Personality test
    #
    #####################################################################

    # Andrea: Start proper story?
    roomie "Oh my goodness you're here! You're here!"
    show roommate happy with dissolve
    roomie "You're my new roommate, right?"

    # ALEXIS: Name input
    python:
        name = renpy.input("What's your name?")
        name = name.strip() or "Default"
    $ player_name = name

    roomie "Ah [player_name]! It's so nice to meet you!"
    player "It's nice to meet you too!"

    # ANDREA: Pronoun Selection
    roomie "Ah, also, what are your pronouns?"
    player "Oh, thanks for asking! I use..."
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

    roomie "Well, it's wonderful to meet you [player_name]. I already know we're going to have a lot of fun together."
    player "What do you mean?"
    roomie "Well, first things first, lets get you settled in!"

    hide roommate happy with dissolve
    
    "Your roommate helps you put your bags down, and leaves you to start unpacking. There's a mirror next to your bed you glance at yourself in."

    player_thinking "What do I look like?"

    # AMELA: Character appearance selection

    screen portrait_selection():
        # Screen that displays 4 image buttons in a 2x2 grid.
        # The player clicks on an image to set their "appearance".

        vpgrid:
            cols 2
            spacing 20

            xalign 0.5
            yalign 0.5

            imagebutton auto "portraits/portrait1_%s.png" action Return(1)
            imagebutton auto "portraits/portrait2_%s.png" action Return(2)
            imagebutton auto "portraits/portrait3_%s.png" action Return(3)
            imagebutton auto "portraits/portrait4_%s.png" action Return(4)

    call screen portrait_selection

    # player appearance set by portrait selected
    $ portrait_number = _return

    roomie "Finished?"

    player "Yeah, I'm almost done. Why, what's up?"

    roomie "I found this personality quiz on a blog earlier, you should take it!"

    show roommate happy with dissolve

    player "A personality quiz?"

    "She hands you a piece of torn notebook paper with some questions written down on them."

    #start of quiz
    roomie "Would you rather"
    menu:
            "Read a book at home":
                $ artistPoints += 1
                $ prepPoints += 1
            "Go out to a raging party":
                $ badboyPoints += 1
                $ tsunPoints += 1

    roomie "Would you rather have"
    menu:
        "A close group of friends":
            $ artistPoints += 1
            $ tsunPoints += 1
        "A large number of acquaintances":
            $ badboyPoints += 1
            $ prepPoints += 1

    roomie "On a first date, you would prefer to go to... "
    menu:
        "To a movie theater":
            $ artistPoints += 1
            $ badboyPoints += 1
        "On a picnic":
            $ prepPoints += 1
            $ tsunPoints += 1

    player "What kind of blog did you find this on, again?" 
    roomie "No matter, thanks!"
    "She looks down at your answers and seems to be counting."
    player_thinking "What was that about?"
    roomie "Ah! I don't know."
    player "Don't know what?"
    roomie "Who you'd be cuter with!"
    player "What do you mean?"
    roomie "My friends! I'd love for you to meet them soon. Some of them have been looking for someone,"
    roomie "some haven't but really should.They're really nice! I'm sure you'd like them."
    roomie "Dominic is my oldest friend. He's a bit of a meanie but he's got the whole badboy look going on"
    roomie "AND he is a serious hottie"
    roomie "Victoria is..." #FIXME
    roomie "August I didn't meet until freshman year, but they've been super helpful whenever I'm feeling down."
    roomie "They have the softest heart."
    roomie "And then there's..." #FIXME
    roomie "But! You'll meet them all eventually, I'm sure"
    roomie "Look at me prattling on, I should let you get some rest before tomorrow! First day!"

    hide roommate happy with dissolve


    #####################################################################
    #
    #  SECOND SCENE ????
    #
    #####################################################################

    #FIXME Change visual to the school
    "After classes"
    player_thinking "First day of school tends to be exciting, but generally uneventful."
    player_thinking "It's been a long day, but there's one last thing I should do before I leave..." 
    jump choose_club

label choose_club:
    "Choose a club."

    menu:
        "Graphic Design Club":
            $ club = "graphic design club"
            jump meet_artist
        "Debate Team":
            $ club = "debate team"
            jump meet_prep
        "No club":
            $ club = "no club"
            jump meet_tsun

label meet_artist:
    "MEET THE ARTIST"
    jump skip_class

label meet_tsun:
    #meet TSUN
    scene school store   #is this the right scene for the store

    player_thinking "None of the clubs sound right for me. I guess I should just go get some food."

    player_thinking "Hey that person over there looks familiar I should go say hi."

    show finley

    finley "Hey what are you staring at?"

    menu:
        "I like your shirt.":
            $ tsunPoints += 1
            finley "Thanks for the compliment, not that I needed one. Oh you must be Jane’s roommate. Name’s Finley."
            player_thinking "Are they blushing, and is that a soft smile on their face."

        "I think you're friends with my roommate.":
            finley "Oh you must be Jane’s roommate. Name’s Finley."

    finley "So what are you doing here?"
    player "I was hungry so I decide to go get some food."
    finley "You should try the grilled cheese its very good."

    menu:
        "I love grilled cheese!":
            finley "I know right it is just good, or whatever."
            player_thinking "Their smile so bright."

        "I was actually thinking some pizza.":
            finley "Well pizza is good to."

    finley "Well I should get going see you later not that I excited to run into you again."

    hide finley

    jump skip_class

label meet_prep:
    "MEET PREP"

    jump skip_class

label skip_class:
    player_thinking "I'm not really feeling it today. Do I skip class?"

    # jump to meet_badboy, else go to class, then jump to free time
    menu:
        "Skip class":
            jump meet_badboy
        "Go to class":
            pass
    player_thinking "I'm going to class."
    jump free_time_1

label meet_badboy:
    $ skippedClass = "true"

    player_thinking "You know what? I think I will skip class."

    player_thinking "I deserve a break! I'm only human, right?"

    scene outside campus 2

    player_thinking "I find myself taking a pleasant stroll throughout campus."

    player_thinking "Around me I hear people chatting, leaves gently rustling, the distant ringing of bells..."

    player_thinking "It's really peaceful. I'm glad I took this time for myself."

    player_thinking "Even if I might regret it later..."

    player_thinking "As I walk around, someone catches my eye."

    show badboy

    player_thinking "A boy wearing all black, with facial piercings and what looks like a snake tattoo peeking out of his shirt."

    player_thinking "He looks so... edgy."

    player_thinking "I don't realize that I'm staring until he looks right back at me with a devilish smirk."

    dom "You like what you see?"

    menu:
        "I like it a lot, actually":
            player "Yes! A lot, actually!"
            dom "Whoa! Didn't expect that..."
            player_thinking "Eep. Was that too forward?"
        "W-what?":
            $ badboyPoints += 1
            player "Er, sorry, I didn't mean to stare..."
            dom "Haha! I was just teasing."

    dom "So, what're you up to? Heading to class?"

    player "No, I'm... skipping class, actually..."

    dom "Whoa! Got ourselves a rule-breaker over here!"

    player_thinking "I feel my face going pink..."

    dom "I'm doing the same, actually. Going to class is lame."

    player_thinking "He's so right."

    dom "You just moved in to ROOMMATE's place, right? [player_name], is it?"

    player_thinking "Oh! This must be Dominic."

    player "Yeah. She told me about you. I didn't expect to bump into you like this."

    dom "Heh. I'm sure she only had great things to say about me..."

    dom "Well, you're least likely to find me in a classroom-setting, that's for sure."

    dom "Because I'm too cool for school, y'know."

    dom "Anyways, I gotta bounce. Got hooligan activities to attend to. Because I'm a hooligan."
    
    player_thinking "He sounds like he's kidding, but somehow I can't tell."

    dom "I'm sure we'll bump into each other again. Later!"

    hide badboy

    player_thinking "Before I can even say goodbye, he starts sprinting past me like a madman."

    player_thinking "Those hooligan activities must be urgent..."
    
    jump free_time_1

label free_time_1:
    player_thinking "I have some free time... what should I do?"

    call screen MapUI(1)
    
    return

label library_1:
    scene library
    "LIBRARY 1: PREP IS THERE"
    jump halloween_party

label schoolstore_1:
    scene school store
    "SCHOOL STORE 1: ARTIST IS THERE"
    jump halloween_party

label tenniscourts_1:
    scene tennis courts

    if skippedClass == "true":
        dom "Well, well, well, if it isn't the rule-breaker! You skipping class again?"
        player_thinking "That voice sounds familiar..."
        show badboy
        player_thinking "It's Dominic!"
        player "No! I just had some free time, actually. I haven't seen the tennis courts before..."
    else:
        dom "Hey! You're ROOMMATE's friend, right? What're you doing here?"
        player_thinking "Who could that be?"
        show badboy
        player_thinking "This must be Dominic."
        player "I just had some free time. I hadn't seen the tennis courts yet..."

    dom "Well, I gotta tell you, there isn't much to see..."

    dom "Apart from me, of course."

    player_thinking "He's so smug..."

    player "Do you come here to play tennis, or just to loiter like a hooligan?"

    player_thinking "He cracks a smirk at that."

    dom "I think you know the answer to that."

    dom "But since we're both here, might as well have ourselves a game, no?"

    player "Pausing the hooligan activities for now?"

    dom "There's always time for them."

    player_thinking "I guess we're playing a friendly game of tennis."

    player_thinking "Should I..."

    menu:
        "Try my best":
            player_thinking "I go 100% tryhard mode. I'm running around the court as swiftly as my legs can take me."
            player_thinking "Dominic looks surprised. Guess he didn't expect me to go so hard."
            player_thinking "But I play to win!"
        "Not sweat it":
            $ badboyPoints += 1
            player_thinking "I don't try too hard. I can see that Dominic clearly doesn't, either."
            player_thinking "We both goof off as we swing our rackets around with the mindset of 'If I don't hit the ball, I don't hit the ball. Big whoop.'"
            player_thinking "By the end of it, we're both laughing at each other."

    dom "Are the tennis courts everything you hoped for?"
    
    player "Oh, yes. And so much more."

    dom "Heh. That was actually the first time I'd done anything besides loitering at this place. Was pretty fun."

    player_thinking "I peek down at my watch and realize it's almost time for my next class."

    player "Hey, I've got class to get to, but thanks for the game."

    if skippedClass == "true":
        dom "Going to class? I'm surprised."
        player "Hey!"

    dom "For sure. I also have... things. But good game."

    hide badboy

    player_thinking "Before I can open my mouth to speak, Dominic sprints towards the nearest tennis net, leaps over it, and continues sprinting towards the exit gate."

    player_thinking "That guy must not have a care in the world..."

    jump halloween_party

label dorms_1:
    scene dorm room
    #"DORMS 1: TSUN IS THERE"
    show finley
    player_thinking "I want to go hangout in my dorm."

    player_thinking "Is that Finley hanging out with roomie?"

    finley "Hey [player_name], me and Jane were just trying  to plan a prank on Dominic want to join us not that we need you or anything."

    menu:
        "Sure sounds like fun!":
            $ tsunPoints += 1
            finley "Awesome I was think about something with spiders since Dominic is afraid of them."
            player "What about a plastic spider that drops down when he opens the door."
            finley "That is perfect! I need to go buy a plastic spider now see you later [player_name]!"
            player_thinking "They are really blushing with such a bright smile. They must really like pranks."

        "No thanks I am going to head to my room to relax.":
            finley "That's fine have fun relaxing."
            player_thinking "Is that a frown on their face?"

    hide finley

    jump halloween_party

label halloween_party:
    player_thinking "HALLOWEEN PARTY -- where should I go?"
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
    player_thinking "I have some free time... what should I do?"

    call screen MapUI(2)

    return

label library_2:
    #"LIBRARY 2: TSUN IS THERE"

    show finley

    scene library  #is this the scene call for the library

    player_thinking "Is that Finley at that table over there? I should go see what they are doing."

    player "What are you doing here Finley?"

    finley "Hey [player_name] I have this big test coming up so I need to really study."

    menu:
        "Do you want some help?":
            $ tsunPoints += 1
            finley "Really thanks for the help not that I couldn’t mange on my own."
            player_thinking "Finley is blushing so much."
            player_thinking "Finley and I spent the next 3 hours studying for their test."

        "Well I will leave you alone to study.":
            finley "Sure see you some other time."
            player_thinking "Well I should get back to looking for some books."

    hide finley

    jump route_determination

label schoolstore_2:
    "SCHOOL STORE 2: BADBOY IS THERE"

    jump route_determination

label tenniscourts_2:
    "TENNIS COURTS 2: PREP IS THERE"

    jump route_determination

label dorms_2:
    "DORMS 2: ARTIST IS THERE"

    jump route_determination

label route_determination:
    # "Badboy points: [badboyPoints], prepPoints: [prepPoints], artistPoints: [artistPoints], tsunPoints: [tsunPoints]"
    python:
        points = [badboyPoints, prepPoints, artistPoints, tsunPoints]
        # print("Max points is " + str(max(points)))
        # print("Index of max points is " + str(points.index(max(points))))
        index_num = points.index(max(points))

    $ route_number = index_num

    if route_number == 0:
        jump BADBOY_START
    elif route_number == 1:
        jump PREP_START
    elif route_number == 2:
        jump ARTIST_START
    else:
        jump TSUNDERE_START

    return

label BADBOY_START:
    # badboy route
    "BADBOY ROUTE START"
    return

label PREP_START:
    # prep route
    "PREP ROUTE START"
    return

label ARTIST_START:
    # artist route
    "ARTIST ROUTE START"
    return

label TSUNDERE_START:
    # tsundere route
    #"TSUNDERE ROUTE START"

    #first event
    scene outside campus 2  #is this the scene for campus

    show finley

    player_thinking "I think that's Finley over there."

    finley "Hey [player_name] do you want to go thrift shopping with me, not that I want to hangout with. I just need someone to help me carry bags."

    menu:
        "Sure I would love to go thrift shopping!":
            $ tsunPoints += 1
            finley "Cool let's head over there now."

            scene school store #this should be another store

            show finley

            player "So Finley why do you love thrift shopping?"

            finley "Well I am the youngest of 8 kids so I always got my sibling hand me downs. After a while I grew to love to mismatch mess so now I go thrift shopping to what weird and wonderful things I might find."

            player_thinking "Finley is reaching over to a shirt on the rack."

            finley "This would look really good on you. You should go try it on."

            player_thinking "I take the shirt from Finley and go put it on I come out to see Finley try to cover their blush."

            finley "That really does look good on you."

            player_thinking "We spent the rest of the afternoon shopping together."

            finley "I had fun maybe your not so bad to hangout with."

            player_thinking "I can see the smirk and blush on Finley’s face. They had more fun than they are saying."

        "No thanks. I have other things I need to get done.":
            finley "That's fine or whatever."

            player_thinking "I can see the sad look on their face as they walk away."

    hide finley


    #second event

    scene outside campus 2  #should be the campus

    show finley

    player_thinking "Finley is over there."

    finley "Hey [player_name] do you want to come over to my dorm to watch some movies?"

    player_thinking "If Finley had a tail it would be wagging right now."

    player "Sure movies sounds like fun!"

    finley "Awesome see you tonight not that I really wanted you to say yes."

    scene dorm room

    player_thinking "I should knock on Finley’s door."

    show finley

    finley "Hey [player_name] I am just finishing up with the popcorn if you want to go pick a movie."

    menu:
        "Choose the bad movie":
            $ tsunPoints += 1
            finley "That movie is bad it's amazing good choice!"
            player_thinking "Finley face lit up like a christmas tree when I chose this movie and is that a blush on their cheeks. Finley picks up the movie to put it in the DVD player."

        "Choose the good movie":
           finley "That's a cool choice."
           player_thinking "Is Finley sad that I didn't choose the bad moive?"

    player_thinking "We spent the night watching movies together."

    hide finley

    scene outside campus 2 #should be the college campus

    finley "Is that Finley walking over to me?"

    show finley

    finley "Hey [player_name], I was wondering if you want to come over and cook with me tonight?"

    menu:
        "No thanks, I want to chill in my dorm.":
            finley "Well whatever so you later."

            player_thinking "Finley seems really sad that I said no."

        "Sure cooking with you sounds like a lot of fun!":
            finley "Well see you tonight!"

            player_thinking "Did Finley ask me to hangout without hiding their feelings, or their blush?"

            player_thinking "No must of been my imagination."

            scene dorm room

            player_thinking "I am here at Finley’s dorm I am going to knock."

            show finley

            finley "Hey come on in I was just setting everything up. We are making grilled cheese."

            player "Finley do you like to cook?"

            finley "When I was young my mom taught me how to cook. She would make me grilled cheese every time I was sick. So now when I am homesick I use her grilled cheese recipe."

            player_thinking "Finley helped me cook some delicious looking grilled cheese we sat down to dig into them."

            finley "[player_name] thanks for hanging out me I really like your company."

            player_thinking "Finley just told me they like to hangout with me and no mean comment. They are even blushing with the softest smile. I can’t believe what is happening!"

            player "Thank you for inviting me Finley. I like hanging out with you too."

    hide finley

    scene dorm room

    player_thinking "Its time for the New Year's Dance."

    return

label FINAL_PARTY:
    # new year's party event
    return

label BAD_END:
    # general bad ending
    return

# TODO: good endings for whichever route
# check flag variable for route
