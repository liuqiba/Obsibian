using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            //简单工厂
            //             Shape c = SimpleFactory.GetShape(ShapeEnum.Circle);
            //             Shape r = SimpleFactory.GetShape(ShapeEnum.Rectangle);

            //反射简单工厂
            //             Shape c = ActiveFactory.GetShape(typeof(Circle));
            //             Shape r = ActiveFactory.GetShape(typeof(Rectangle));
            //工厂方法
//             Shape c = (new CircleFactorMethod()).GetShape();
//             Shape r = (new RectangleFactorMethod()).GetShape();
// 
// 
//             c.Draw();
//             r.Draw();

            AbShapeFactor colorShapeFactor =new ColorShapeFactor();

            AbShapeFactor BlackShapeFactor = new BlackWhiteShapeFactor();


            void print(AbShapeFactor factor)
            {
                Shape c = factor.GetCircle();
                Shape r = factor.GetRectangle();
                c.Draw();
                r.Draw();
            }
            print(colorShapeFactor);
            print(BlackShapeFactor);
        }
    }
    #region 简单工厂
    /// <summary>
    /// 简单工厂模式
    /// 意义：通过一个工厂类创建多个类似产品，减少创建的复杂度统一调用
    /// 实现：一个工厂类，一个产品基类，n个产品子类，通过工厂传入参数创建某个子类
    /// 缺点：每次新增产品都要修改工厂类代码，耦合性很高，不满足封装，可扩展
    /// 
    /// </summary>
    class SimpleFactory
    {
        public static Shape GetShape(ShapeEnum shape)
        {
            switch (shape)
            {
                case ShapeEnum.Circle:
                    return new Circle();
                    
                case ShapeEnum.Rectangle:
                    return new Rectangle();
                default:
                    throw new Exception("请传入正确参数");
                    
            }
        }
    }
    enum ShapeEnum
    {
        Circle,
        Rectangle
    }

    public abstract class Shape
    {
        public abstract void Draw();

    }

    public class Circle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Circle");
        }
    }
    public class Rectangle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Rectangle");
        }
    }
    #endregion

    #region 简单工厂反射模式
    class ActiveFactory
    {
        public static Shape GetShape(Type type )
        {
            Shape shape = (Shape)Activator.CreateInstance(type, true);
            return shape;
        }
    }


    #endregion

    //目的：封装扩展，解耦性简单工厂模式
    //实现：创建一个工厂类， 一个产品接口，n个产品实现接口，n个工厂子类一一对应产品子类，通过产品接口中的方法创建对应产品子类
    //    然后
    //缺点：产品多了之后会很繁多复杂
    //
    #region 工厂方法模式

    
    interface  IShapeFactorMethod
    {
        public abstract Shape GetShape();
    }

    class CircleFactorMethod : IShapeFactorMethod
    {
        public Shape GetShape()
        {
            return new Circle();
        }
    }
    class RectangleFactorMethod : IShapeFactorMethod
    {
        public Shape GetShape()
        {
            return new Rectangle();
        }
    }


    #endregion

    //目的：让多个产品公用的产出物体的 公有化，比如抽象酒厂，子类五粮液酒厂和茅台酒厂
    //实现：一个抽象工厂类，n个子类。抽象工厂里面有相关联的一系列方法和类型
    //
    //
    #region 抽象工厂

    class ColorCircle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("彩色圆形");
        }
    }
    class BlackWhiteCircle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("黑白圆形");
        }
    }
    class ColorRectangle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("彩色方形");
        }
    }
    class BlackWhiteRectangle  : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("黑白方形");
        }
    }
    abstract class AbShapeFactor
    {
        public abstract Shape GetCircle();
        public abstract Shape GetRectangle();
    }
    /// <summary>
    /// 彩色绘制图形
    /// </summary>
    class ColorShapeFactor:AbShapeFactor
    {
        public override Shape GetCircle()
        {
            return new ColorCircle();
        }

        public override Shape GetRectangle()
        {
            return new ColorRectangle();
        }
    }
    /// <summary>
    /// 黑白绘制图形
    /// </summary>
    class BlackWhiteShapeFactor : AbShapeFactor
    {
        public override Shape GetCircle()
        {
            return new BlackWhiteCircle();
        }

        public override Shape GetRectangle()
        {
            return new BlackWhiteRectangle();
        }
    }

    #endregion
}
