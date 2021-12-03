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

<<<<<<< HEAD
# Have you met a romanceable character?
default knowAugust = "false"

=======
>>>>>>> ca92d3217f76a80dae2c37b71baf4490dced9713
# Pronoun data setup
default subj_pron = ""
default obj_pron = ""
default posses_adj = ""
default posses_pron = ""
default reflex_pron = ""

# General Flags
default club = "no club"
default meetPrep = "false"
default skipped_class = "false"
default haunted_house = "none"

# Flags for Dom route
default slept_in = "false"
default answered_door = "false"

label start:
    $ portrait_number = 0
    $ badboyPoints = 0
    $ prepPoints = 0
    $ artistPoints = 0
    $ tsunPoints = 0

    play music "music/on-my-way.mp3" loop fadein 1.0
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
    show roommate happy with moveinleft
    roomie "You're my new roommate, right?"

    # ALEXIS: Name input
    python:
        name = renpy.input("What's your name?")
        name = name.strip() or "Default"
    $ player_name = name

    roomie "Ah [player_name]! It's so nice to meet you!"
    player "It's nice to meet you too!"

    # # ANDREA: Pronoun Selection
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

    show roommate happy with moveinright

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
<<<<<<< HEAD
    roomie "My friends! I'd love for you to meet them soon. Some of them have been looking for someone, some haven't but really should.They're really nice! I'm sure you'd like them."
    roomie "Dominic is my oldest friend. He's a bit of a meanie but he's got the whole badboy look going on AND he is a serious hottie"
    roomie "Victoria is..." #FIXME
    roomie "August I didn't meet until freshman year, but they've been super helpful whenever I'm feeling down. They have the softest heart."
    roomie "And then there's..." #FIXME
=======
    roomie "My friends! I'd love for you to meet them soon. Some of them have been looking for someone,"
    roomie "some haven't but really should.They're really nice! I'm sure you'd like them."
    roomie "Dominic is my oldest friend. He's a bit of a meanie but he's got the whole badboy look going on"
    roomie "AND he is a serious hottie"
    roomie "Victoria is honest. She can be blunt, but she's also the most sincere person I've met."
    roomie "August I didn't meet until freshman year, but they've been super helpful whenever I'm feeling down."
    roomie "They have the softest heart."
    roomie "And then there's is Finley they have a tough exterior but they are really sweet once you get to know them." #FIXME
>>>>>>> ca92d3217f76a80dae2c37b71baf4490dced9713
    roomie "But! You'll meet them all eventually, I'm sure"
    roomie "Look at me prattling on, I should let you get some rest before tomorrow! First day!"

    hide roommate happy with dissolve

<<<<<<< HEAD
    player_thinking "First day... I'm a little nervous. I wonder what tomorrow will be like."

=======
>>>>>>> ca92d3217f76a80dae2c37b71baf4490dced9713
    #####################################################################
    #
    #  SECOND SCENE -- Choose Club
    #  > Choose Club
    #  > If Graphic Design -- Meet Artist
    #  > If Debate Team -- Meet Prep
    #  > If No Club -- Meet Tsundere
    #
    #####################################################################

    scene outside campus 1 with fade
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

