using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 选择题选项
    /// </summary>
    public class OptionItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 标题
        /// </summary>
        private string _caption;

        /// <summary>
        /// 数量
        /// </summary>
        private int _qty;

        /// <summary>
        /// 宽度
        /// </summary>
        private int _width;

        /// <summary>
        /// 高度
        /// </summary>
        private int _height;

        /// <summary>
        /// 字体颜色
        /// </summary>
        private Brush _color;

        /// <summary>
        /// 背景颜色
        /// </summary>
        private Brush _backColor;

        /// <summary>
        /// 学生列表
        /// </summary>
        private List<QuickAnswerStudent> _students;

        /// <summary>
        /// 答案标题
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set { 
                _caption = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Caption"));
                }
            }
        }

        /// <summary>
        /// 选择该答案的学生数量
        /// </summary>
        public int Qty
        {
            get { return _qty; }
            set { 
                _qty = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Qty"));
                }
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { 
                _width = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                }
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { 
                _height = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Height"));
                }
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Brush Color { 
            get { return _color; } 
            set { 
                _color = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Color"));
                }
            } 
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Brush BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("BackColor"));
                }
            }
        }

        /// <summary>
        /// 提交答案的学生列表
        /// </summary>
        public List<QuickAnswerStudent> Students
        {
            get { return _students; }
            set { 
                _students = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Students"));
                }
            }
        }

    }
}
