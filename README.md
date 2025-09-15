# Bubbles Prototype

This project started as a **recruitment assignment** for a game dev role.  
The goal was to build a grid-based bubble game with some special mechanics and scoring rules.

---

## Time

I went over the planned time, finishing the whole project in around **14.5 hours**.  
If you want to see what fits into a 10-hour limit, check out commit `0982dbc53b82ab6c959ce3f3964bf8215406c191`.  
At that point, I had a basic working version + adding new colors every 50 points.  
I initially planned to implement point 1 first, so most of the code was written with that functionality in mind.  
Eventually, I added it after the main timebox.

---

## Design

I wrote everything so that **all values can be tweaked in the editor**. Some numbers are scattered across the project, but they are editable and the game should still work.  

The code is designed to be extendable in the future. I didn't focus heavily on optimization, but I avoided unnecessary operations.  
For example, when checking for 5+ bubbles in a line, I only check around the newly spawned bubble instead of scanning the whole grid.  

I also used **object pools** everywhere, so no unnecessary Instantiation. There are some abstractions that could be simplified to squeeze more performance, but I prioritized flexibility.

---

## Default Indicators

I didn’t want to spend too much extra time, so the game doesn’t have clear hints for all bubble types. Graphics were done quickly, so it might take a moment to understand what each bubble is.  
Quick reference:

- White star bubble – has a special ability  
- Explosion bubble – red  
- Memory bubble – blue  
- Monument – black trapezoid  
- Luck bubble – green  
- Rainbow bubble – rainbow  
- Problem bubble – yellow  
- Locked bubbles – black outline  
- Imp bubble – magenta  
- Imp character – purple (don’t look too closely xD)  
- Next imp move – purple diamond  
- Harvester bubble – gray (be careful, no special visual indicator; you pick the target when activated)  

---

## Edge Cases

There are quite a few, especially on the largest map: spawning 10 new bubbles per turn, each with 100% ability chance, requiring 3 in a line to clear.  
You have to watch out for harvesters, as shots can stack and sometimes it’s not obvious when a bubble is cleared.  

Some design decisions:

- **Imp blocking:** The assignment didn’t specify what happens if the player tries to block the imp’s next position. I decided that the next imp location is blocked, which is both the easiest to program and makes sense gameplay-wise.  
- **Spawning / game over:** I chose to spawn all 3 new bubbles at once, then check for lines. The alternative would be checking after each individual spawn. The difference mostly matters at the end of the game. Using my approach, a game over happens when there’s no room to spawn 3 bubbles. This might cause some lines of 5 to appear visually just before the end screen, which could confuse players a bit, but most users probably won’t notice.  

---

This is a **prototype**, not a polished game. The main focus was flexibility, keeping the game editable, and showing that the mechanics work intuitively.
