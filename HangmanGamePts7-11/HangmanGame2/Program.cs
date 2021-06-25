using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;


namespace HangmanGame2
{
    class Program
    {
        static void Main(string[] args)
        {
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
                string precapitol = cityCountry[1].Trim();
                string capitol = precapitol.ToUpper();

                StringBuilder capitolHidden = new StringBuilder();

                for (int i = 0; i < capitol.Length; i++)
                {
                    capitolHidden.Append("_");
                }

                Console.WriteLine(capitol);
                Console.WriteLine();
                Console.WriteLine(capitolHidden);

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
                        char letterr = Convert.ToChar(letter);


                        if (capitol.Contains(letter))
                        {

                            for (int i = 0; i < capitol.Length; i++)
                            {
                                if (capitol[i] == letterr)
                                {
                                    indexesToReplace.Add(i);
                                }
                            }


                            for (int i = 0; i < indexesToReplace.Count; i++)
                            {
                                int index = indexesToReplace[i];
                                capitolHidden.Remove(index, 1);
                                capitolHidden.Insert(index, letter);
                            }


                            Console.WriteLine();
                            Console.WriteLine(capitolHidden);
                            counter++;
                            goodMoves += indexesToReplace.Count;


                            if (goodMoves < capitol.Length)
                                playingthegame();

                            else winning();
                        }
                        else
                        {
                            life--;
                            not_in_word.Add(letter);
                            
                            Console.WriteLine();
                            Console.WriteLine(capitolHidden);
                            counter++;

                            if (life > 0)
                                playingthegame();
                            else losing();
                        }
                    }
                    void guessWord()
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
                            life -= 2;
                            if (life > 0)
                                playingthegame();
                            else losing();
                        }


                    }

                    void winning()
                    {
                        stopwatch.Stop();
                        TimeSpan timeSpan = stopwatch.Elapsed;
                        Console.WriteLine();
                        Console.WriteLine($"YES! {capitol} is a capitol of {country}");
                        Console.WriteLine($"Congrats, you win the game in {timeSpan.TotalSeconds} seconds with {counter} moves");
                        Console.WriteLine("Do you want to save your score? Enter Y ");
                        string a = Console.ReadLine();
                        
                        switch(a)
                        {
                            case "y":
                            case "Y":
                                saveScore();
                                break;
                         
                            default:
                                start();
                                break;
                        }
                        void saveScore()
                        {
                            
                            DateTime dateTime = DateTime.Now;
                            Console.WriteLine();
                            Console.WriteLine("Please type your name: ");
                            string name = Console.ReadLine();

                            
                            

                            using (StreamWriter writetext = new StreamWriter("write.txt", true))
                            {
                                writetext.Write($"Name: {name}" +" ");
                                writetext.Write($"Moves: {counter}" + " ");
                                writetext.Write($"Seconds: {timeSpan.TotalSeconds}" + " ");
                                writetext.WriteLine($"Date: {dateTime}" + " ");
                            }

                            start();

                        }


                    }
                       

                    void losing()
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Srry, you lost your game. Correct answer was {capitol}, the capitol of {country}");
                        start();
                    }

                
                }
            
        



    }
        }
    }
}
