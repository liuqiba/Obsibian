using System;
using System.Collections.Generic;
using System.Text;

namespace Design_Pattern.CreationalPatterns
{
    /// <summary>
    /// 饱汉模式
    /// 静态类直接初始化
    /// </summary>
    static class  SingletonPattern
    {

        
        
    }
    /// <summary>
    /// 饿汉模式
    /// 在第一次调用的时候创建
    /// 并且设置构造方法为私有其他不能创建
    /// </summary>
    public class SingletonPattern2
    {
        
        private SingletonPattern2()
        {

        }
        private SingletonPattern2 _instance;
        public SingletonPattern2 Instance 
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new SingletonPattern2();
                }
                return _instance;
            } 
        }

    }

    /// <summary>
    /// 有多线程调用的单例
    /// </summary>
    public class SingletonPattern3
    {
        private SingletonPattern3 instance;

        private readonly object lockOb = new object();
        public SingletonPattern3 Instance
        {
            get
            {
                //防止创建后还每次都锁
                if (instance == null)//1
                {
                    lock (lockOb)//2
                    {
                        //锁后再判断，避免已经有线程进入1 2 之间的时候创建好了
                        if (instance == null)
                        {
                            instance = new SingletonPattern3();
                        }
                    }
                }
                return instance;
            }
           
        }
    }
}
