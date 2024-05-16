using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Design_Pattern
{
    /// <summary>
    /// 
    /// </summary>
    class TemplatePattern
    {
        public static void miain()
        {

        }
    }
    class Trading
    {
        public virtual void  Init()
        {

        }

        public virtual bool CheckCanTrade()
        {
            return true;
        }
        public virtual (bool,decimal) GetIsBuyAndAmount()
        {
            return (true, 1);
        }

        public async void DoTrade()
        {
            Init();
            while (true)
            {
                if (CheckCanTrade())
                {
                    GetIsBuyAndAmount();
                }
                await Task.Delay(1000);
            }
        }
    }
    class Trading30DayMV:Trading
    {
        public override void Init()
        {
            base.Init();
        }
        public override bool CheckCanTrade()
        {
            return base.CheckCanTrade();
        }
        public override (bool, decimal) GetIsBuyAndAmount()
        {
            return base.GetIsBuyAndAmount();
        }
    }
}
