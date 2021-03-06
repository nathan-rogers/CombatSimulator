﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CombatSim
{
    class Program
    {
        //enemy count 
        static int EnemiesLeft = 0;
        //life count
        static int BulletsLeft = 0;
        //money count
        static int MoneyLeft = 0;
        //if the user wants to stop playing
        static bool ContinuePlaying = true;
        //auto atk
        static int AutomaticFire = 0;
        //semi auto atk
        static int SemiAuto = 0;
        //chance for the current atk to do damage
        static int ChanceToHit = 0;
        //user select choice
        static int UserSelection = 0;
        //string to store if the user wants to keep playing
        static string KeepPlayingSelection = null;
        //only animate captions once
        static int CaptionCount = 0;
        //how much damage enemy does
        static int EnemyDamage = 0;
        //how many cities have been cleared
        //can go up infinitely
        static int CitiesCleared = 0;
        //how many cities have been lost
        //lost game at 5 cities lost
        static int CitiesLost = 0;
        //Confirm attack type
        static string UserConfirmation = null;
        //stats
        static int katanaAtk = 0;
        static int katanaDmg = 0;
        static int pistolAtk = 0;
        static int pistolDmg = 0;
        static int APDAtk = 0;
        static int APDDmg = 0;
        static int TacticalNuke = 0;
        //how many times to give payment
        static int MoneyIncrement = 0;
        //sets reinforcement max, scales with levels
        static int MaxReinforcement = 20;

        //tracks enemy kills for cash rewards
        static int enemiesKill = 0;
        //counts how many times user buys ammo during game
        static int buyAmmo = 0;
        //tracks total cash earned
        static int CashEarned = 0;
        //tracks whether grenade debuff is active or not
        static int grenade = 0;
        //tracks # of grenades purchased
        static int GrenadeUsage = 0;
        //base auto accuracy
        static int APDAccuracy = 30;
        //base katana accuracy
        static int KatAccuracy = 65;
        //implants buff on or off
        static int Implants = 0;
        //total implants purcased
        static int ImplantsTotal = 0;
        //increases difficutly
        static int EnemySpawnChance = 150;
        //increases enemy damage per round
        static int EnemyScalingDamage = 10;
        //increases money reward per ever 50 extra enemies every 2 rounds
        static int MoneyReward = 0;
        //counts current level
        static int LevelCounter = 1;
        static void Main(string[] args)
        {

            Random rand = new Random();
            //set window size
            Console.WindowHeight = 69;

            //scroll text from top
            TitleScrollFromTop();
            Console.Clear();
            //basic hud template without data
            CityLogo();
            TitleScroll();
            //user prompt
            Console.WriteLine("\n                               Press Enter To Play: ");
            Console.ReadKey();
            //StoryLine();

            //set all initial gam values
            BulletsLeft = 100;
            EnemiesLeft = EnemySpawnChance;
            MoneyLeft = 50;
            //continue playing
            while (ContinuePlaying == true)
            {
                //while player is alive
                while (BulletsLeft > 0 && EnemiesLeft > 0)
                {
                    //reset user selection each iteration
                    UserSelection = 0;
                    HeadsUpDisplay();
                    //prompt
                    GameScreen();

                    //try to parse user input to int
                    int.TryParse(Console.ReadLine(), out UserSelection);
                    //checks case of user input
                    CombatSelector(UserSelection);
                    //adds money kill awards
                    NewMoneyAdder();
                    //checks if implants are active
                    if (Implants > 0)
                    {
                        //then reduce by 1
                        //implants are active for 5 rounds
                        Implants--;

                    }
                    //implants deactivate, reset default values
                    else if (Implants <= 0)
                    {
                        KatAccuracy = 65;
                        APDAccuracy = 30;
                    }

                }
                //if there are NO ENEMEIES LEFT or OUT OF LIFE
                while (EnemiesLeft <= 0 || BulletsLeft <= 0)
                {
                    //bullets/enemies = 0
                    //keeps numbers from display negative
                    if (BulletsLeft <= 0)
                    {
                        BulletsLeft = 0;
                    }
                    if (EnemiesLeft <= 0)
                    {
                        EnemiesLeft = 0;
                    }
                    //if user loses 3 cities game over
                    if (CitiesLost == 3)
                    {
                        //give a value so that loop is broken
                        BulletsLeft = 1;
                        EnemiesLeft = 1;
                        ContinuePlaying = false;

                        GameOverAnimation();
                    }
                    //User loses when 5 cities are lost
                    else if (CitiesLost < 5)
                    {
                        //when enemies are all killed
                        if (EnemiesLeft <= 0)
                        {

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            //you win!
                            YouWinAnimation();


                            //ask if user keeps playing
                            KeepPlaying();
                            //count number of cities cleared
                            CitiesCleared++;
                            //reset reward counter
                            enemiesKill = 0;

                        }
                        //when the user runs ouf of bullets/life
                        else if (BulletsLeft <= 0)
                        {
                            //does not display negative number
                            BulletsLeft = 0;
                            Console.ForegroundColor = ConsoleColor.DarkCyan;

                            YouLose();
                            //ask if user wnats to keep playing
                            KeepPlaying();

                            MoneyLeft = 50;
                            BulletsLeft = 100;
                            //reset money and ammo to default
                            CitiesLost++;
                            //reset reward counter
                            enemiesKill = 0;
                            //reset difficulty 
                            EnemySpawnChance = 150;

                            EnemyScalingDamage = 15;

                            EnemiesLeft = EnemySpawnChance;

                            MaxReinforcement = 20;


                        }

                        //increases enemy difficulty Per Level
                        if (CitiesCleared > 0)
                        {

                            EnemySpawnChance = EnemySpawnChance + 25;

                            EnemyScalingDamage = EnemyScalingDamage + 5;

                            EnemiesLeft = EnemySpawnChance;

                            MaxReinforcement = MaxReinforcement + 5;
                        }
                        //Grants an extra 5 bucks per 50 extra enemies
                        if (CitiesCleared > 0 && CitiesCleared % 2 == 0)
                        {
                            //earn an extra 5 bucks per 50 extra enemies
                            MoneyReward += 5;
                            //if there are a lot of enemies, more bonus
                            if (EnemiesLeft > 250)
                            {
                                MoneyReward = MoneyReward + 5;
                            }
                        }
                        LevelCounter = CitiesCleared + CitiesLost + 1;
                    }
                }
            }
            //refresh display
            HeadsUpDisplay();
            Stat();
            string endingText = "\nThe world will continue on in darkness...\n      ...until all of the Replicants are dead. ";
            //scroll text
            foreach (object c in endingText)
            {
                Console.Write(c);
                Thread.Sleep(40);
            }
            Console.ReadKey();
            //Game Over
            GameOverAnimation();
            Console.ReadKey();
        }

        /// <summary>
        /// ask the user to keep playing
        /// </summary>
        public static void KeepPlaying()
        {

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            //refrersh display
            HeadsUpDisplay();
            //prompt
            KeepPlayingSelection = null;
            Console.WriteLine("\nTime to clear another city? Y/N");
            while (KeepPlayingSelection == null)
            {
                Random rand = new Random();
                KeepPlayingSelection = Console.ReadLine().ToUpper();
                switch (KeepPlayingSelection)
                {
                    //yes
                    case "Y":
                        //reset counters for new game


                        //do not reset money, or bullets. 
                        //money is earned in the game
                        break;
                    //no
                    case "N":
                        // + 1 breaks out of input loop
                        //playing = false breaks out of game loop
                        ContinuePlaying = false;
                        EnemiesLeft = 1;
                        BulletsLeft = 1;


                        break;
                    default:
                        //re prompt for real input
                        Console.WriteLine("You must press Y or N to advance: ");
                        KeepPlayingSelection = null;
                        break;
                }
            }

        }
        /// <summary>
        /// YOU WIN!!!
        /// </summary>
        public static void YouWinAnimation()
        {
            int loopCount = 0;
            while (loopCount < 5)
            {
                Console.Clear();
                CityLogo();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"
 __      __  ______   __    __        __       __  ______  __    __ 
/  \    /  |/      \ /  |  /  |      /  |  _  /  |/      |/  \  /  |
$$  \  /$$//$$$$$$  |$$ |  $$ |      $$ | / \ $$ |$$$$$$/ $$  \ $$ |
 $$  \/$$/ $$ |  $$ |$$ |  $$ |      $$ |/$  \$$ |  $$ |  $$$  \$$ |
  $$  $$/  $$ |  $$ |$$ |  $$ |      $$ /$$$  $$ |  $$ |  $$$$  $$ |
   $$$$/   $$ |  $$ |$$ |  $$ |      $$ $$/$$ $$ |  $$ |  $$ $$ $$ |
    $$ |   $$ \__$$ |$$ \__$$ |      $$$$/  $$$$ | _$$ |_ $$ |$$$$ |
    $$ |   $$    $$/ $$    $$/       $$$/    $$$ |/ $$   |$$ | $$$ |
    $$/     $$$$$$/   $$$$$$/        $$/      $$/ $$$$$$/ $$/   $$/ 
");
                Thread.Sleep(200);
                Console.Clear();

                CityLogo();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"
 __      __   ______   __    __        __       __  ______  __    __ 
|  \    /  \ /      \ |  \  |  \      |  \  _  |  \|      \|  \  |  \
 \$$\  /  $$|  $$$$$$\| $$  | $$      | $$ / \ | $$ \$$$$$$| $$\ | $$
  \$$\/  $$ | $$  | $$| $$  | $$      | $$/  $\| $$  | $$  | $$$\| $$
   \$$  $$  | $$  | $$| $$  | $$      | $$  $$$\ $$  | $$  | $$$$\ $$
    \$$$$   | $$  | $$| $$  | $$      | $$ $$\$$\$$  | $$  | $$\$$ $$
    | $$    | $$__/ $$| $$__/ $$      | $$$$  \$$$$ _| $$_ | $$ \$$$$
    | $$     \$$    $$ \$$    $$      | $$$    \$$$|   $$ \| $$  \$$$
     \$$      \$$$$$$   \$$$$$$        \$$      \$$ \$$$$$$ \$$   \$$
                                                                     
");
                Thread.Sleep(200);
                Console.Clear();

                CityLogo();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"
$$\     $$\  $$$$$$\  $$\   $$\       $$\      $$\ $$$$$$\ $$\   $$\ 
\$$\   $$  |$$  __$$\ $$ |  $$ |      $$ | $\  $$ |\_$$  _|$$$\  $$ |
 \$$\ $$  / $$ /  $$ |$$ |  $$ |      $$ |$$$\ $$ |  $$ |  $$$$\ $$ |
  \$$$$  /  $$ |  $$ |$$ |  $$ |      $$ $$ $$\$$ |  $$ |  $$ $$\$$ |
   \$$  /   $$ |  $$ |$$ |  $$ |      $$$$  _$$$$ |  $$ |  $$ \$$$$ |
    $$ |    $$ |  $$ |$$ |  $$ |      $$$  / \$$$ |  $$ |  $$ |\$$$ |
    $$ |     $$$$$$  |\$$$$$$  |      $$  /   \$$ |$$$$$$\ $$ | \$$ |
    \__|     \______/  \______/       \__/     \__|\______|\__|  \__|
                                                                    
");
                Thread.Sleep(200);
                Console.Clear();

                CityLogo();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"
 /$$     /$$ /$$$$$$  /$$   /$$       /$$      /$$ /$$$$$$ /$$   /$$
|  $$   /$$//$$__  $$| $$  | $$      | $$  /$ | $$|_  $$_/| $$$ | $$
 \  $$ /$$/| $$  \ $$| $$  | $$      | $$ /$$$| $$  | $$  | $$$$| $$
  \  $$$$/ | $$  | $$| $$  | $$      | $$/$$ $$ $$  | $$  | $$ $$ $$
   \  $$/  | $$  | $$| $$  | $$      | $$$$_  $$$$  | $$  | $$  $$$$
    | $$   | $$  | $$| $$  | $$      | $$$/ \  $$$  | $$  | $$\  $$$
    | $$   |  $$$$$$/|  $$$$$$/      | $$/   \  $$ /$$$$$$| $$ \  $$
    |__/    \______/  \______/       |__/     \__/|______/|__/  \__/
");
                Thread.Sleep(200);

                loopCount++;
            }

            Console.Clear();

            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
██╗   ██╗ ██████╗ ██╗   ██╗    ██╗    ██╗██╗███╗   ██╗
╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║    ██║██║████╗  ██║
 ╚████╔╝ ██║   ██║██║   ██║    ██║ █╗ ██║██║██╔██╗ ██║
  ╚██╔╝  ██║   ██║██║   ██║    ██║███╗██║██║██║╚██╗██║
   ██║   ╚██████╔╝╚██████╔╝    ╚███╔███╔╝██║██║ ╚████║
   ╚═╝    ╚═════╝  ╚═════╝      ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝
");
            Thread.Sleep(3000);
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void YouLose()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine(@"
                                                 /
                                                /      /
                                               /  /   /  /         /
                                              /      /       /    /
                                            ,---------------.   ,-,
                                           /                 `-'  |
                                          [                   |   |
                                           \                 ,-.  |
                                            `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"


                                            /
                                           /      /
                                          /  /   /  /         /
                                         /      /       /    /
                                       ,---------------.   ,-,
                                      /                 `-'  |
                                     [                   |   |
                                      \                 ,-.  |
                                       `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"




                                    /
                                   /      /
                                  /  /   /  /         /
                                 /      /       /    /
                               ,---------------.   ,-,
                              /                 `-'  |
                             [                   |   |
                              \                 ,-.  |
                               `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"




                                    /
                                   /    /
                                  /  /    /         /
                                 /    /       /    /
                               ,-------------.   ,-,
                              /               `-'  |
                             [                 |   |
                              \               ,-.  |
                               `-------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"






                                /

                               /    /
                              /  /    /         /
                             /    /       /    /
                           ,-----------.   ,-,
                          /             `-'  |
                         [               |   |
                          \             ,-.  |
                           `-----------'   `-`








                       __________________");

            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"







                                
                               /    /
                              /  /    /
                             /    //
                          ,----.,-,                          
                         [      ||
                          `----' `-`




                _________________________________");

            Thread.Sleep(200);
            Console.Clear();

            Console.WriteLine(@"











                                
                           /    /
                          /  /    /
                         /    //
                      ,----.,-,                          
                     [      ||
                      `----' `-`
                _________________________________");

            Thread.Sleep(200);
            Console.Clear();
            Console.WriteLine(@"








            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                   
             ▄· ▄▌      ▄• ▄▌    ▄▄▌        .▄▄ · ▄▄▄ .▄▄ ▄▄ ▄▄ 
            ▐█▪██▌▪     █▪██▌    ██•  ▪     ▐█ ▀. ▀▄.▀·██▌██▌██▌
            ▐█▌▐█▪ ▄█▀▄ █▌▐█▌    ██▪   ▄█▀▄ ▄▀▀▀█▄▐▀▀▪▄▐█·▐█·▐█·
             ▐█▀·.▐█▌.▐▌▐█▄█▌    ▐█▌▐▌▐█▌.▐▌▐█▄▪▐█▐█▄▄▌.▀ .▀ .▀ 
              ▀ •  ▀█▄▀▪ ▀▀▀     .▀▀▀  ▀█▄▀▪ ▀▀▀▀  ▀▀▀  ▀  ▀  ▀
                                _        _  _ _     _       .   .   .   
 .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
    .       .           .   (   /(/ (  )  ) )\          .     .               
                        
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"







            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                   
             ▄· ▄▌      ▄• ▄▌    ▄▄▌        .▄▄ · ▄▄▄ .▄▄ ▄▄ ▄▄ 
            ▐█▪██▌▪     █▪██▌    ██•  ▪     ▐█ ▀. ▀▄.▀·██▌██▌██▌
            ▐█▌▐█▪ ▄█▀▄ █▌▐█▌    ██▪   ▄█▀▄ ▄▀▀▀█▄▐▀▀▪▄▐█·▐█·▐█·
             ▐█▀·.▐█▌.▐▌▐█▄█▌    ▐█▌▐▌▐█▌.▐▌▐█▄▪▐█▐█▄▄▌.▀ .▀ .▀ 
              ▀ •  ▀█▄▀▪ ▀▀▀     .▀▀▀  ▀█▄▀▪ ▀▀▀▀  ▀▀▀  ▀  ▀  ▀   
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
--------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"





            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                   
              ▄· ▄▌      ▄• ▄▌    ▄▄▌        .▄▄ · ▄▄▄ .▄▄ ▄▄ ▄▄ 
            ▐█▪██▌▪     █▪██▌    ██•  ▪     ▐█ ▀. ▀▄.▀·██▌██▌██▌
            ▐█▌▐█▪ ▄█▀▄ █▌▐█▌    ██▪   ▄█▀▄ ▄▀▀▀█▄▐▀▀▪▄▐█·▐█·▐█·
             ▐█▀·.▐█▌.▐▌▐█▄█▌    ▐█▌▐▌▐█▌.▐▌▐█▄▪▐█▐█▄▄▌.▀ .▀ .▀ 
              ▀ •  ▀█▄▀▪ ▀▀▀     .▀▀▀  ▀█▄▀▪ ▀▀▀▀  ▀▀▀  ▀  ▀  ▀
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
     .      .    .     (  . ( ( ( | | ) ) )\   )               .            
                        (   /(| / ( )) ) ) )) )    .   .  .       .  .  .   
 .     .       .  .   (  .  ( ((((_(|)_)))))     )            .             
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"
            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                   
             ▄· ▄▌      ▄• ▄▌    ▄▄▌        .▄▄ · ▄▄▄ .▄▄ ▄▄ ▄▄ 
            ▐█▪██▌▪     █▪██▌    ██•  ▪     ▐█ ▀. ▀▄.▀·██▌██▌██▌
            ▐█▌▐█▪ ▄█▀▄ █▌▐█▌    ██▪   ▄█▀▄ ▄▀▀▀█▄▐▀▀▪▄▐█·▐█·▐█·
             ▐█▀·.▐█▌.▐▌▐█▄█▌    ▐█▌▐▌▐█▌.▐▌▐█▄▪▐█▐█▄▄▌.▀ .▀ .▀ 
              ▀ •  ▀█▄▀▪ ▀▀▀     .▀▀▀  ▀█▄▀▪ ▀▀▀▀  ▀▀▀  ▀  ▀  ▀  
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
     .      .    .     (  . ( ( ( | | ) ) )\   )               .            
                        (   /(| / ( )) ) ) )) )    .   .  .       .  .  .   
 .     .       .  .   (  .  ( ((((_(|)_)))))     )            .             
         .  .          (    . ||\(|(|)|/|| . . )        .        .          
     .           .   (   .    |(||(||)||||   .    ) .      .         .  .   
 .      .      .       (     //|/l|||)|\\ \     )      .      .   .         
                     (/ / //  /|//||||\\  \ \  \ _)                         
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(2500);

        }
        /// <summary>
        /// Displayed on GAME OVER, either USER LOSE, or USER STOPS PLAYING
        /// </summary>
        public static void GameOverAnimation()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            int loop = 0;
            while (loop < 10)
            {
                Console.Clear();
                CityLogo();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
  ▄████  ▄▄▄       ███▄ ▄███▓▓█████     ▒█████   ██▒   █▓▓█████  ██▀███  
 ██▒ ▀█▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀    ▒██▒  ██▒▓██░   █▒▓█   ▀ ▓██ ▒ ██▒
▒██░▄▄▄░▒██  ▀█▄  ▓██    ▓██░▒███      ▒██░  ██▒ ▓██  █▒░▒███   ▓██ ░▄█ ▒
░▓█  ██▓░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄    ▒██   ██░  ▒██ █░░▒▓█  ▄ ▒██▀▀█▄  
░▒▓███▀▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒   ░ ████▓▒░   ▒▀█░  ░▒████▒░██▓ ▒██▒
 ░▒   ▒  ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░   ░ ▒░▒░▒░    ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░
  ░   ░   ▒   ▒▒ ░░  ░      ░ ░ ░  ░     ░ ▒ ▒░    ░ ░░   ░ ░  ░  ░▒ ░ ▒░
░ ░   ░   ░   ▒   ░      ░      ░      ░ ░ ░ ▒       ░░     ░     ░░   ░ 
      ░       ░  ░       ░      ░  ░       ░ ░        ░     ░  ░   ░     
                                                     ░        
");
                Thread.Sleep(200);
                Console.Clear();
                CityLogo();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
");
                Thread.Sleep(200);
                loop++;

            }
            CityLogo();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
  ▄████  ▄▄▄       ███▄ ▄███▓▓█████     ▒█████   ██▒   █▓▓█████  ██▀███  
 ██▒ ▀█▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀    ▒██▒  ██▒▓██░   █▒▓█   ▀ ▓██ ▒ ██▒
▒██░▄▄▄░▒██  ▀█▄  ▓██    ▓██░▒███      ▒██░  ██▒ ▓██  █▒░▒███   ▓██ ░▄█ ▒
░▓█  ██▓░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄    ▒██   ██░  ▒██ █░░▒▓█  ▄ ▒██▀▀█▄  
░▒▓███▀▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒   ░ ████▓▒░   ▒▀█░  ░▒████▒░██▓ ▒██▒
 ░▒   ▒  ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░   ░ ▒░▒░▒░    ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░
  ░   ░   ▒   ▒▒ ░░  ░      ░ ░ ░  ░     ░ ▒ ▒░    ░ ░░   ░ ░  ░  ░▒ ░ ▒░
░ ░   ░   ░   ▒   ░      ░      ░      ░ ░ ░ ▒       ░░     ░     ░░   ░ 
      ░       ░  ░       ░      ░  ░       ░ ░        ░     ░  ░   ░     
                                                     ░        
");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Sets default values for combat upon selection by user
        /// </summary>
        /// <param name="userInput"></param>
        public static void CombatSelector(int userInput)
        {
            //random number
            Random rand = new Random();
            //switch on 1, 2, 3, 4
            switch (userInput)
            {
                //automatic gun
                case 1:
                    HeadsUpDisplay();
                    Console.WriteLine(@"
      THE ADP:
         Cost: 5 Bullets");
                    Console.Write("Chance to Hit: {0}\n   Max Damage: 40", 100 - APDAccuracy);
                    Console.WriteLine("\n\nDo you want to continue using the ADP? Y/N");
                    //Are you sure?
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {

                        UserConfirmation = Console.ReadLine().ToUpper();
                        switch (UserConfirmation)
                        {
                            case "Y":
                                //It takes 5 bullets to atk with this weapon
                                BulletsLeft = BulletsLeft - 5;
                                if (BulletsLeft < 0)
                                {
                                    BulletsLeft = 0;
                                }
                                CityLogo();
                                //animations for auto gun
                                AutoGunAnimation();
                                //how much damage to do
                                AutomaticFire = rand.Next(15, 40);
                                //user hit chance
                                ChanceToHit = rand.Next(1, 101);
                                //70% chance to hit
                                if (ChanceToHit > APDAccuracy)
                                {
                                    //kill enemies for amount of damage done
                                    EnemiesLeft = EnemiesLeft - AutomaticFire;
                                    enemiesKill = enemiesKill + AutomaticFire;
                                    //stats
                                    APDAtk++;
                                    APDDmg = APDDmg + AutomaticFire;
                                    //if user lands critical hit
                                    if (AutomaticFire > 31)
                                    {

                                        CityLogo();
                                        //special animation
                                        BulletImpactAnimation();
                                        HeadsUpDisplay();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.WriteLine("\nYour aim was critically precise, killing {0} Replicants.", AutomaticFire);
                                    }
                                    //normal hit
                                    else
                                    {
                                        //normal text, no critical hit
                                        HeadsUpDisplay();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.WriteLine("\nYou killed {0} Replicants.", AutomaticFire);
                                    }
                                }
                                //if the user misses
                                else
                                {

                                    HeadsUpDisplay();
                                    //You Miss!
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYou miss every shot!");
                                }
                                //Keeps combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                Console.ForegroundColor = ConsoleColor.White;
                                //now enemy attacks
                                EnemyAttack();
                                break;
                            //do not continue with combat action
                            //chose again
                            case "N":
                                break;
                            //Not valid input
                            default:
                                Console.WriteLine("\nPlease enter Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }
                    break;

                //Pistol 100% chance to hit
                case 2:
                    HeadsUpDisplay();
                    Console.WriteLine(@"
.45 Cal ANTI-MATTER PISTOL: 
                      Cost: 2 Bullets 
             Chance to Hit: 100 
                Max Damage: 25");

                    Console.WriteLine("\nDo you want to continue using the Side-Arm? Y/N");
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {
                        UserConfirmation = Console.ReadLine().ToUpper();
                        //Are you sure?

                        switch (UserConfirmation)
                        {
                            //Yes
                            case "Y":
                                CityLogo();
                                //pistol animation
                                SemiAutoGunAnimation();
                                //subtract bullets spent by action
                                BulletsLeft = BulletsLeft - 2;
                                if (BulletsLeft < 0)
                                {
                                    BulletsLeft = 0;
                                }
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                SemiAuto = rand.Next(10, 25);
                                //enemies reduced by atk
                                EnemiesLeft = EnemiesLeft - SemiAuto;
                                if (EnemiesLeft <= 0)
                                {
                                    EnemiesLeft = 0;
                                }
                                //keep track of enemy kills
                                enemiesKill = enemiesKill + SemiAuto;
                                //stats
                                pistolAtk++;
                                pistolDmg = pistolDmg + SemiAuto;
                                //If user lands crit
                                if (SemiAuto > 18)
                                {
                                    CityLogo();
                                    //special animation
                                    BulletImpactAnimation();
                                    HeadsUpDisplay();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYour aim was critically precise, killing {0} Replicants!", SemiAuto);
                                }
                                //no crit
                                else
                                {
                                    //normal messages
                                    HeadsUpDisplay();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYou killed {0} Replicants.", SemiAuto);
                                }
                                //keep combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                //enemy attack
                                EnemyAttack();
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            //do not continue with combat action
                            //make new slection
                            case "N":
                                break;
                            //invalid input
                            default:
                                Console.WriteLine("\nPlease Select Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }
                    break;
                //supply drop
                case 3:
                    //if user has money left
                    if (MoneyLeft > 0)
                    {
                        int supplyDrop = 0;
                        int newBullets = 0;
                        int numOfDrops = 0;
                        Console.WriteLine("\nHow many supply drops would you like to call? \n       1 Drop: $5");
                        Console.WriteLine("   (Select: 5, 10, 15, 20, 25 etc.)");
                        int.TryParse(Console.ReadLine(), out supplyDrop);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        //if input is a number
                        if (supplyDrop != 0)
                        {
                            //if they didn't type in too much money
                            if (supplyDrop % 5 == 0)
                            {

                                //and if input is divisible by 5
                                if (supplyDrop <= MoneyLeft)
                                {
                                    //subtract money
                                    MoneyLeft = MoneyLeft - supplyDrop;
                                    //total number of supply drops = input / 5
                                    numOfDrops = supplyDrop / 5;
                                    //how many crates of ammo were purchased
                                    buyAmmo = buyAmmo + numOfDrops;

                                    while (numOfDrops > 0)
                                    {
                                        //random number of bullets awarded in bulk
                                        newBullets = newBullets + rand.Next(10, 20);
                                        numOfDrops--;
                                    }
                                    //add new bullets to count
                                    BulletsLeft = BulletsLeft + newBullets;

                                    HeadsUpDisplay();
                                    Console.WriteLine("\nYou purchased {0} bullets.", newBullets);
                                }
                                else if (supplyDrop > MoneyLeft)
                                {
                                    //you are cheating!
                                    HeadsUpDisplay();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("You don't have that kind of cash!");
                                }

                            }
                            //if input is too high

                            //if input is not divisible by 5
                            else if (supplyDrop % 5 != 0)
                            {
                                //5, 10, 15, are valid inputs only
                                HeadsUpDisplay();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("That isn't in $5 increments.");
                            }

                            //bullets added to pool
                            //how many bullets are awarded
                        }
                        //subtract cost

                    }
                    //User is out of money
                    else
                    {

                        HeadsUpDisplay();
                        Console.WriteLine("\nYou're out of money!!!");
                    }
                    //keep combat messages on screen
                    Console.WriteLine("\nPress Enter: ");
                    Console.ReadKey();
                    //enemy attacks now
                    EnemyAttack();
                    break;
                //KATANA!!!!!!!!!!!
                case 4:
                    HeadsUpDisplay();
                    Console.WriteLine(@"
       KATANA:
         Cost: Free"); Console.WriteLine("Chance to Hit: {0}\n   Max Damage: 100", 100 - KatAccuracy);

                    Console.WriteLine("\nDo you want to continue using the KATANA? Y/N");
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {

                        UserConfirmation = Console.ReadLine().ToUpper();
                        //are you sure?
                        switch (UserConfirmation)
                        {
                            //Yes
                            case "Y":
                                //Katana anmimation
                                Katana();
                                ChanceToHit = rand.Next(1, 101);

                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                //IF user hits
                                if (ChanceToHit > KatAccuracy)
                                {
                                    katanaAtk++;
                                    //how much damage
                                    SemiAuto = rand.Next(30, 101);
                                    //stats
                                    katanaDmg = katanaDmg + SemiAuto;
                                    //subtract damage from enemy total
                                    EnemiesLeft = EnemiesLeft - SemiAuto;
                                    if (EnemiesLeft < 0)
                                    {
                                        EnemiesLeft = 0;
                                    }
                                    //keep track of enemy kills for kill rewards
                                    enemiesKill = enemiesKill + SemiAuto;
                                    HeadsUpDisplay();

                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYou killed {0} Replicants!", SemiAuto);
                                }
                                //user misses
                                else
                                {
                                    HeadsUpDisplay();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYou missed completely.");
                                }
                                //keep combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                EnemyAttack();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            //do not continue with combat action
                            //make new selection
                            case "N":
                                break;
                            //invalid input
                            default:
                                Console.WriteLine("\nPlease Enter Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }
                    break;
                case 5:

                    HeadsUpDisplay();
                    Console.WriteLine(@"
      GRENADE:
         Cost: $10
Chance to Hit: 55%
   Max Damage: 50
        Bonus: 100% Chance to reduce enemy Reinforments for 3 rounds
");
                    //only displays while effect active
                    if (grenade > 0)
                    {
                        Console.WriteLine("\nRounds Left of Bonus Effect: {0}", grenade);
                    }
                    Console.WriteLine("Do you want to continue using the Grenade? Y/N");
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {

                        UserConfirmation = Console.ReadLine().ToUpper();
                        //are you sure?
                        switch (UserConfirmation)
                        {
                            //Yes
                            case "Y":
                                //check to see if user has money
                                if (MoneyLeft <= 10)
                                {
                                    //User has NO money
                                    HeadsUpDisplay();

                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nYou trying to cheat me, boy?\n  You aint got the funds for these munitions.");
                                }
                                //user HAS money
                                else if (MoneyLeft > 10)
                                {

                                    GrenadeBoom();
                                    //Grenade animation
                                    MoneyLeft = MoneyLeft - 10;
                                    ChanceToHit = rand.Next(1, 101);

                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    //IF user hits
                                    if (ChanceToHit > 45)
                                    {
                                        //does damage
                                        SemiAuto = rand.Next(30, 50);
                                        //counts grenades
                                        GrenadeUsage++;
                                        //stats
                                        grenade = 3;
                                        //subtract damage from enemy total
                                        EnemiesLeft = EnemiesLeft - SemiAuto;
                                        if (EnemiesLeft < 0)
                                        {
                                            EnemiesLeft = 0;
                                        }
                                        //keep track of enemy kills for kill rewards
                                        enemiesKill = enemiesKill + SemiAuto;
                                        HeadsUpDisplay();

                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        //combat message
                                        Console.WriteLine("\nYou killed {0} Replicants!", SemiAuto);
                                        Console.WriteLine("\nReplicants have a reduced chance to reinforce for a while.");
                                    }
                                    else
                                    {
                                        GrenadeUsage++;
                                        grenade = 3;
                                        //combat message
                                        HeadsUpDisplay();
                                        Console.WriteLine("\nYour grenade missed, but the explosion can be heard for miles.");
                                        Console.WriteLine("\nDue to the EMP Effect, \n      Replicants have a reduced chance to reinforce for a while.");
                                    }
                                }
                                //keep combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                EnemyAttack();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            //do not continue with combat action
                            //make new selection
                            case "N":
                                break;
                            //invalid input
                            default:
                                Console.WriteLine("\nPlease Enter Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }
                    break;
                case 6:
                    HeadsUpDisplay();
                    Console.WriteLine(@"
     IMPLANTS:
         Cost: $50
        Bonus: 100% Accuracy with ADP and KATANA for 5 Rounds!
");
                    //only displays while effect active
                    if (Implants > 0)
                    {
                        Console.WriteLine("\nRounds Left of Bonus Effect: {0}", Implants);
                    }
                    Console.WriteLine("Do you wish to purchase IMPLANTS? Y/N");
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {

                        UserConfirmation = Console.ReadLine().ToUpper();
                        //are you sure?
                        switch (UserConfirmation)
                        {
                            //Yes
                            case "Y":
                                if (MoneyLeft >= 50)
                                {
                                    //implants total purchased this game
                                    ImplantsTotal++;
                                    //subtract cost
                                    MoneyLeft = MoneyLeft - 50;
                                    //activate implants for 5 rounds
                                    //since  implants decrements after each round
                                    //user has 5 rounds of COMBAT to use implants
                                    Implants = 6;
                                    //change accuracy for both items
                                    APDAccuracy = 1;
                                    KatAccuracy = 20;
                                    HeadsUpDisplay();
                                    Console.WriteLine("\nYou now have perfect accuracy with the APD, \n     and improved accuracy with the KATANA for 5 rounds.");
                                }
                                //if user doesn't have enough money
                                else if (MoneyLeft < 50)
                                {
                                    //don't activate implant effect
                                    HeadsUpDisplay();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nDon't try to cheat me! \n    I can see you don't have enough money for those implants!");
                                }
                                //keep combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                EnemyAttack();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            //do not continue with combat action
                            //make new selection
                            case "N":
                                break;
                            //invalid input
                            default:
                                Console.WriteLine("\nPlease Enter Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }



                    break;
                case 7:
                    if (MoneyReward > 100)
                        HeadsUpDisplay();
                    Console.WriteLine(@"
TACTICAL NUKE:
         Cost: $100
       Effect: Reduces Enemy Count to 0
       Debuff: Reduce bonus cash to default
");
                    Console.WriteLine("Do you wish to purchase  TACTICAL NUKE? Y/N");
                    UserConfirmation = null;
                    while (UserConfirmation == null)
                    {

                        UserConfirmation = Console.ReadLine().ToUpper();
                        //are you sure?
                        switch (UserConfirmation)
                        {
                            //Yes
                            case "Y":
                                if (MoneyLeft >= 100)
                                {

                                    MoneyReward = 0;
                                    //implants total purchased this game
                                    TacticalNuke++;
                                    //subtract cost
                                    MoneyLeft = MoneyLeft - 100;
                                    //defeats infinite nuke at end of game until very very high levels
                                    if (enemiesKill <= 100)
                                    {
                                        enemiesKill = EnemiesLeft + (enemiesKill / 2);
                                    }
                                    if(enemiesKill>= 100)
                                    {
                                        enemiesKill = EnemiesLeft + (enemiesKill / 4);
                                    }
                                    //activate implants for 5 rounds
                                    //since  implants decrements after each round
                                    //user has 5 rounds of COMBAT to use implants
                                    Console.WriteLine("You killed {0} Replicants!!!", EnemiesLeft);
                                    EnemiesLeft = 0;
                                    //change accuracy for both items
                                    TacticalNukeAnimation();
                                    TacticalNuke++;

                                }
                                    //user doesn't have enough money
                                else if (MoneyLeft < 100)
                                {

                                    //if user doesn't have enough money
                                    Console.WriteLine("You can't use this option yet!!!");
                                }
                                //keep combat messages on screen
                                Console.WriteLine("\nPress Enter: ");
                                Console.ReadKey();
                                EnemyAttack();

                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            //do not continue with combat action
                            //make new selection
                            case "N":
                                break;
                            //invalid input
                            default:
                                Console.WriteLine("\nPlease Enter Y or N: ");
                                UserConfirmation = null;
                                break;
                        }
                    }

                    break;

                //user did not select 1, 2, 3, 4 or 5
                default:
                    //User gun jams, loses 1 bullet
                    BulletsLeft = BulletsLeft - 1;
                    HeadsUpDisplay();

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    //suggests that the user tries putting correct input 
                    Console.WriteLine("\nYour gun jammed! \n\nYou can't be losing your precious ammo like that!");
                    Console.WriteLine("\nPress Enter: ");
                    Console.ReadKey();
                    //enemy still attacks
                    EnemyAttack();
                    break;
            }
        }
        /// <summary>
        /// instructions on how to play
        /// </summary>
        public static void HowToPlay()
        {
            //list of instructions to play game
            List<string> instructions = new List<string>()
            {
                "As Blade Runner, \n    you have 2 tools at your disposal for eliminating Replicant Husks.",
                "First is your Automatic Machine Dispersion Pistol, or \"ADP\".",
                "With a high cyclic rate of fire, \n    you are much more likely to MISS using this weapon.",
          
            };
            for (int i = 0; i < instructions.Count; i++)
            {

                Console.WriteLine("How to play: \n\n");
                //scroll text from list
                foreach (object c in instructions[i])
                {
                    Console.Write(c);
                    Thread.Sleep(10);
                }
                Thread.Sleep(300);
                Console.WriteLine("\n\n           Press any key to continue: ");
                Console.ReadKey();
                Console.Clear();
                //show static automatic gun
                AutoMaticGun();
                Console.ForegroundColor = ConsoleColor.White;
            }
            //second set of instructions for pistol
            List<string> instructions2 = new List<string>()
            {
                      "Next is your semi-automatic side-arm.",
                "The .45 Cal Anti-Matter Pistol is much more reliable, \n    but is much less effective \n      against the metallic frame of the Replicant Husks.",
                "\n     Each attack uses ammo:  \"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"  \n            so be careful.",
                "If you run out of ammo while there are still Replicants left to eliminate, \n    they will overwhelm the city, and eat it's remnants alive.",
                "We cannot lose 3 of our major cities, Blade Runner. \nWe're counting on you!"
            };
            for (int i = 0; i < instructions2.Count; i++)
            {
                Console.WriteLine("How to play: \n\n");
                //scroll text
                foreach (object a in instructions2[i])
                {
                    Console.Write(a);
                    Thread.Sleep(10);
                }
                Thread.Sleep(300);
                Console.WriteLine("\n\n          Press any key to continue: ");
                Console.ReadKey();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                //show static pistol
                SemiAutoGun();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /// <summary>
        /// scrolls text from top of screen
        /// </summary>
        public static void TitleScrollFromTop()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            int middle = 0;
            string title = @"
 ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███╗   ██╗███████╗████████╗██╗██╗  ██╗
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗████╗  ██║██╔════╝╚══██╔══╝██║╚██╗██╔╝
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██╔██╗ ██║█████╗     ██║   ██║ ╚███╔╝ 
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║╚██╗██║██╔══╝     ██║   ██║ ██╔██╗ 
╚██████╗   ██║   ██████╔╝███████╗██║  ██║██║ ╚████║███████╗   ██║   ██║██╔╝ ██╗
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝   ╚═╝   ╚═╝╚═╝  ╚═╝
";
            while (middle < 17)
            {
                Console.Clear();
                title = "\n\n" + title;
                Console.WriteLine(title);
                Thread.Sleep(200);
                middle++;

            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Basic Display Logo
        /// </summary>
        public static void TitleScroll()
        {

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string caption = "Humans Vs. Machines\n";
            string title = @"
 ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███╗   ██╗███████╗████████╗██╗██╗  ██╗
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗████╗  ██║██╔════╝╚══██╔══╝██║╚██╗██╔╝
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██╔██╗ ██║█████╗     ██║   ██║ ╚███╔╝ 
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║╚██╗██║██╔══╝     ██║   ██║ ██╔██╗ 
╚██████╗   ██║   ██████╔╝███████╗██║  ██║██║ ╚████║███████╗   ██║   ██║██╔╝ ██╗
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝   ╚═╝   ╚═╝╚═╝  ╚═╝
";

            Console.WriteLine(title);
            Thread.Sleep(700);
            Console.WriteLine();
            //create space for caption placement
            Console.Write("                               ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            //only scroll caption once
            if (CaptionCount == 0)
            {
                //scroll text from caption
                foreach (object c in caption)
                {

                    Console.Write(c);
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                //only scroll caption once
                CaptionCount++;
            }
            //regular caption placement
            else
            {
                Console.WriteLine(caption);
            }
        }
        /// <summary>
        /// Story Text
        /// </summary>
        public static void StoryLine()
        {
            //keeps city at top of screen
            CityLogo();
            TitleScroll();
            int loopCounter = 0;

            //plot 
            List<string> plot = new List<string>()
            {
            "The year is 2199.",
            "Mankind has created the first artificial humans.",
            "Empty mechanized husks, \n    known as Replicants, \n      have become the key to perfect health, and immortality.",
            "The worlds wealthiest men \n     had their identities implanted into these Replicants...",
            "...but what nobody told them \n    will change the course of history forever.",
            "The innate physical needs of man: \n    sleep, hunger, touch; \n     those needs were not overcome by Replicant technology.",
            "Slowly, \n\n    those who were implanted into Replicant Husks began spiraling downard \n         ...into insanity.",
            "Without the ability to sleep or eat, \n   these Replicants became mechanized zombies, \n     attacking anything that moved, out of ever worsening dimentia.",
            "You are a Blade Runner.\n You and your team are all that stand between humanity, \n     and humanities self wrought extinction.",
             };
            for (int i = 0; i < plot.Count; i++)
            {

                //scroll list text
                foreach (object c in plot[i])
                {
                    Console.Write(c);
                    Thread.Sleep(25);
                }
                Console.WriteLine("\n");
                Thread.Sleep(500);
                loopCounter++;
                //prints out 5 items from list, then asks for user to continue
                if (loopCounter == 5)
                {
                    loopCounter = 0;
                    Console.WriteLine("Press any key to continue: ");
                    Console.ReadLine();
                    Console.Clear();
                    CityLogo();
                    TitleScroll();
                }
            }

            Console.WriteLine("           Press any key to continue: ");
            Console.ReadKey();
            CityLogo();
            //call how to play text
            HowToPlay();
        }
        /// <summary>
        /// Heads up display
        /// Showing bullets, money, cities count etc
        /// </summary>
        public static void HeadsUpDisplay()
        {
            //keeps track of """"""
            string bulletHud = "";
            string enemiesHud = "";
            string moneyHud = "";
            //indentation count
            int hudReset = 0;
            CityLogo();
            TitleScroll();
            //cities count and implants status
            //displays current level
            Console.WriteLine("Cash Bonus: ${1}/20 kills    Enemy Max Damage: {2}    Level: {0}\n", LevelCounter, 5 + MoneyReward, EnemyScalingDamage);
            //implants on
            if (Implants > 0 && grenade > 0)
            {
                Console.WriteLine("  Implants: (⌐■_■) on                                         Cities Cleared: {0}", CitiesCleared);
                Console.WriteLine("EMP Effect:  \\-^-/ on                                           Cities Lost: {0}\n", CitiesLost);
            }
            else if (Implants > 0)
            {
                Console.WriteLine("  Implants: (⌐■_■) on                    Cities Cleared: {0}", CitiesCleared);
                Console.WriteLine("EMP Effect:  /---\\ off                      Cities Lost: {0}\n", CitiesLost);
            }
            else if (grenade > 0)
            {
                Console.WriteLine("  Implants: ( o_o) off                    Cities Cleared: {0}", CitiesCleared);
                Console.WriteLine("EMP Effect:  \\-^-/ on                        Cities Lost: {0}\n", CitiesLost);

            }
            //implants off
            else if (Implants <= 0 && grenade <= 0)
            {
                Console.WriteLine("  Implants: ( o_o) off                    Cities Cleared: {0}", CitiesCleared);
                Console.WriteLine("EMP Effect:  /---\\ off                       Cities Lost: {0}\n", CitiesLost);
            }
            //scroll through and ad """"" to bullets hud
            for (int i = 0; i < BulletsLeft; i += 2)
            {
                //add " to string
                bulletHud = bulletHud + "\"";
                //onlhy print 25 per line
                hudReset++;
                if (hudReset == 25)
                {
                    bulletHud = bulletHud + "\n";
                    hudReset = 0;
                }
            }
            //reset  hud count for each item
            //build enemies hud
            hudReset = 0;
            Console.WriteLine("Bullets Left: {1}\n{0}", bulletHud, BulletsLeft);
            for (int i = 0; i < EnemiesLeft; i += 2)
            {
                enemiesHud = enemiesHud + "\"";
                hudReset++;
                if (hudReset == 25)
                {
                    enemiesHud = enemiesHud + "\n";
                    hudReset = 0;
                }
            }
            //build money hud
            hudReset = 0;
            Console.WriteLine("Enemies Left: {1}\n{0}", enemiesHud, EnemiesLeft);
            for (int i = 0; i < MoneyLeft; i += 2)
            {
                moneyHud = moneyHud + "\"";
                hudReset++;
                if (hudReset == 25)
                {
                    moneyHud = moneyHud + "\n";
                    hudReset = 0;
                }
            }
            hudReset = 0;
            Console.WriteLine("Money Left: ${1}\n{0}", moneyHud, MoneyLeft);

        }
        /// <summary>
        /// City image
        /// </summary>
        public static void CityLogo()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Clear();
            Console.WriteLine(@"
         /\ \ \ \/_______/     ______/\      \  /\ \/ /\ \/ /\  \_____________
        /\ \ \ \/______ /     /\    /:\\      \ ::\  /::\  /::\ /____  ____ __
       /\ \ \ \/_______/     /:\\  /:\:\\______\::/  \::/  \::///   / /   //
      /\ \ \ \/_______/    _/____\/:\:\:/_____ / / /\ \/ /\ \///___/ /___//___
_____/___ \ \/_______/    /\::::::\\:\:/_____ / \ /::\  /::\ /____  ____  ____
         \ \/_______/    /:\\::::::\\:/_____ /   \\::/  \::///   / /   / /   /
          \/_______/    /:\:\\______\/______/_____\\/ /\ \///___/ /___/ /_____
\          \______/    /:\:\:/_____:/\      \ ___ /  /::\ /____  ____  _/\::::
\\__________\____/    /:\:\:/_____:/:\\      \__ /_______/____/_/___/_ /  \:::
//__________/___/   _/____:/_____:/:\:\\______\ /                     /\  /\::
///\          \/   /\ .----.\___:/:\:\:/_____ // \                   /  \/  \:
///\\          \  /::\\ \_\ \\_:/:\:\:/_____ //:\ \                 /\  /\  /\
//:/\\          \//\::\\ \ \ \\/:\:\:/_____ //:::\ \               /  \/  \/+/
/:/:/\\_________/:\/:::\`----' \\:\:/_____ //o:/\:\ \_____________/\  /\  / /
:/:/://________//\::/\::\_______\\:/_____ ///\_\ \:\/____________/  \/  \/+/\
/:/:///_/_/_/_/:\/::\ \:/__  __ /:/_____ ///\//\\/:/ _____  ____/\  /\  / /  \
:/:///_/_/_/_//\::/\:\///_/ /_//:/______/_/ :~\/::/ /____/ /___/  \/  \/+/\  /
/:///_/_/_/_/:\/::\ \:/__  __ /:/____/\  / \\:\/:/ _____  ____/\  /\  / /  \/
:///_/_/_/_//\::/\:\///_/ /_//:/____/\:\____\\::/ /____/ /___/  \/  \/+/\  /\
///_/_/_/_/:\/::\ \:/__  __ /:/____/\:\/____/\\/____________/\  /\  / /  \/  \
//_/_/_/_//\::/\:\///_/ /_//::::::/\:\/____/  /----/----/--/  \/  \/+/\  /\  /
/_/_/_/_/:\/::\ \:/__  __ /\:::::/\:\/____/ \/____/____/__/\  /\  / /  \/  \/_
_/_/_/_//\::/\:\///_/ /_//\:\::::\:\/____/ \_____________/  \/  \/+/\  /\  /
/_/_/_/:\/::\ \:/__  __ /\:\:\::::\/____/   \ _ _ _ _ _ /\  /\  / /  \/  \/___
_/_/_//\::/\:\///_/ /_//\:\:\:\              \_________/  \/  \/+/\  /\  /   /
/_/_/:\/::\ \:/__  __ /\:\:\:\:\______________\       /\  /\  / /  \/  \/___/_
_/_//\::/\:\///_/ /_//::\:\:\:\/______________/      /  \/  \/+/\  /\  /   /
/_/:\/::\ \:/__  __ /::::\:\:\/______________/\     /\  /\  / /  \/  \/___/___
_//\::/\:\///_/ /_//:\::::\:\/______________/  \   /  \/  \/+/\  /\  /   /   /
/:\/::\ \:/__  __ /:\:\::::\/______________/    \ /\  /\  / /  \/  \/___/___/
/\::/\:\///_/ /_//:\:\:\                         \  \/\\\/+/\  /\  /   /   /+/
\/::\ \:/__  __ /:\:\:\:\_________________________\ ///\\\/  \/  \/___/___/ /_
::/\:\///_/ /_//:\:\:\:\/_________________________////::\\\  /\  /   /   /+/
");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Katana Image
        /// </summary>

        public static void Katana()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            string fullKatana = @"
           ___ 
　　　　　　　 |//|
          |//|
          |//|
          |//|
          |//|
          |//|
          |//|
       ___|//|___
      /<>______<>\
     // / |/\| \ \\
     //   //\\   \\
         //  \\
        |/)dd(\|
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          )  (
          (  )
           \/";
            int katanaFly = 0;
            //katana flies down the screen
            while (katanaFly < 15)
            {
                fullKatana = "\n" + fullKatana;
                Console.WriteLine(fullKatana);
                Thread.Sleep(10);
                Console.Clear();
                katanaFly++;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Gun animation
        /// </summary>
        public static void AutoGunAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
  OO                    QQQQ     OO
 OOOO                    QQ     OOOO
WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
XXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
XXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
XXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
     OO                    QQQQ     OO
    OOOO                    QQ     OOOO
   WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
SSSXXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
SSSXXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
SSSXXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
  OO                    QQQQ     OO
 OOOO                    QQ     OOOO
WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
XXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
XXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
XXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(@"                                                                        .
     OO                    QQQQ     OO
    OOOO                    QQ     OOOO
   WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
SSSXXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
SSSXXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
SSSXXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
  OO                    QQQQ     OO
 OOOO                    QQ     OOOO
WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
XXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
XXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
XXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
     OO                    QQQQ     OO
    OOOO                    QQ     OOOO
   WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
SSSXXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
SSSXXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
SSSXXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
  OO                    QQQQ     OO
 OOOO                    QQ     OOOO
WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
XXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
XXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
XXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.White;

        }
        /// <summary>
        /// still image of auto gun
        /// </summary>
        public static void AutoMaticGun()
        {

            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                                                                        .
     OO                    QQQQ     OO
    OOOO                    QQ     OOOO
   WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
SSSXXXXXXXXXXXX--------XXXXXXXXXXXXXXXXXWmmmmmm
SSSXXXXXXXXXXXX--------XXXXXXWWWWWWWWWWWWMMMMMM
SSSXXXXXXXXXXXXxxxxxxxxXXXXXXWWWWWWWWWWWW
SSS   SSS     CEEEEEEEE C  Z
SSS SS        CWXXXXXXW  C Z
SSSS          CWXXXXXXWZZZZ
SSS            WXXXXXXW
SSS            WXXXXXXW
               WXX++XXW
                NNNNNN
                NNNNNN
");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Pistol Animation
        /// </summary>
        public static void SemiAutoGunAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
 8888o.o.o.o.ooooooo00||00oooooooooooooooooooo88o   ./'
  88:8:8:8:8:888888800||0088888888888888888888888://____
  88:8:8:8:8:888888888888888888888888888888888888:\
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP    ''
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
8o.o.o.o.ooooooo00||00oooooooooooooooooooo88o  ':/
:8:8:8:8:888888800||0088888888888888888888888://        _______
8:8:8:8:8:888888888888888888888888888888888888:\\
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP\\\
  ` 8oooooooooooooooooooooooooo                  . >\
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();

            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                   /
.o.o.ooooooo00||00oooooooooooooooooooo88o____     ../
 8:8:888888800||0088888888888888888888888:   |                    _______ 
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP   \\
  ` 8oooooooooooooooooooooooooo                       .
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
 ooooooo00||00oooooooooooooooooooo88o________
 888888800||0088888888888888888888888:       |                                ______
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
 .ooooooo00||00oooooooooooooooooooo88o_______
 :888888800||0088888888888888888888888:      |                                     
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
.o.o.ooooooo00||00oooooooooooooooooooo88o____
:8:8:888888800||0088888888888888888888888:   |   
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.Clear();
            CityLogo();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
 8888o.o.o.o.ooooooo00||00oooooooooooooooooooo88o
  88:8:8:8:8:888888800||0088888888888888888888888:       
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.White;

        }
        /// <summary>
        /// static image of pistol
        /// </summary>
        public static void SemiAutoGun()
        {
            Console.Clear();
            CityLogo();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
 8888o.o.o.o.ooooooo00||00oooooooooooooooooooo88o
  88:8:8:8:8:888888800||0088888888888888888888888:
  88:8:8:8:8:888888888888888888888888888888888888:
 Y88:8:8:8:8:8888888888888888888oooooooooooooooP
  ` 8oooooooooooooooooooooooooo
   .88888888888.`:::      8
   88888888888Yo   `` *   8
  .88888888888 `oooooooood8o
  88888888888'
 .88888888888
 88888888888'
.88888888888
98888888888'
");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// critical hit animation
        /// </summary>
        public static void BulletImpactAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                    ____                         |          '|
              _ -- |    \                        |          '|
                -- |    /                        |          '|
                    ~~~~                          \_________/
");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                         _____                   |          '|
                   _ -- |     \                  |          '|
                     -- |     /                  |          '|
                         ~~~~                     \_________/

");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                              ____               |          '|
                        _ -- |     \             |          '|
                          -- |     /             |          '|
                              ~~~~~               \_________/

");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                                   _____         |          '|
                             _ -- |      \       |          '|
                               -- |      /       |          '|
                                   ~~~~           \_________/
");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                                         _____   |          '|
                                   _ -- |      \ |          '|
                                     -- |      / |          '|
                                         ~~~~     \_________/
");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                   ___  '___
                    CRITICAL HIT!!!               /   \I/  '\
                                             ____I          '|
                                       _ -- |    I          '|
                                         -- |    I          '|
                                             ~~~~ \ ________/
");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
               '
                                                              '
                                                  ___  '__'/
                    CRITICAL HIT!!!            \ /   \I/ ///
                                               \       ==%//='          .
                                               = >    %%%%=       
                                               /     ##%%%       
                                               /  \ ____ =\\      
                                                        ~ '\\    .            
                                                            '
                                                              
");

            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"               '

                                                              '
                                                  ___  '__'//
                                               \ /   \I/ ///
                                               \       ==%//=    .
                                               = >    %%%%====%=-'       
                                               /     ##%%%==\      
                                               /  \ ____ =\\%     
                                                        ~ '\\\    .            
                                                            '\
                                                              

");
            Thread.Sleep(300);

            Console.Clear();
            CityLogo();
            Console.WriteLine(@"
                                                              '//'
                                                  ___  '__'///'  .
                                               \ /   \I/ /////'     ^/.
                                               \       ==%//='          .
                                               = >    %%%%====%=-'===_##&$6      
                                               /     ##%%%==\\'          
                                               /  \ ____ =\\%\\'      ?.,\  
                                                        ~ '\\\'       .            
                                                            '\\' .
                                                              ''
");
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.White;


        }
        /// <summary>
        /// enemy attack function
        /// determines whether the enemy attacks, reinforces etc
        /// </summary>
        public static void EnemyAttack()
        {
            Random rand = new Random();
            ///enemies chance to hit
            int reinforcements = 0;
            int reinforcementChance = 70;
            ChanceToHit = rand.Next(1, 101);
            //80% chance to hit
            if (ChanceToHit > 12)
            {
                //if there are still enemies left, attack
                if (EnemiesLeft > 0)
                {
                    //enemy damage
                    EnemyDamage = rand.Next(5, EnemyScalingDamage);
                    BulletsLeft = BulletsLeft - EnemyDamage;
                    if (BulletsLeft < 0)
                    {
                        BulletsLeft = 0;
                    }
                    HeadsUpDisplay();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nA replicant attacked a civilian. \n\nYou used {0} bullets to take him down!", EnemyDamage);
                    Console.WriteLine("\nPress Enter: ");
                    Console.ReadKey();

                }

            }
            ChanceToHit = 0;
            //enemies have a 30% chance to reinforce their numbers
            reinforcements = rand.Next(1, 101);
            //if grenade debuff is active, only 5% chance of reiforcments for 3 rounds

            //enemies reinforce or don't
            if (grenade > 0)
            {
                reinforcementChance = 95;
                grenade--;
            }
            if (reinforcements > reinforcementChance && EnemiesLeft > 0)
            {
                HeadsUpDisplay();
                //reinforce with 15-50 enemies per wave
                ChanceToHit = rand.Next(15, MaxReinforcement);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("A WAVE OF {0} REPLICANTS HAS STORMED THE CITY!!!", ChanceToHit);
                Console.WriteLine(@"
      @  \@/ |@__ \@ __@ __@/  @/ __@|
    /|\  |   |    |\  /|   |  /|    |
    / \ / \ / \  / \  / \ / \ / \  / \");
                Console.WriteLine("       Enemy numbers reinforced!");

                EnemiesLeft = EnemiesLeft + ChanceToHit;
                Console.WriteLine("\nPress Enter: ");
                Console.ReadKey();




                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        /// <summary>
        /// tracks average damage per weapon
        /// </summary>
        public static void Stat()
        {
            //avg katana %
            int katanaStat = 0;
            //avg pistol %
            int pistolStat = 0;
            //avg 
            int APDStat = 0;
            int totalDmg = 0;
            int totalAtk = 0;
            if (katanaAtk > 0)
            {
                katanaStat = katanaDmg / katanaAtk;
            }
            if (pistolAtk > 0)
            {

                pistolStat = pistolDmg / pistolAtk;
            }
            if (APDAtk > 0)
            {
                APDStat = APDDmg / APDAtk;
            }
            totalDmg = katanaDmg + pistolDmg + APDDmg;
            totalAtk = (katanaAtk + pistolAtk + APDAtk);

            int totalStat = totalDmg / totalAtk;
            CityLogo();
            TitleScroll();

            //final scoreboard tally
            Console.WriteLine(@"
      Cities Cleared: {0}
         Cities Lost: {1}", CitiesCleared, CitiesLost);
            Thread.Sleep(500);
            Console.WriteLine("You did an average of {0} damage with the APD.", APDStat);
            Thread.Sleep(500);
            Console.WriteLine("You did an average of {0} damage with the Side-Arm.", pistolStat);
            Thread.Sleep(500);
            Console.WriteLine("You did an average of {0} damage with the KATANA.", katanaStat);
            Thread.Sleep(500);
            Console.WriteLine("           You bought {0} grenades", GrenadeUsage);
            Thread.Sleep(500);
            Console.WriteLine("           You bought {0} supply crates.", buyAmmo);
            Thread.Sleep(500);
            Console.WriteLine("             You used {0} implants.", ImplantsTotal);
            Thread.Sleep(500);
            Console.WriteLine("          You earned ${0}.", CashEarned);
            Thread.Sleep(500);
            Console.WriteLine("             You used {0} Tactical Nukes.", TacticalNuke);
            Thread.Sleep(500);
            Console.WriteLine("\nYou did an overall average of {0} damage.", totalStat);
            Thread.Sleep(500);

        }
        /// <summary>
        /// grenade throw animation
        /// </summary>
        public static void GrenadeBoom()
        {
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                      
                          
                          
       o,                 
      /                  
-O-                      
  \                      
  /\ ...............................
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"

                         
              o,            
             /                                  
  _O/                        
  /|                         
  /\ .............................

");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"

                        = o,
                          
                                                    
   O___                        
  /|                         
  /\ .............................
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                     
                        
                         \
                          o, 
                                                    
   O                          
  /|\                          
  /\ .............................
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"           
                        
                        
                           \ 
                            o,                       
   O                          
  /|\                          
  /\ .............................
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                    
                        
                        
                        
                             \                        
   O                          o,
  /|\                          
  /\ .............................
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"                     
                        
                        
                        
                                                     
   O                          \
  /|\                          o,
  /\ .............................
 
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"\         .  ./
                           \      .: ;'.:..    /
                               (M^^.^~~:.' ).
                         -   (/  .    . . \ \)  -
                          ((| :. ~ ^  :. .|))
   O                     -   (\- |  \ /  |  /)  -
  /|\                         -\  \     /  /-
  /\ ...............................\  \   /  /
");
            Thread.Sleep(200);
            Console.Clear();
            CityLogo();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"        .  ./
                           \      .: ;'.:..    /
                               (M^^.^~~:.' ).
                         -   (/  .    . . \ \)  -
  .                        ((| :. ~ ^  :. .|))
   \O__.                   -   (\- |  \ /  |  /)  -
   /                         -\  \     /  /-
  /\ ...............................\  \   /  /
");
            Thread.Sleep(1000);
            Console.Clear();
        }
        /// <summary>
        /// this function rewards players for getting accumulated kills
        /// </summary>
        public static void NewMoneyAdder()
        {

            int newMoney = 0;

            //adds money to stash for each 25 enemies killed
            if (enemiesKill >= 20)
            {
                HeadsUpDisplay();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                //for every 20 kills get 5 bucks
                //how many increments are payed out
                MoneyIncrement = enemiesKill / 20;
                newMoney = MoneyIncrement * 5;
                MoneyLeft = MoneyLeft + newMoney + (MoneyReward * MoneyIncrement);
                CashEarned = CashEarned + (MoneyReward * MoneyIncrement) + newMoney;
                Console.WriteLine("\nYou killed {1} enemies.\n\n ${0} added to your stash.", (MoneyReward * MoneyIncrement)+ newMoney, enemiesKill);
                //while subtracting 25 is not negative
                while (enemiesKill - 20 > 0)
                {
                    //subtract 25
                    //this old kills aren't lost
                    enemiesKill -= 20;
                }

                Console.WriteLine("\nPress any key to continue: ");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /// <summary>
        /// tatctical nuke animation
        /// </summary>
        public static void TacticalNukeAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine(@"
                                                 /
                                                /      /
                                               /  /   /  /         /
                                              /      /       /    /
                                            ,---------------.   ,-,
                                           /                 `-'  |
                                          [                   |   |
                                           \                 ,-.  |
                                            `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"


                                            /
                                           /      /
                                          /  /   /  /         /
                                         /      /       /    /
                                       ,---------------.   ,-,
                                      /                 `-'  |
                                     [                   |   |
                                      \                 ,-.  |
                                       `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"




                                    /
                                   /      /
                                  /  /   /  /         /
                                 /      /       /    /
                               ,---------------.   ,-,
                              /                 `-'  |
                             [                   |   |
                              \                 ,-.  |
                               `---------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"




                                    /
                                   /    /
                                  /  /    /         /
                                 /    /       /    /
                               ,-------------.   ,-,
                              /               `-'  |
                             [                 |   |
                              \               ,-.  |
                               `-------------'   `-`");
            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"






                                /

                               /    /
                              /  /    /         /
                             /    /       /    /
                           ,-----------.   ,-,
                          /             `-'  |
                         [               |   |
                          \             ,-.  |
                           `-----------'   `-`








                       __________________");

            Thread.Sleep(200);
            Console.Clear();


            Console.WriteLine(@"







                                
                               /    /
                              /  /    /
                             /    //
                          ,----.,-,                          
                         [      ||
                          `----' `-`




                _________________________________");

            Thread.Sleep(200);
            Console.Clear();

            Console.WriteLine(@"











                                
                           /    /
                          /  /    /
                         /    //
                      ,----.,-,                          
                     [      ||
                      `----' `-`
                _________________________________");

            Thread.Sleep(200);
            Console.Clear();
            Console.WriteLine(@"








            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                 
                                _        _  _ _     _       .   .   .   
 .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
    .       .           .   (   /(/ (  )  ) )\          .     .               
                        
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"







            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )                  
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
--------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"





            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )         
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
     .      .    .     (  . ( ( ( | | ) ) )\   )               .            
                        (   /(| / ( )) ) ) )) )    .   .  .       .  .  .   
 .     .       .  .   (  .  ( ((((_(|)_)))))     )            .             
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(300);
            Console.Clear();
            Console.WriteLine(@"
            .           .   ________________    .        .                 
                  .    ____/ (  (    )   )  \___                           
            .         /( (  (  )   _    ))  )   )\        .   .            
                    ((     (   )(    )  )   (   )  )   .                   
         .    .   ((/  ( _(   )   (   _) ) (  () )  )        .   .         
                 ( (  ( (_)   ((    (   )  .((_ ) .  )_                    
                                         )             )         
  .       .     (_((__(_(__(( ( ( |  ) ) ) )_))__))_)___)   .              
      .         ((__)        \\||lll|l||///          \_))       .          
               .       . / (  |(||(|)|||//  \     .    .      .      .    
 .       .           .   (   /(/ (  )  ) )\          .     .               
     .      .    .     (  . ( ( ( | | ) ) )\   )               .            
                        (   /(| / ( )) ) ) )) )    .   .  .       .  .  .   
 .     .       .  .   (  .  ( ((((_(|)_)))))     )            .             
         .  .          (    . ||\(|(|)|/|| . . )        .        .          
     .           .   (   .    |(||(||)||||   .    ) .      .         .  .   
 .      .      .       (     //|/l|||)|\\ \     )      .      .   .         
                     (/ / //  /|//||||\\  \ \  \ _)                         
-------------------------------------------------------------------------------
                ");
            Thread.Sleep(2500);

        }

        /// <summary>
        /// different display screens for whether tactical nuke is available
        /// </summary>
        public static void GameScreen()
        {
            if (MoneyLeft < 100)
            {


                Console.WriteLine(@"
Replicants are infesting the city!   .......  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
1. Automatic Dispersion Pistol (ADP) ....... @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                     ....... @@@@@@@@@@ oo o@@@@o @@@@@@@@@oooo
2. 45 Caliber Anti-Matter Pistol     ....... @@@ @a0000000000000000a  a00000a00
                                     ....... @@@@ 0000000000000000000 000000000
3. Resupply Drop                     ........ @@ 0000 0000000000000000000000000
                                     ........ @@@ 0000 000000000000000000000000
4. Katana                            ........ @@@ 000 0000000000000000000000000
                                     ........ @@@ 000 0000000000000000000000000
5. EMP Grenade                       ......... @@@ 00 00000000000 00000 0000000
                                     ...... 00;.                 aaaaa a       
6. Accuracy Implants                 .......0000 00000ta        /00000 000\  
                                     .......`000 000000000000mn000000 0000mn000
  ,-.__________________,======= ,    ....... 000 00000000000000000000 000000000
[ (   _  _  _ )_______)  \\\\\ ((t   ........ 00 00000000000000000000 000000000
  /================.-.______,--'_\   ......... 0 0000000000000000 00000 0000000
  \_,__,_________\     [ ]    /      ........... 0a  00000000000 0000000 000000
            \ (   )) )   o   (       ............ 000a00000000000mm   mm0000000
             \ \____/    \    \      ............ 0000 000000000000000000000000
              ' ===''\    \    \     ............  000000000000            0000
                      \    \    \    ......        a 000000000m0000000000000000
                       )____\   |    .....      ,' 00a 000000000          00000
                       ) __, __,'    ...      ,'   0000a 0000000000000000000000
                        '--''        .      ,'     `00000a 00000000000000000000
");
            }
            if (MoneyLeft >= 100)
            {
                Console.WriteLine(@"
Replicants are infesting the city!   .......  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
1. Automatic Dispersion Pistol (ADP) ....... @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                     ....... @@@@@@@@@@ oo o@@@@o @@@@@@@@@oooo
2. 45 Caliber Anti-Matter Pistol     ....... @@@ @a0000000000000000a  a00000a00
                                     ....... @@@@ 0000000000000000000 000000000
3. Resupply Drop                     ........ @@ 0000 0000000000000000000000000
                                     ........ @@@ 0000 000000000000000000000000
4. Katana                            ........ @@@ 000 0000000000000000000000000
                                     ........ @@@ 000 0000000000000000000000000
5. EMP Grenade                       ......... @@@ 00 00000000000 00000 0000000
                                     ...... 00;.                 aaaaa a       
6. Accuracy Implants                 .......0000 00000ta        /00000 000\  
                                     .......`000 000000000000mn000000 0000mn000
7. --TACTICAL NUKE ONLINE--          ....... 000 00000000000000000000 000000000
[ (   _  _  _ )_______)  \\\\\ ((t   ........ 00 00000000000000000000 000000000
  /================.-.______,--'_\   ......... 0 0000000000000000 00000 0000000
  \_,__,_________\     [ ]    /      ........... 0a  00000000000 0000000 000000
            \ (   )) )   o   (       ............ 000a00000000000mm   mm0000000
             \ \____/    \    \      ............ 0000 000000000000000000000000
              ' ===''\    \    \     ............  000000000000            0000
                      \    \    \    ......        a 000000000m0000000000000000
                       )____\   |    .....      ,' 00a 000000000          00000
                       ) __, __,'    ...      ,'   0000a 0000000000000000000000
                        '--''        .      ,'     `00000a 00000000000000000000
");

            }
        }


    }
}
