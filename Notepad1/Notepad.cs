using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Text;
using System.Collections;
using System.IO;

namespace Notepad1
{
    public partial class Notepad : Form
    {
        public Notepad()
        {
            InitializeComponent();
            this.Text = "文本编辑器";
            this.label1.Visible = false;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //取消编辑模式
            //e.Handled = true;//与DropDownStyle的DropDown连用产生显示文本，但只能下拉无法编辑的状态
        }
        //加粗
        private void button3_Click(object sender, EventArgs e)
        {//普通-》加粗；加粗-》普通
            if (richTextBox1.Font.Bold == false)
            {
                richTextBox1.Font = new Font(richTextBox1.Font, richTextBox1.Font.Style | FontStyle.Bold);
            }
            else
            {
                richTextBox1.Font = new Font(richTextBox1.Font, richTextBox1.Font.Style & FontStyle.Regular);
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //键入不是数字，也不是enter，也不是删除键
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (button1.Image != null)
            {//将图片按要求缩小或者放大
                //1.获取要改变的图片；2.改变图片的宽度；3.改变图片的高度
                button1.Image = new Bitmap(button1.Image, button1.Image.Width - 100, button1.Image.Height - 70);
            }
            if (button2.Image != null)
            {//将图片按要求缩小或者放大
                //1.获取要改变的图片；2.改变图片的宽度；3.改变图片的高度
                button2.Image = new Bitmap(button2.Image, button2.Image.Width - 100, button2.Image.Height - 101);
            }
            if (button3.Image != null)
            {//将图片按要求缩小或者放大
                //1.获取要改变的图片；2.改变图片的宽度；3.改变图片的高度
                button3.Image = new Bitmap(button3.Image, button3.Image.Width - 100, button3.Image.Height - 101);
            }
            if (button4.Image != null)
            {//将图片按要求缩小或者放大
                //1.获取要改变的图片；2.改变图片的宽度；3.改变图片的高度
                button4.Image = new Bitmap(button4.Image, button4.Image.Width - 50, button4.Image.Height - 75);
            }
            if (button5.Image != null)
            {
                button5.Image = new Bitmap(button5.Image, button5.Image.Width - 73, button5.Image.Height - 100);
            }
            //窗体加载时，加载系统字体
            InstalledFontCollection myfont = new InstalledFontCollection();
            FontFamily[] ff = myfont.Families;
            ArrayList list = new ArrayList();
            int count = ff.Length;
            for (int i = 0; i < count; i++)
            {
                string fontname = ff[i].Name;
                comboBox3.Items.Add(fontname);
            }
        }
        //倾斜
        private void button4_Click(object sender, EventArgs e)
        {//判断是否倾斜
            if (richTextBox1.Font.Italic == false)
            {//未倾斜就加倾斜
                richTextBox1.Font = new Font(richTextBox1.Font, richTextBox1.Font.Style | FontStyle.Italic);
            }
            else
            {//倾斜就取消
                richTextBox1.Font = new Font(richTextBox1.Font, richTextBox1.Font.Style & FontStyle.Regular);
            }
        }
        //字号
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fs = Convert.ToInt32(comboBox2.Text);
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, fs, richTextBox1.Font.Style);
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.comboBox2_SelectedIndexChanged(sender, e);
            }
        }
        //修改字体
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fontname = comboBox3.Text;
            richTextBox1.Font = new Font(fontname, richTextBox1.Font.Size, richTextBox1.Font.Style);
        }
        //保存
        private void button2_Click(object sender, EventArgs e)
        {

            if (richTextBox1.Text.Trim() != "")
            {
                if (this.Text == "文本编辑器")
                {
                    //创建一个筛选器/过滤器
                    saveFileDialog1.Filter = ("文本文档（*.txt）|*.txt");//只能存.txt格式的
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string sfilepath = saveFileDialog1.FileName;
                        //保存文件到指定路径
                        StreamWriter sw = new StreamWriter(sfilepath, false);
                        sw.WriteLine(richTextBox1.Text.Trim());
                        //将保存的路径设置为当前窗体名称
                        this.Text = sfilepath;
                        //释放资源
                        sw.Flush();
                        sw.Close();
                    }
                }
                else
                {

                    string path = this.Text;
                    StreamWriter sw = new StreamWriter(path, false);
                    sw.WriteLine(richTextBox1.Text.Trim());
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                MessageBox.Show("空文档无法保存！", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }
        //打开
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ("文本文档（*.txt）|*.txt");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                string text = sr.ReadToEnd();
                richTextBox1.Text = text;
                //将打开文本的路径显示在窗体名称，方便之后保存
                this.Text = path;
                sr.Close();
            }
        }
        //窗体关闭时提示
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //判断记号状态
            if (this.label1.Text == "*")
            {
                DialogResult dr = MessageBox.Show("文档已被编辑但未保存，是否继续退出？", "信息询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Dispose();
                    button2_Click(sender, e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        //文本被编辑
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.label1.Text = "*";
        }
        //新建
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            this.label1.Text = "";
            this.Text = "";
        }
    }
}
