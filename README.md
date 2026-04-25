# Hangman

This is a classic Hangman game developed in C# and WPF (Windows Presentation Foundation), following the MVVM (Model-View-ViewModel) architecture.

## Features
* **User System:** You can create a new profile and choose an avatar from the available options. The profile will save your data.
* **Word Categories:** You can play a specific category (Animals, Countries, Movies, Fruits, Cars) or play with all of them mixed.
* **Game Mechanics:** The game includes a timer and a level system. You have a limited number of allowed mistakes before the character gets "hanged".
* **Save/Load:** If you don't have time to finish a game, you can save your progress and continue later using the menu shortcuts.
* **Statistics:** The application keeps track of the games played and won for each individual category.

## Technologies Used
* C#
* .NET 8.0 (Windows)
* WPF (Windows Presentation Foundation)
* MVVM (Model-View-ViewModel) Architecture

## How to Run the Project
1. Make sure you have **Visual Studio** installed (2022 recommended) with the ".NET desktop development" workload.
2. Clone the repository or download the source code archive.
3. Open the `Hangman.sln` file in Visual Studio.
4. Build the solution and press `F5` (or the Start button) to run the game.

*(Note: The words for the game are read from the `Data/words.txt` file, and user profiles, along with their statistics and saved games, are stored locally in a `users.json` file.)*