# Meet Artist: Andrea
label meet_artist:
    scene design club with dissolve
    player_thinking "There's only one spot left..."
    show artist with moveinleft
    $ knowAugust = "true"
    $ assholeToAugust = "false"
    player_thinking "The person sitting next to me is looking at me. Why are they blushing?"
    menu:
        "Introduce yourself":
            player "Hi, I'm [player_name]. I'm new here."
            "The person sitting next to you is smiling at their screen."
            august "I'm August."
            player "August? I like your name!"
            august "Really? I like yours too..."
            player_thinking "Am I blushing now?"
            "You start up your laptop and open your most recent design project"
            august "Oh wow, you're really talented!"
            player "Ah, you think so? Thank you!"
        "...":
            pass

    "The club president calls everyone's attention and introduces you to all the club members. They are very welcoming."
    "The person sitting next to you, August, keeps glancing at you. "

    menu:
        "Ask August about what they are working on":
            player "What have you been working on?"
            august "Do you want to see?"
            player "Absolutely!"
            "August shows you a sketch of a creepy but beautiful mermaid."
            menu:
                "Oh wow, that's stunning!": 
                    august "Thank you!"
                    $ artistPoints += 1
                "...Ew, that's weird.":
                    august "Oh. Sorry."
                    $ assholeToAugust = "true"        
        "Tell August they're being creepy":
            player "What are you looking at me for?"
            august "Oh! I uh..."
            player "You what? huh? need anything?"
            august "No, no! I'm sorry, I didn't mean to bother you."
            player "Yeah, I don't care, just stop being creepy."
            $ assholeToAugust = "true"
        "..."
    
    "You spend the rest of the hour desigining in August's company."
    if assholeToAugust == "true":
        "They looked really uncomfortable the entire time."
    else:
        "It was nice."

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

    $ meetPrep = "true"

    player_thinking "I guess there’s a debate team recruiting event going on today. I could probably improve my argumentation skills…"

    player_thinking "I guess I'll give it a try."

    scene debate room

    player_thinking "The room I enter is filled to the brim with other students."

    player_thinking "I walk up to a table filled with debate team keychains, pens, and pamphlets."

    player_thinking "A student is standing next to the table, scribbling something on a clipboard. "

    player_thinking "Her hair is a deep brown, and her eyes are grey. "

    player_thinking "She’s wearing a beige sweater with black slacks, a pretty bland outfit overall."

    player_thinking "But despite that, something about her is sort of intimidating..."

    player_thinking "She notices me after a few seconds and steps towards me."

    show prep

    victoria "So you think you might be good enough to join the debate team, huh?"

    victoria "It’s not going to be up to me, but I can tell you right now that it’s not easy."

    victoria "I’m Victoria, by the way. I’ve seen you around before. "

    victoria "You’re Jane’s roommate, [player_name]."

    player "Yep, that's me."

    player_thinking "She smirks and looks down at her clipboard again."

    victoria "So, you still want to sign up?"

    menu:
        "Heck yeah!":
            $ prepPoints += 1

            player "Yeah, I could hold my own in a debate!"

            victoria "Wow, you seem really confident. I hope that means you're actually good."

            victoria "Good luck! I'll see you around."

            player "Thanks!"

            player_thinking "She turns around and walks towards another student."

            player_thinking "She seems hard to please, and a bit bland (at least fashion-wise), but maybe there's more to her."

            player_thinking "And now I have to try out... let's see how this goes."

            scene dorm room

            player_thinking "I crushed it! I get to be on the debate team!"

            player_thinking "I wonder what Victoria has to do with the team... she must be pretty high-up in the ranks."

        "I'm not sure...":
            player "I don't know... I guess I'll give it a shot..."

            victoria "You won't make it with that attitude. Maybe a less competitive club would suit you better?"

            player "Um... I'm sorry?"

            victoria "*groan*"

            victoria "Whatever. Later."

            hide prep

            player_thinking "She turns around and walks towards another student."

            player_thinking "Since I'm here, I'll try out anyways."

            player_thinking "I doubt I'll make it, though."

            scene dorm room

            player_thinking "Somehow, I got in. I think I did well enough."

            player_thinking "Victoria doesn't seem to like me, though."

            player_thinking "Maybe I'll have another chance to prove myself."

    jump skip_class

    #####################################################################
    #
    #  THIRD SCENE -- Free time
    #  > If skip class == meet bad boy
    #  > Free time 
    #    > If library == see prep
    #    > If school store == see artist
    #    > If tennis court == see bad boy
    #    > If dorm == see tsundere
    #
    #####################################################################

label skip_class:
<<<<<<< HEAD

    scene outside campus 2 with fade
    "The next day..."
    player_thinking "I'm not really feeling it today. Do I skip class?"
=======
    play music "music/easy-lemon.mp3" loop
    scene dorm room with fade
    "BRRRING BRRRING..."
    player_thinking "Ah... another day of university..."
    player_thinking "Hrrg... I'm not really feeling it today. Do I skip class?"
>>>>>>> ca92d3217f76a80dae2c37b71baf4490dced9713

    menu:
        "Skip class":
            jump meet_badboy
        "Go to class":
            player_thinking "Yeah, I guess I should. That's what I'm paying for, anyway."
            scene outside campus 1 with fade
            player_thinking "I went to my only class of the day, which in itself felt like a massive undertaking. I can feel myself getting smarter, though."

    jump free_time_1

label meet_badboy:
    $ skipped_class = "true"

    player_thinking "You know what? I think I will skip class."
    player_thinking "I deserve a break! I'm only human, right?"

    scene outside campus 2

    player_thinking "I find myself taking a pleasant stroll throughout campus."
    player_thinking "Around me I hear people chatting, leaves gently rustling, the distant ringing of bells..."
    player_thinking "It's really peaceful. I'm glad I took this time for myself."
    player_thinking "Even if I might regret it later..."
    player_thinking "As I walk around, someone catches my eye."

    show badboy
    play music "music/acid-trumpet.mp3" loop fadein 1.0

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
    dom "You just moved in to Jane's place, right? [player_name], is it?"
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

    play music "music/easy-lemon.mp3" loop fadein 1.0

    jump free_time_1

# Free time choice, MAP UI interaction 
label free_time_1:
    
    player_thinking "I have some free time... what should I do?"

    call screen MapUI(1)

    return

