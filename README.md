### Minesweeper ###

This project is an inmplementation of Minesweeper game using C#
it includes a fully interactive console UI, a game engine, high scores,
and full unit test coverage.

## How to buld and run the game 

**1 Restore dependencies**
From the solution root:
dotnet restore

**2 build the solution**
dotnet build

**3 Run the consle game**
Make sure Minesweeper.Console is the startup project.
Then hit the green start button at the top or use 
dotnet run --project src/Minesweeper.Console

## How to play
When the game starts you will select a board size 
1. Small (8x8)
2. Medium (12x12)
3. Large (16x16)
OR
Q. Quit

## Seed Usage 
After choosing a board size you will be prompted 
to enter a seed value 
**Enter returns a random seed**
**Enter a number to generate a specific board**

## Input Commands 
**r row col' - reveal the tile at (row, col) ex 1,2**
**f row col' - flag the tile at (row, col) ex 1,2**
**q - quit the game**

## Board symbols 
**# - Hidden tile**
**F - Flagged tile**
//* - Mine shown only after losing.//
** . - Revealed empty tile with no adjacent mines**
**1-8 - Revealed tile with the number of adjacent mines**

## High Score System 
**High scores are stored in data/highscores.csv**
**CSV Format**
**Each row containts : size, seconds, moves, seed, timestamp**
**EX Small8x8.42.18,1234,2024-04-08T12:34:33Z**

## Rules 
** Only the top 5 scores per board size are kept.**
** Sorted by seconds and moves.**

## Running Unit Tests
**From the solution root:**
dotnet test
**Runs all test in test/Minesweeper.Tests**
**Test include : Board generation, Adjacency count, Cascade reveal,
Win/lose rule, Seed determinism

## Project structure 
src/
	Minesweeper.Console/  # Console UI project
	Minesweeper.Core/     # Game engine and logic
tests/ 
	Minesweeper.Tests/    # Unit tests for the game engine
UML/
	MineSweeperUML.drawio #UML diagram of the game engine
