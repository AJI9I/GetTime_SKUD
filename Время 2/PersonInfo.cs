using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Время_2
{
    public partial class PersonInfo : UserControl
    {
        public PersonInfo()
        {
            InitializeComponent();
        }

        private void PersonInfo_Load(object sender, EventArgs e)
        {
            this.Size = new Size(500,50);
        }

        #region Имя
        Label ProfileLabel;
        public void ProfileNames(string ProfileName)
        {
            if(ProfileLabel == null)
            ProfileLabel = new Label();
            ProfileLabel.Size = new Size(25, 150);
            ProfileLabel.Location = new Point(10, (this.Size.Height/2)-(ProfileLabel.Size.Height/2));
            ProfileLabel.Text = ProfileName;
            Controls.Add(ProfileLabel);
        }
        #endregion

        #region Отработанное время в месяц
        Label MonthLabel;
        public void MonthTimeLabel(string Hour, string minute)
        {
            if (ProfileLabel == null)
                MonthLabel = new Label();
            MonthLabel.Size = new Size(25, 100);
            MonthLabel.Location = new Point(10, (this.Size.Height / 2) - (MonthLabel.Size.Height / 2));
            MonthLabel.Text = Hour + "ч " + minute + "v";
            Controls.Add(MonthLabel);
        }
        #endregion

        #region Отработанное время за день
        Label DayLabel;
        public void DayTimeLabel(string Hour, string minute)
        {
            if (ProfileLabel == null)
                DayLabel = new Label();
            DayLabel.Size = new Size(25, 100);
            DayLabel.Location = new Point(MonthLabel.Size.Width + MonthLabel.Location.X + 30, (this.Size.Height / 2) - (DayLabel.Size.Height / 2));
            DayLabel.Text = Hour + "ч " + minute + "v";
            Controls.Add(DayLabel);
        }
        #endregion

        #region Информация где находитя человек НА ТЕРРИТОРИИ ИЛИ ОТСУТСТВУЕТ НА РАБОТЕ
        Label EnterExitLabel;
        public void EnterExitTimeLabel(string EnterExit)
        {
            if (EnterExit == "Вход" )
            {
                EnterExit = "На территории";
            }
            if (EnterExit == "Выход")
            {
                EnterExit = "Отсутствует";
            }
            if (EnterExitLabel == null)
                EnterExitLabel = new Label();
            EnterExitLabel.Size = new Size(25, 100);
            EnterExitLabel.Location = new Point(DayLabel.Size.Width + DayLabel.Location.X + 30, (this.Size.Height / 2) - (EnterExitLabel.Size.Height / 2));
            EnterExitLabel.Text = EnterExit;
            Controls.Add(EnterExitLabel);
        }
        #endregion

        #region Информация где находитя человек НА ТЕРРИТОРИИ ИЛИ ОТСУТСТВУЕТ НА РАБОТЕ
        Label KPPLabel;
        public void KPPTimeLabel(string KPP)
        {

            if (KPPLabel == null)
                KPPLabel = new Label();
            KPPLabel.Size = new Size(25, 100);
            KPPLabel.Location = new Point(EnterExitLabel.Size.Width + EnterExitLabel.Location.X + 30, (this.Size.Height / 2) - (KPPLabel.Size.Height / 2));
            KPPLabel.Text = KPP;
            Controls.Add(KPPLabel);
        }
        #endregion
    }
}
