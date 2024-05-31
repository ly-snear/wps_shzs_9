using CalligraphyAssistantMain.Controls.works;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CalligraphyAssistantMain.Code
{
    public class EventNotify
    {
        public static event EventHandler CheckWorksClick = null;
        public static event EventHandler MaterialSendClick = null;
        public static event EventHandler MaterialCloseClick = null;
        /// <summary>
        /// 关闭拼图统计
        /// </summary>
        public static event EventHandler MaterialCountCloseClick = null;
        /// <summary>
        /// 关闭任务统计
        /// </summary>
        public static event EventHandler TaskCountCloseClick = null;
        public static event EventHandler<long> StudentCompleteMessage = null;
        /// <summary>
        /// 打开分享页面
        /// </summary>
        public static event EventHandler SendCopyBookClick = null;
        /// <summary>
        /// 关闭分享页面
        /// </summary>
        public static event EventHandler SendCopyBookCloseClick = null;
        /// <summary>
        /// 打开备课文件选择字帖
        /// </summary>
        public static event EventHandler CopyBookPrepareLessonOpenClick = null;
        /// <summary>
        /// 关闭备课文件选择字帖
        /// </summary>
        public static event EventHandler CopyBookPrepareLessonCloseClick = null;
        /// <summary>
        /// 打开虎妞单字选择字帖
        /// </summary>
        public static event EventHandler CopyBookTigerRordOpenClick = null;
        /// <summary>
        /// 关闭虎妞单字选择字帖
        /// </summary>
        public static event EventHandler CopyBookTigerRordCloseClick = null;
        /// <summary>
        /// 打开虎妞单字碑帖字帖
        /// </summary>
        public static event EventHandler CopyBookTigerOpenClick = null;
        /// <summary>
        /// 关闭虎妞单字选择字帖
        /// </summary>
        public static event EventHandler CopyBookTigerCloseClick = null;

        /// <summary>
        /// 打开快速问答答题界面
        /// </summary>
        public static event EventHandler<QuickAnswerInfo> QuickQuestionAnswersOpenClick = null;

        /// <summary>
        /// 关闭快速问答答题界面
        /// </summary>
        public static event EventHandler QuickQuestionAnswersCloseClick = null;

        /// <summary>
        /// 打开学生作品互评
        /// </summary>
        public static event EventHandler<StudentWorkDetailsInfo> StudentWorkMutualCommenOpenClick = null;

        /// <summary>
        /// 关闭学生作品互拼
        /// </summary>
        public static event EventHandler StudentWorkMutualCommentCloseClick = null;

        public static event Action<StudentWorkDetailsInfo, string> CheckShareListClick = null;
        public static event Action<List<StudentWorkDetailsInfo>> CheckWorksContrastClick = null;
        public static event Action<List<StudentWorkDetailsInfo>> CheckCommentListClick = null;
        public static event Action<StudentWorkDetailsInfo, ShareInfo, string> CheckShareItemClick = null;
        public static event Action<CameraItemInfo> CheckCameraGroup = null;
        public static event Action<CameraItemInfo, StudentInfo> CheckCamera = null;
        public static event Action<int, Action> CountdownTrigger = null;
        public static event Action<int> CountdownStopTrigger = null;
        public static event Action<string> ShowDocEvent = null;
        public static event Action CameraContrastEvent = null;
        /// <summary>
        /// 关闭任务统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnTaskCountClose(object sender, EventArgs e)
        {
            TaskCountCloseClick?.Invoke(sender, e);
        }
        public static void OnCheckWorks(object sender, EventArgs e)
        {
            CheckWorksClick?.Invoke(sender, e);
        }
        public static void OnMaterialSend(object sender, EventArgs e)
        {
            MaterialSendClick?.Invoke(sender, e);
        }
        public static void OnMaterialClose(object sender, EventArgs e)
        {
            MaterialCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 拼图统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnMaterialCountClose(object sender, EventArgs e)
        {
            MaterialCountCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 接收学生提交的消息
        /// 完成作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        public static void OnStudentComplete(object sender, long id)
        {
            StudentCompleteMessage?.Invoke(sender, id);
        }
        public static void OnSendCopyBook(object sender, EventArgs e)
        {
            SendCopyBookClick?.Invoke(sender, e);
        }
        public static void OnSendCopyBookClose(object sender, EventArgs e)
        {
            SendCopyBookCloseClick?.Invoke(sender, e);
        }
        /// <summary>
        /// 打开从备课文件中选择字帖框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnCopyBookPrepareLessonOpen(object sender, EventArgs e)
        {
            CopyBookPrepareLessonOpenClick?.Invoke(sender, e);
        }
        public static void OnCopyBookPrepareLessonClose(object sender, EventArgs e)
        {
            CopyBookPrepareLessonCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 打开虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnCopyBookTigerRordOpen(object sender, EventArgs e)
        {
            CopyBookTigerRordOpenClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 关闭虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnCopyBookTigerRordClose(object sender, EventArgs e)
        {
            CopyBookTigerRordCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 打开虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnCopyBookTigerOpen(object sender, EventArgs e)
        {
            CopyBookTigerOpenClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 关闭虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnCopyBookTigerClose(object sender, EventArgs e)
        {
            CopyBookTigerCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 显示快速问题答题界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnQuickQuestionAnswersOpen(object sender, QuickAnswerInfo e)
        {
            QuickQuestionAnswersOpenClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 关闭快速问题答题界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnQuickQuestionAnswersClose(object sender, EventArgs e)
        {
            QuickQuestionAnswersCloseClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 打开学生作品互评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnStudentWorkMutualCommenOpen(object sender, StudentWorkDetailsInfo e)
        {
            //System.Windows.MessageBox.Show("执行");
            StudentWorkMutualCommenOpenClick?.Invoke(sender, e);
        }

        /// <summary>
        /// 关闭学生作品互评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnStudentWorkMutualCommentClose(object sender, EventArgs e)
        {
            StudentWorkMutualCommentCloseClick?.Invoke(sender, e);
        }

        public static void OnCheckShareList(StudentWorkDetailsInfo info, string type)
        {
            CheckShareListClick?.Invoke(info, type);
        }
        public static void OnCheckShareItem(StudentWorkDetailsInfo workInfo, ShareInfo info, string type)
        {
            CheckShareItemClick?.Invoke(workInfo, info, type);
        }
        public static void OnCheckWorksContrastClick(List<StudentWorkDetailsInfo> data)
        {
            CheckWorksContrastClick?.Invoke(data);
        }
        public static void OnCheckCameraGroup(CameraItemInfo info)
        {
            CheckCameraGroup?.Invoke(info);
        }
        public static void OnCheckCamera(CameraItemInfo camera, StudentInfo info)
        {
            CheckCamera?.Invoke(camera, info);
        }
        public static void OnCountdownTrigger(int id, Action action)
        {
            CountdownTrigger?.Invoke(id, action);
        }
        public static void OnCountdownStopTrigger(int id)
        {
            CountdownStopTrigger?.Invoke(id);
        }
        public static void OnShowDocEvent(string filePath)
        {
            ShowDocEvent?.Invoke(filePath);
        }
        public static void OnCameraContrastEvent()
        {
            CameraContrastEvent?.Invoke();
        }
        public static void OnCheckCommentListClick(List<StudentWorkDetailsInfo> data)
        {
            CheckCommentListClick?.Invoke(data);
        }
    }
}
