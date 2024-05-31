using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CalligraphyAssistantMain.Controls.fivePoint
{
    public class FivePointStarModel : NotifyObject
    {
        private int id;


        private double radius = 20;


        private double currentValue = 1;


        private Brush selectBackground = new SolidColorBrush(Colors.GreenYellow);


        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);


        private Thickness margins = new Thickness(0);


        public int ID
        {
            get { return id; }


            set
            {
                id = value;


                this.OnPropertyChanged("Radius");
            }
        }


        public double Radius
        {
            get { return radius; }


            set
            {
                radius = value;


                this.OnPropertyChanged("Radius");
            }
        }


        public double CurrentValue
        {
            get { return currentValue; }


            set
            {
                currentValue = value;


                this.OnPropertyChanged("CurrentValue");
            }
        }


        public Brush SelectBackground
        {
            get { return selectBackground; }


            set
            {
                selectBackground = value;


                this.OnPropertyChanged("SelectBackground");
            }
        }


        public Brush UnselectBackgroud
        {
            get { return unselectBackgroud; }


            set
            {
                unselectBackgroud = value;


                this.OnPropertyChanged("UnselectBackgroud");
            }
        }




        public Thickness Margins
        {
            get { return margins; }


            set
            {
                margins = value;


                this.OnPropertyChanged("Radius");
            }
        }
    }


    public abstract class NotifyObject : INotifyPropertyChanged
    {


        public void OnPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
