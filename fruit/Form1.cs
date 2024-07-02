using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fruit
{
    public partial class Form1 : Form
    {
        int speed1 = 15,speed2 = 15;                //跳的高度
        int uptime1 = 0, uptime2 = 0;               //控制跳的次數
        int score = 0;                              //總分數
        int pear = 0, strawberry = 0;               //西洋梨跟草莓的分數
        bool goup1 = false, goup2 = false;          //向上
        bool goleft1 = false, goleft2 = false;      //向左
        bool goright1 = false, goright2 = false;    //向右

        

        bool gg1 = false, gg2 = false;              //是否在平台上

        public Form1()
        {
            InitializeComponent();
            this.SetBounds(0, 0, 1000, 600);        //設定邊界 0, 0, 1000, 600
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //player位置初始設定
            player2.Left = 20; 
            player2.Top = this.Height - player2.Height - 50;
            player1.Left = this.Width - player1.Width - 30;
            player1.Top = this.Height - player1.Height - 55;

            timer1.Start();
        }
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { Application.Exit(); }       //ESC 離開程式
            switch (e.KeyValue)
            {
                case 37:
                    if (goleft1 == false)
                        goleft1= true;
                    break;
                case 39:
                    if (goright1 == false)
                        goright1 = true;
                    break;
                case 38:
                    if (goup1 == false)
                        goup1 = true;
                    break;
                case 'A':
                    if (goleft2 == false)
                        goleft2 = true;             // 按鍵"A"按下此標誌為true，直到放開
                    break;
                case 'D':
                    if (goright2 == false)
                        goright2 = true;            // 按鍵"D"按下此標誌為true，直到放開
                    break;
                case 'W':
                    if (goup2 == false)
                        goup2 = true;               // 按鍵"W"按下此標誌為true，直到放開
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 37:
                    goleft1 = false;
                    break;
                case 39:
                    goright1 = false;
                    break;
                case 38:
                    goup1 = false;
                    break;
                case 'A':
                    goleft2 = false;                // 按鍵"A"放開此標誌為false
                    break;
                case 'D':
                    goright2 = false;               // 按鍵"D"放開此標誌為false
                    break;
                case 'W':
                    goup2 = false;
                    break;
            }
            gg1 = false;
            gg2 = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //player設定上左右不超出視窗
            if (player1.Left<0) player1.Left = 0;
            if (player1.Left+player1.Width>1000) player1.Left = 1000-player1.Width;
            if (player1.Top<0) player1.Top = 0;
            if (player2.Left<0) player2.Left = 0;
            if (player2.Left+player2.Width>1000) player2.Left = 1000-player2.Width;
            if (player2.Top<0) player2.Top = 0;

            //if (goleft1 && player1.Left <= wall.Left+wall.Width && player1.Bounds.IntersectsWith(wall.Bounds)) player1.Left = wall.Left+wall.Width;
            //if (goright1 && wall.Left <= player1.Left+player1.Width && player1.Bounds.IntersectsWith(wall.Bounds)) player1.Left = wall.Left-player1.Width;

            if (player1.Bounds.IntersectsWith(label1.Bounds)) player1.Left = label1.Left;                   //player1撞到牆的右邊
            if (player1.Bounds.IntersectsWith(label2.Bounds)) player1.Left = label2.Left-player1.Width;     //player1撞到牆的左邊
            if (player2.Bounds.IntersectsWith(label1.Bounds)) player2.Left = label1.Left;                   //player2撞到牆的右邊
            if (player2.Bounds.IntersectsWith(label2.Bounds)) player2.Left = label2.Left-player2.Width;     //player2撞到牆的左邊
            player1.Top += speed1;
            player2.Top += speed2;
            
            if (uptime1 < 0 && goup1) { goup1 = false; }            //player已經跳起來一次了
            if (uptime2 < 0 && goup2) { goup2 = false; }
            if (goleft1){ player1.Left -= 5; }                      //player向左
            if (goleft2){ player2.Left -= 5; }
            if (goright1) { player1.Left += 5; }                    //player向右
            if (goright2) { player2.Left += 5; }

            if (goup1)                                  //player跳
            {
                speed1 = -15;
                uptime1 -= 1;
            }
            else
            {
                speed1 = 15;                            //不跳時往下掉
                if (gg1) speed1 = 0;                    //如果在平台上
            }
            if (goup2)
            {
                speed2 = -15;
                uptime2 -= 1;
            }
            else
            {
                speed2 = 15;
                if (gg2) speed2 = 0;
            }


            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "board")
                {
                    //若player碰到board、非跳躍、player跳起來比board高、(從左邊上board、從右邊上board // 個別是OK => 一起就error  )
                    if (player1.Bounds.IntersectsWith(x.Bounds) && !goup1 && x.Top>=player1.Top+player1.Height-15 )
                       //&& (x.Left <= player1.Left+player1.Width-50 || player1.Left <= x.Left+x.Width-45))
                    {
                        
                        player1.Top = x.Top - player1.Height;
                        uptime1 = 8;
                        gg1 = true;
                    }

                    if (player2.Bounds.IntersectsWith(x.Bounds) && !goup2 && x.Top>=player2.Top+player2.Height-15)
                    {
                        player2.Top = x.Top - player2.Height;
                        uptime2 = 8;
                        gg2 = true;
                    }
                } //改上面怕掉下去
                if (x is PictureBox && x.Tag == "bottom_board")
                {
                    //若player碰到board、非跳躍、player跳起來比board高
                    if (player1.Bounds.IntersectsWith(x.Bounds) && !goup1 && x.Top>=player1.Top+player1.Height-15)
                    {
                        player1.Top = x.Top - player1.Height;
                        uptime1 = 8;
                        gg1 = true;
                    }

                    if (player2.Bounds.IntersectsWith(x.Bounds) && !goup2 && x.Top>=player2.Top+player2.Height-15)
                    {
                        player2.Top = x.Top - player2.Height;
                        uptime2 = 8;
                        gg2 = true;
                    }
                }
            }

            //player吃到果實
            foreach (Control p1 in this.Controls)
            {
                if (p1 is PictureBox && p1.Tag == "point1")
                {
                    if (player1.Bounds.IntersectsWith(p1.Bounds))
                    {
                        /*p1.Visible = false;                             //吃到後即消失
                        p1.Enabled = false;*/                             //使無法觸發（只有消失的話 經過還是會加分）
                        this.Controls.Remove(p1);                       //移除果實(點數) 避免累加
                        score += 1;                                     //吃到後 分數+1
                        pear += 1;                                      //吃到後 梨+1
                    }
                    
                }
            }
            foreach (Control p2 in this.Controls)
            {
                if (p2 is PictureBox && p2.Tag == "point2")
                {
                    if (player2.Bounds.IntersectsWith(p2.Bounds))
                    {
                        /*p2.Visible = false;
                        p2.Enabled = false;*/
                        this.Controls.Remove(p2);
                        score += 1;
                        strawberry += 1;                                //吃到後 草莓+1
                    }
                }
            }

            //過關
            if((player1.Bounds.IntersectsWith(Gdoor.Bounds) && player1.Left <= Gdoor.Left+Gdoor.Width/5) && 
                (player2.Bounds.IntersectsWith(Rdoor.Bounds) && player2.Left >= Rdoor.Left-Rdoor.Width/5))
            {
                player1.Visible = false;
                player2.Visible = false;
                timer1.Stop();
                MessageBox.Show("success!!!!!", "過關");
                result.Text = "草莓玩家獲得了 " + strawberry.ToString() + " 點\n" + "西洋梨玩家獲得 " + pear.ToString() + " 點";
                result.Left = (1000 - result.Width)/2;
                result.Top = (600 - result.Height)/2;
                result.Visible = true;
            }
        }
    }
}
