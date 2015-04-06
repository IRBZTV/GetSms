using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://smshome.ir/");
            webBrowser2.ScriptErrorsSuppressed = true;
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser2.ScriptErrorsSuppressed = true;
            webBrowser1.ScriptErrorsSuppressed = true;
            if (webBrowser1.Url.Equals("http://smshome.ir/"))
            {

                //richTextBox1.Text += "\n  Try Loggin \n";
                //richTextBox1.SelectionStart = richTextBox1.Text.Length;
                //richTextBox1.ScrollToCaret();
                //Application.DoEvents();

                HtmlDocument Doc = webBrowser1.Document;

                HtmlElementCollection TxtColl = Doc.Body.GetElementsByTagName("input");
                if (TxtColl.Count > 2)
                {
                    foreach (HtmlElement item in TxtColl)
                    {
                        if (item.GetAttribute("id") == "txtUsername")
                        {
                            item.InnerText = "bazar10";
                        }
                        if (item.GetAttribute("id") == "txtPassword")
                        {
                            item.InnerText = "38387";
                        }

                    }
                }
            }
            if (webBrowser1.Url.Equals("http://smshome.ir/List/AlertNew.aspx"))
            {
                webBrowser2.Url = new Uri("http://smshome.ir/List/MessageReceive.aspx");
                tabControl1.SelectedIndex = 1;

            }

        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser2.ScriptErrorsSuppressed = true;
            webBrowser1.ScriptErrorsSuppressed = true;
            timer1.Enabled = false;
            try
            {
                if (webBrowser2.Url.Equals("http://smshome.ir/List/MessageReceive.aspx"))
                {
                    if (webBrowser2.ReadyState.Equals(WebBrowserReadyState.Complete))
                    {
                        HtmlDocument Doc = webBrowser2.Document;
                        HtmlElement SmsNumberElement33333 = Doc.GetElementById("ctl00_ContentPlaceHolder1_GridView1_ctl02_Label3");
                        if (SmsNumberElement33333 != null)
                        {
                            for (int i = 2; i < 12; i++)
                            {
                                string Number = "";
                                string Txt = "";

                                HtmlElement SmsNumberElement = Doc.GetElementById("ctl00_ContentPlaceHolder1_GridView1_ctl" + i.ToString("00") + "_Label3");
                                if (SmsNumberElement != null)
                                {
                                    if (SmsNumberElement.InnerText != null)
                                    {
                                        Number = SmsNumberElement.InnerText;
                                    }

                                    HtmlElement SmsTextElement = Doc.GetElementById("ctl00_ContentPlaceHolder1_GridView1_ctl" + i.ToString("00") + "_pnlBody");
                                    if (SmsTextElement.InnerText != null)
                                    {
                                        Txt = SmsTextElement.InnerText;
                                    }

                                    SqlConnection conn = new SqlConnection(
                                             "Data Source=192.168.10.41;Initial Catalog=sms;User ID=8668;Password=09112064568");

                                    //SqlConnection conn = new SqlConnection(
                                    //        "Data Source=.;Initial Catalog=sms;User ID=dbuser;Password=dbuser");

                                    string insertString = @"
                                     insert into insms
                                     (msg, orig,dest)
                                     values (N'" + Txt.Replace("'", " ")
                                + "' , '" + Number.Replace("'", " ") + "' , '1000010' )";
                                    conn.Open();

                                    // 1. Instantiate a new command with a query and connection
                                    SqlCommand cmd = new SqlCommand(insertString, conn);

                                    // 2. Call ExecuteNonQuery to send command
                                    cmd.ExecuteNonQuery();
                                    conn.Close();

                                }
                            }

                            //Check All Sms
                            Doc.GetElementById("ctl00_ContentPlaceHolder1_GridView1_ctl01_checkBoxAll").InvokeMember("Click");

                            //Change Ddl to move to Delete:
                            Doc.GetElementById("ctl00_ContentPlaceHolder1_ddlOperation").Children[5].SetAttribute("selected", "selected");


                            //Click Do Action Btn:
                            Doc.GetElementById("ctl00_ContentPlaceHolder1_btnOperation").InvokeMember("Click");
                        }
                    }
                }
            }
            catch
            {
                timer1.Enabled = true;
            }
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser2.Url = new Uri("http://smshome.ir/List/MessageReceive.aspx");
            tabControl1.SelectedIndex = 1;
        }
    }
}