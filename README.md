A turn-based logic puzzle game implemented in Unity. Two players (or Player vs AI) compete on a grid, placing numbered chips to form scoring combinations.

The game is played on a 6x6 grid.
- Players take turns placing chips.
- Each turn, a player gets 3 random numbers (1-7) to choose from.
- Points are awarded for placing chips that form specific patterns with adjacent cells:
    * Sum = 7 (e.g., 3 + 4)
    * Sum = 11 (e.g., 5 + 6)
    * Three "1"s in a row

When the grid is full, the player with the highest score wins.


Technical Highlights
* **Grid System:** Custom 2D array logic for managing cell states and checking adjacencies.
* **Pattern Matching Algorithm:** Recursive search algorithm to detect scoring combinations (horizontal/vertical) instantly after placement.
* **AI Opponent:** Implemented a bot that evaluates available moves to block the player or maximize its own score.
* **UI Architecture:** Dynamic UI updates decoupled from the game logic (Observer pattern / Events).
