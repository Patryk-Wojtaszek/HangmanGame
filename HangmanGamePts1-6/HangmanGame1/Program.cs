using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HangmanGame1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to European capitals hangman game.");
            start();
            void start()
            {
                Console.WriteLine("Enter 1 to turn on the game, 0 to exit");
                string a = Console.ReadLine();
                switch (a)
                {
                    case "1":
                        gameInit();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Unknown operation");
                        start();
                        break;

                }

            }

            void gameInit()
            {
                // 1.Create a list of European capitals, pick one of them randomly and let the user guess it. At the beginning the program should represent each letter as a dash
                //__("_")__ and display them at the screen.Additionally, the program should
                //show the player's life points (let's say, 5).
                List<string> capitols = new List<string> {"Tirana","Andorra","Yerevan","Vienna","Baku","Minsk","Brussels",
                    "Sarajevo","Sofia","Zagreb","Nicosia","Prague","Copenhagen","Tallinn","Helsinki","Paris","Tbilisi","Berlin",
                    "Athens","Budapest","Reykjavik","Dublin","Rome","Pristina","Riga","Vaduz","Vilnius","Luxembourg","Valletta",
                    "Chisinau","Monaco","Podgorica","Amsterdam","Skopje","Oslo","Warsaw","Lisbon","Bucharest","Moscow",
                    "Belgrade","Bratislava","Ljubljana","Madrid","Stockholm","Bern","Ankara","Kiev","London" };

                int life = 5;

                int counter = 0; // counters counts all moves , 6. OPTIONAL Add an information about guessing count and guessing time at the  end of game

                int goodMoves = 0; // if good moves reach the length of capitol, you win the game e.g. PARIS - 5 good moves

                List<string> not_in_word = new List<string>(); // 3. If the player survives a wrong letter guess - that letter should be added to the
                                                                //"not-in-word" list and displayed on the screen.

               


                Random random = new Random();
                string precapitol = capitols[random.Next(capitols.Count)];
                string capitol = precapitol.ToUpper();                  // it was a difference between "a" and "A" , so i decided to make only big letters.


                StringBuilder capitolHidden = new StringBuilder();

                for (int i = 0; i < capitol.Length; i++)
                {
                    capitolHidden.Append("_");
                }


                Console.WriteLine();
                Console.WriteLine(capitolHidden);

                Stopwatch stopwatch = new Stopwatch();  // 6. OPTIONAL Add an information about guessing count and guessing time at the  end of game
                stopwatch.Start();
                playingthegame();



                void playingthegame()
                {


                    // if not_in_word list is empty , it isn't necessary to show it 
                    if (not_in_word.Count == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Enter 1 to guess a letter or 2 to guess a whole word. You have {life} life points left");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.Write($"A list of letters which are not correct: ");
                        foreach (string letters in not_in_word)
                        {
                            Console.Write(letters + " ");
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Enter 1 to guess a letter or 2 to guess a whole word. You have {life} life points left");
                    }
                    string a = Console.ReadLine();

                    //2. Program should ask the user if he/she would like to guess a letter or whole word(s)

                    switch (a)
                    {
                        case "1":

                            guessLetter();
                            break;

                        case "2":
                            guessWord();
                            break;

                        default:
                            playingthegame();
                            break;
                    }

                    void guessLetter()
                    {
                        List<int> indexesToReplace = new List<int>();  // it is necessary to create a list of indexes eg. in word WARSAW you have 2xA so if player choose A , both of them should be revelead
                        Console.WriteLine();
                        Console.WriteLine("Enter the letter");
                        string preletter = Console.ReadLine();
                        if (preletter.Length > 1)
                        {
                            Console.WriteLine("Please, only 1 letter.");
                            guessLetter();
                        }

                        string letter = preletter.ToUpper();
                        char letterr = Convert.ToChar(letter); // making char to compare it with all chars in the capitol in next if


                        if (capitol.Contains(letter))
                        {

                            for (int i = 0; i < capitol.Length; i++)
                            {
                                if (capitol[i] == letterr)
                                {
                                    indexesToReplace.Add(i);        
                                }
                            }

                            // deleting "_" in place of guessed letter
                            for (int i = 0; i < indexesToReplace.Count; i++)
                            {
                                int index = indexesToReplace[i];
                                capitolHidden.Remove(index, 1);
                                capitolHidden.Insert(index, letter);
                            }


                            Console.WriteLine();
                            Console.WriteLine(capitolHidden);
                            counter++;
                            goodMoves += indexesToReplace.Count; //eg. in word WARSAW you have 2xA so in one move "A" you get 2 good moves 


                            if (goodMoves < capitol.Length) //4. If the player guesses the final letter or whole word(s) - he/she is the winner!
                                                            // if good moves reach the capital length you win the game 

                                playingthegame();

                            else winning();
                        }
                        else
                        {
                            life--;                     //  2.(...)  If the ntered letter doesn't exist in word or entered word is not correct - player will lose a life point
                            not_in_word.Add(letter);       // 3. If the player survives a wrong letter guess - that letter should be added to the
                                                           //"not-in-word" list and displayed on the screen.
                            Console.WriteLine();
                            Console.WriteLine(capitolHidden);
                            counter++;

                            if (life > 0)               // 2. (...) If this action brings player life to zero - the game is over!
                                playingthegame();
                            else losing();
                        }
                    }
                    void guessWord()                        // the same as guessLetter
                    {
                        Console.WriteLine();
                        Console.WriteLine("Type whole word: ");
                        string word = Console.ReadLine().ToUpper();
                        if (word == capitol)
                        {
                            counter++;
                            winning();
                        }
                        else
                        {
                            Console.WriteLine("It isn't correct anwswer :( ");
                            counter++;
                            life--;
                            if (life > 0)
                                playingthegame();
                            else losing();
                        }


                    }

                    void winning()     //6. OPTIONAL Add an information about guessing count and guessing time at the
                                       // end of game(i.e. "You guessed the capital after 12 letters. It took you 45
                                       // seconds").
                    {
                        stopwatch.Stop();
                        TimeSpan timeSpan = stopwatch.Elapsed;
                        Console.WriteLine();
                        Console.WriteLine($"Congrats, you win the game in {timeSpan.TotalSeconds} seconds with {counter} moves");

                        start(); // 5. Add a question about restarting the program after wins or loses. 
                                 // start shows "Enter 1 to turn on the game, 0 to exit"

                    }

                    void losing()
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Srry, you lost your game. Correct answer was {capitol}");
                        start();  // 5. Add a question about restarting the program after wins or loses.
                                  //  start shows "Enter 1 to turn on the game, 0 to exit"

                    }
                }
            }
        }
    }
}

