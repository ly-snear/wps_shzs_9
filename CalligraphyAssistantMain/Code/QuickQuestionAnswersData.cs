using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 答题界面
    /// </summary>
    public class QuickQuestionAnswersData: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _caption;
        private Visibility _selectSubject;
        private Visibility _subjectiveSubject;
        private string _title;
        private string _subjectiveQuestionAnswer;
        private int _selectAnswerBarHeight;
        private string _audioUrl;
        private int _row;
        private int _col;

        /// <summary>
        /// 答题界面UI
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
        /// 显示选择题
        /// </summary>
        public Visibility SelectSubject
        {
            get { return _selectSubject; }
            set { 
                _selectSubject = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SelectSubject"));
                }
            }
        }

        /// <summary>
        /// 显示主观题
        /// </summary>
        public Visibility SubjectiveSubject
        {
            get { return _subjectiveSubject; }
            set
            {
                _subjectiveSubject = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SubjectiveSubject"));
                }
            }
        }

        /// <summary>
        /// 题目标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Title"));
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
        /// 柱状图高度
        /// </summary>
        public int SelectAnswerBarHeight
        {
            get { return _selectAnswerBarHeight; }
            set
            {
                _selectAnswerBarHeight = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SelectAnswerBarHeight"));
                }
            }
        }

        /// <summary>
        /// 主观题声音答案地址
        /// </summary>
        public string AudioUrl
        {
            get { return _audioUrl; }
            set
            {
                _audioUrl = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("AudioUrl"));
                }
            }
        }

        /// <summary>
        /// 主观题坐位行数
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
        /// 主观题坐位列数
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

    }
}
