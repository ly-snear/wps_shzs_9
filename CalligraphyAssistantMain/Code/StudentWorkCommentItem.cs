using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 学生作品评价条目
    /// </summary>
    public class StudentWorkCommentItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private long _id;
        private long _pid;
        private long _id_work;
        private string _title_work;
        private string _url_work;
        private int _type;
        private string _name_type;
        private long _id_comment;
        private string _name_comment;
        private string _title;
        private string _content;
        private string _image;
        private string _audio;
        private string _video;
        private decimal _score;
        private int _grade;
        private decimal _star;
        private int _self;
        private string _time;

        /// <summary>
        /// 点评ID
        /// </summary>
        public long id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("id"));
                }
            }
        }

        /// <summary>
        /// 关联点评ID
        /// 形成点评树
        /// </summary>
        public long pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("pid"));
                }
            }
        }

        /// <summary>
        /// 作品ID
        /// </summary>
        public long id_work
        {
            get { return _id_work; }
            set { 
                _id_work = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("id_work"));
                }
            }
        }

        /// <summary>
        /// 作品名称
        /// </summary>
        public string title_work
        {
            get { return _title_work; }
            set { 
                _title_work = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("title_work"));
                }
            }
        }

        /// <summary>
        /// 作品地址
        /// </summary>
        public string url_work
        {
            get { return _url_work; }
            set
            {
                _url_work = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("url_work"));
                }
            }
        }

        /// <summary>
        /// 点评人类型
        /// 1:学生 2:教师
        /// </summary>
        public int type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("type"));
                }
            }
        }

        /// <summary>
        /// 点评人类型名称
        /// </summary>
        public string name_type
        {
            get { return _name_type; }
            set
            {
                _name_type = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("name_type"));
                }
            }
        }

        /// <summary>
        /// 发表评论的学生或者教师ID
        /// </summary>
        public long id_comment
        {
            get { return _id_comment; }
            set
            {
                _id_comment = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("id_comment"));
                }
            }
        }

        /// <summary>
        /// 点评人姓名
        /// </summary>
        public string name_comment
        {
            get { return _name_comment; }
            set
            {
                _name_comment = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("name_comment"));
                }
            }
        }

        /// <summary>
        /// 点评标题
        /// </summary>
        public string title
        {
            get { return _title; }
            set {
                _title = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("title"));
                }
            }
        }

        /// <summary>
        /// 点评内容
        /// </summary>
        public string content
        {
            get { return _content; }
            set
            {
                _content = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("content"));
                }
            }
        }

        /// <summary>
        /// 点评图片
        /// </summary>
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("image"));
                }
            }
        }

        /// <summary>
        /// 点评声音地址
        /// </summary>
        public string audio
        {
            get { return _audio; }
            set
            {
                _audio = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("audio"));
                }
            }
        }

        /// <summary>
        /// 点评视频地址
        /// </summary>
        public string video
        {
            get { return _video; }
            set
            {
                _video = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("video"));
                }
            }
        }

        /// <summary>
        /// 点评积分
        /// </summary>
        public decimal score
        {
            get { return (decimal)_score; }
            set { 
                _score= value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("score"));
                }
            }
        }

        /// <summary>
        /// 点评等级
        /// </summary>
        public int grade
        {
            get { return _grade; }
            set { 
                _grade = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("grade"));
                }
            }
        }

        /// <summary>
        /// 点评得星
        /// </summary>
        public decimal star
        {
            get { return _star; }
            set { 
                _score= value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("star"));
                }
            }
        }

        /// <summary>
        /// 是否自己的点评
        /// 0:不是
        /// 1:是
        /// </summary>
        public int self
        {
            get { return _self; }
            set
            {
                _self = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("self"));
                }
            }
        }

        /// <summary>
        /// 点评时间
        /// </summary>
        public string time
        {
            get { return _time; }
            set
            {
                _time = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("time"));
                }
            }
        }
    }
}
