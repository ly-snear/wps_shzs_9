using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class MQMessageEvent : PubSubEvent<Message>
    {
    }
    public class Message
    {
        public int classId { get; set; }
        public int lessonId { get; set; }
        public int classRoomId { get; set; }
        public int sendUserId { get; set; }
        public MessageType type { get; set; }
        public UserType userType { get; set; }
        public object data { get; set; }

    }

    public enum MessageType
    {
        Unknown = -1,
        ShareScreen = 0,
        StartVote = 1,
        PublishVote = 2,
        StopVote = 3,

        /// <summary>
        /// 快速问题.下发
        /// </summary>
        StartQuickAnswer = 4,

        /// <summary>
        /// 快速问题.答题时间到
        /// </summary>
        StopQuickAnswer = 5,

        /// <summary>
        /// 快速问题.公布答案
        /// </summary>
        PublishQuickAnswer = 6,

        FileDistribute = 7,
        PaperDistribute = 8,
        ControlOperation = 9,
        WorkShare = 10,
        StartLesson = 11,
        StopLesson = 12,
        LockScreen = 13,
        ReminderPosture = 14,        //提醒坐姿 
        RandomlySelected = 15,      //随机抽人 
        Praise = 16,               //表扬
        RollCall = 17,            //点名
        StopPaper = 18,


        CompleteVote = 20,
        CompleteQuickAnswer = 21,
        PaperAccept = 22,
        FileAccept = 23,
        SubmitPaper = 24,
        HandUp = 25,
        AskQuestion = 26,

        /// <summary>
        /// 白屏消息
        /// </summary>
        WhiteScreen = 27,

        /// <summary>
        /// 教师分享桌面
        /// </summary>
        ShareDesk = 28,

        /// <summary>
        /// 分享素材
        /// 文档、视频、图片等
        /// </summary>
        ShareMaterial = 29,

        /// <summary>
        /// 分享沙画
        /// 图片
        /// </summary>
        ShareSandDraw = 30,

        /// <summary>
        /// 学生完成学习任务
        /// </summary>
        StudentComplete = 31,

        /// <summary>
        /// 分享字帖
        /// </summary>
        ShareCopyBook = 32,

        /// <summary>
        /// 分享演示台
        /// </summary>
        ShareDemo = 33,

        /// <summary>
        /// 分享教室后面挂图
        /// </summary>
        ShareClassRoomBack = 34,

        /// <summary>
        /// 学生端提交快速问题消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":35,"userType":1,"data":{"answer": "A","start": 1643825046,"elapsed": 34,"type":0, "style":0,"id": 398,"student": 2854,"name": "张三"}}
        /// {"classId":80,"lessonId":1027,"classRoomId":19,"sendUserId":2854,"type":35,"userType":1,"data":{"answer": "我的问题", "audio":"https://video.nnyun.net/wav/OSR_cn_000_0075_8k.wav", "start": 1643825046,"elapsed": 34,"type":1, "style":2,"id": 398,"student": 2854,"name": "张三"}}
        /// </summary>
        CommitQuickQuestionAnswer = 35,

        /// <summary>
        /// 下发消息快速问答题已经完成抢答
        /// 仅发送给消息提交者
        /// </summary>
        FirstAnswerAlreadyComplete = 36,

        /// <summary>
        /// 下发消息快速问答题已经完成抢答
        /// 发送给除开答题者外的所有学生
        /// </summary>
        FirstAnswerComplete = 37,

        /// <summary>
        /// 下发消息学生作品互评.开始
        /// 仅发送给参与互评的学生
        /// </summary>
        StudentWorkMutualCommentStart = 38,

        /// <summary>
        /// 下发消息学生作品互评.新消息
        /// 仅发送给参与互评的学生.本身除外
        /// 文本消息
        /// </summary>
        StudentWorkMutualCommentNew = 39,

        /// <summary>
        /// 接收学生端发送评论消息
        /// 文本消息
        /// {"classId":80,"lessonId":1051,"classRoomId":19,"sendUserId":75,"type":40,"userType":1,"data":{"id":9,"pid":0,"id_work":259,"title_work":"","url_work":"http://video.nnyun.net/wxmini/2853/书画助手.png","type":1,"name_type":"学生","id_comment":2861,"name_comment":"谭雅","title":"很喜欢","content":"很喜欢","audio":"","video":"","score":2.0,"grade":2,"star":0,"self":0,"time":"2024-02-22 09:33:08"}}
        /// </summary>
        StudentWorkMutualCommentRX = 40,

        /// <summary>
        /// 接收学生端发送评论消息
        /// 图片消息
        /// {"classId":80,"lessonId":1051,"classRoomId":19,"sendUserId":75,"type":40,"userType":1,"data":{"id":9,"pid":0,"id_work":259,"title_work":"","url_work":"http://video.nnyun.net/wxmini/2853/书画助手.png","type":1,"name_type":"学生","id_comment":2861,"name_comment":"谭雅","title":"很喜欢","content":"很喜欢","audio":"","video":"","score":2.0,"grade":2,"star":0,"self":0,"time":"2024-02-22 09:33:08"}}
        /// </summary>
        StudentWorkMutualCommentRXImage = 41,

        /// <summary>
        /// 接收学生端发送评论消息
        /// 图片消息
        /// {"classId":80,"lessonId":1051,"classRoomId":19,"sendUserId":75,"type":40,"userType":1,"data":{"id":9,"pid":0,"id_work":259,"title_work":"","url_work":"http://video.nnyun.net/wxmini/2853/书画助手.png","type":1,"name_type":"学生","id_comment":2861,"name_comment":"谭雅","title":"很喜欢","content":"很喜欢","audio":"","video":"","score":2.0,"grade":2,"star":0,"self":0,"time":"2024-02-22 09:33:08"}}
        /// </summary>
        StudentWorkMutualCommentRXVoice = 42,

        /// <summary>
        /// 接收学生端发送评论消息
        /// 打分消息
        /// {"classId":80,"lessonId":1051,"classRoomId":19,"sendUserId":75,"type":40,"userType":1,"data":{"id":9,"pid":0,"id_work":259,"title_work":"","url_work":"http://video.nnyun.net/wxmini/2853/书画助手.png","type":1,"name_type":"学生","id_comment":2861,"name_comment":"谭雅","title":"很喜欢","content":"很喜欢","audio":"","video":"","score":2.0,"grade":2,"star":0,"self":0,"time":"2024-02-22 09:33:08"}}
        /// </summary>
        StudentWorkMutualCommentRXScore = 43,

        /// <summary>
        /// 学生端提交拼图完成消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":42,"userType":1,"data":{"url": "https://www.cctv.com/image/abcd.jpg","start": 1643825046,"elapsed": 34, "student": 2854,"name": "张三"}}        
        /// </summary>
        CommitMaterialComplete = 44,

        /// <summary>
        /// 下发拼图倒计时结束消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":43,"userType":0,"data":{}}        
        /// </summary>
        CommitMaterialStop = 45,

        /// <summary>
        /// 下发学习任务开始消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":44,"userType":0,"data":{}}        
        /// </summary>
        CommitTaskStart = 46,

        /// <summary>
        /// 学生端提交学习任务完成消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":44,"userType":1,"data":{"start": 1643825046,"elapsed": 34, "student": 2854,"name": "张三"}}        
        /// </summary>
        CommitTaskComplete = 47,

        /// <summary>
        /// 下发学习任务倒计时消息
        /// {"classId":80,"lessonId":991,"classRoomId":19,"sendUserId":2854,"type":45,"userType":0,"data":{}}        
        /// </summary>
        CommitTaskStop = 48,

        /// <summary>
        /// 下发消息学生作品互评.结束
        /// 仅发送给参与互评的学生
        /// </summary>
        StudentWorkMutualCommentEnd = 49,

        /// <summary>
        /// 一号位1号摄像头
        /// </summary>
        ShareUsbCamera = 10001,
    }
    public enum UserType
    {
        Unknown = -1,
        teacher = 0,
        student = 1,
    }

}
