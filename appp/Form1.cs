using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace appp
{
    public partial class Form1 : Form
    {
        string value_path = "C:\\ak.txt";
        string value_path2 = "C:\\bk.txt";
        public Form1()
        {
            InitializeComponent();
        }
        bool start = false;
        string value = string.Empty;
        string value1 = string.Empty;
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte[] InputBuf = new Byte[128];
            try
            {
                System.Threading.Thread.Sleep(30);
                if (serialPort1.IsOpen)
                {
                    serialPort1.Read(InputBuf, 0, serialPort1.BytesToRead);                                //读取缓冲区的数据直到“}”即0x7D为结束符  
                                                                                                           //InputBuf = UnicodeEncoding.Default.GetBytes(strRD);             //将得到的数据转换成byte的格式  
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string str = encoding.GetString(InputBuf);
                    str = str.Replace("\0", "");
                    if (str.StartsWith("N") && str.Length == 22)
                    {
                        str = str.Substring(6, 10);
                        str = str.Replace("+", "");
                        str = str.Replace(" ", "");
                        value = str;
                    }
                }

            }
            catch (TimeoutException ex)         //超时处理  
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {



            Byte[] InputBuf = new Byte[128];
            try
            {
                System.Threading.Thread.Sleep(30);
                if (serialPort2.IsOpen)
                {
                    serialPort2.Read(InputBuf, 0, serialPort2.BytesToRead);                                //读取缓冲区的数据直到“}”即0x7D为结束符  
                                                                                                           //InputBuf = UnicodeEncoding.Default.GetBytes(strRD);             //将得到的数据转换成byte的格式  
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string str = encoding.GetString(InputBuf);
                    str = str.Replace("\0", "");
                    if (str.StartsWith("N") && str.Length == 22)
                    {
                        str = str.Substring(6, 10);
                        str = str.Replace("+", "");
                        str = str.Replace(" ", "");
                        value1 = str;
                    }
                }

            }
            catch (TimeoutException ex)         //超时处理  
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Write(textBox1.Text);
        }
        private void Thread1()
        {
            string str;
            str = Ini.Read("config", "com1");
            try
            {

                if (str != "null")
                {
                    textBox3.Text = str;
                    serialPort2.PortName = "COM" + Ini.Read("config", "com1");
                    serialPort2.Open();

                }
                else
                {
                    MessageBox.Show("串口未配置！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("COM{0}打开失败！", str));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = value_path;
            label6.Text = value_path2;

            // Thread thread1 = new Thread(new ThreadStart(Thread1));
            //thread1.Start();
            Thread1();
            open();
            start = true;
            if (!File.Exists(value_path))
            {
                FileStream fs = new FileStream(value_path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                //获得字节数组
                byte[] data = Encoding.Default.GetBytes("0.00");
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            if (!File.Exists(value_path2))
            {
                FileStream fs1 = new FileStream(value_path2, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                //获得字节数组
                byte[] data1 = Encoding.Default.GetBytes("0.00");
                //开始写入
                fs1.Write(data1, 0, data1.Length);
                //清空缓冲区、关闭流
                fs1.Flush();
                fs1.Close();
            }
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            MessageBox.Show(e.ExceptionObject.ToString());
        }
        private void open()
        {
            string str;
            str = Ini.Read("config", "com");
            try
            {

                if (str != "null")
                {
                    textBoxCom.Text = str;
                    serialPort1.PortName = "COM" + Ini.Read("config", "com");
                    serialPort1.Open();
                }
                else
                {
                    MessageBox.Show("串口未配置！");
                }
            }
            catch
            {
                MessageBox.Show(string.Format("COM{0}打开失败！", str));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(value_path, Encoding.Default);
                String line;
                line = sr.ReadLine();
                //label1.Text = line;
                sr.Close();

                if (line != value && value.Length > 0)
                {
                    textBox2.Text = value;
                    FileStream fs = new FileStream(value_path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    //获得字节数组
                    byte[] data = System.Text.Encoding.Default.GetBytes(value + "\0\0\0\0\0\0\0\0\0\0");
                    //开始写入
                    fs.Write(data, 0, data.Length);
                    //清空缓冲区、关闭流
                    fs.Flush();
                    fs.Close();
                }


                StreamReader sr1 = new StreamReader(value_path2, Encoding.Default);
                String line1;
                line1 = sr1.ReadLine();
                //label1.Text = line;
                sr1.Close();

                if (line1 != value1 && value1.Length > 0)
                {
                    textBox5.Text = value1;
                    FileStream fs1 = new FileStream(value_path2, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    //获得字节数组
                    byte[] data2 = System.Text.Encoding.Default.GetBytes(value1 + "\0\0\0\0\0\0\0\0\0\0");
                    //开始写入
                    fs1.Write(data2, 0, data2.Length);
                    //清空缓冲区、关闭流
                    fs1.Flush();
                    fs1.Close();
                }
            }
            catch
            {
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (start)
            {
                string str = textBox3.Text.Replace(" ", "");
                Ini.Write("config", "com1", str);

                if (serialPort2.IsOpen)
                {
                    serialPort2.Close();
                }

                //重新打开
                Thread1();
            }
        }
        private void textBoxCom_TextChanged(object sender, EventArgs e)
        {
            if (start)
            {
                string str = textBoxCom.Text.Replace(" ", "");
                Ini.Write("config", "com", str);

                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }

                //重新打开
                open();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen)
                serialPort2.Write(textBox4.Text);
        }
    }
}