label library_1:
    player_thinking "I'll go to the library. Maybe I can get some homework done."

    scene library

    player_thinking "I walk into the library, and look around for a good spot to study."

    if meetPrep == "true":
        player_thinking "However, I notice Victoria sitting in a dark corner of the library at a desk."
    else:
        player_thinking "I notice a girl I recognize sitting in a dark corner of the library at a desk."
        player_thinking "I realize it's Victoria, one of Jane's friends. She showed me a picture of her last night."


    player_thinking "She has earphones in and looks really focused, with multiple textbooks, papers, and a laptop scattered about her desk."

    player_thinking "I probably shouldn't bother her... but seeing the look on her face if I startle her might be worth it."

    menu:
        "No scaring today. Go in front of her and wave.":
            $ prepPoints += 1
            player_thinking "Nah, I don't fee like messing with her. It's probably not a good idea anyways."

            player_thinking "I walk closer to her desk and give a little wave. She surprisingly looks up at me, smiles, and takes out her earphones."

            show prep

            victoria "Hey, [player_name]! Long time no see!"

            player "Hi, Victoria! You look like you're hard at work. I don't mean to bother you. I'm here to study, myself."

            player_thinking "Her face lights up at this. I haven't seen her this happy to see me before."

            victoria "Oh, it's no problem! I'm studying for my public policy test. Have I ever told you that I'm majoring in political science?"

            player_thinking "She pats the chair next to her. I have a seat and set my backpack down."

            player "I don't think you have. I have some homework to do, do you mind if I do it here?"

            victoria "Not at all!"

            player_thinking "For a couple hours, we sit together and study our respective materials, occasionally exchanging small-talk and glancing over at one another. A few of her glances linger."

            victoria "Sigh."

            victoria "Alright, I think I'm done for the day. I'm going back to my dorm, I have plans with my roommate tonight."

            victoria "It was fun studying with you, let's do it again sometime!"

            player "Yeah, let's!"

            hide prep

            player_thinking "I watch her walk away. I'm starting to think that we could becom friends! Or possibly more..."
        "Go behind her and scare her!":
            player_thinking "Yeah, why not? This will be hilarious!"

            player_thinking "She hasn't noticed me yet, so I sneak around the edge of the room until I'm behind her, and slowly approach."

            player "BOO!"

            player_thinking "I grab her shoulders."

            show prep

            victoria "Aaaah!"

            player_thinking "She screams and shoots out of her seat, nearly knocking over her desk."

            player_thinking "She immediately turns around."

            victoria "What the heck, [player_name]? What was that for!?!"

            player "I just thought it would be funny..."

            victoria "Well, you were dead wrong, buddy. No one's laughing at your stupid prank."

            victoria "Just go away."

            player_thinking "I notice she's rubbing her knee. She must've slammed it into the desk when she stood up."

            player_thinking "Oops."

            player_thinking "She huffs, turns away, and sits back down, not giving me any time to respond."

            hide prep

            player_thinking "I should probably go. I shouldn't have done that... that was mean."

            player_thinking "As I walk out of the library, I notice everyone is staring at me. I really messed up."

            player_thinking "I'll do my homework in my dorm room."

    jump halloween_party

# See Artist in school store (Andrea)
label schoolstore_1:
    scene school store with dissolve
    "You decided you need to buy some supplies at the school store."
    
    if knowAugust == "true":    
        "Walking around, you see a familiar figure."
        player "August?"
        show artist with dissolve
        if assholeToAugust == "true":
            august "[player_name]."
            hide artist with moveoutright
            menu:
                "Apologize":
                    player "August, wait!"
                    show artist with dissolve
                    august "Yes?"
                    player "I wanted to apologize about what I said during graphic design club. It was mean and you didn't deserve it."
                    august "Oh?"
                    player "I don't know why I said those cruel things, I promise I'm not usually like that."
                    august "Okay. It's okay... We all have bad days."
                    player "Yeah, but it's still no excuse."
                    $ assholeToAugust = "false"
                "...":
                    "You get your supplies and leave."
                    jump halloween_party
        else:
            august "[player_name]! It's lovely to see you again."
            player "It's lovely to see you too! What are you up to?"
            august "Oh! I was just buying some supplies for our next meeting, but then I got distracted by the snacks."
            player_thinking "They're funny in a kind of awkward way..."
            august "I was planning on getting some chocolates for Jane, she really likes them! I'll probably drop by your dorm at some point to give these to her."
            august "If that's okay with you, of course."
            player "Absolutely!"
    else:
        "You run into someone by the art supply section."
        show artist with dissolve
        "They seem to recognize you"
        august "Hey, you're Jane's new roommate, aren't you?"
        player "I am!" 
        august "My name is August! What's your name?"
        player "I'm [player_name]."
        august "It's really nice to meet you!"
        $ knowAugust = "true"

    # If you just met them, if you were nice to them, and if you apologized.
    player "Are you and Jane close?"
    august "We are! At least, I think we are. We've only been close for a little bit, but I consider her one of my best friends!"
    player_thinking "They look embarassed..."
    menu:
        "Well, I should get going":
            player "It'll be a long day tomorrow, and I should really go work on my homework."
            august "I completely understand, it was nice seeing you!"
            player "It was nice seeing you too!"
            jump halloween_party
        "I can tell you're a good friend":
            pass
    august "I! Well, I try to be! She's a wonderful friend to me, too."
    player "She told me you take really good care of her."
    august "Well... I haven't had that many friends in my life. I just want to make sure the friends I have are happy."
    player "You haven't had many friends?"
    august "No, I'm not sure why... People make me really nervous sometimes."
    player "You don't seem nervous right now."
    august "...I'm not. I'm really comfortable around you, I don't know why."
    menu:
        "I'm glad you feel that way. I feel that way about you, too":
            august "I guess that means we have to be friends!"
            player "Yeah, I guess so!"
            $ artistPoints += 1
        "I just try to be nice and honest.": 
            august "I wish more people were like you then."
            $ artistPoints += 1
        "You just met me.": 
            august "...Yeah. You're right."

    "August's phone starts ringing."
    august "It's my mom. I should take this. It was nice seeing you, though, [player_name]."
    player "It was nice seeing you too."

    hide artist with fade

    jump halloween_party


