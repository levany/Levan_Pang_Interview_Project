Pang prototype
by levan beniashvilli



Bonuses :
------------------
i did all the bonuses :)



------------------------------------
Getting started :
------------------------------------
everything is in one scene : "PANG_MASTER_SCENE"
its in the folder "Assets/_Levan/_Main"

there is a script called "PangApp"
that is the root of everything



------------------------------------------------------------------------
top 10 things to know:
------------------------------------------------------------------------

1. i split the app to 2 folders : "App" and "Game"
   to show different approches for architecture.

   "App"  - menus and stuff - is more classicly suited for MVC
   "Game" - gameplay stuff  - still designed it on close to MVC but alittle more flexible as needed

2.  The basic flow is :

	1. "PangApP" is the main entry point and controller
	2. the main flow it starts is : "MainMenuController" > "GameController" > "LeaderboardsController"

3. the Game controller handles the actual game :

	1. Setup    : sets up all the managers, the stage, the mechanics, the levels, etc...
	2. gameplay : calls LevelController to handle each level
	3. Cleanup  : Disposes of what needs to be disposed (but storing things in object pools, so more efficiand if we play again)

4.  the Levelcontroller contoles a single Level (each time) :

	1. Loads the Levels  - (for easy edit - levels are scriptable objects with a list of prefabs and values that make the level) 
	2. starts playing
	3. Waits for the "LastBallKilled" event to know we won. or "Last Hero killed" to know we lost.
	4. Finishes and cleans up.
	
5.  mechanics and object :

	in this prototype we have :

	- "Player"s  = represents human user. and its data (name, score, ...). = NOT A GAMEPLAY OBJECT !
	- "Hero"s	 = The cute characters you controll
	- "Harpoon"s = the harpoon the heros shoot. (Wikipedia says that in the original pang its calld "Harpoon")
	- "Ball"s	 = The things that hurt you. They are called "Balls". But look like friuts. dont let them deceive you.

	- each of the above may also have managers and models aif needed.

6. "PangMechanics"

	- the c# objects each of the above DO NOT KNOW ABOUT EACH OTHER.
	  they emit and respond to events.

	- the "PangMechanics" controller is the only object that knows evryone involved.
	  it routs the events and manages the specific gameplay for "Pang"

	 but everything else is generic and can easlly makeup another game for example.
	 

7. Data :

	- Data is seperated from views. 
	- data object are serialzable scriptable objects and can easily be viwed and edited in the inspector.

	- there is a "StorageService" to store data locally.
	- there is s "SuperSimpleCloudService" to store data in the cloud.

8. Controllers : 

	- they do the things and maipulate the data.

	- "Controllers" = the basic objects that controll flow or controll on specific thing (such as Game, or level)
	- "Services"    = 'controllers' that provide common app services and utility, they do not know the context of the app. (e.g. StorageService)
	- "Managers     = 'controllers' that usually manage a collection of something. for simplicity. (e.g. "BallManager" \ "Hero Manager"). they hold object pools. they don't control flow.

9. Views :

	- they show the stuff.
	- they do not controll the stuff. they use events to call the thing that controll the stuff, and to refresh after stuff has been controlled.
	- They Use Data, but do not change the data. only show the data.


10. Cheats :

	- there are Cheats for easy testing. (they are self explenetory.)
	- enable or disable them in the AppSettings scriptable object :)
	- its easy to find in runtime : there is a reference to that object in the PangApp gameobject that is the root of the hirarchy :)
	


------------------------------------	
Notes :
------------------------------------

I've worked in many systems - some simple, some complex,
and this prototype is just one example of you you can do things.

there are more ways, and this prototype is a decent middle ground.

if you'll like to know more about my design decisions feel free to ask :)


------------------------------------
Known Isses :
------------------------------------
- Leaderboard sometimes doesnt update correctly
- I Disabled some object pools (like the one in the "BallManager" class because of bugs),  but i do recomend the use of object pools ! :)



------------------------------------
Requirements Checklist (from the exercise doc) :
-----------------------------------------
1. the game screen is adaptive
2. i used some art but kept it minimalistic
3. the game has a mai menu
4. it wirks on landscape
5. i used best practices in the architecture (hope you'll like it) and in performence (e.g. object pools and sprite atlases)
6. its really fun to play if you ask me :)
7. used latest LTS


---------------------------------------
Extra Credit Requirements (checklist) :
----------------------------------------
1. Desigen with MVC :)
2. three levels (and can easly make more) with increasing difficulty
3. used a shader for the Xray effect, used some custom visuals, created some myself.
4. levels have minimlistic music and SFX. menus do not - because its anoyying when you test a lot - hope youll like it :)
5. Two player same screen multiplayer , and its fun ! :)
6. Leaderboards + online leaderboards 

more extras :
- added a (VERY BASIC (!)) onlne storage service for the leaderboards :)
- created cute visuals and effects, and added squishy sound effects to make it fun.
- preety UI, i hade some UI assets that are cute.
- Cute animated title text :)
- The sound pitch increases when you pop them fruit balls ! ;)







