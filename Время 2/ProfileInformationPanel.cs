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
    public partial class ProfileInformationPanel : UserControl
    {
        public ProfileInformationPanel()
        {
            InitializeComponent();
        }

        private void ProfileInformationPanel_Load(object sender, EventArgs e)
        {
            this.Size = new Size(550, 150);

        }

        PersonInfo[] persouinfo = new PersonInfo[0];
        string[] PersonName = new string[0];

        public void personinfo(string personname,string[] MonthTime, string[] DayTime, string EnterExit, string KPP, int index)
        {
            if (PersonName.Length == 0)
            {
                Array.Resize(ref PersonName, PersonName.Length + 1);
                PersonName[PersonName.Length - 1] = personname;

                Array.Resize(ref persouinfo, persouinfo.Length + 1);
                persouinfo[persouinfo.Length - 1] = new PersonInfo();
                int[] location = LocationGet(index);
                persouinfo[persouinfo.Length - 1].Location = new Point(location[0], location[0]);
                Controls.Add(persouinfo[persouinfo.Length - 1]);

                persouinfo[persouinfo.Length - 1].ProfileNames(personname);
                persouinfo[persouinfo.Length - 1].MonthTimeLabel(MonthTime[0], MonthTime[1]);
                persouinfo[persouinfo.Length - 1].DayTimeLabel(DayTime[0], DayTime[1]);
                persouinfo[persouinfo.Length - 1].EnterExitTimeLabel(EnterExit);
                persouinfo[persouinfo.Length - 1].KPPTimeLabel(KPP);

            }
            else
            {
                int indexperson = -1;
                for (int i = 0; i < PersonName.Length;i++)
                {
                    if (personname == PersonName[i]) ;
                    {
                        indexperson = i;
                    }
                }

                if (indexperson == -1)
                {
                    Array.Resize(ref PersonName, PersonName.Length + 1);
                    PersonName[PersonName.Length - 1] = personname;

                    Array.Resize(ref persouinfo, persouinfo.Length + 1);
                    persouinfo[persouinfo.Length - 1] = new PersonInfo();
                    int[] location = LocationGet(index);
                    persouinfo[persouinfo.Length - 1].Location = new Point(location[0], location[0]);
                    Controls.Add(persouinfo[persouinfo.Length - 1]);

                    persouinfo[indexperson].ProfileNames(personname);
                    persouinfo[indexperson].MonthTimeLabel(MonthTime[0], MonthTime[1]);
                    persouinfo[indexperson].DayTimeLabel(DayTime[0], DayTime[1]);
                    persouinfo[indexperson].EnterExitTimeLabel(EnterExit);
                    persouinfo[indexperson].KPPTimeLabel(KPP);
                }
                else
                {
                    persouinfo[indexperson].ProfileNames(personname);
                    persouinfo[indexperson].MonthTimeLabel(MonthTime[0], MonthTime[1]);
                    persouinfo[indexperson].DayTimeLabel(DayTime[0], DayTime[1]);
                    persouinfo[indexperson].EnterExitTimeLabel(EnterExit);
                    persouinfo[indexperson].KPPTimeLabel(KPP);
                }
            }
        }

        private int[] LocationGet(int index)
        {

            int[] Location = new int[] { 10 , 0 };
            if (index == 0)
            {
                this.Size = new Size(this.Size.Width, this.Size.Height + 50);
                Location[1] = this.Size.Height - 50;
            }
            else
            {
                this.Size = new Size(this.Size.Width, this.Size.Height + 80);
                Location[1] = this.Size.Height - 80;
            }
            return Location;
        }

    }
}