label tenniscourts_1:
    scene tennis courts
    play music "music/windswept.mp3" loop fadein 1.0
    player_thinking "I make my way over to the school tennis courts."
    player_thinking "They're pretty huge, with multiple courts arranged in a grid. I walk onto one that isn't occupied and can see some other students having an intense game past the gate in front of me."
    player_thinking "The fresh air is pretty nice."
    stop music

    if skipped_class == "true":
        dom "Well, well, well, if it isn't the rule-breaker! You skipping class again?"
        player_thinking "That voice sounds familiar..."
        show badboy
        play music "music/acid-trumpet.mp3" loop fadein 1.0
        player_thinking "It's Dominic!"
        player "No! I just had some free time, actually. I haven't seen the tennis courts before..."
    else:
        dom "Hey! You're Jane's friend, right? What're you doing here?"
        player_thinking "Who could that be?"
        show badboy
        play music "music/acid-trumpet.mp3" loop fadein 1.0
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

    if skipped_class == "true":
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

    player_thinking "Is that Finley hanging out with Jane?"

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

    #####################################################################
    #
    #  FOURTH SCENE: Halloween Party 
    #  > If Haunted House -- see bad boy & tsundere
    #  > If Pumpkin Patch -- see prep & artist
    #
    #####################################################################

label halloween_party:
    scene autumn trees with fade
    play music "music/the-show-must-be-go.mp3" loop fadein 1.0
    player_thinking "It's Halloween and the festivities on campus are kicking in."
    player_thinking "I'm surprised how many people are walking around in costume. I feel a little out of place."
    player_thinking "I don't have any classes today, so I might as well have some fun."
    player_thinking "There's a haunted house attraction, and I heard there's a pumpkin patch nearby..."
    player_thinking "Where should I go?"

    menu:
        "Haunted house":
            jump haunted_house
        "Pumpkin patch":
            jump pumpkin_patch

label haunted_house:
    scene haunted house outside
    play music "music/dark-times.mp3" loop fadein 1.0

    player_thinking "So this is the haunted house... I wonder if it's actually scary..."
    player_thinking "Wait, those two look familiar."

    show badboy at left with easeinleft
    show finley at right with easeinright

    dom "C'mon, don't be such a wuss! Let's go in!"
    finley "Says the man willing to do any dare."
    dom "And what about it?"
    player "Hey! What's going on?"
    finley "Dominic wants to go to the haunted house, but I don't want to be the minority character at the start of a horror movie."
    dom "Finley here is a wuss who thinks they might actually die if they go inside."
    player "I mean, it does look pretty scary..."
    finley "What do you think, [player_name]? Would you go in?"

    menu:
        "Side with Dominic":
            $ badboyPoints += 1
            $ haunted_house = "true"
            player "Yeah, why not? It looks fun."
            player_thinking "A smug grin overtakes Dominic's face, showing the triumph he feels over coming out the victor in this battle."
            player_thinking "Finley looks like they're over it."
            dom "See? [player_name] has the right idea."
            dom "So, whaddaya say we go inside?"
            player "Let's do it!"
            finley "Have fun possibly dying. I'm gonna get some food."
            player_thinking "And with that, Finley unbothered-ly vacated the haunted house premises."
            hide badboy
            hide tsundere
            scene haunted house inside
            play music "music/unseen-horrors.mp3" loop fadein 1.0 fadeout 1.0
            player_thinking "As I enter the haunted house, I suddenly question the choice I made."
            player_thinking "It's dark, weird noises are coming from everywhere, the decorations are spooky..."
            player_thinking "Dominic still has that smug look on his face as always. I think he's having a good time."
            player_thinking "Suddenly, someone with zombie makeup jumps out at both of us and yells Boo!"
            player "Eeeeeek!"
            dom "Haha! Gonna have to do better than that, kid!"
            player_thinking "I clung closely to Dominic as we traversed through the rest of the house, with ghosts and ghouls meeting us at every corner."
            player_thinking "Hearing him laugh made my uneasiness fade and by the end of it, I was laughing along with him."
            player_thinking "I think I might have caught him blushing, too..."
            scene haunted house outside
            show badboy with dissolve
            play music "music/acid-trumpet.mp3" loop fadein 1.0
            dom "Well, that was as stupid as I expected."
            player "Yeah. Totally."
            dom "What? You were totally scared!"
            player "They got me the first time, yeah, but the scares got kinda cheap after that..."
            dom "Well, as corny as it was, it was pretty fun."
            dom "Finley missed out, for sure. Probably eating cheese curds somewhere."
            dom "We should do more stupid stuff together sometime."
            player "Oh. Yeah, I'd like that."
            dom "Sweet. Catch you later, nerd!"
            hide badboy
            stop music
            player_thinking "Aaaaaaaaand he's gone..."
            player_thinking "I think this was a Halloween well spent."
        "Side with Finley":
            $ tsunPoints += 1
            $ haunted_house = "false"
            player "No, you have a point, Finley. Why risk it?"
            player_thinking "Are they blushing? Dominic looks tired."
            if subj_pron == "they":
                finley "See, [subj_pron] get it. You have fun, Dominic."
            else:
                finley "See, [subj_pron] gets it. You have fun, Dominic."
            dom "You know what? I think I will. See ya later, losers!"
            hide badboy
            player_thinking "And with that, Dominic almost comically skipped off into the darkness past the haunted house's front door."
            finley "I'm going to go get some cheese curds. You want to come or not?"
            player "Sure!"
            hide tsundere
            play music "music/the-show-must-be-go.mp3" loop fadein 1.0
            scene autumn trees
            show finley
            player_thinking "I think the cheese curd stand isn't far from here. Finley is coming back from the counter with two baskets of cheese curds."
            finley "Here, you want one? Not that I bought you some for siding with me or anything. It was just cheaper to buy two."
            player "Sure, thanks. We should probably find somewhere to eat these."
            finley "That tree over there looks pretty comfy. Let's head over there."
            player_thinking "I take a bite of my cheese curds and taste the gooey goodness of the cheese wash over my taste buds."
            player_thinking "I look over to see Finley with their eyes closed and the softest smile on their face."
            finley "What are you looking at?"
            player "Nothing. These cheese curds are really good."
            finley "Of course they are. It's fried cheese. What could be better?"
            finley "I guess we should probably go make sure Dominic isn't dead."
            hide tsundere
            player_thinking "As we head back to the haunted house, I can still see Finley's soft smile in my head."
            stop music

    jump free_time_2

