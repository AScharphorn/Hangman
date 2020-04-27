using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hangman
{
    // ********************************************************
    //
    // Title: Hangman
    // Description: Two Player game of Hangman where one person
    //              chooses the word or phrase and the other
    //              has to guess it one letter at a time.
    // Application Type: Console
    // Author: Scharphorn, Austin
    // Dated Created: 4/24/2020
    // Last Modified: 4/26/2020
    //
    // ********************************************************

    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        static void DisplayMainMenu()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Start New Game");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        GetWordOrPhrase();
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void SetTheme()
        {
            //
            // Set the theme of the application
            //
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
        }
        #endregion

        #region Game
        static void GetWordOrPhrase()
        {
            bool gameCompleted = false;
            bool choiceConfirmed = false;
            bool isInt = false;
            bool isSpecial = false;
            int incorrectGuesses = 0;
            string userResponse;
            string letterGuessed = "";
            string[] text = new string[0];
            string[] guessedLetters = new string[26];

            do
            {
                DisplayScreenHeader("Enter Word");

                //
                // Get word or phrase from the user and split it
                //
                Console.WriteLine("Please enter a word or phrase.");
                Console.WriteLine();
                text = Regex.Split(Console.ReadLine().ToLower(), string.Empty);


                //
                // Check for special characters and numbers in word or phrase
                //
                foreach (string character in text)
                {
                    isInt = character.Any(c => char.IsDigit(c));
                    isSpecial = character.Any(p => !char.IsLetterOrDigit(p));

                    if (character.Equals(" "))
                    {
                        isSpecial = false;
                    }

                    if (isInt == true && isSpecial == true)
                    {
                        Console.WriteLine("Please do not use numbers or special characters.");
                        DisplayContinuePrompt();
                        break;
                    }
                    if (isInt == true)
                    {
                        Console.WriteLine("Please do not use numbers.");
                        DisplayContinuePrompt();
                        break;
                    }
                    else if (isSpecial == true)
                    {
                        Console.WriteLine("Please do not use special characters.");
                        DisplayContinuePrompt();
                        break;
                    }
                }
                Console.Clear();

                if (!isInt && !isSpecial)
                {
                    do
                    {
                        DisplayScreenHeader("Confirm Word/Phrase Choice");

                        //
                        // Confirm the choice with the user
                        //
                        foreach (string character in text)
                        {
                            Console.Write(character);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Is this the word or phrase that you would like to use?");
                        userResponse = Console.ReadLine().ToLower();

                        if (userResponse == "yes")
                        {
                            choiceConfirmed = true;
                        }
                        else if (userResponse == "no")
                        {
                            Console.WriteLine("Then please go back and make the changes that you would like.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter 'yes' or 'no'");
                        }
                        DisplayContinuePrompt();
                    } while (userResponse != "yes" && userResponse != "no");
                }
            } while (!choiceConfirmed);

            do
            {
                letterGuessed = GetUserGuess(letterGuessed, text, guessedLetters);

                guessedLetters = ProcessUserGuess(letterGuessed, text, guessedLetters);

                incorrectGuesses = GetIsCorrectAnswer(letterGuessed, text, incorrectGuesses);

                DisplayHangmanStand(incorrectGuesses);

                DisplayIncorrectLettersGuessed(text, guessedLetters);

                DisplayWordOrPhraseProgress(text, guessedLetters);

                gameCompleted = DisplayGameEnd(incorrectGuesses, guessedLetters, text, gameCompleted);

                Console.SetCursorPosition(20, 28);
                DisplayContinuePrompt();
            } while (!gameCompleted);
        }

        static string[] ProcessUserGuess(string letterGuessed, string[] text, string[] guessedLetters)
        {
            int numberStore = 0;

            //
            // Adding the users guess to an array
            //
            foreach (string character in guessedLetters)
            {
                if (guessedLetters[numberStore] != null)
                {
                    numberStore++;
                }
            }

            guessedLetters[numberStore] = letterGuessed;

            return guessedLetters;
        }

        static bool DisplayGameEnd(int incorrectGuesses, string[] guessedLetters, string[] text, bool gameCompleted)
        {
            string final;
            int numberStore = 0;
            int storedNumber = 0;
            int wordPhraseLength;

            //
            // Determining if the user has won or lost
            //
            if (incorrectGuesses >= 6)
            {
                Console.WriteLine("Sorry you have lost.");
                gameCompleted = true;
            }
            
            string temp = string.Concat(text);
            temp = temp.Replace(" ", "");
            wordPhraseLength = temp.Length;
            string[] foundLetters = new string[wordPhraseLength];

            foreach (string character in text)
            {
                numberStore = 0;
                foreach (string letter in guessedLetters)
                {
                    if (character == guessedLetters[numberStore])
                    {
                        foundLetters[storedNumber] = character;
                        storedNumber++;
                        break;
                    }
                    numberStore++;
                }
            }

            string temp2 = string.Concat(foundLetters);

            if(temp == temp2)
            {
                final = string.Concat(text);
                Console.Clear();
                gameCompleted = true;
                Console.WriteLine("\n\t\tCongratulations, you have won.");
                Console.WriteLine();
                Console.WriteLine($"The word or phrase was: '{final}'");
                DisplayContinuePrompt();
            }

            return gameCompleted;
        }

        static int GetIsCorrectAnswer(string letterGuessed, string[] text, int incorrectGuesses)
        {
            int correctGuesses = 0;

            //
            // Determining if the guess was correct or not
            //
            foreach (string character in text)
            {
                if (character == letterGuessed)
                {
                    correctGuesses++;
                }
            }
            if (correctGuesses == 0)
            {
                incorrectGuesses++;
            }

            return incorrectGuesses;
        }

        static string GetUserGuess(string letterGuessed, string[] text, string[] guessedLetters)
        {
            bool validGuess = false;

            //
            // Get guess from user and validate it
            //
            do
            {
                Console.WriteLine("Please enter the letter that you would like to guess");
                letterGuessed = Console.ReadLine().ToLower();
                validGuess = letterGuessed.Any(c => char.IsLetter(c));

                if (!validGuess)
                {
                    Console.WriteLine("Please enter a letter.");
                }

                if (letterGuessed.Length != 1)
                {
                    validGuess = false;
                    Console.WriteLine("Please enter a single letter.");
                }

                if (guessedLetters.Contains(letterGuessed) == true)
                {
                    validGuess = false;
                    Console.WriteLine("Please enter a letter you have not already entered.");
                }
                DisplayContinuePrompt();
            } while (!validGuess);

            return letterGuessed;
        }

        static void DisplayWordOrPhraseProgress(string[] text, string[] guessedLetters)
        {
            int numberStore = 0;

            //
            // Displaying the incorrect letters that were guessed
            //
            Console.SetCursorPosition(55, 8);
            Console.Write("Correct letters guessed:");
            Console.SetCursorPosition(55, 9);

            foreach (string character in guessedLetters)
            {
                if (text.Contains(guessedLetters[numberStore]))
                {
                    Console.Write($"{guessedLetters[numberStore]}");
                    numberStore++;
                }
            }
        }

        static void DisplayIncorrectLettersGuessed(string[] text, string[] guessedLetters)
        {
            int numberStore = 0;

            //
            // Displaying the incorrect letters that were guessed
            //
            Console.SetCursorPosition(55, 12);
            Console.Write("Incorrect letters guessed:");
            Console.SetCursorPosition(55, 13);

            foreach (string character in guessedLetters)
            {
                if (!text.Contains(guessedLetters[numberStore]))
                {
                    Console.Write($"{guessedLetters[numberStore]}");
                    numberStore++;
                }
            }
        }

        static void DisplayHangmanStand(int incorrectGuesses)
        {
            DisplayScreenHeader("Hangman");

            //
            // Creating the Stand
            //
            Console.WriteLine();
            Console.WriteLine("========================".PadLeft(50));
            Console.WriteLine("========================".PadLeft(50));
            Console.WriteLine("||||                  |".PadLeft(50));
            Console.WriteLine("||||                  |".PadLeft(50));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("||||".PadLeft(31));
            Console.WriteLine("======================".PadLeft(40));
            Console.WriteLine("======================".PadLeft(40));
            Console.WriteLine("======================".PadLeft(40));
            
            //
            // Adding to the drawing based on the number of incorrect guesses
            //
            if (incorrectGuesses >= 1)
            {
                Console.SetCursorPosition(49, 8);
                Console.WriteLine("O");
            }

            if (incorrectGuesses >= 2)
            {
                Console.SetCursorPosition(49, 9);
                Console.WriteLine("|");
                Console.SetCursorPosition(49, 10);
                Console.WriteLine("|");
            }
            
            if (incorrectGuesses >= 3)
            {
                Console.SetCursorPosition(48, 9);
                Console.WriteLine("/");
            }
            
            if (incorrectGuesses >= 4)
            {
                Console.SetCursorPosition(50, 9);
                Console.WriteLine(@"\");
            }
            
            if (incorrectGuesses >= 5)
            {
                Console.SetCursorPosition(48, 11);
                Console.WriteLine("/");
            }
            
            if (incorrectGuesses >= 6)
            {
                Console.SetCursorPosition(50, 11);
                Console.WriteLine(@"\");
            }
        }
        #endregion

        #region User Interphase
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tHangman");
            Console.WriteLine();
            Console.WriteLine("This application will allow you to play the game Hangman with your friend(s).");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for playing Hangman using my application!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }
        #endregion
    }
}