Team name: Team Dating Sim

Team members:
* [Amela Aganovic (Project Manager, Developer)](https://github.com/aganovia/CIS350-HW2-Aganovic)
* [Samantha Knight (Developer)](https://github.com/knigsam/CIS350-HW2-Knight) 
* [Alexis Webster (Developer)](https://github.com/lexiwexie01/CIS350-HW2-Webster)
* [Andrea Mantke (Developer)](https://github.com/neptunehues/CIS350-HW2-Mantke)

# Introduction

Our team will develop a dating simulation game. The player has the option of dating one (or more) of several romanceable characters, with the game’s story progressing in different directions depending on which options are selected, resulting in multiple possible endings.

The player will be able to choose their gender pronouns, with the game featuring a diversely gendered cast. In addition to multiple story branches and character customization, the game will also include mini-games, an interactive world map, a gallery with unlockable visuals, unlockable “secret” routes, and a simulated time mechanic, allowing for multiple unique experiences and replayability.

# Anticipated Technologies

We will develop our project using the Ren’Py visual novel engine.
For creating our own game assets, we will use art/photo manipulation software such as Clip Studio Paint or Paint Tool SAI.
We will use GitHub/GitHub Projects to track our work.

# Method/Approach

Our team will follow an agile approach for development. We will meet at least once per week to discuss what we worked on, what we had trouble with, and what we will work on.

We will create a GitHub Projects board to store work items, or user stories, in order to track development. Work items will be sorted by completion progress (To-Do, In Progress, Completed). Completed work items will ideally be tested both by the team member who completed it and at least one other team member who did not.

Towards the beginning of the project, the primary goal will be to map out the story and structure of the game before any programming or implementation takes place. Once we have a skeleton story planned, we will begin to implement the different routes programmatically. We will likely start development using placeholder and/or royalty-free assets before creating custom game assets later in development.

# Estimated Timeline

## Phase 1 (October 2)
* Structure of main story planned
* Structure of routes and endings planned
* Main characters designed

## Phase 2 (October 16)
* Basic game flow implemented (clicking the screen/choosing dialogue options progresses the story)
* Fully functional menus (main menu, gallery)
* Save/load functionality
* Chat history window
* 25+% of story implemented

## Phase 3 (October 30)
* Player attribute questionnaire implemented
* Texting/messaging system implemented
* Interactive world map implemented
* “Time mechanic” implemented
* Mini-game with “hotspot” functionality added
* Persistent data implemented (ex. unlockable routes, gallery items)
* 50+% of story implemented

## Phase (November 12)
* Intro cutscene upon starting game implemented
* Drag and drop functionality/mini-game added
* File manipulation mechanic implemented
* Explore possibility of exporting game to web/mobile
* 75+% of story implemented

## Phase 4 (End of November)
* 100+% of story implemented
* Custom assets finished and implemented (character sprites, custom menus, etc.)
* Custom animations/visual indicators of favorable responses implemented
* Complete end-to-end testing/validation

# Anticipated Problems

No one in our group has experience with the Ren’Py engine, so time will be spent getting familiar with the Ren’Py language and its capabilities.

Our group also has varying experience and comfort levels with using Git. We may have to address merge conflicts and conflicting versions of the game’s “current” build among our four members. The goal is to pull and push often and keep communication open in order to avoid these situations.

Keeping different story routes in check may be a challenge depending on how we approach it. For instance, if we use variables to keep track of “favor points” between romanceable characters, we will need to account for all possible scenarios regarding whether or not the points meet or exceed certain thresholds, or whether or not different characters have the same number of “favor points".