using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;


namespace HangmanGame2
{
    class Program
    {
        static void Main(string[] args)
        {

            // i make comments only to pts 7-11. Pts 1-6 are commented in previous version.


            /*  There is a file attached ”countries-and-capitals.txt" containing a list of
                countries and their capitals (i.e. Poland | Warsaw). Your program should read
                that file at the beginning and randomly select one country-capital pair. Then,
                the capital should be the target word(s) to guess. The country should also be
                remembered - if player will reached his/her life points program should display a
                hint (i.e. "The capital of Poland")
                   */
           

            string text = System.IO.File.ReadAllText(@"C:\Users\sniqq\Desktop\HangmanGame\HangmanGamePts7-11\HangmanGame2\countries_and_capitals.txt");
            char[] delims = new[] { '\r', '\n' };
            string[] capitalsAndCountries = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("Welcome to World capitals hangman game.");
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

                    int life = 5;
                    int counter = 0;
                    int goodMoves = 0;
                    List<string> not_in_word = new List<string>();

                    Random random = new Random();
                    string randomPair = capitalsAndCountries[random.Next(capitalsAndCountries.Length)];
                    string[] cityCountry = randomPair.Split('|', StringSplitOptions.RemoveEmptyEntries);
                    string precountry = cityCountry[0].Trim();
                    string country = precountry.ToUpper();
                    string precapital = cityCountry[1].Trim();
                    string capital = precapital.ToUpper();

                    StringBuilder capitalHidden = new StringBuilder();

                    for (int i = 0; i < capital.Length; i++)
                    {
                        capitalHidden.Append("_");
                    }

                    
                    Console.WriteLine();
                    Console.WriteLine(capitalHidden);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    playingthegame();

                    void playingthegame()
                    {



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
                            List<int> indexesToReplace = new List<int>();
                            Console.WriteLine();
                            Console.WriteLine("Enter the letter");
                            string preletter = Console.ReadLine();
                            if (preletter.Length > 1)
                            {
                                Console.WriteLine("Please, only 1 letter.");
                                guessLetter();
                            }

                            string letter = preletter.ToUpper();
                            if (not_in_word.Contains(letter))
                            {
                                Console.WriteLine("This letter is already used. Please type another letter");
                                guessLetter();
                            }
                            char letterr = Convert.ToChar(letter);


                            if (capital.Contains(letter))
                            {

                                for (int i = 0; i < capital.Length; i++)
                                {
                                    if (capital[i] == letterr)
                                    {
                                        indexesToReplace.Add(i);
                                    }
                                }


                                for (int i = 0; i < indexesToReplace.Count; i++)
                                {
                                    int index = indexesToReplace[i];
                                    capitalHidden.Remove(index, 1);
                                    capitalHidden.Insert(index, letter);
                                }


                                Console.WriteLine();
                                Console.WriteLine(capitalHidden);
                                counter++;
                                goodMoves += indexesToReplace.Count;


                                if (goodMoves < capital.Length)
                                    playingthegame();

                                else winning();
                            }
                            else
                            {
                                life--;
                                not_in_word.Add(letter);
                                hangmanArt();                               // 11. OPTIONAL Add ASCII art!       
                                Console.WriteLine();
                                Console.WriteLine(capitalHidden);
                                counter++;

                                if (life > 0)  //The country should also be remembered - if player will reached his / her life points program should display a hint(i.e. "The capital of Poland")
                                {
                                    if (life == 1)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($"HINT ! The capitol of {country}");
                                    }
                                    playingthegame();
                                }
                                else losing();
                            }
                        }
                        void guessWord()
                        {
                            Console.WriteLine();
                            Console.WriteLine("Type whole word: ");
                            string word = Console.ReadLine().ToUpper();
                            if (word == capital)
                            {
                                counter++;
                                winning();
                            }
                            else
                            {
                                Console.WriteLine("It isn't correct anwswer :( ");
                                counter++;
                                life -= 2;
                                hangmanArt();                                   // 11. OPTIONAL Add ASCII art! 
                                if (life > 0)
                                {
                                    if (life == 1)
                                    {
                                        Console.WriteLine();       //8. Guessing the whole word should be more-risk-more-reward(...) failing the whole word guess should result in losing 2 life points!
                                                                   //The country should also be remembered - if player will reached his / her life points program should display a hint(i.e. "The capital of Poland")
                                        Console.WriteLine($"HINT ! The capitol of {country}");
                                    }
                                    playingthegame();
                                }
                                else losing();
                            }


                        }

