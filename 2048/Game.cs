using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    [Serializable]
    class Game
    {
        #region ========================初始设置========================
        /// <summary>
        /// 使用6*6数组的原因是可以简化避免监测是否Game Over时的数组索引报错
        /// </summary>
        public int[,] i = new int[6, 6];

        /// <summary>
        /// 存储当前成绩
        /// </summary>
        public int grade { get; set; }

        /// <summary>
        /// 存储最好成绩  
        /// </summary>
 
        public int bestGrade { get; set; }

        /// <summary>
        /// 当前有多少个方块不为0
        /// </summary>
        private int quantity { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        private Random ra = new Random();

        /// <summary>
        /// 是否Game Over
        /// </summary>
        public bool die = false;

        /// <summary>
        /// 按下某个方向键后会记录方块可否移动        
        /// </summary>
        public bool change = false;
        #endregion

        #region ========================游戏算法========================
        /// <summary>
        /// 游戏刷新，重新开始
        /// </summary>
        public void Reset()
        {
            //遍历数组中X的元素，从0开始，到5结束
            for (int x = 0; x <= 5; x++)
            {
                //遍历数组中Y的元素，从0开始，到5结束
                for (int y = 0; y <= 5; y++)
                {
                    i[x, y] = 0;
                }
            }
            quantity = 0;
            die = false;
            //如果当前成绩大于最好成绩
            if (grade > bestGrade)
            {
                //把当前成绩值赋值给最好成绩
                bestGrade = grade;
            }
            grade = 0;
            Add();
            Add();
        }

        /// <summary>
        /// 添加一个元素
        /// 采用递归方法，随机选出一个元素，如果这个元素不为0，那就继续随机增加一个，直到成功
        /// </summary>
        public void Add()
        {
            int x = ra.Next(1, 5);
            int y = ra.Next(1, 5);
            if (i[x, y] == 0)
            {
                if (ra.Next(1, 101) >= 90)
                {
                    i[x, y] = 4;
                }

                else
                {
                    i[x, y] = 2;
                }
                quantity++;
                Die();
            }
            else
            { 
                Add();
            }
        }

        /// <summary>
        /// 获得当前有多少个方块不为0
        /// </summary>
        private void GetQuantity()
        {
            int count = 0;
            //遍历x中的元素，从1开始，4结束
            for (int x = 1; x <= 4; x++)
            {
                //遍历y中的元素，从1开始，4结束
                for (int y = 1; y <= 4; y++)
                {
                    //如果数组中不为空
                    if (i[x, y] != 0)
                    { 
                        count++;
                    }
                }
            }
            quantity = count;
        }                   

        /// <summary>
        /// 是否为game over
        /// 获得当前方块数，如果是16，再检测互不相邻的8个方块与自身周围的方块是否相等，如果都不相等，则Game Over
        /// </summary>
        private void Die()
        {
            int count = 0;
            //当前方块数量等于16
            if (quantity == 16)
            {
                //遍历x中的元素，从1开始,到3结束
                for (int x = 1; x <= 3; x += 2)
                {
                    //遍历y中的元素，从1开始，到3结束
                    for (int y = 1; y <= 3; y += 2)
                    {
                        if (!GetEqual(x, y))//判断相邻元素是否相等
                        {
                            count++;
                        }
                    }
                }
                for (int x = 2; x <= 4; x += 2)//遍历x中的元素，从2开始，到4结束
                {
                    for (int y = 2; y <= 4; y += 2)//遍历y中的元素，从2开始，到4结束
                    {
                        if (!GetEqual(x, y))//判断相邻的元素是否相等
                        {
                            count++;
                        }
                    }
                }
                if (count == 8)
                {
                    die = true;
                }
            }
        }                           

        /// <summary>
        /// 相邻元素是否存在并且相等
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns>T/F</returns>
        private bool GetEqual(int x, int y)
        {
            //左移一位，如果有元素
            if (i[x, y] == i[x - 1, y])
            {
                return true;//返回true
            }
            //右移一位，如果有元素
            else if (i[x, y] == i[x + 1, y])
            {
                return true;
            }
            //上移一位，如果有元素
            else if (i[x, y] == i[x, y - 1])
            {
                return true;
            }
            //下移一位，如果有元素
            else if (i[x, y] == i[x, y + 1])
            {
                return true;
            }
            else
            { 
                return false; 
            }

        }          
        #endregion

        #region ========================方向动作========================

        /// <summary>
        /// 方向键上操作
        /// </summary>
        public void Up()
        {
            change = false;
            up();
            for (int x = 1; x <= 4; x++)
            {
                /*
                 * 对所有纵坐标进行赋值操作，已达到向上移动的效果 
                 */
                if (i[x, 1] == i[x, 2] && i[x, 1] + i[x, 2] != 0)
                {
                    if (i[x, 3] == i[x, 4])
                    {
                        i[x, 1] *= 2;
                        i[x, 2] = i[x, 3] * 2;
                        i[x, 3] = 0;
                        i[x, 4] = 0;
                        grade += i[x, 1] + i[x, 2];
                    }
                    else
                    {
                        i[x, 1] *= 2;
                        i[x, 2] = i[x, 3];
                        i[x, 3] = i[x, 4];
                        i[x, 4] = 0;
                        grade += i[x, 1];
                    }
                    change = true;
                }
                else if (i[x, 2] == i[x, 3] && i[x, 2] + i[x, 3] != 0)
                {
                    i[x, 2] *= 2;
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    grade += i[x, 2];
                }
                else if (i[x, 3] == i[x, 4] && i[x, 3] + i[x, 4] != 0)
                {
                    i[x, 3] *= 2;
                    i[x, 4] = 0;
                    change = true;
                    grade += i[x, 3];
                }
            }
            GetQuantity();
        }

        /// <summary>
        /// 方向键下键操作
        /// </summary>
        public void Down()
        {
            change = false;
            down();
            for (int x = 1; x <= 4; x++)
            {
                /*
                 * 对所有纵坐标进行赋值操作，已达到向下移动的效果
                 */
                if (i[x, 4] == i[x, 3] && i[x, 4] + i[x, 3] != 0)//确定范围
                {
                    if (i[x, 2] == i[x, 1])
                    {
                        i[x, 4] *= 2;
                        i[x, 3] = i[x, 2] * 2;
                        i[x, 2] = 0;
                        i[x, 1] = 0;
                        grade += i[x, 4] + i[x, 3];
                    }
                    else
                    {
                        i[x, 4] *= 2;
                        i[x, 3] = i[x, 2];
                        i[x, 2] = i[x, 1];
                        i[x, 1] = 0;
                        grade += i[x, 4];
                    }
                    change = true;
                }
                else if (i[x, 3] == i[x, 2] && i[x, 3] + i[x, 2] != 0)//确定范围
                {
                    i[x, 3] *= 2;
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                    grade += i[x, 3];
                }
                else if (i[x, 2] == i[x, 1] && i[x, 2] + i[x, 1] != 0)//确定范围
                {
                    i[x, 2] *= 2;
                    i[x, 1] = 0;
                    change = true;
                    grade += i[x, 2];
                }
            }
            GetQuantity();
        }

        /// <summary>
        /// 方向键左操作
        /// </summary>
        public void Left()
        {
            change = false;
            left();
            for (int y = 1; y <= 4; y++)
            {
                /*
                 * 对所有横坐标进行赋值操作，已达到向左移动的效果 
                 */
                if (i[1, y] == i[2, y] && i[1, y] + i[2, y] != 0)//确定范围
                {
                    if (i[3, y] == i[4, y])
                    {
                        i[1, y] *= 2;
                        i[2, y] = i[3, y];
                        i[3, y] = 0;
                        i[4, y] = 0;
                        grade += i[1, y] + i[2, y];

                    }
                    else
                    {
                        i[1, y] *= 2;
                        i[2, y] = i[3, y];
                        i[3, y] = i[4, y];
                        i[4, y] = 0;
                        grade += i[1, y];
                    }
                    change = true;
                }
                else if (i[2, y] == i[3, y] && i[2, y] + i[3, y] != 0)//确定范围
                {
                    i[2, y] *= 2;
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    grade += i[2, y];
                }
                else if (i[3, y] == i[4, y] && i[3, y] + i[4, y] != 0)//确定范围
                {
                    i[3, y] *= 2;
                    i[4, y] = 0;
                    change = true;
                    grade += i[3, y];
                }
            }
            GetQuantity();
        }

        /// <summary>
        /// 方向键右操作
        /// </summary>
        public void Right()
        {
            change = false;
            right();
            for (int y = 1; y <= 4; y++)
            {
                /*
                 * 对所有横坐标进行赋值操作，已达到向右移动的效果 
                 */
                if (i[4, y] == i[3, y] && i[4, y] + i[3, y] != 0)
                {
                    if (i[2, y] == i[1, y])
                    {
                        i[4, y] *= 2;
                        i[3, y] = i[2, y];
                        i[2, y] = 0;
                        i[1, y] = 0;
                        grade += i[4, y] + i[3, y];
                    }
                    else
                    {
                        i[4, y] *= 2;
                        i[3, y] = i[2, y];
                        i[2, y] = i[1, y];
                        i[1, y] = 0;
                        grade += i[4, y];
                    }
                    change = true;
                }
                else if (i[3, y] == i[2, y] && i[3, y] + i[2, y] != 0)
                {
                    i[3, y] *= 2;
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                    grade += i[3, y];
                }
                else if (i[2, y] == i[1, y] && i[3, y] + i[1, y] != 0)
                {
                    i[2, y] *= 2;
                    i[1, y] = 0;
                    change = true;
                    grade += i[2, y];
                }
            }
            GetQuantity();
        }

        #region ========================方向算法========================

        /// <summary>
        /// 方向键↑
        /// 横坐标【X】值不确定的情况下，确定纵坐标【Y】的值，纵坐标之间进行位置替换，实现方向上的实现
        /// </summary>
        private void up()
        {
            for (int x = 1; x <= 4; x++)
            {
                /*
                 * 纵坐标从1-4，从小到大开始赋值，从下到上，表示向上操作
                 */
                if (i[x, 1] == 0 && i[x, 4] + i[x, 3] + i[x, 2] != 0)//确定范围
                {
                    i[x, 1] = i[x, 2];//位置互换
                    i[x, 2] = i[x, 3];
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    up();//刷新
                }
                else if (i[x, 2] == 0 && i[x, 4] + i[x, 3] != 0)//确定范围
                {
                    i[x, 2] = i[x, 3];
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    up();
                }
                else if (i[x, 3] == 0 && i[x, 4] != 0)//确定范围
                {
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                }
            }
        }

        /// <summary>
        /// 方向键↓
        /// 横坐标【X】不确定的情况下，确定纵坐标【Y】的值，纵坐标之间进行位置替换，实现方向下的实现
        /// </summary>
        private void down()
        {
            for (int x = 1; x <= 4; x++)
            {
                /*
                 * 纵坐标从4-1.从大到小开始赋值，依次向下,表示向下操作
                 */
                if (i[x, 4] == 0 && i[x, 1] + i[x, 2] + i[x, 3] != 0)//确定范围
                {
                    i[x, 4] = i[x, 3];//位置互换
                    i[x, 3] = i[x, 2];
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;//方向键移动
                    down();//刷新
                }
                else if (i[x, 3] == 0 && i[x, 1] + i[x, 2] != 0)//确定范围
                {
                    i[x, 3] = i[x, 2];
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                    down();
                }
                else if (i[x, 2] == 0 && i[x, 1] != 0)//确定范围
                {
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                }
            }
        }

        /// <summary>
        /// 方向键←
        /// 纵坐标【Y】值不确定的情况下，确定横坐标【X】的值，横坐标之间进行位置替换，实现方向左的实现
        /// </summary>
        private void left()
        {
            for (int y = 1; y <= 4; y++)
            {
                /*
                 * 横坐标从1-4，从小到大开始赋值，从左到右，表示向左操作
                 */
                if (i[1, y] == 0 && i[4, y] + i[3, y] + i[2, y] != 0)//确定范围
                {
                    i[1, y] = i[2, y];//位置互换
                    i[2, y] = i[3, y];
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    left();//刷新
                }
                else if (i[2, y] == 0 && i[4, y] + i[3, y] != 0)//确定范围
                {
                    i[2, y] = i[3, y];
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    left();
                }
                else if (i[3, y] == 0 && i[4, y] != 0)//确定范围
                {
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                }
            }
        }

        /// <summary>
        /// 方向键→
        /// 纵坐标【Y】值不确定的情况下，确定横坐标【X】的值，横坐标之间进行位置替换，实现方向右的实现
        /// </summary>
        private void right()
        {
            for (int y = 1; y <= 4; y++)
            {
                /*
                 * 横坐标从4-1，从大到小开始赋值，从右到左，表示向右操作
                 */
                if (i[4, y] == 0 && i[1, y] + i[2, y] + i[3, y] != 0)//确定范围
                {
                    i[4, y] = i[3, y];//位置互换
                    i[3, y] = i[2, y];
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;//方向键移动
                    right();//刷新
                }
                else if (i[3, y] == 0 && i[1, y] + i[2, y] != 0)
                {
                    i[3, y] = i[2, y];
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                    right();
                }
                else if (i[2, y] == 0 && i[1, y] != 0)
                {
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                }
            }
        }
        #endregion
        #endregion

    }
}
