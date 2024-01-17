using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace ConsoleApp3
{
    enum ESlotType
    {
        Empty,
        Text,
        Card,
        EnemyCard
    }
    internal class Program
    {
        static int MyScore = 0;
        static int EnemyScore = 0;
        static int input = 0;
        static int[] indexes = { 1, 2, 3, 4, 5 };
        static string[] BattlePhrases = { "Draw", "Cmon maaan that's too easy", "Never back down never what? Never give up!", "GG bro..." };
        static int Battleresult = 0;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = "C:\\Games\\ConsoleApp3\\ConsoleApp3\\Papka\\Text.txt";
            while (true)
            {
                Card[] cards = GetCardsFromFile(path);
                Card[] playersCards = { cards[0], cards[1], cards[2], cards[3], cards[4] };
                Card[] enemyCards = { cards[5], cards[6], cards[7], cards[8], cards[9] };
                Card TempCard;
                Card EnemyTempCard;
                Hand enemyHand = new Hand(15, 9, enemyCards);
                Hand playersHand = new Hand(15, 9, playersCards);
                Field field = new Field(16);
                List<int> indexes = new List<int> { 1, 2, 3, 4, 5 };
                List<int> EnemyIndexes = new List<int> { 0, 1, 2, 3, 4 };
                bool ContainIndex;
                // draw field
                while (true) // turns loop
                {
                    Console.Clear();
                    if (playersCards[0] == null && playersCards[1] == null && playersCards[2] == null && playersCards[3] == null && playersCards[4] == null && enemyCards[0] == null && enemyCards[1] == null && enemyCards[2] == null && enemyCards[3] == null && enemyCards[4] == null)
                    {
                        break;
                    }
                    // draw field
                    Console.WriteLine($"Score: {MyScore}:{EnemyScore}");
                    Console.WriteLine();
                    enemyHand.DrawHand(enemyCards, false);
                    field.DrawField();
                    playersHand.DrawHand(playersCards, true);
                    // wait for input
                    Console.Write("Enter the card number: ");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int a))
                    {
                        Console.WriteLine("The input doesn't contain a number");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    int InputInt = int.Parse(input);
                    ContainIndex = indexes.Contains(InputInt);
                    if (!ContainIndex)
                    {
                        Console.WriteLine("The input doesn't contain a number");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    // move
                    // remove card from hand
                    indexes.Remove(InputInt);
                    Random rand = new Random();
                    int EnemyInput = 0;
                    do
                    {
                        EnemyInput = rand.Next(0, 5);
                    }
                    while (enemyCards[EnemyInput] == null);
                    TempCard = playersCards[InputInt - 1];
                    EnemyTempCard = enemyCards[EnemyInput];
                    playersCards[InputInt - 1] = null;
                    enemyCards[EnemyInput] = null;
                    PlayCard(TempCard, EnemyTempCard, enemyHand, playersHand, enemyCards, playersCards, InputInt, EnemyInput);
                    Thread.Sleep(5000);
                    Battle(TempCard, EnemyTempCard);
                    Thread.Sleep(5000);
                    AfterBattle(enemyHand, playersHand, enemyCards, playersCards, field);
                    Thread.Sleep(3000);
                    Console.Clear();
                    ClearField(enemyHand, playersHand, enemyCards, playersCards, field);
                    // add card to field
                    // enemy move
                    // remove card from hand
                    // add card to field
                    // battle
                    // check for winner
                    // remove both cards
                    // add points
                    // check for end


                }
                WriteResult();
                MyScore = 0;
                EnemyScore = 0;
                Console.WriteLine("Реванш?");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Пробіл - реванш");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Усе інше - вийти з гри");
                ConsoleKeyInfo KeyInput = Console.ReadKey();
                if (KeyInput.Key != ConsoleKey.Spacebar)
                    break;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        private static Card[] GetCardsFromFile(string path)
        {
            // read the file
            string[] lines = File.ReadAllLines(path);
            // transform the text in the file into cards
            Card[] cards = new Card[lines.Length / 4];
            for (int i = 0; i < lines.Length; i += 4)
            {
                string name = lines[i];
                int damage = int.Parse(lines[i + 1]);
                int HP = int.Parse(lines[i + 2]);
                int AttackSpeed = int.Parse(lines[i + 3]);
                cards[i / 4] = new Card(name, damage, HP, AttackSpeed);
            }
            // mix cards
            Random random = new Random();
            for (int i = 0; i < cards.Length; i++)
            {
                int index = random.Next(0, cards.Length - 1);
                Card temp = cards[i];
                cards[i] = cards[index];
                cards[index] = temp;
            }
            // return the cards
            return cards;
        }
        private static void PlayCard(Card card1, Card card2, Hand EnemyHand, Hand PlayersHand, Card[] EnemyCards, Card[] PlayersCards, int playerInput, int enemyInput)
        {
            Console.Clear();
            Console.WriteLine($"Score: {MyScore}:{EnemyScore}");
            Console.WriteLine();
            EnemyHand.DrawHand(EnemyCards, false);
            for (int i = 0; i < 4; i++)
                Console.WriteLine();
            Console.Write("          ");
            Console.Write($"{card1.Name}       ");
            Console.Write(playerInput);
            Console.Write("      ");
            Console.Write($"{card2.Name}       ");
            Console.Write(enemyInput);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("          ");
            Console.Write("HP:    ");
            Console.Write($"0{card1.HP}");
            Console.Write("      ");
            Console.Write("HP:    ");
            Console.Write($"0{card2.HP}");
            Console.WriteLine();
            Console.Write("          ");
            Console.Write("Dmg:   ");
            Console.Write($"0{card1.Damage}");
            Console.Write("      ");
            Console.Write("Dmg:   ");
            Console.Write($"0{card2.Damage}");
            Console.WriteLine();
            Console.Write("          ");
            Console.Write("Spd:   ");
            Console.Write($"0{card1.AttackSpeed}");
            Console.Write("      ");
            Console.Write("Spd:   ");
            Console.Write($"0{card2.AttackSpeed}");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine();
            }
            PlayersHand.DrawHand(PlayersCards, true);
        }
        private static void Battle(Card card1, Card card2)
        {
            for (int i = 0; i < card1.AttackSpeed; i++)
                card2.GetDamage(card1.Damage);
            for (int i = 0; i < card2.AttackSpeed; i++)
                card1.GetDamage(card2.Damage);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\sekira-i-mech-soshlis-v-jestokoy-shvatke.wav");
            player.Play();
            if (card1.HP > card2.HP)
            {
                MyScore += 1;
                Battleresult = 1;
            }
            else if (card1.HP < card2.HP)
            {
                EnemyScore += 1;
                Battleresult = -1;
            }
            else
            {
                Battleresult = 0;
            }
        }
        private static void AfterBattle(Hand EnemyHand, Hand PlayersHand, Card[] EnemyCards, Card[] PlayersCards, Field field1)
        {
            Console.Clear();
            Console.WriteLine($"Score: {MyScore}:{EnemyScore}");
            Console.WriteLine();
            EnemyHand.DrawHand(EnemyCards, false);
            for (int i = 0; i < 8; i++)
                Console.WriteLine();
            Console.Write("        ");
            if (PlayersCards[0] == null && PlayersCards[1] == null && PlayersCards[2] == null && PlayersCards[3] == null && PlayersCards[4] == null && EnemyCards[0] == null && EnemyCards[1] == null && EnemyCards[2] == null && EnemyCards[3] == null && EnemyCards[4] == null)
                Console.Write(BattlePhrases[3]);
            else if (Battleresult == 0)
                Console.Write(BattlePhrases[0]);
            else if (Battleresult == 1)
                Console.Write(BattlePhrases[1]);
            else if (Battleresult == -1)
            {
                Console.Write(BattlePhrases[2]);
            }
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine();
            }
            PlayersHand.DrawHand(PlayersCards, true);
        }
        private static void ClearField(Hand EnemyHand, Hand PlayersHand, Card[] EnemyCards, Card[] PlayersCards, Field field1)
        {
            Console.Clear();
            Console.WriteLine($"Score: {MyScore}:{EnemyScore}");
            Console.WriteLine();
            EnemyHand.DrawHand(EnemyCards, false);
            field1.DrawField();
            PlayersHand.DrawHand(PlayersCards, true);
        }
        private static void WriteResult()
        {
            Console.Clear();
            if (MyScore > EnemyScore)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Ви виграли! Туда цього бота!");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Рахунок: {MyScore}:{EnemyScore}");
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\МУШ\zvuk-pobedyi-vyiigryisha.wav");
                player.Play();
            }
            else if (MyScore < EnemyScore)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Ви програли! І хто тепер бот???");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Рахунок: {MyScore}:{EnemyScore}");
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\МУШ\zvuk-proigryisha.wav");
                player.Play();
            }
            else
            {
                Console.WriteLine("Нічия! Потна катка!");
                Console.WriteLine($"Рахунок: {MyScore}:{EnemyScore}");
            }
        }
        public class Card
        {
            public Card(string name, int damage, int hP, int attackSpeed)
            {
                Name = name;
                Damage = damage;
                HP = hP;
                AttackSpeed = attackSpeed;
            }

            public string Name { get; private set; }
            public int Damage { get; private set; }
            public int HP { get; private set; }
            public int AttackSpeed { get; private set; }

            public void GetDamage(int value)
            {
                HP -= value;
            }
        }
        public class Field
        {
            public Field(int height)
            {
                Height = height;
            }
            public int Height;
            public Card card1 { get; set; }
            public Card card2 { get; set; }
            public void DrawField()
            {
                for (int i = 0; i < Height; i++)
                {
                    Console.WriteLine();
                }
            }
        }
        public class Hand
        {
            public Hand(int height, int width, Card[] cards)
            {
                Width = width;
                Height = height;
            }
            public int Width;
            public int Height;
            public bool ShowStats;
            public char[] CardLetters = { 'A', 'B', 'C', 'D', 'E' };
            public void DrawHand(Card[] cards, bool ShowStats)
            {
                if (ShowStats)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write("----------");
                    }
                    Console.Write('-');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write($"       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write($"{cards[i].Name}       ");
                            Console.Write(i + 1);
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("         ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("         ");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("Dmg:   ");
                            Console.Write($"0{cards[i].Damage}");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write($"  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("HP:    ");
                            Console.Write($"0{cards[i].HP}");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("Spd:   ");
                            Console.Write($"0{cards[i].AttackSpeed}");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write("----------");
                    }
                    Console.Write('-');
                    Console.WriteLine();
                }
                else
                {
                    // first line
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write("----------");
                    }
                    Console.Write('-');
                    Console.WriteLine();
                    // second line
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("         ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("?????????");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    // third line
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write('|');
                        Console.Write("         ");
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    // fourth line
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("Dmg:   ");
                            Console.Write("??");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    // fifth line
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("HP:    ");
                            Console.Write("??");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    for (int i = 0; i < 5; i++)
                    {
                        if (cards[i] == null)
                        {
                            Console.Write('|');
                            Console.Write("       ");
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write('|');
                            Console.Write("Spd:   ");
                            Console.Write("??");
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine();
                    // sixth line
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write("----------");
                    }
                    Console.Write('-');
                }
            }
        }
    }
}
