# Ball Game

Ball Game is an assignment created to demonstrate competency with the client-server architecture. The rules of the game are shown below, where the client and server have been coded in Java and C#.

## Rules of the game:
### Client rules
- Each player gets a unique ID when they join, that won't be reused after they've left
- All players are notified when a new player joins
- One player has the ball and can pass it to any player (including themselves)
- Each player can see their ID, players and the ball holder

### Server rules
- The server shows a player joining/leaving the game (including their ID and the list of all players in game)
- The server shows who the ball is passed to if the ball holder leaves (displaying the new ball holders ID)
- The server shows players who pass the ball (including the ID of the source and destination of the passed ball)
- If the player with the ball leaves, it's passed to a random player; if the game is empty it's passed to the first person to join
