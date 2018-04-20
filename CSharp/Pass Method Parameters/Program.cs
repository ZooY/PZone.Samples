using System;


namespace Pass_Method_Parameters
{
    class Program
    {
        static void Main()
        {
            IInterface classA = new ClassA();
            var testClass = new TestClass();

            testClass.TestMethod(classA);
            Console.Write("Write from Main: ");
            classA.Foo();
            Console.WriteLine();

            testClass.RefTestMethod(ref classA);
            Console.Write("Write from Main: ");
            classA.Foo();
            Console.WriteLine();

            Console.ReadKey();
        }
    }


    interface IInterface
    {
        void Foo();
    }


    class ClassA : IInterface
    {
        public void Foo()
        {
            Console.WriteLine("A");
        }
    }


    class CalssB : IInterface
    {
        public void Foo()
        {
            Console.WriteLine("B");
        }
    }


    class TestClass
    {
        public void TestMethod(IInterface classX)
        {
            classX = new CalssB();
            Console.Write("Write from TestMethod: ");
            classX.Foo();
            Console.WriteLine();
        }

        public void RefTestMethod(ref IInterface classX)
        {
            classX = new CalssB();
            Console.Write("Write from TestMethod: ");
            classX.Foo();
            Console.WriteLine();
        }
    }
}
