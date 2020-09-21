using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Время_2
{
    class XmlFileInformation
    {

        #region Получение последней полученной отметки с проходной
        public object[] LastTimeEnter(string PersonName, string Month)
        {
            string FileName = Month + ".xml";

            string Patch = FindFolder(PersonName);

            string FilePatch = Patch + FileName;
            SF:
            //Если есть такой фаил приходит ДА и идет чтение
            if (FindFile(FilePatch))
            {
               return FindLastChek(FilePatch);
            }
            //Если файла нет то приходит НЕТ и идет создание с теми данными которыми обладаем
            else
            {
                NewFileCreate(PersonName, Month, FilePatch);
                goto SF;
            }
            
        }
        #endregion

        #region Добавить информацию в XML фаил
        public DateTime AddNewInformationXml(string PersonName, string Month, object[] EnterTimeParseObject, int index)
        {
            string FileName = Month + ".xml";

            string Patch = FindFolder(PersonName);

            string FilePatch = Patch + FileName;

            return AddInformationXml(FilePatch, (string[])EnterTimeParseObject[0], (string[])EnterTimeParseObject[1], (string[])EnterTimeParseObject[2], index);
        }

        private DateTime AddInformationXml(string FilePatch, string[] DataTime, string[] EnterExit, string[] KPP, int index)
        {
            DateTime date = new DateTime();

            XDocument xdocH = XDocument.Load(@FilePatch);
            XElement o21 = xdocH.Element("o21");
            XElement ChekedInformation = o21.Element("ChekedInformation");

            DateTime TimeSumm = Convert.ToDateTime(o21.Element("LastSummTimeMonth").Value);
            if(DataTime.Length != 0) 
            {
                XElement LastChek = o21.Element("LastChek");
                LastChek.Element("Time").Value = DataTime[0];
                LastChek.Element("EnterExit").Value = EnterExit[0];
                LastChek.Element("KPP").Value = KPP[0];

                for (int i = 0; i < index; i++)
                {
                    if (DataTime[i] != null)
                    {
                        date = Convert.ToDateTime(DataTime[i]);
                        string Day = "Day_" + Convert.ToString(date.Day);

                        XElement xDay = ChekedInformation.Element(Day);
                        if (xDay != null)
                        {
                            xDay.Add(new XElement("Chek",
                                                new XElement("DataTime", DataTime[i]),
                                                new XElement("EnterExit", EnterExit[i]),
                                                new XElement("KPP", KPP[i])
                                                ));
                        }
                        else
                        {
                            ChekedInformation.Add(new XElement(Day,
                                new XElement("Chek",
                                                new XElement("DataTime", DataTime[i]),
                                                new XElement("EnterExit", EnterExit[i]),
                                                new XElement("KPP", KPP[i])
                                                )));
                        }

                    }
                    xdocH.Save(@FilePatch);
                }
                    
            }
            
            return TimeSumm;
        }
        #endregion

        #region Проверка инфраструктуры файловой директории
        #region Проверка директории
        private string FindFolder(string PersonName)
        {
            string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string FolderName = "Time\\" + PersonName + "\\";

            string Patch = BaseDirectory + FolderName;
            if (!Directory.Exists(BaseDirectory + FolderName))
            {
                Directory.CreateDirectory(BaseDirectory + FolderName);
            }

            return Patch;
        }
        #endregion

        #region  Проверка есть ли фаил с данными
        private bool FindFile(string FilePatch)
        {
            if (File.Exists(FilePatch))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion

        #region Создать структуру XML файла
        private void NewFileCreate(string PersonName, string Month, string FilePatch)
        {
            XDocument xdocP = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("В этом файле хранится история посещений " + PersonName + " за " + Month),
                new XElement("o21",
                new XElement("Person", PersonName),
                new XElement("LastChek", 
                new XElement("Time","01-01-1999 01:01:01"),
                new XElement("EnterExit","nbvf"),
                new XElement("KPP", "20")),
                new XElement("LastSummTimeMonth", "01-01-0001 0:00:00"),
                new XElement("WorckTime"),
                new XElement("ChekedInformation")
                ));
            xdocP.Save(@FilePatch);
        }
        #endregion

        #region Получение последней отметки
        private object[] FindLastChek(string FilePatch)
        {
            XDocument xdocH = XDocument.Load(@FilePatch);
            XElement o21 = xdocH.Element("o21");
            XElement LastChek = o21.Element("LastChek");
            string Time = LastChek.Element("Time").Value;
            string EnterExit = LastChek.Element("EnterExit").Value;
            string KPP = LastChek.Element("KPP").Value;

            object[] OBJECT = new object[] { Time, EnterExit, KPP };
            return OBJECT;
        }
        #endregion
    }
}
