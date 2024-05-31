using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CalligraphyAssistantMain.Code
{
    public class QuickAnswerStudent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private long _id;
        private string _name;
        private string _caption;
        private long _startTime;
        private long _endTime;
        private long _useTime;
        private int _row;
        private int _col;
        private Brush _color;
        private Brush _captionColor;
        private Brush _nameColor;
        private Brush _backColor;
        private Brush _captionBackColor;
        private Brush _nameBackColor;
        private Brush _borderColor;
        private Boolean _isChecked;
        private Visibility _visibled;
        private string _subjectiveQuestionAnswer;
        private string _subjectiveAudioUrl;
        private string _comment;
        private decimal _score;

        /// <summary>
        /// 学生ID
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { 
                _id = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }

        /// <summary>
        /// 开始答题时间
        /// </summary>
        public long StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("StartTime"));
                }
            }
        }

        /// <summary>
        /// 结束答题时间
        /// </summary>
        public long EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                }
            }
        }

        /// <summary>
        /// 答题总用时
        /// </summary>
        public long UseTime
        {
            get { return _useTime; }
            set
            {
                _useTime = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("UseTime"));
                }
            }
        }

        /// <summary>
        /// 所在行
        /// </summary>
        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Row"));
                }
            }
        }

        /// <summary>
        /// 所在列
        /// </summary>
        public int Col
        {
            get { return _col; }
            set
            {
                _col = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Col"));
                }
            }
        }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        /// <summary>
        /// 选择的答案
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Caption"));
                }
            }
        }

        /// <summary>
        /// 前景色
        /// </summary>
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Color"));
                }
            }
        }

        /// <summary>
        /// 选择的答案前景色
        /// </summary>
        public Brush CaptionColor
        {
            get { return _captionColor; }
            set
            {
                _captionColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CaptionColor"));
                }
            }
        }

        /// <summary>
        /// 学生名字前景色
        /// </summary>
        public Brush NameColor
        {
            get { return _nameColor; }
            set
            {
                _nameColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NameColor"));
                }
            }
        }

        /// <summary>
        /// 控件背景色
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
        /// 选择答案的背景色
        /// </summary>
        public Brush CaptionBackColor
        {
            get { return _captionBackColor; }
            set
            {
                _captionBackColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CaptionBackColor"));
                }
            }
        }

        /// <summary>
        /// 学生姓名的背景色
        /// </summary>
        public Brush NameBackColor
        {
            get { return _nameBackColor; }
            set
            {
                _nameBackColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("NameBackColor"));
                }
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Brush BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("BorderColor"));
                }
            }
        }
                
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set { 
                _isChecked = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public Visibility Visibled
        {
            get { return _visibled; }
            set { 
                _visibled = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Visibled"));
                }
            }
        }

        /// <summary>
        /// 主观题答案
        /// </summary>
        public string SubjectiveQuestionAnswer
        {
            get { return _subjectiveQuestionAnswer; }
            set
            {
                _subjectiveQuestionAnswer = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SubjectiveQuestionAnswer"));
                }
            }
        }

        /// <summary>
        /// 主观题学生提交的声音地址
        /// </summary>
        public string SubjectiveAudioUrl
        {
            get { return _subjectiveAudioUrl; }
            set
            {
                _subjectiveAudioUrl = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SubjectiveAudioUrl"));
                }
            }
        }


        /// <summary>
        /// 评语
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { 
                _comment = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Comment"));
                }
            }
        }

        /// <summary>
        /// 得分
        /// </summary>
        public decimal Score
        {
            get { return _score; }
            set { 
                _score = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }
    }
}