label pumpkin_patch:
    "Pumpkin patch event"

    menu:
        "Side with prep":
            pass
        "Side with artist":
            pass
    jump free_time_2

    #####################################################################
    #
    #  FIFTH SCENE -- Free Time pt. 2 
    #    > If library == see tsundere
    #    > If school store == see bad boy
    #    > If tennis court == see prep
    #    > If dorm == see tsundere
    #
    #####################################################################

label free_time_2:
    scene dorm room with fade
    play music "music/on-my-way.mp3" loop fadein 1.0

    if haunted_house == "true":
        player_thinking "It's been a week since I went to the haunted house with Dominic."
    elif haunted_house == "false":
        player_thinking "It's been a week since I hung out with Finley."
    else:
        player_thinking "A week has passed since Halloween. I find myself still missing all of the festivities."

    player_thinking "I wish it could be Halloween every day..."
    player_thinking "Well, I have some free time now. What should I do?"

    call screen MapUI(2)

    return

label library_2:
    #"LIBRARY 2: TSUN IS THERE"


    scene campus library  #is this the scene call for the library

    show finley

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
    player_thinking "I might as well head to the school store and shop with my eyes."

    scene school store

    player_thinking "As I peruse the various university-branded whats-its and thingy-mabobs, I spot a familiar figure in the corner of my eye..."

    show badboy
    play music "music/acid-trumpet.mp3" loop fadein 1.0

    player_thinking "...now, directly in front of my eyes."
    player "Dominic!"

    if skipped_class == "true":
        dom "Hey, class-skipper!"
    elif club == "debate team":
        dom "Hey, debate team!"
        player_thinking "Eh? He knows I'm on the debate team?"
    elif club == "graphic design":
        dom "Hey, graphic designer!"
        player_thinking "Eh? He knows I'm in the graphic design club?"
    else:
        dom "Hey, Jane's roomie!"

    player "What're you doing here? Doesn't seem like a place I'd find you at."
    dom "Why not? Where else would I shoplift, if not at a shop?"
    player "You shoplift?!"
    dom "Pfft! Duh! I've got an image to maintain!"
    player_thinking "I can't tell if he's joking..."
    dom "Whaddaya say? Wanna help me pocket a keychain without getting caught?"

    menu:
        "Yeah! Let's do it!":
            dom "Whoa, really? Well, alright! Stand in front of me but don't look suspicious."
            player_thinking "I can't believe I'm doing this... those keychains are overpriced, anyway, I suppose."
            player_thinking "I stood with my back to Dominic, doing my best to not look shady, but no doubt failing."
            dom "Got it. Good work, comrade."
            player "Do you even want the thing? It's not even a cool keychain..."
            player_thinking "His face got really serious, but I can't tell if it was an act or not."
            dom "It's not about the keychain. It's about the message."
            player_thinking "Almost magically, his face returns to its comfortable smug state."
            dom "Well, time for me to dip."
            dom "You should probably pay for that keychain in your pocket, though."
            player_thinking "I stick my hands in my pockets and sure enough, there's a keychain in one of them."
            player "What? How did you--"
            dom "Toodle-loo!"
            hide badboy
            player_thinking "Aaaaand he's gone..."
            player_thinking "Guess I better put this keychain back before I leave."
        "What? Of course not!":
            $ badboyPoints += 1
            dom "Haha! Don't want to make yourself an accomplice... no matter. I am a professional, anyhow."
            dom "Aaaand done."
            player "What?! You didn't even move!"
            dom "Sure I did. But to your untrained eyes, I remained completely still."
            player_thinking "Again, I have no idea if he's being serious."
            player_thinking "If he did snag it, his sleight of hand skills are really impressive..."
            player_thinking "...he must be great at card tricks..."
            dom "Well, my work here is nearly done."
            dom "There's one last thing I have to do. Come with me to the register?"
            player "Uh... sure."
            hide badboy
            player_thinking "I awkwardly follow him to the cash register, a little confused."
            player_thinking "My confusion grew when he produced a keychain from one pocket and his wallet from the other."
            show badboy
            dom "Smell you later, nerd. Also -- check your pocket."
            player "Check my pocket?"
            hide badboy
            player_thinking "Aaaand he's gone."
            player_thinking "I stick my hands in both of my pant pockets and felt something small and metallic."
            player_thinking "Confused beyond belief, with a shiny new keychain in my hand, I stood there, dumbfounded."
    
    stop music
    player_thinking "I'll never understand that guy."
    
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
    scene bedroom
    play music "music/easy-lemon.mp3" loop fadein 1.0

    player_thinking "BRRING BRRING..."
    player "... another great day to be alive."
    player_thinking "I sleepily rise from bed and look at the time. I really don't feel like going to class today."
    player_thinking "I should..."

    menu:
        "Sleep in":
            $ slept_in = "true"
            player "I won't die if I miss this one class."
            if skipped_class == "true":
                player "I probably shouldn't make this a habit, though... I think Dom's rubbing off on me..."
            player_thinking "Before I'm able to crawl back into bed, I hear a knock at the door."
        "Get out of bed":
            $ slept_in = "false"
            player "Yeah, I guess I better get out of bed."
            player "Life is hard, being a human with obligations."
            player_thinking "Before I take even one step out of my room, I hear a knock at the door."

    stop music
    player_thinking "That's weird. That can't be Jane. She has a key, and would text if she lost it."
    player_thinking "Jane's also in class right now, so she wouldn't have a friend over, right?"
    player_thinking "Who could that be...? Are we expecting maintenance or something?"

    menu:
        "Answer the door":
            $ answered_door = "true"
            scene dorm room
            player_thinking "A little apprehensively, I rise from bed, still in my pajamas, and make my way to the front door."
            player_thinking "I'm really curious who it could be... is it a prank?"
            player_thinking "I open the door to a unexpected familiar face."
            show badboy
            play music "music/acid-trumpet.mp3" loop fadein 1.0
            dom "Hey, does [player_name] live here?"
            player "Um... no."
            dom "Oh. Bummer. I'll try all the other rooms, then."
            player "Er... did you need something? Jane's in class."
            dom "Yeah, I have some hooligan errands I need to run and could use some assistance. You in?"
            player "Like, what kind of 'hooligan errands'?"
            dom "Like loitering outside the lecture halls. If I don't, who else will?"
            player_thinking "Is this how he asks people to hang out? 'Hey, come loiter with me'?"
            player "That's a fair point. Let's go!"
            hide badboy
            jump dominic_event_1
        "Ignore it":
            $ answered_door = "false"
            if slept_in == "true":
                player "Maybe they have the wrong dorm. I'll pretend I didn't hear that."
                player_thinking "Time to enjoy some shut-eye!"
            else:
                player "Er... I really don't wanna get that..."
                player_thinking "I'll get ready and leave once I think they're gone."

    # Time skip to MC with Jane at the dorm

    scene dorm room with fade
    play music "music/on-my-way.mp3" loop fadein 1.0
    show roommate happy

    roomie "Hey, were you here around 10 this morning?"
    player "Yeah, why do you ask?"
    roomie "Dominic sent me a text earlier saying that he came by but no one answered the door."
    roomie "I was in class, but I thought you were still home."
    player_thinking "Oh, shoot... what should I say?"
    menu:
        "I didn't hear it":
            player "Oh, really? I didn't hear anything. Maybe he came while I was still asleep."
            roomie "Haha! Bummer. He's the spontaneous type, as you've probably noticed."
            roomie "I'll let him know to knock louder next time."
        "I didn't know who it was":
            player "Er... I didn't answer because I thought it was a stranger..."
            roomie "Haha! You're a careful one. It was just Dom."
            roomie "He seemed kinda bummed. I'll tell him to text next time, but he's not one to plan things."

    roomie "Oh, well. Don't tell him I said this, but I think he likes you."
    player "Huh? What makes you say that?"
    roomie "I don't know. It's just a feeling. I could be wrong."
    roomie "I think you remind him of someone..."
    roomie "Well, I've got some cramming to do. Talk to you later?"
    player "Yeah, me too. Catch you later."

    hide roommate happy
    stop music
    scene bedroom with fade

    player_thinking "I remind him of someone?"
    player_thinking "I sat on my bed and did some homework, but it was difficult to concentrate."

    jump dominic_event_2