                        void winning()
                        {
                            stopwatch.Stop();
                            TimeSpan timeSpan = stopwatch.Elapsed;
                            Console.WriteLine();
                            Console.WriteLine($"YES! {capital} is a capital of {country}");


                            Console.WriteLine($"Congrats, you win the game in {timeSpan.TotalSeconds} seconds with {counter} moves");
                            Console.WriteLine();
                            highscores();                                                   //10. OPTIONAL Expand high score - program should remember 10 best scores (read
                                                                                            // from and write to file) and display them at the end, after success / failure
                            Console.WriteLine();
                            Console.WriteLine("Do you want to save your score? Enter Y ");   // 9.Add a high score
                            string a = Console.ReadLine();

                            switch (a)
                            {
                                case "y":
                                case "Y":
                                    saveScore();  // 9.Add a high score
                                    break;

                                default:
                                    start();
                                    break;
                            }
                            void saveScore()   // // 9.Add a high score
                            {

                                DateTime dateTime = DateTime.Now;
                                Console.WriteLine();
                                Console.WriteLine("Please type your name: ");
                                string name = Console.ReadLine();




                                using (StreamWriter writetext = new StreamWriter("write.txt", true))
                                {
                                    writetext.Write($"{timeSpan.TotalSeconds}" + " | ");
                                    writetext.Write($"{name}" + " | ");
                                    writetext.Write($"{counter}" + " | ");
                                    writetext.Write($"{dateTime}" + " | ");
                                    writetext.WriteLine($"{capital}");
                                }
                                Console.WriteLine("Score export to the file");
                                Console.WriteLine();
                                start();

                            }


                        }


                        void losing()
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Srry, you lost your game. Correct answer was {capital}, the capital of {country}");
                            Console.WriteLine();
                            highscores();                                                                   //10. OPTIONAL Expand high score - program should remember 10 best scores (read
                            Console.WriteLine();                                                            //from and write to file) and display them at the end, after success / failure
                            start();
                        }

                        void highscores()                                                                      // 10.
                        {
                            using (StreamReader readtext = new StreamReader("write.txt"))
                            {
                                string readText = readtext.ReadToEnd();

                                char[] delims = new[] { '\r', '\n' };
                                string[] allScores = readText.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                                    
                                var top10 = (from h in allScores
                                             orderby h.ElementAt(0)
                                             select h).Take(10);

                         

                                Console.WriteLine();
                                Console.WriteLine("Top 10 scores sorted by seconds");
                                Console.WriteLine();
                                Console.WriteLine("Legenda:");
                                Console.WriteLine("Seconds |Name |Moves |Date |Capital ");
                                foreach (var item in top10)
                                {
                                    Console.WriteLine(item);
                                }

                            }


                        }
                        void hangmanArt()                                                  // 11. OPTIONAL Add ASCII art! 
                        {
                            if (life == 4)
                            {
                                string art = " +---+\n |   |\n     |\n     |\n     |\n     |\n========='''";
                                Console.WriteLine();
                                Console.WriteLine(art);
                            }

                            else if (life == 3)
                            {
                                string art = " +---+\n |   |\n O   |\n     |\n     |\n     |\n========='''";
                                Console.WriteLine();
                                Console.WriteLine(art);
                            }

                            else if (life == 2)
                            {
                                string art = " +---+\n |   |\n O   |\n |   |\n     |\n     |\n========='''";
                                Console.WriteLine();
                                Console.WriteLine(art);
                            }
                            else if (life == 1)
                            {
                                string art = " +---+\n |   |\n O   |\n/|\\  |\n     |\n     |\n========='''";
                                Console.WriteLine();
                                Console.WriteLine(art);
                            }
                            else if (life < 1)
                            {
                                string art = " +---+\n |   |\n O   |\n/|\\  |\n/ \\  |\n     |\n========='''";
                                Console.WriteLine();
                                Console.WriteLine(art);
                            }
                        }
                    }




                }
    
        }
    }
}
