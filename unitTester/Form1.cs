using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
namespace unitTester
{
    public partial class Form1 : Form
    {
        static Random rand = new Random();
        private static List<Card> talon;
        private Player player;
        private Player mi;
        private static Card adu;
        private string winner;
        private Card playerDroped;
        private Card miDroped;
        private int first5Counter;
        Image hatlap;
        Boolean playerSaidKing = false;
        Boolean miSaidKing = false;
        Boolean waitForMiCall = false;
        

        public Form1()
        {
            InitializeComponent();
            talon = updateTalon();
            player = new Player("player", dealHand());
            mi = new Player("mi", dealHand());
            adu = talon[rand.Next(talon.Count)];
            talon.Remove(adu);
            winner = "player";
            first5Counter = 0;
            hatlap = new Bitmap(@"C:\Users\me\source\repos\testelek\kepek\kartyaHatlap.png");
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            mi0_bt.BackgroundImage = hatlap;
            mi1_bt.BackgroundImage = hatlap;
            mi2_bt.BackgroundImage = hatlap;
            mi3_bt.BackgroundImage = hatlap;
            mi4_bt.BackgroundImage = hatlap;

            mi0_bt.Enabled = false;
            mi1_bt.Enabled = false;
            mi2_bt.Enabled = false;
            mi3_bt.Enabled = false;
            mi4_bt.Enabled = false;


            player0_bt.BackgroundImage = player.Hand[0].CardImage;
            player1_bt.BackgroundImage = player.Hand[1].CardImage;
            player2_bt.BackgroundImage = player.Hand[2].CardImage;
            player3_bt.BackgroundImage = player.Hand[3].CardImage;
            player4_bt.BackgroundImage = player.Hand[4].CardImage;

            player0_bt.Enabled = false;
            player1_bt.Enabled = false;
            player2_bt.Enabled = false;
            player3_bt.Enabled = false;
            player4_bt.Enabled = false;

            adu_bt.BackgroundImage = adu.CardImage;
            adu_bt.Enabled = false;
            caller_bt.Enabled = false;
            responder_bt.Enabled = false;
            
        }