label dominic_event_1:
    scene outside campus snow
    # play music 

label dominic_event_2:

    scene outside dorms snow with fade
    play music "music/past-sadness.mp3" loop fadein 1.0

    player_thinking "Man, it's cold outside. But it's kinda nice."
    player_thinking "With the end of the semester approaching, I've had a lot on my plate."
    player_thinking "The cold air is refreshing."
    player_thinking "After a couple minutes of aimless vibing, I take a look around and see a familiar figure a few feet away."
    player_thinking "I walk over to him, leaving footprints in the snow behind me."
    
    show badboy

    player "Hey, Dominic. What are you doing here?"
    player_thinking "I catch him a little off guard. Something seems different about him."

    dom "Oh, hey, [player_name]. Loitering, of course."
    player_thinking "He doesn't have the usual smugness about him."
    player_thinking "We stand in silence for a bit. It's more comforting than awkward."
    player_thinking "After stewing in the silence, Dominic sighs."

    dom "... you know, I wasn't always like this."
    player "What do you mean?"
    dom "I wasn't always this... fake edgy 'hooligan' type I make myself out to be."
    player_thinking "His voice's usual brightness is gone, along with the default smirk on his face."
    player_thinking "For the first time, I'm certain he's serious."
    player "Well, what were you like before?"
    player_thinking "He ponders that question for a moment, takes a short breath."
    dom "I was... a 'good kid'. I think. As good as I could be compared to my brother, at least."
    dom "Then I stopped trying because it felt pointless. All work, no play wasn't working for me."
    dom "Heh. I'm surprised I'm still in school, to be honest. I think part of me still doesn't want to disappoint my folks."
    player "Huh. I can't imagine you being that different."
    player_thinking "Suddenly, the familiar smirk returns to his face."
    dom "Can't picture me being a hard-worker, eh? I guess I have changed a lot."
    player_thinking "Snowflakes begin to gently fall onto us."
    player_thinking "Is this the real Dominic? Has he been putting on an act this whole time?"
    player_thinking "I wonder what's gotten into him..."
    
    if skipped_class == "true":
        dom "... you shouldn't skip class, [player_name]. Even if it's just one time, it's easy to keep doing it."
        player "Wow. You, telling me I should go to class? I'm shocked!"
        player_thinking "This whole time, I thought Dominic didn't have a care in the world. He must really care to give me advice like that."

    dom "[player_name]... do you think I did the right thing?"
    player "What do you mean?"
    dom "Becoming a delinquent...  should I have kept trying?"
    player_thinking "What should I tell him?"

    menu:
        "Do what makes you happy":
            player "I think life is too short to be perfect all the time."
            player "If trying to stack up against your brother was bringing you pain, I can see why you stopped trying."
            player "If that makes you happier, I say it was the right decision."
            player "Pleasing yourself is more important than pleasing others, right?"
            dom "Wow. That's what you think, huh?"
            dom "At least someone approves of my lifestyle..."
            player_thinking "Despite his words, he looks a little disappointed. Did I say the wrong thing?"
        "Do what's best for you":
            $ badboyPoints += 1
            player "Well... to be honest, I think you should have kept trying in school."
            player "Even if it's hard, or you feel like you're not good enough, it's for your own good."
            player "It's easy to do what makes you happy in the short-term, but harder to do what's best for the long-run."
            player "I'd love to skip class all the time and do what I want, but I know it'd only hurt me in the future."
            player "I can't tell you how to live your life, but that's the way I see it."
            dom "Hm."
            player_thinking "There's a silence. Was I too harsh?"
            dom "You know, only one other person has actually said that to me before."
            dom "Not even my parents told me that. There was no protest when I started getting in trouble. It was like they expected it."
            if answered_door == "false":
                player_thinking "One other person... could that be who Jane mentioned?"

    player_thinking "Dominic takes my hands in his and looks down at them pensively. My heart beats loud in my chest."
    player_thinking "He lets go and looks back up at me."
    dom "Thanks for the talk. I think I've done enough loitering today. It's pretty cold, anyway."
    player "No problem. Catch you later."

    hide badboy
    stop music

    player_thinking "That was weird... I've never seen that side of him before..."
    player_thinking "I hope he's okay."

    jump dominic_event_3

