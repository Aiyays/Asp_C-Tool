using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SnapLibrary;

namespace TestSnap
{
    class Program
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            //web 页面快照
            WebPageSnapshot wps = new WebPageSnapshot();

            if (args != null && args.Length > 1)
                wps.Url = args[0];
            else
                wps.Url = "http://www.cnblogs.com";

            try
            {
                //保存到文件
                wps.TakeSnapshot().Save("1.bmp");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            wps.Dispose();
        }


    }

    /// <summary>
    /// web 页面快照类
    /// </summary>
    public class WebPageSnapshot : IDisposable
    {
        string url = "about:blank";

        /// <summary>
        /// 简单构造一个 WebBrowser 对象
        /// 更灵活的应该是直接引用浏览器的com对象实现稳定控制
        /// </summary>
        WebBrowser wb = new WebBrowser();
        /// <summary>
        /// URL 地址
        /// http://www.cnblogs.com
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        int width = 1024;
        /// <summary>
        /// 图象宽度
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        int height = 768;
        /// <summary>
        /// 图象高度
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void InitComobject()
        {
            try
            {
                wb.ScriptErrorsSuppressed = false;
                wb.ScrollBarsEnabled = false;
                wb.Size = new Size(1024, 768);
                wb.Navigate(this.url);
                //因为没有窗体，所以必须如此
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                    System.Windows.Forms.Application.DoEvents();
                wb.Stop();
                if (wb.ActiveXInstance == null)
                    throw new Exception("实例不能为空");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取快照
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap TakeSnapshot()
        {
            try
            {
                InitComobject();
                //构造snapshot类，抓取浏览器ActiveX的图象
                SnapLibrary.Snapshot snap = new SnapLibrary.Snapshot();
                return snap.TakeSnapshot(wb.ActiveXInstance, new Rectangle(0, 0, this.width, this.height));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void Dispose()
        {
            wb.Dispose();
        }

    }
}