        public void miCallACard()
        {
            if (sayKing(mi.Hand) != null && !miSaidKing)
            {
                miDroped = sayKing(mi.Hand);
                if (miDroped.Color == adu.Color)
                {
                    mi.Point += 40;
                    string message = "+40";
                    string caption = "MI Said!";

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {                    
                    mi.Point += 20;
                    string message = "+20";
                    string caption = "MI Said!";

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }
                
                
                miSaidKing = true;
                responder_bt.BackgroundImage = default;
            }
            else
            {
                Card min = mi.getFirstNotNullCard();
                for (int i = 0; i < mi.Hand.Length; i++)
                {
                    if (mi.Hand[i] != null)
                    {
                        if (min.CardValue > mi.Hand[i].CardValue && mi.Hand[i].Color != adu.Color)
                        {
                            min = mi.Hand[i];
                        }
                    }
                }
                miDroped = min;
                responder_bt.BackgroundImage = default;
            }           
        }
        public void playerCallACard()
        {
            if (sayKing(player.Hand) != null && !playerSaidKing)
            {
                playerDroped = sayKing(player.Hand);
                if (playerDroped.Color == adu.Color)
                {
                    player.Point += 40;                    
                    string message = "+40";
                    string caption = "Player Said!";

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {
                    player.Point += 20;
                    string message = "+20";
                    string caption = "Player Said!";

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                }

                playerSaidKing = true;
                responder_bt.BackgroundImage = default;
               
            }
            else
            {
                Card min = player.getFirstNotNullCard();
                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] != null)
                    {
                        if (min.CardValue < player.Hand[i].CardValue && player.Hand[i].Color != adu.Color)
                        {
                            min = player.Hand[i];
                        }
                    }
                }
                playerDroped = min;
            }
            
        }
        public Card sayKing(Card[] hand)
        {
            Boolean kiraly = false;
            Boolean felso = false;
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null)
                {
                    if (hand[i].Name == "zoldKiraly")
                        kiraly = true;
                    if (hand[i].Name == "zoldFelso")
                        felso = true;
                }
            }
            if(kiraly && felso)
                return new Card("zoldKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldKiraly.png"), "zold", 4);

            kiraly= felso= false;
            for (int i = 0; i < hand.Length; i++)
            {
                if(hand[i] != null)
                {
                    if (hand[i].Name == "tokKiraly")
                        kiraly = true;
                    if (hand[i].Name == "tokFelso")
                        felso = true;
                }                
            }
            if (kiraly && felso)
                return new Card("tokKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokKiraly.png"), "tok", 4);
            kiraly = felso = false;
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null)
                {
                    if (hand[i].Name == "pirosKiraly")
                        kiraly = true;
                    if (hand[i].Name == "pirosFelso")
                        felso = true;
                }                
            }
            if (kiraly && felso)
                return new Card("pirosKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosKiraly.png"), "piros", 4);
            kiraly = felso = false;
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i] != null)
                {
                    if (hand[i].Name == "makkKiraly")
                        kiraly = true;
                    if (hand[i].Name == "makkFelso")
                        felso = true;
                }
            }
            if (kiraly && felso)
                return new Card("makkKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkKiraly.png"), "makk", 4);

            return null;
        }
        public int checkScore(Card card, Card enemyDroped)
        {
            if (card.Color == enemyDroped.Color)
            {
                if (card.CardValue > enemyDroped.CardValue)
                {
                    return card.CardValue + enemyDroped.CardValue;
                }
                else
                {
                    return (card.CardValue + enemyDroped.CardValue) * (-1);
                }
            }
            else
            {
                if (card.Color == adu.Color)
                {
                    return card.CardValue + enemyDroped.CardValue;
                }
                else
                {
                    return (card.CardValue + enemyDroped.CardValue) * (-1);
                }
            }
        }
        public void miRespondToCall()
        {
            int max = 0;
            Card cardToDrop = mi.getFirstNotNullCard();
            for (int i = 0; i < mi.Hand.Length; i++)
            {
                if (mi.Hand[i] != null)
                {
                    if (checkScore(mi.Hand[i], playerDroped) > max)
                    {
                        max = checkScore(mi.Hand[i], playerDroped);
                        cardToDrop = mi.Hand[i];
                    }
                }
            }
            miDroped = cardToDrop;
        }
        public void playerRespondToCall()
        {
            int max = 0;
            Card cardToDrop = player.getFirstNotNullCard();
            for (int i = 0; i < player.Hand.Length; i++)
            {
                if (player.Hand[i] != null)
                {
                    if (checkScore(player.Hand[i], miDroped) > max)
                    {
                        max = checkScore(player.Hand[i], miDroped);
                        cardToDrop = player.Hand[i];
                    }
                }
            }
            playerDroped = cardToDrop;
        }
        public void checkWinner()
        {
            if (playerDroped.Color == miDroped.Color)
            {
                if (playerDroped.CardValue > miDroped.CardValue)
                {
                    winner = "player";
                    player.Point += playerDroped.CardValue + miDroped.CardValue;
                }
                else
                {
                    winner = "mi";
                    mi.Point += playerDroped.CardValue + miDroped.CardValue;
                }
            }else if (winner == "player")
            {
                if (isAdu(miDroped))
                {
                    winner = "mi";
                    mi.Point += playerDroped.CardValue + miDroped.CardValue;
                }
                else
                {
                    winner = "player";
                    player.Point += playerDroped.CardValue + miDroped.CardValue;
                }
            }else if(winner == "mi")
            {
                if (isAdu(playerDroped))
                {
                    winner = "player";
                    player.Point += playerDroped.CardValue + miDroped.CardValue;
                }
                else
                {
                    winner = "mi";
                    mi.Point += playerDroped.CardValue + miDroped.CardValue;
                }
            }

            if (first5Counter >= 5)
            {
                if(winner == "mi")
                {
                    next_bt.Enabled = true;
                    waitForMiCall = true;
                    player0_bt.Enabled = false;
                    player1_bt.Enabled = false;
                    player2_bt.Enabled = false;
                    player3_bt.Enabled = false;
                    player4_bt.Enabled = false;

                }
                else
                {
                    for (int i = 0; i < player.Hand.Length; i++)
                    {
                        if (player.Hand[i] != null)
                        {
                            enablePlayerButtons(i);
                        }
                    }
                }
                
            }
            miPoint_lb.Text = mi.Point.ToString();
            playerPoint_lb.Text = player.Point.ToString();

            if (player.Point >= 66)
            {
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=snapszer;";
                string query = "INSERT INTO eredmenyek(`id`, `player1`, `player1_point`, `player2`, `player2_point`) VALUES (NULL, '" + "Player" + "', '" + player.Point.ToString() + "', '" + "Mi" + "', '" + mi.Point.ToString() + "')";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();                  

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string message = "The Player was reached the minimum 66 point!\nWould you like to play a new one?";
                string caption = "We have a winner!";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    this.Close();
                }
            }
            if (mi.Point >= 66)
            {
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=snapszer;";
                string query = "INSERT INTO eredmenyek(`id`, `player1`, `player1_point`, `player2`, `player2_point`) VALUES (NULL, '" + "Player" + "', '" + player.Point.ToString() + "', '" + "Mi" + "', '" + mi.Point.ToString() + "')";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string message = "The Mi was reached the minimum 66 point!\nWould you like to play a new one?";
                string caption = "We have a winner!";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    this.Close();
                }
            }
            if (!emptyHand())
            {

                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=snapszer;";
                string query = "INSERT INTO eredmenyek(`id`, `player1`, `player1_point`, `player2`, `player2_point`) VALUES (NULL, '" + "Player" + "', '" + player.Point.ToString() + "', '" + "Mi" + "', '" + mi.Point.ToString() + "')";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    databaseConnection.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string message = "DRAW!\nWould you like to play a new one?";
                string caption = "DRAW!";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    this.Close();
                }
            }

        }
        public Boolean emptyHand()
        {
            Boolean hasACard = false;
            for (int i = 0; i < player.Hand.Length; i++)
            {
                if (player.Hand[i] != null)
                {
                    hasACard = true;
                }
            }
            return hasACard;
        }
        public void firstRound()
        {
            if (winner == "mi")
            {                
                miCallACard();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                caller_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] != null)
                    {
                        enablePlayerButtons(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] != null)
                    {
                        enablePlayerButtons(i);
                    }
                }
            }

            
        }
        public void enablePlayerButtons(int index)
        {
            switch (index)
            {
                case 0:
                    player0_bt.Enabled = true;
                    break;
                case 1:
                    player1_bt.Enabled = true;
                    break;
                case 2:
                    player2_bt.Enabled = true;
                    break;
                case 3:
                    player3_bt.Enabled = true;
                    break;
                case 4:
                    player4_bt.Enabled = true;
                    break;
            }
        }
        public void dropCardInVisualPlayerHand(int index) 
        {
            switch (index)
            {
                case 0:
                    player0_bt.BackgroundImage = default;
                    break;
                case 1:
                    player1_bt.BackgroundImage = default;
                    break;
                case 2:
                    player2_bt.BackgroundImage = default;
                    break;
                case 3:
                    player3_bt.BackgroundImage = default;
                    break;
                case 4:
                    player4_bt.BackgroundImage = default;
                    break;
            }
        }
        public void dropCardInVisualMiHand(int index)
        {
            switch (index)
            {
                case 0:
                    mi0_bt.BackgroundImage = default;
                    break;
                case 1:
                    mi1_bt.BackgroundImage = default;
                    break;
                case 2:
                    mi2_bt.BackgroundImage = default;
                    break;
                case 3:
                    mi3_bt.BackgroundImage = default;
                    break;
                case 4:
                    mi4_bt.BackgroundImage = default;
                    break;
            }
        }
        public void drawCardToVisualPlayerHand(int index)
        {
            switch (index)
            {
                case 0:
                    player0_bt.BackgroundImage = player.Hand[0].CardImage;
                    break;
                case 1:
                    player1_bt.BackgroundImage = player.Hand[1].CardImage;
                    break;
                case 2:
                    player2_bt.BackgroundImage = player.Hand[2].CardImage;
                    break;
                case 3:
                    player3_bt.BackgroundImage = player.Hand[3].CardImage;
                    break;
                case 4:
                    player4_bt.BackgroundImage = player.Hand[4].CardImage;
                    break;
            }
        }
        public void drawCardToVisualMiHand(int index)
        {
            switch (index)
            {
                case 0:
                    mi0_bt.BackgroundImage = hatlap;
                    break;
                case 1:
                    mi1_bt.BackgroundImage = hatlap;
                    break;
                case 2:
                    mi2_bt.BackgroundImage = hatlap;
                    break;
                case 3:
                    mi3_bt.BackgroundImage = hatlap;
                    break;
                case 4:
                    mi4_bt.BackgroundImage = hatlap;
                    break;
            }
        }
        public Boolean isAdu(Card card)
        {
            if (card.Color == adu.Color)
                return true;
            else
                return false;
        }

        public class Card
        {
            private string name;
            private Image cardImage;
            private string color;
            private int cardValue;

            public Card(string newName, Image newCardImage, string newColor, int newCardValue)
            {
                this.name = newName;
                this.cardImage = newCardImage;
                this.color = newColor;
                this.cardValue = newCardValue;

            }
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public Image CardImage
            {
                get { return cardImage; }
                set { cardImage = value; }
            }
            public string Color
            {
                get { return color; }
                set { color = value; }
            }
            public int CardValue
            {
                get { return cardValue; }
                set { cardValue = value; }
            }
        }

        public static List<Card> updateTalon()
        {
            List<Card> talon = new List<Card>();
            talon.Add(new Card("pirosAlso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosAlso.png"), "piros", 2));
            talon.Add(new Card("pirosFelso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosFelso.png"), "piros", 3));
            talon.Add(new Card("pirosKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosKiraly.png"), "piros", 4));
            talon.Add(new Card("pirosTizes", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosTizes.png"), "piros", 10));
            talon.Add(new Card("pirosAsz", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\pirosAsz.png"), "piros", 11));
            talon.Add(new Card("tokAlso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokAlso.png"), "tok", 2));
            talon.Add(new Card("tokFelso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokFelso.png"), "tok", 3));
            talon.Add(new Card("tokKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokKiraly.png"), "tok", 4));
            talon.Add(new Card("tokTizes", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokTizes.png"), "tok", 10));
            talon.Add(new Card("tokAsz", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\tokAsz.png"), "tok", 11));
            talon.Add(new Card("makkAlso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkAlso.png"), "makk", 2));
            talon.Add(new Card("makkFelso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkFelso.png"), "makk", 3));
            talon.Add(new Card("makkKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkKiraly.png"), "makk", 4));
            talon.Add(new Card("makkTizes", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkTizes.png"), "makk", 10));
            talon.Add(new Card("makkAsz", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\makkAsz.png"), "makk", 11));
            talon.Add(new Card("zoldAlso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldAlso.png"), "zold", 2));
            talon.Add(new Card("zoldFelso", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldFelso.png"), "zold", 3));
            talon.Add(new Card("zoldKiraly", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldKiraly.png"), "zold", 4));
            talon.Add(new Card("zoldTizes", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldTizes.png"), "zold", 10));
            talon.Add(new Card("zoldAsz", Image.FromFile(@"C:\Users\me\source\repos\testelek\kepek\zoldAsz.png"), "zold", 11));
            return talon;
        }

        public static Card[] dealHand()
        {
            Card[] hand = new Card[5];
            for (int i = 0; i < hand.Length; i++)
            {
                Card randomCard = talon[rand.Next(talon.Count)];
                talon.Remove(randomCard);
                hand[i] = randomCard;
            }
            return hand;
        }

        public void drawCardForPlayer()
        {
            if (talon.Count > 0)
            {
                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] == null)
                    {
                        player.Hand[i] = talon[rand.Next(talon.Count)];
                        talon.Remove(player.Hand[i]);
                        drawCardToVisualPlayerHand(i);                        
                    }
                }
            }
            else
            {
                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] == null)
                    {
                        player.Hand[i] = adu;
                        adu_bt.BackgroundImage = default;
                        drawCardToVisualPlayerHand(i);
                    }
                }
            }
        }
        public void drawCardForMi()
        {
            if (talon.Count > 0)
            {
                for (int i = 0; i < mi.Hand.Length; i++)
                {
                    if (mi.Hand[i] == null)
                    {
                        mi.Hand[i] = talon[rand.Next(talon.Count)];
                        talon.Remove(mi.Hand[i]);
                        drawCardToVisualMiHand(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < mi.Hand.Length; i++)
                {
                    if (mi.Hand[i] == null)
                    {
                        mi.Hand[i] = adu;
                        adu_bt.BackgroundImage = default;
                        drawCardToVisualMiHand(i);
                    }
                }
            }
        }

        public class Player
        {
            private string name;
            private  Card[] hand;
            private  int point;

            public Player(string newName, Card[] newHand)
            {
                name = newName;
                hand = newHand;
                point = 0;
            }
            public string Name
            {
                get { return name; }
            }
            public Card[] Hand
            {
                get { return hand; }
                set { hand = value; }
            }
            public int Point
            {
                get { return point; }
                set { point = value; }
            }
            public int getCardIndex(Card card)
            {
                for (int i = 0; i < this.hand.Length; i++)
                {
                    if (this.hand[i] != null)
                    {
                        if (card.Name == this.hand[i].Name)
                        {
                            return i;
                        }
                    }
                }
                return 0;
            }
           
            public Card getFirstNotNullCard()
            {
                for (int i = 0; i < this.hand.Length; i++)
                {
                    if (this.hand[i] != null && this.Hand[i].Color != adu.Color)
                    {
                        return this.hand[i];
                    }
                }
                return this.hand[0];
            }
        }

        private void next_bt_Click(object sender, EventArgs e)
        {

            if (!waitForMiCall)
            {
                if (first5Counter < 5)
                {
                    if (first5Counter > 0)
                    {
                        drawCardForPlayer();
                        drawCardForMi();
                    }
                    if (winner == "player")
                    {
                        playerCallACard();
                        miRespondToCall();
                        Console.WriteLine("\nPlayerDropedIndex: " + player.getCardIndex(playerDroped) + "\n");
                        Console.WriteLine("\nMiDropedIndex: " + mi.getCardIndex(miDroped) + "\n");
                        dropCardInVisualPlayerHand(player.getCardIndex(playerDroped));
                        dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                        player.Hand[player.getCardIndex(playerDroped)] = null;
                        mi.Hand[mi.getCardIndex(miDroped)] = null;
                        caller_bt.BackgroundImage = playerDroped.CardImage;
                        responder_bt.BackgroundImage = miDroped.CardImage;
                        Console.WriteLine("\nPlayer called: " + playerDroped.Name);
                        Console.WriteLine("\nMi respond: " + miDroped.Name + "\n");
                    }
                    else
                    {
                        miCallACard();
                        playerRespondToCall();
                        Console.WriteLine("\nPlayerDropedIndex: " + player.getCardIndex(playerDroped) + "\n");
                        Console.WriteLine("\nMiDropedIndex: " + mi.getCardIndex(miDroped) + "\n");
                        dropCardInVisualPlayerHand(player.getCardIndex(playerDroped));
                        dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                        player.Hand[player.getCardIndex(playerDroped)] = null;
                        mi.Hand[mi.getCardIndex(miDroped)] = null;
                        caller_bt.BackgroundImage = miDroped.CardImage;
                        responder_bt.BackgroundImage = playerDroped.CardImage;
                        Console.WriteLine("\nMi called: " + miDroped.Name);
                        Console.WriteLine("\nPlayer respond: " + playerDroped.Name + "\n");
                    }
                    checkWinner();
                    Console.WriteLine("Player Score: " + player.Point + "\n");
                    Console.WriteLine("Mi Score: " + mi.Point + "\n");
                    first5Counter++;
                    Console.WriteLine(first5Counter);
                }
                else
                {
                    drawCardForPlayer();
                    drawCardForMi();
                    caller_bt.BackgroundImage = default;
                    responder_bt.BackgroundImage = default;
                    next_bt.Enabled = false;
                    firstRound();
                    Console.WriteLine(first5Counter);
                    for (int i = 0; i < mi.Hand.Length; i++)
                    {
                        if (mi.Hand[i] != null)
                        {
                            Console.WriteLine(mi.Hand[i].Name);
                        }
                    }
                }
            }

            if (waitForMiCall)
            {
                miCallACard();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                caller_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;

                for (int i = 0; i < player.Hand.Length; i++)
                {
                    if (player.Hand[i] != null)
                    {
                        enablePlayerButtons(i);
                    }
                }
                waitForMiCall = false;
                next_bt.Enabled = false;
            }
            

        }

        public void playerSaidKingOnButtonClick()
        {
            if (sayKing(player.Hand) != null && !playerSaidKing)
            {
                if (playerDroped.Color == sayKing(player.Hand).Color && (playerDroped.Name == (sayKing(player.Hand).Color + "Kiraly") || playerDroped.Name == (sayKing(player.Hand).Color + "Felso")))
                {
                    if (playerDroped.Color == adu.Color)
                    {
                        player.Point += 40;
                        string message = "+40";
                        string caption = "Player Said!";

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result;

                        // Displays the MessageBox.
                        result = MessageBox.Show(message, caption, buttons);
                    }
                    else
                    {
                        player.Point += 20;
                        string message = "+20";
                        string caption = "Player Said!";

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result;

                        // Displays the MessageBox.
                        result = MessageBox.Show(message, caption, buttons);
                    }

                    playerSaidKing = true;
                }
            }
        }

        private void player0_bt_Click(object sender, EventArgs e)
        {
            playerDroped = player.Hand[0];
            player.Hand[0] = null;
            player0_bt.BackgroundImage = default;
            player0_bt.Enabled = false;
            
            
            if (winner == "player")
            {
                playerSaidKingOnButtonClick();
                caller_bt.BackgroundImage = playerDroped.CardImage;
                miRespondToCall();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                responder_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                checkWinner();
            }
            else
            {
                responder_bt.BackgroundImage = playerDroped.CardImage;
                checkWinner();
            }
        }

        private void player1_bt_Click(object sender, EventArgs e)
        {
            playerDroped = player.Hand[1];
            player.Hand[1] = null;
            player1_bt.BackgroundImage = default;
            player1_bt.Enabled = false;


            if (winner == "player")
            {
                playerSaidKingOnButtonClick();
                caller_bt.BackgroundImage = playerDroped.CardImage;
                miRespondToCall();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                responder_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                checkWinner();
            }
            else
            {
                responder_bt.BackgroundImage = playerDroped.CardImage;
                checkWinner();
            }
        }

        private void player2_bt_Click(object sender, EventArgs e)
        {
            playerDroped = player.Hand[2];
            player.Hand[2] = null;
            player2_bt.BackgroundImage = default;
            player2_bt.Enabled = false;


            if (winner == "player")
            {
                playerSaidKingOnButtonClick();
                caller_bt.BackgroundImage = playerDroped.CardImage;
                miRespondToCall();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                responder_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                checkWinner();
            }
            else
            {
                responder_bt.BackgroundImage = playerDroped.CardImage;
                checkWinner();
            }
        }

        private void player3_bt_Click(object sender, EventArgs e)
        {
            playerDroped = player.Hand[3];
            player.Hand[3] = null;
            player3_bt.BackgroundImage = default;
            player3_bt.Enabled = false;


            if (winner == "player")
            {
                playerSaidKingOnButtonClick();
                caller_bt.BackgroundImage = playerDroped.CardImage;
                miRespondToCall();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                responder_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                checkWinner();
            }
            else
            {
                responder_bt.BackgroundImage = playerDroped.CardImage;
                checkWinner();
            }
        }

        private void player4_bt_Click(object sender, EventArgs e)
        {
            playerDroped = player.Hand[4];
            player.Hand[4] = null;
            player4_bt.BackgroundImage = default;
            player4_bt.Enabled = false;


            if (winner == "player")
            {
                playerSaidKingOnButtonClick();
                caller_bt.BackgroundImage = playerDroped.CardImage;
                miRespondToCall();
                dropCardInVisualMiHand(mi.getCardIndex(miDroped));
                responder_bt.BackgroundImage = miDroped.CardImage;
                mi.Hand[mi.getCardIndex(miDroped)] = null;
                checkWinner();
            }
            else
            {
                responder_bt.BackgroundImage = playerDroped.CardImage;
                checkWinner();
            }
        }
        public void newGame()
        {

            talon = updateTalon();
            player = new Player("player", dealHand());
            mi = new Player("mi", dealHand());
            adu = talon[rand.Next(talon.Count)];
            talon.Remove(adu);
            winner = "player";
            first5Counter = 0;
            waitForMiCall = false;
            playerSaidKing = false;
            miSaidKing = false;
            mi0_bt.BackgroundImage = hatlap;
            mi1_bt.BackgroundImage = hatlap;
            mi2_bt.BackgroundImage = hatlap;
            mi3_bt.BackgroundImage = hatlap;
            mi4_bt.BackgroundImage = hatlap;

            mi0_bt.Enabled = false;
            mi1_bt.Enabled = false;
            mi2_bt.Enabled = false;
            mi3_bt.Enabled = false;
            mi4_bt.Enabled = false;


            player0_bt.BackgroundImage = player.Hand[0].CardImage;
            player1_bt.BackgroundImage = player.Hand[1].CardImage;
            player2_bt.BackgroundImage = player.Hand[2].CardImage;
            player3_bt.BackgroundImage = player.Hand[3].CardImage;
            player4_bt.BackgroundImage = player.Hand[4].CardImage;

            player0_bt.Enabled = false;
            player1_bt.Enabled = false;
            player2_bt.Enabled = false;
            player3_bt.Enabled = false;
            player4_bt.Enabled = false;

            adu_bt.BackgroundImage = adu.CardImage;
            adu_bt.Enabled = false;
            caller_bt.Enabled = false;
            responder_bt.Enabled = false;
            caller_bt.BackgroundImage = default;
            responder_bt.BackgroundImage = default;
            playerPoint_lb.Text = player.Point.ToString();
            miPoint_lb.Text = mi.Point.ToString();
            next_bt.Enabled = true;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=snapszer;";
            string query = "SELECT * FROM eredmenyek";

            // Prepare the connection
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // As our database, the array will contain : ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
                        // Do something with every received database ROW
                        //string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)};
                        Console.WriteLine(reader.GetString(0)+" "+reader.GetString(1)+" "+reader.GetString(2)+" "+reader.GetString(3)+" "+reader.GetString(4));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                // Finally close the connection
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