label dominic_event_3:
    play music "music/past-sadness.mp3" loop fadein 1.0
    scene outside campus snow with fade

    player_thinking "A couple weeks have passed since I spoke to Dominic in the snow."
    player_thinking "I've been thinking about the conversation we had. He showed a completely different side of him."
    player_thinking "I hadn't bumped into him anywhere on campus since then, which was odd. I normally see him hanging around somewhere."
    player_thinking "I texted him, but he hasn't replied..."
    player_thinking "I hope he's okay."

    scene dorm room with fade
    show roommate happy

    player "Hey, Jane, have you heard from Dominic recently?"
    roomie "Oh, you haven't heard?"
    player "Haven't heard what?"
    roomie "He got arrested. Tagged some buildings on campus."
    player "What?!"
    player_thinking "So that's why I haven't seen him..."
    roomie "Usually he gets left off the hook, but he actually got jailtime this time."
    player_thinking "How can she be so nonchalant about this? Has he been arrested before?"
    player "When does he get out?"
    roomie "Today, I think. I'm not sure."
    player "You're his friend and you don't know when he's getting out of jail?!"
    player "How are you so okay with this?"
    player_thinking "Jane looks a little embarrassed."
    roomie "I mean... that's just the way he is... You've noticed that by now, right?"
    player "Just because that's how he is doesn't make it right!"

    scene bedroom with fade
    player_thinking "I head to my room to think. I take out my phone and text Dominic right away."
    player_thinking "Is this really so normal for him? He must have been caught a lot for them to actually arrest him this time."
    player_thinking "I need to talk to him."

    scene outside dorms snow with fade
    player_thinking "It's the next day. Dominic texted me back and said he'd meet me by the dorms."
    player_thinking "After waiting a bit, I see him approaching in the distance."

    show badboy

    dom "What's up, [player_name]? You wanted to see my beautiful face?"
    player_thinking "There's the smug Dominic I've come to know."
    player "Hey. Are you okay?"
    dom "Never been better. Why?"
    player "Why? Jane told me you were in jail for the past two weeks!"
    dom "Oh, that? Yeah, it happens. What did you think the life of a bad boy was like?"
    player_thinking "He's put up this joking, teasing wall around himself again. Months ago, I would have played along, but this has gone too far."
    player "I need to say something to him..."

    menu:
        "Stop causing trouble":
            $ badboyPoints += 1
            player "I know your whole thing is that you do what you want and you like breaking rules, but I think it's getting out of hand."
            player "Having a criminal record isn't a minor thing, Dom. It can only get worse from here."
            player "If you get caught breaking the law again, they could lock you up for even longer..."
            player "I was worried. Jane didn't even seem concerned when she told me."
            player_thinking "The smug air around him faded. The facade, the mask he puts on, finally came off."
            player_thinking "He looked down at the ground, like a dog with its tail between its legs."
            player_thinking "Why am I scolding a grown adult? Did I overstep?"
            dom "You know, [player_name], you remind me a lot of this buddy I had growing up."
            dom "He was my best friend in school. I messed with him all the time because his reactions were funny."
            dom "I never told him I was having a hard time. We only had fun together. I didn't want to have it any other way."
            dom "When I started skipping school, he was the only one who cared."
            dom "Not my parents, not my brother, only him."
            dom "Y'know, Jane and them, I don't blame them for not saying anything. They accept me as I am, and I'm grateful for it."
            dom "But you... you are the only person to tell me what I'm doing is wrong. And I know it's because you care."
            player_thinking "This is the most sentimental I've seen him."
            player "Do you still talk to your friend?"
            dom "No, he's not around anymore. Hasn't been since we graduated."
            player "Oh. I'm sorry."
            dom "Nothing to be sorry about. He wouldn't want me to be sad about it."
            dom "He passed in the winter, too. That's why I get so weird this time of the year."
            dom "You know... he called me Dom, too."
            dom "Anyway, thanks for that. I needed to hear it."
            player_thinking "As he starts walking away, he stops in his tracks and turns to look at me."
            dom "[player_name]. Never change, okay?"
            player "I'll try not to."
            hide badboy
            player_thinking "He laughs at this before giving me a wink and heading on his way."
            player_thinking "Now, I can see the difference between this happiness and the usual smugness he comes to me with. This one is genuine."
        "Don't get caught next time":
            player "Look, tag buildings as much as you want, but you really need to be more careful."
            player "And why would you do it on campus? There are always people around. No wonder you got caught."
            player "If you're going to cause trouble, you shouldn't be so careless about it..."
            player_thinking "Dominic cracked a chuckle, but one that almost sounded a little sad."
            dom "I've been doing this a long time, [player_name]. I know the risks, and I can't say I really care."
            dom "It happens all the time. Trust me, it's not a big deal. 'Preciate the concern, though."
            dom "I can't imagine how hard it must have been without having me around to mess with you."
            player_thinking "I'm not sure how to respond to that. It's like he's put up a wall that I can't break through."
            player_thinking "He awkwardly looks at his feet before sighing. Is he annoyed?"
            dom "Is that all you had to say?"
            player "Um... yeah."
            dom "Aight. Well, I've got some more tagging to do, so I'm gonna go."
            dom "I won't get caught this time, though. I'll be careful."
            player_thinking "He gives me a wink and starts walking away."
            dom "See ya around."
            hide badboy
            player_thinking "Maybe I'll never understand him..."

    jump FINAL_PARTY

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

            scene thrift shop #this should be another store

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
            player_thinking "Is Finley sad that I didn't choose the bad movie?"

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

label dominic_ending:
    pass
    # TODO graphic