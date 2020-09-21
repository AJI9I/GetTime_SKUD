using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Время_2
{
    public partial class Form1 : Form
    {
        XmlFileInformation xmlfileinformation;

        public Form1()
        {
            InitializeComponent();
            BrowserAddForm();
        }

        WebBrowser webbrowser;

        private void BrowserAddForm()
        {
            webbrowser = new WebBrowser();
            webbrowser.Location = new Point(5,454);
            Controls.Add(webbrowser);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProgrammUpdate();
            webbrowser.BrowserUrlLoad();
        }

        private void ProgrammUpdate()
        {
            if (StaticClassParametr.PersonInfo.Length != 0)
            {
                foreach (object[] OBJECT in StaticClassParametr.PersonInfo)
                {
                    ObjectParametr(OBJECT);
                }
            }
        }

        private void ObjectParametr(object[] OBJECT)
        {
            string PersonName = (string)OBJECT[0];
            string[] WorkTime = (string[])OBJECT[1];
            string[] EnterTime = (string[])OBJECT[2];
            string PeopleInformationOtpusk = (string)OBJECT[3];

            //Получение последней отметки
            object[] LastChekObject = LastEnterTime(PersonName, DateNow());
            string LastChek = (string)LastChekObject[0];
            string LastChekEnterExit = (string)LastChekObject[1];
            string LastChekKPP = (string)LastChekObject[2];

            //Парсинг строк дата время вхд кпп
            object[] EnterTimeParseObject= TimeParse(EnterTime);
            string[] TimeChek = (string[])EnterTimeParseObject[0];
            string[] EnterExit = (string[])EnterTimeParseObject[1];
            string[] KPP = (string[])EnterTimeParseObject[2];

            if (PersonName == "Якупов Альберт Равилевич")
            {
                int i = 1;
            }

            //Индекс последней отметки
            int index = FindLastChekIndex(LastChek, (string[])EnterTimeParseObject[0]);

            //Удаление из массива имеющихся данных об отметках
            object[] ReplaceMassive = MassiveRaplase(TimeChek, EnterExit, KPP, index);
            TimeChek = (string[])ReplaceMassive[0];
            EnterExit = (string[])ReplaceMassive[1];
            KPP = (string[])ReplaceMassive[2];

            if (PersonName == "Якупов Альберт Равилевич")
            {
                int i = 1;
            }
            ReplaceMassive = MassiveTimeNormalize(TimeChek, EnterExit, KPP, index);
            TimeChek = (string[])ReplaceMassive[0];
            EnterExit = (string[])ReplaceMassive[1];
            KPP = (string[])ReplaceMassive[2];

            if (PersonName == "Якупов Альберт Равилевич")
            {
                int i = 1;
            }


            //Приведение чеков к нормальному виду удаление повторения ВХОДОВ И ВЫХДОВ
            object[] NormaleEnterExit = NormalizeMassiveEnterExit(TimeChek, EnterExit, KPP, LastChekEnterExit, LastChekKPP);
            string[]  NormaleMassiveDataTime = (string[])NormaleEnterExit[0];
            string[]  NormaleMassiveEnterExit = (string[])NormaleEnterExit[1];
            string[]  NormaleMassiveKPP = (string[])NormaleEnterExit[2];

            if (PersonName == "Якупов Альберт Равилевич")
            {
                int i = 1;
            }

            //Очистка от нулевых строк
            NormaleEnterExit = DeleteNullString(NormaleMassiveDataTime, NormaleMassiveEnterExit, NormaleMassiveKPP);
            NormaleMassiveDataTime = (string[])NormaleEnterExit[0];
            NormaleMassiveEnterExit = (string[])NormaleEnterExit[1];
            NormaleMassiveKPP = (string[])NormaleEnterExit[2];


            if (PersonName == "Якупов Альберт Равилевич")
            {
                int i = 1;
            }

            //Добавление новой информации в XML фаил в ответ приходит последняя сумма отработанного времени
            DateTime Time = AddNewInfomationXmlFile(PersonName, DateNow(), NormaleEnterExit, index);

            

            TimeSumm(NormaleMassiveDataTime, NormaleMassiveEnterExit, NormaleMassiveKPP, LastChek, LastChekEnterExit, LastChekKPP);

        }

        #region Очиска от нулевых строк
        private object[] DeleteNullString(string[] NormaleMassiveDataTime, string[] NormaleMassiveEnterExit, string[] NormaleMassiveKPP)
        {
            string[] time = new string[0];
            string[] datatime = new string[0];
            string[] kpp = new string[0];
            int j = 0;
            for (int i = 0; i < NormaleMassiveDataTime.Length; i++)
            {
                if (NormaleMassiveDataTime[i] != null)
                {
                    Array.Resize(ref time, time.Length + 1);
                    Array.Resize(ref datatime, datatime.Length + 1);
                    Array.Resize(ref kpp, kpp.Length + 1);

                    time[j] = NormaleMassiveDataTime[i];
                    datatime[j] = NormaleMassiveEnterExit[i];
                    kpp[j] = NormaleMassiveKPP[i];
                }
            }

            object[] OBJECT = new object[] { time, datatime, kpp };
            return OBJECT;

        }
        #endregion

        #region Удаление имеющихся данных из массива (полученных повторно но вместе с новыми)
        private object[] MassiveRaplase(string[] TimeChek, string[] EnterExit,string[] KPP,int index)
        {
            string[] timechek = new string[index];
            string[] enterexit = new string[index];
            string[] kpp = new string[index];

            for (int i = 0; i<index;i++)
            {
                timechek[i] = TimeChek[i];
                enterexit[i] = EnterExit[i];
                kpp[i] = KPP[i];
            }
            object[] OBJECT = new object[] { timechek, enterexit, kpp };
            return OBJECT;
        }
        #endregion

        #region Приведение чеков к нормальному виду удаление повторения ВХОДОВ И ВЫХДОВ
        private object[] NormalizeMassiveEnterExit(string[] DataTime, string[] EnterExit, string[] KPP, string LastChekEnterExit, string LastChekKPP)
        {
            string[] datatime = new string[0];
            string[] enterexit = new string[0];
            string[] kpp = new string[0];

            string Last = LastChekEnterExit;
            string LastKPP = LastChekKPP;

            int c = 0;
            for (int i = 0; i < DataTime.Length; i++)
            {

                if (Last == "nbvf" && EnterExit[i] == "Выход")
                {

                }
                if (Last == "nbvf" && EnterExit[i] == "Вход")
                {
                    Array.Resize(ref datatime, datatime.Length + 1);
                    Array.Resize(ref enterexit, enterexit.Length + 1);
                    Array.Resize(ref kpp, kpp.Length + 1);
                    datatime[c] = DataTime[i];
                    enterexit[c] = EnterExit[i];
                    kpp[c] = KPP[i];
                    c++;
                    Last = EnterExit[i];
                    LastKPP = KPP[i];
                }
                if (Last == "Вход" && EnterExit[i] == "Выход")
                {
                    Array.Resize(ref datatime, datatime.Length + 1);
                    Array.Resize(ref enterexit, enterexit.Length + 1);
                    Array.Resize(ref kpp, kpp.Length + 1);
                    datatime[c] = DataTime[i];
                    enterexit[c] = EnterExit[i];
                    kpp[c] = KPP[i];
                    c++;
                    Last = EnterExit[i];
                    LastKPP = KPP[i];
                }
                if (Last == "Выход" && EnterExit[i] == "Вход")
                {
                    Array.Resize(ref datatime, datatime.Length + 1);
                    Array.Resize(ref enterexit, enterexit.Length + 1);
                    Array.Resize(ref kpp, kpp.Length + 1);
                    datatime[c] = DataTime[i];
                    enterexit[c] = EnterExit[i];
                    kpp[c] = KPP[i];
                    c++;
                    Last = EnterExit[i];
                    LastKPP = KPP[i];
                }
                if (Last == "Выход" && EnterExit[i] == "Выход")
                {
                    if (LastKPP == KPP[i])
                    {
                        c--;
                        Array.Resize(ref datatime, datatime.Length + 1);
                        Array.Resize(ref enterexit, enterexit.Length + 1);
                        Array.Resize(ref kpp, kpp.Length + 1);
                        datatime[c] = DataTime[i];
                        enterexit[c] = EnterExit[i];
                        kpp[c] = KPP[i];
                        c++;
                        Last = EnterExit[i];
                        LastKPP = KPP[i];
                    }
                    if (LastKPP != KPP[i])
                    {

                    }

                    if (Last == "Вход" && EnterExit[i] == "Вход")
                    {
                        if (LastKPP == KPP[i])
                        {

                        }
                        if (LastKPP != KPP[i])
                        {
                            c--;
                            Array.Resize(ref datatime, datatime.Length + 1);
                            Array.Resize(ref enterexit, enterexit.Length + 1);
                            Array.Resize(ref kpp, kpp.Length + 1);
                            datatime[c] = DataTime[i];
                            enterexit[c] = EnterExit[i];
                            kpp[c] = KPP[i];
                            c++;
                            Last = EnterExit[i];
                            LastKPP = KPP[i];
                        }

                    }

                //Последний чек Вход, сравнение с Выходом, если Кпп равны запись
                //Последний чек Вход, сравнение с Входом
                //, если Кпп равны, не чего не происходит
                // если Кпп не равны запись в ячейку с последним элементом

                //Последний чек Выход, сравнение с Выходом, Если Кпп равны,
                //Запись в ячейку с предыдущей записью Выход из массива
                }
            }

            object[] OBJECT = new object[] { datatime, enterexit, kpp };
            return OBJECT;
        }
        #endregion


        #region Подсчет времени
        private object[] TimeSumm(string[] NormaleMassiveDataTime, string[] EnterExit, string[] KPP, string LastChek, string LastChekEnterExit, string LastChekKPP)
        {
            DateTime TimeEnter = new DateTime();
            DateTime TimeExit = new DateTime();

            System.TimeSpan SummTime = new System.TimeSpan();
            System.TimeSpan SummTimeMont = new System.TimeSpan();

            bool BoolExit = false;
            bool BoolEnter = false;

            DateTime TikDateEnter = new System.DateTime(1996, 6, 3, 22, 15, 0);
            DateTime TikDatelast = new System.DateTime();

            if (LastChekEnterExit == "Вход")
            {
                BoolEnter = true;
                TikDatelast = Convert.ToDateTime(LastChek);
                TimeEnter = Convert.ToDateTime(LastChek);
            }

            for (int i = 0; i < NormaleMassiveDataTime.Length;i++)
            {
                if (EnterExit[i] == "Вход" && LastChekEnterExit == "Выход")
                {
                    TimeEnter = Convert.ToDateTime(NormaleMassiveDataTime[i]);
                    LastChekEnterExit = "Вход";
                    
                    BoolEnter = true;
                }
                if (EnterExit[i] == "Выход" && LastChekEnterExit == "Вход")
                {
                    TimeExit = Convert.ToDateTime(NormaleMassiveDataTime[i]);
                    LastChekEnterExit = "Выход";
                    BoolExit = true;
                }
                if (BoolEnter == true && BoolExit == true)
                {
                    if (TikDatelast.Date == TimeExit.Date)
                    {
                        System.TimeSpan Raznica = TimeExit.Subtract(TimeEnter);
                        SummTime = SummTime.Add(Raznica);

                        TikDatelast = Convert.ToDateTime(NormaleMassiveDataTime);

                        BoolEnter = false;
                        BoolExit = false;
                    }
                    else
                    {
                        SummTimeMont = SummTimeMont.Add(SummTime);
                        SummTime = new System.TimeSpan();

                        System.TimeSpan Raznica = TimeExit.Subtract(TimeEnter);
                        SummTime = SummTime.Add(Raznica);

                        TikDatelast = Convert.ToDateTime(NormaleMassiveDataTime);

                        BoolEnter = false;
                        BoolExit = false;
                    }
                }          
            }

            object[] OBJECT = new object[] { };
            return OBJECT;
        }

        ProfileInformationPanel profileinformationpanel;

        private void ProfileInformationPanelControl(string personname, string[] MonthTime, string[] DayTime, string EnterExit, string KPP, int index)
        {
            if (profileinformationpanel == null)
            {
                profileinformationpanel = new ProfileInformationPanel();
                profileinformationpanel.Location = new Point(10,10);
                Controls.Add(profileinformationpanel);
            }
            profileinformationpanel.personinfo(personname, MonthTime, DayTime, EnterExit, KPP, index);
        }

        private object[] MassiveTimeNormalize(string[] TimeChek, string[] EnterExit, string[] KPP, int index)
        {

            string[] timechek = new string[index];
            string[] enterexit = new string[index];
            string[] kpp = new string[index];
            int b = TimeChek.Length - 1;
            for (int i = 0; i < timechek.Length; i++)
            {
                timechek[i] = TimeChek[b];
                enterexit[i] = EnterExit[b];
                kpp[i] = KPP[b];
                b--;
            }

            object[] OBJECT = new object[] { timechek, enterexit , kpp };
            return OBJECT;
        }
        #endregion

        #region Обновение информации в файле
        private DateTime AddNewInfomationXmlFile(string PersonName, string Month, object[] EnterTimeParseObject, int index)
        {
            xmlfileinformation = new XmlFileInformation();
            return xmlfileinformation.AddNewInformationXml(PersonName, Month, EnterTimeParseObject, index);
        }
        #endregion

        #region поиск индекса последней отметки
        private int FindLastChekIndex(string LastChek, string[] EnterDateTime)
        {
            for (int i =0; i< EnterDateTime.Length;i++)
            {
                if (LastChek == EnterDateTime[i])
                {
                    return i;
                }
            }
            return EnterDateTime.Length;
        }
        #endregion

        #region Разбор строк с датой временем
        private object[] TimeParse(string[] EnterTime)
        {
            string[] DataTime = new string[0];

            string[] EnterExit = new string[0];

            string[] KPP = new string[0];

            foreach (string str in EnterTime)
            {
                Array.Resize(ref DataTime, DataTime.Length + 1);
                Array.Resize(ref EnterExit, EnterExit.Length + 1);
                Array.Resize(ref KPP, KPP.Length + 1);

                string[] st = str.Split(new Char[] { ' ' });

                DataTime[DataTime.Length - 1] = st[0] + " " + st[1];
                EnterExit[EnterExit.Length - 1] = st[2];
                KPP[KPP.Length - 1] = st[3];
            }

            object[] OBJECT = new object[] { DataTime, EnterExit , KPP };
            return OBJECT;
        }
        #endregion

        #region Получение последней отметки с проходных
        
        private object[] LastEnterTime(string PersonName, string Month)
        {
            xmlfileinformation = new XmlFileInformation();
            return xmlfileinformation.LastTimeEnter(PersonName, Month);
        }
        #endregion

        #region Получение текущей даты месяц год
        private string DateNow()
        {
            DateTime localDate = DateTime.Now;
            string MonthJar = localDate.ToString("y");
            return MonthJar;
        }
        #endregion

    }
}
