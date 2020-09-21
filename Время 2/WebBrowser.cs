using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Время_2
{
    public partial class WebBrowser : UserControl
    {
        public WebBrowser()
        {
            InitializeComponent();
        }

        private void WebBrowser_Load(object sender, EventArgs e)
        {

        }

        public void BrowserUrlLoad()
        {
            string url = "http://papka/indexxx.html";
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlAgilityPack.HtmlDocument docc = new HtmlAgilityPack.HtmlDocument();
            if (sender != null || e != null)
            {
                string s = webBrowser1.DocumentText;
                docc.LoadHtml(webBrowser1.DocumentText);
                ParsePage(docc);
            }
        }

        #region обработка полученной страници
        private void ParsePage(HtmlAgilityPack.HtmlDocument html)
        {
            // Получаем полную структуру полученного хтмл файла
            var HmlFileFull = html.DocumentNode.ChildNodes.Where(x => x.Name == "html").ToArray();
            var BodyHtmlFileFull = HmlFileFull[0].ChildNodes.Where(x => x.Name == "body").ToArray();
            var div = BodyHtmlFileFull[0].ChildNodes.Where(x => x.Name == "div").ToArray();
            var div1 = div[0].ChildNodes.Where(x => x.Name == "div").ToArray();
            var table = div1[0].ChildNodes.Where(x => x.Name == "table").ToArray();
            var tr = table[0].ChildNodes.Where(x => x.Name == "tr").ToArray();
            TableParser(tr);
        }

        string[] separator = new string[] { "<br>", "<strong>", "\r\n", "</strong>" };
        private void TableParser(HtmlNode[] trCollection)
        {
            InformationPersonClear();

            foreach (var tr in trCollection)
            {
                var td = tr.ChildNodes.Where(x => x.Name == "td").ToArray();

                if (td[0].Attributes.Count != 0)
                {
                    if (td[0].Attributes[0].Name != "colspan")
                    {
                        if (td.Length != 0)
                        {
                            foreach (var t in td)
                            {
                                string[] stroki = t.InnerHtml.Split(separator, StringSplitOptions.None);

                                PeopleInformationParse(MassiveNormale(stroki));
                            }
                        }
                    }
                }
                
            }
        }

        private string[] MassiveNormale(string[] stroki)
        {
            string[] normaleMassive = new string[0];

            foreach (var s in stroki)
            {
                if (s != "")
                {
                    Array.Resize(ref normaleMassive, normaleMassive.Length+1);
                    normaleMassive[normaleMassive.Length - 1] = s;
                }
            }

            return normaleMassive;
        }

        

        private void PeopleInformationParse(string[] stroki)
        {
            bool WorkingTime = false;
            bool EntersTime = false;
            bool otpuskk = false;

            string PeopleName = "";

            string PeopleInformationOtpusk = "";

            string[] WorkTime = new string[0];
            string[] EnterTime = new string[0];

            for (int i= 0; i< stroki.Length; i++)
            {
                if (i == 0)
                {
                    PeopleName = stroki[i];
                }
                else
                {
                    if (stroki[i] == "График работы")
                    {
                        WorkingTime = true;
                        EntersTime = false;
                        otpuskk = false;
                    }

                    if (stroki[i] == "Проходы")
                    {
                        WorkingTime = false;
                        EntersTime = true;
                        otpuskk = false;
                    }

                    string[] otpusk = stroki[i].Split(new Char[] { ' ' });
                    if (otpusk.Length != 0)
                    {
                        if (otpusk[0] == "Отпуск")
                        {
                            WorkingTime = false;
                            EntersTime = false;
                            otpuskk = true;
                        }
                    }
                }

                if (WorkingTime == true)
                {
                    if (stroki[i] != "График работы")
                    {
                        Array.Resize(ref WorkTime, WorkTime.Length + 1);
                        WorkTime[WorkTime.Length - 1] = stroki[i];
                    }
                }

                if (EntersTime == true)
                {
                    if (stroki[i] != "Проходы")
                    {
                        Array.Resize(ref EnterTime, EnterTime.Length + 1);
                        EnterTime[EnterTime.Length - 1] = stroki[i];
                    }
                }

                if (otpuskk == true)
                {
                    PeopleInformationOtpusk = stroki[i];
                    WorkingTime = false;
                    EntersTime = true;
                    otpuskk = false;
                }

            }
            InformationUpdate(PeopleName, WorkTime, EnterTime, PeopleInformationOtpusk);
        }

        private void InformationUpdate(string PeopleName, string[] WorkTime, string[] EnterTime, string PeopleInformationOtpusk)
        {
            object person = new object[] { PeopleName, WorkTime, EnterTime, PeopleInformationOtpusk };
            Array.Resize(ref StaticClassParametr.PersonInfo, StaticClassParametr.PersonInfo.Length+1);
            StaticClassParametr.PersonInfo[StaticClassParametr.PersonInfo.Length - 1] = person;
        }

        private void InformationPersonClear()
        {
            StaticClassParametr.PersonInfo = new object[0];
        }
        #endregion 
    }
}
