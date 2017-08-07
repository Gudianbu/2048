using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace _2048
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Game g;

        Bitmap bit = new Bitmap(400, 400);
        messageBox mes = new messageBox();
        //private string bitfile = Directory.GetCurrentDirectory() + "\\成绩截图";
        private string bitfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\成绩截图";
        string paths = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)+"\\2048Save";
        //string sb = @"save/2048Save";
        /// <summary>
        /// 应用启动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(paths);
            if (File.Exists(paths))//监测是否有存档
            {
                ToLoad();
            }
            else//没有存档生成新游戏
            {
                messageBox mes1 = new messageBox();
                mes1.a = "提示";
                mes1.b = "检测到您是第一次打开，可按F1打开帮助。\r\n    游戏为无限模式，直到GameOver才会终止。";
                mes1.StartPosition = FormStartPosition.CenterScreen;//窗体居中
                mes1.ShowDialog();
                g = new Game();
                g.Reset();
            }
            mes.a = "提示";
            mes.b = "F2:窗口总在最前\r\nF3:解锁/锁定窗口\r\nF4:隐藏任务栏图标\r\nF5:重新开始\r\nF6:截图并保存\r\nShife + ↑↓：调整透明度\r\n↑↓←→：控制方块移动\r\nESC:退出\r\n作者:Ch\r\nQQ:153563565";
            Drawing();
            Game_pictureBox.Refresh();
        }

        /// <summary>
        /// 捕捉键盘事件
        /// </summary>
        /// <param name="sender">键盘模型</param>
        /// <param name="e">键盘事件参数</param>
        private void MainForm_KeyDown(object sender,KeyEventArgs e)
        {
                switch (e.KeyCode)
                {
                    #region 方向键 ↑
                    case Keys.Up:
                        if (e.Modifiers == Keys.Shift)
                        {
                            this.Opacity -= 0.1;
                        }
                        else
                        {
                            g.Up();
                            if (g.change)
                            {
                                g.Add();
                            }
                        }
                        break;
                    #endregion

                    #region 方向键 ↓
                    case Keys.Down:
                        if (e.Modifiers == Keys.Shift)
                        {
                            this.Opacity += 0.1;
                        }
                        else
                        {
                            g.Down();
                            if (g.change)
                            {
                                g.Add();
                            }
                        }
                        break;
                    #endregion

                    #region 方向键 ←
                    case Keys.Left:
                        g.Left();
                        if (g.change)
                        {
                            g.Add();
                        }
                        break;
                    #endregion

                    #region 方向键 →
                    case Keys.Right:
                        g.Right();
                        if (g.change)
                        {
                            g.Add();
                        }
                        break;
                    #endregion

                    #region F1
                    case Keys.F1:
                        mes.ShowDialog();
                        break;
                    #endregion

                    #region F2
                    case Keys.F2:
                        this.TopMost = !this.TopMost;
                        break;
                    #endregion

                    #region F3
                    case Keys.F3:
                        if(this.FormBorderStyle==FormBorderStyle.FixedToolWindow)
                        {
                            this.FormBorderStyle = FormBorderStyle.None;
                        }
                        break;
                    #endregion

                    #region F4
                    case Keys.F4:
                        this.ShowInTaskbar = !this.ShowInTaskbar;
                        break;
                    #endregion

                    #region F5
                    case Keys.F5:
                        g = new Game();
                        g.Reset();
                        Game_pictureBox.Refresh();
                        break;
                    #endregion

                    #region F6
                    case Keys.F6:
                        GameScreen();//截图
                        messageBox mes2 = new messageBox();
                        mes2.a = "保存成功";
                        mes.b = "保存在" + Directory.GetCurrentDirectory() + "\\成绩截图.bmp";
                        mes2.ShowDialog();
                        break;
                    #endregion

                    #region ESC
                    case Keys.Escape:
                        Close();
                        break;
                    #endregion
                }
                Drawing();
                Game_pictureBox.Refresh();
                grade.Text = g.grade.ToString();//当前成绩
                if (g.die)//判断游戏是否结束
                {
                    GameOver();
                }
        }

        /// <summary>
        /// 在pic上画出方块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Game_pictureBox.BackgroundImage = bit;
            grade.Text = g.grade.ToString();
        }

        /// <summary>
        /// 窗体关闭时的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender,FormClosedEventArgs e)
        {
            classSave();
        }

        /// <summary>
        /// 画出方块上和其数字
        /// </summary>
        /// <param name="m">M</param>
        /// <param name="Coordinate">坐标</param>
        private void DrawDisplay(int m, Point Coordinate)
        {
            Graphics gra = Graphics.FromImage(bit);

            switch (m)
            {
                case 0:
                    {
                        gra.FillRectangle(new SolidBrush(Color.BurlyWood), Coordinate.X, Coordinate.Y, 90, 90);//原木色
                    }
                    break;
                case 2:
                    {
                        gra.FillRectangle(new SolidBrush(Color.LightSalmon), Coordinate.X, Coordinate.Y, 90, 90);//橙红
                    }
                    break;
                case 4:
                    {
                        gra.FillRectangle(new SolidBrush(Color.Peru), Coordinate.X, Coordinate.Y, 90, 90);//秘鲁色
                    }
                    break;
                case 8:
                    {
                        gra.FillRectangle(new SolidBrush(Color.Chocolate), Coordinate.X, Coordinate.Y, 90, 90);//巧克力色
                    }
                    break;
                case 16:
                    {
                        gra.FillRectangle(new SolidBrush(Color.Gray), Coordinate.X, Coordinate.Y, 90, 90);//灰色
                    }
                    break;
                case 32:
                    {
                        gra.FillRectangle(new SolidBrush(Color.DarkSeaGreen), Coordinate.X, Coordinate.Y, 90, 90);//深海藻绿
                    }
                    break;
                case 64:
                    {
                        gra.FillRectangle(new SolidBrush(Color.Gold), Coordinate.X, Coordinate.Y, 90, 90);//金色
                    }
                    break;
                case 128:
                    {
                        gra.FillRectangle(new SolidBrush(Color.HotPink), Coordinate.X, Coordinate.Y, 90, 90);//粉色
                    }
                    break;
                case 256:
                    {
                        gra.FillRectangle(new SolidBrush(Color.DarkOrange), Coordinate.X, Coordinate.Y, 90, 90);//深橙
                    }
                    break;
                case 512:
                    {
                        gra.FillRectangle(new SolidBrush(Color.LightPink), Coordinate.X, Coordinate.Y, 90, 90);//浅粉红色
                    }
                    break;
                case 1024:
                    {
                        gra.FillRectangle(new SolidBrush(Color.DarkRed), Coordinate.X, Coordinate.Y, 90, 90);//暗红
                    }
                    break;
                case 2048:
                    {
                        gra.FillRectangle(new SolidBrush(Color.Red), Coordinate.X, Coordinate.Y, 90, 90);//红色
                    }
                    break;
            }

            switch (m)
            {
                case 2:
                case 4:
                case 8:
                    {
                        gra.DrawString(m.ToString(), new Font("隶书", 40.5f, FontStyle.Bold), new SolidBrush(Color.White), Coordinate.X + 22, Coordinate.Y + 17);
                    }
                    break;
                case 16:
                case 32:
                case 64:
                    {
                        gra.DrawString(m.ToString(), new Font("隶书", 40.5f, FontStyle.Bold), new SolidBrush(Color.White), Coordinate.X + 8, Coordinate.Y + 17);
                    }
                    break;
                case 128:
                case 256:
                case 512:
                    {
                        gra.DrawString(m.ToString(), new Font("隶书", 35.5f, FontStyle.Bold), new SolidBrush(Color.White), Coordinate.X + 0, Coordinate.Y + 20);
                    }
                    break;
                case 1024:
                case 2048:
                case 4096:
                case 8192:
                    {
                        gra.DrawString(m.ToString(), new Font("隶书", 30.5f, FontStyle.Bold), new SolidBrush(Color.White), Coordinate.X - 4, Coordinate.Y + 23);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 画出每个方块
        /// </summary>
        private void Drawing()
        {
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    Point p = new Point(x * 100 - 95, y * 100 - 95);
                    DrawDisplay(g.i[x, y], p);
                }
            }
        }
 

        /// <summary>
        /// 序列化Game类，并保存，相当于存档
        /// </summary>
        private void classSave()
        {
            //FileStream fw = new FileStream(@"save/2048Save", FileMode.Create, FileAccess.Write);
            FileStream fw = new FileStream(paths, FileMode.Create, FileAccess.Write);
            BinaryFormatter formatter_w = new BinaryFormatter();
            formatter_w.Serialize(fw, g);

            //File.SetAttributes("D:\\2048记录", FileAttributes.Hidden);
            fw.Close();
        }

        /// <summary>
        /// 从文件反序列化读取存档
        /// </summary>
        private void ToLoad()
        {
            //FileStream fr = new FileStream(@"save/2048Save", FileMode.Open, FileAccess.Read);
            FileStream fr = new FileStream(paths, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter_r = new BinaryFormatter();
            g = (Game)formatter_r.Deserialize(fr);
            grade.Text = g.grade.ToString();
            bestgrade.Text = g.grade.ToString();
            fr.Close();

        }

        /// <summary>
        /// 截图
        /// </summary>
        private void GameScreen()
        {
            Bitmap b = new Bitmap(this.Width, this.Height);
            Graphics gr = Graphics.FromImage(b);
            gr.CopyFromScreen(this.Location, new Point(0, 0), this.Size);
            gr.Dispose();
            if (!File.Exists(bitfile))//判断截图是否已存在，如果存在，在路径后面加上一个空格继续保存，避免覆盖
            {
                //Random r=new Random();
                //bitfile += DateTime.Now.ToString("yyyy-MM-dd-mm-ss");
                //bitfile += r.Next(1, 100).ToString();
                bitfile += " ";
            }
            b.Save(bitfile + ".jpg");

        }     

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void GameOver()
        {
            if(g.grade<g.bestGrade)
            {
                g.grade = g.bestGrade;//当前成绩赋值于最好成绩
                bestgrade.Text = g.bestGrade.ToString();
                GameScreen();//截图保存
                messageBox mes3 = new messageBox();
                mes3.a = "恭喜！";
                mes3.b = "新的记录！自动为您保存截图。\r\n保存在" + bitfile;
                mes3.ShowDialog();
                g.Reset();
                Drawing();
                Game_pictureBox.Refresh();
            }
            else
            {
                Game_OverFrom game = new Game_OverFrom();
                game.bg = g.bestGrade;
                game.gv = g.grade;
                DialogResult d = game.ShowDialog();
                switch (d)
                {
                    case DialogResult.Abort:
                        GameScreen();
                        messageBox mes2 = new messageBox();
                        mes2.a = "保存成功";
                        mes2.b = "保存在" + bitfile;
                        mes2.ShowDialog();
                        g.Reset();
                        classSave();
                        Drawing();
                        Game_pictureBox.Refresh();
                        break;
                    case DialogResult.No:
                        g.Reset();
                        this.Close();
                        break;
                    case DialogResult.Retry:
                        g.Reset();
                        Drawing();
                        Game_pictureBox.Refresh();
                        grade.Text = g.grade.ToString();
                        bestgrade.Text = g.bestGrade.ToString();
                        break;
                }
            }
        }
    }
}
