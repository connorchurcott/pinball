# **Pinball**

A pinball game built from scratch in Unity as a personal project

# **What is it?**

This is an ongoing solo project. The goal is to build a fully featured pinball game in Unity C#. Currently the game is functional but early in development, with placeholder visuals and no audio yet.
I have plans to add progression, and other rougelite mechanics instead of keeping this as a standard pinball game. 

# **What is Currently Implemented?**
- Flipper physics using Rigidbody2D with angular interpolation via MoveRotation for responsive, smooth feel
- Ball launcher with a moving timing bar and green zone detection, rewarding precise input with increased launch force
- Bumper collision system using UnityEvents, scaling score by incoming ball speed with coroutine based hit animations
- Scoring, ball count, game over, and restart systems managed through a singleton GameManager
- Component based architecture with clean separation between game state, UI, physics, and input logic
- Input handled through Unity's new input system

TLDR: It's essentially a completly working, standard pinball game right now

# **What is Coming Next?**
- More ball powerups
- Other stat based / table powerups
- Ability to select powerups for your run via some condition met
- Sound design
- Custom art (eventually)

# **Most Recent Update!**
-  fixed some table generation glitches including: bumper size and amounts, and launcher active during screen transition
-  custom sprites for normal ball and multiball ball

