using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_5
{
    public partial class Form1 : Form
    {
        private const int BUTTON_SIZE = 35;
        private Button[,] board;
        private int nMines;
        private int nUncovered;
        private int minutes;
        private int seconds;

        public Form1()
        {
            InitializeComponent();
        }

        private void reveal(int row, int col)
        {
            if (row < 0 || row >= 10 || col < 0 || col >= 10)  
                return;
            if (board[row, col].Tag.Equals("0"))  
            {
                board[row, col].Text = "0";
                board[row, col].Tag = "x"; 
                board[row, col].Enabled = false; 
                nUncovered++;
                reveal(row - 1, col - 1);  
                reveal(row - 1, col);
                reveal(row - 1, col + 1);
                reveal(row, col - 1);
                reveal(row, col + 1);
                reveal(row + 1, col - 1);
                reveal(row + 1, col);
                reveal(row + 1, col + 1);
            }
            else if (!board[row, col].Tag.Equals("x")) 
            {
                board[row, col].Text = board[row, col].Tag.ToString();
                board[row, col].Tag = "x"; 
                board[row, col].Enabled = false; 
                nUncovered++;
            }
        }
        protected void buttonClick(object sender, EventArgs e)
        {
            Button current = (Button)sender;
            int curRow = (current.Location.Y - 10) / BUTTON_SIZE;
            int curCol = (current.Location.X - 5) / BUTTON_SIZE;
            if (current.Tag.Equals("*"))
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        if (board[i, j].Enabled)
                        {
                            board[i, j].Text = board[i, j].Tag.ToString(); 
                            board[i, j].Enabled = false;
                        }
                timer1.Enabled = false; 
                MessageBox.Show("You landed on a Mine!");
            }
            else
            {
                if (current.Tag.Equals("0")) 
                    reveal(curRow, curCol);  
                else
                {
                    current.Text = current.Tag.ToString();
                    current.Tag = "x"; 
                    current.Enabled = false;
                    nUncovered++;
                }
                if (nUncovered + nMines == 100)
                {
                    timer1.Enabled = false; 
                    MessageBox.Show("You WIN!");
                    for (int i = 0; i < 10; i++)
                        for (int j = 0; j < 10; j++)
                            board[i, j].Enabled = false; 
                }
            }
        }

        private void startGame()
        {
            Random rnd = new Random();
            nMines = 0;
            nUncovered = 0;
           
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    int value = rnd.Next(10);
                    if (value == 0)
                    {
                        board[i, j].Tag = "*";
                        nMines++;
                    }
                    else
                        board[i, j].Tag = " ";
                    board[i, j].Text = "";
                }
           
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    if (!board[i, j].Tag.Equals("*"))
                    {
                        int mines = 0;
                        if (i > 0)
                        {
                            if (j > 0 && board[i - 1, j - 1].Tag.Equals("*"))
                                mines++;
                            if (board[i - 1, j].Tag.Equals("*"))
                                mines++;
                            if (j < 9 && board[i - 1, j + 1].Tag.Equals("*"))
                                mines++;
                        }
                        if (j > 0 && board[i, j - 1].Tag.Equals("*"))
                            mines++;
                        if (board[i, j].Tag.Equals("*"))
                            mines++;
                        if (j < 9 && board[i, j + 1].Tag.Equals("*"))
                            mines++;
                        if (i < 9)
                        {
                            if (j > 0 && board[i + 1, j - 1].Tag.Equals("*"))
                                mines++;
                            if (board[i + 1, j].Tag.Equals("*"))
                                mines++;
                            if (j < 9 && board[i + 1, j + 1].Tag.Equals("*"))
                                mines++;
                        }
                        board[i, j].Tag = mines.ToString();
                    }
                }
           
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    board[i, j].Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            board = new Button[10, 10];
            Random rnd = new Random();
            nMines = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = new Button();     
                    board[i, j].Size = new Size(BUTTON_SIZE, BUTTON_SIZE);   
                    board[i, j].Location = new Point(j * BUTTON_SIZE + 10, i * BUTTON_SIZE + 20);    
                    board[i, j].Click += new EventHandler(buttonClick); 
                    board[i, j].Enabled = false;
                    groupBox1.Controls.Add(board[i, j]);
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;  
            if (seconds >= 60) 
            {
                minutes++;
                seconds = 0;
            }
            Timer.Text = String.Format("{0:D2}:{1:D2}", minutes, seconds);
            if (minutes == 5) 
            {
                timer1.Enabled = false;  
                MessageBox.Show("Time is up!");
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        board[i, j].Enabled = false; 
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            startGame();   
            Timer.Text = "00:00";
            minutes = 0;
            seconds = 0;
            timer1.Enabled = true; 
        }

        private void Timer_Click(object sender, EventArgs e)
        {

        }
    }
}
