using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // firstClicked指向第一个Label控件
        // 玩家单击的内容,但该内容将为空
        // 如果玩家还没有点击标签
        Label firstClicked = null;

        // secondClicked指向第二个标签控件
        // 玩家单击的内容
        Label secondClicked = null;


        // 使用这个随机对象为方块选择随机图标
        Random random = new Random();

        // 每个字母都是一个有趣的图标
        // 在Webdings字体中
        // 并且每个图标在列表中出现两次

        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };


        /// <summary>
        /// 将图标列表中的每个图标分配给一个随机方块
        /// </summary>
        private void AssignIconsToSquares()
        {
            // TableLayoutPanel有16个标签
            // 图标列表有16个图标
            // 所以从列表中随机抽取一个图标
            // 并添加到每个标签中
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        /// <summary>
        /// 每个标签的单击事件都由该事件处理程序处理
        /// </summary>
        /// <param name="sender">被单击的标签</param>
        /// <param name="e"></param>
        /// 

        private void label1_Click(object sender, EventArgs e)
        {
            //计时器仅在两次不匹配后开启
            //图标已经显示给玩家
            //因此，如果计时器正在运行，请忽略任何点击
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;
            if (clickedLabel != null)
            {
                //如果点击的标签是黑色的，则玩家点击了
                //一个已经曝光的图标
                //忽略点击
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                //如果firstClicked为空，这是第一个图标
                //在玩家点击的对子中
                //因此将firstClicked设置为播放器的标签
                //单击，将其颜色更改为黑色并返回
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }
                // 如果玩家玩到这一步，计时器不会
                // running和firstClicked不为null
                // 所以这一定是玩家点击的第二个图标
                // 将其颜色设置为黑色
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                //检查玩家是否获胜
                CheckForWinner();

                //如果玩家单击了两个匹配的图标，请保留它们
                //黑色并重置firstClicked和secondClicked
                //这样玩家可以单击另一个图标
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                //如果玩家到此为止，玩家单击了两个不同的图标
                //因此启动计时器（将等待四分之三的时间一秒钟，然后隐藏图标）
                timer1.Start();
            }
        }

        /// <summary>
        //////此计时器在玩家点击时启动
        //////两个不匹配的图标
        //////所以它计算四分之三秒
        //////然后自行关闭并隐藏两个图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //停止计时器
            timer1.Stop();

            //隐藏两个图标
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            //重置firstClicked和secondClicked
            //所以下一次标签被单击时，程序知道这是第一次单击


            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        //检查每个图标是否匹配，方法是将其前景色与其背景色进行比较。 
        //如果所有图标都匹配，玩家获胜
        /// </summary>
        private void CheckForWinner()
        {
            //浏览TableLayoutPanel中的所有标签
            //检查每个图标以查看其图标是否匹配
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            //如果循环没有返回，它就没有找到任何不匹配的图标
            //这意味着用户赢了。显示一条消息并关闭窗体
            MessageBox.Show("你匹配了所有图标！","恭喜");
            Close();
        }

    }
}
