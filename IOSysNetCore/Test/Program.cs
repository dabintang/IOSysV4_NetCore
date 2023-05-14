using IOSys.BLL;
using IOSys.DAL;
using IOSys.DTO;
using IOSys.DTO.Account;
using IOSys.Helper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 1、缓存
/// </summary>
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigHelper.Init();

            //TestAsync();

            //var test = InOutDAL.Inst.GetMinTurnoverDate(4).Result;

            //TestLock test = new TestLock();
            //test.Test2();

            TestDBBackup();

            Console.ReadLine();
        }

        private static void TestDBBackup()
        {
            var bll = new MysqlBLL();
            bll.Backup();
        }

        private static void TestDB()
        {
            using (var db = new IOSysContext())
            {
                var item = db.Familys.Where(m => m.ID == 3).FirstOrDefault();
            }
        }

        private static void TestJWT()
        {
            var user = new LoginInfo();
            user.NickName = "NickName";
            user.FamilyName = "FamilyName";

            var token = JWTHelper.CreateToken(user, "127.0.0.1");
            Console.WriteLine(token);

            Thread.Sleep(9000);

            var ret = JWTHelper.CheckToken<LoginInfo>(token, "127.0.0.1");
            Console.WriteLine(ret.Msg);
        }

        private static void TestNLog()
        {
            LogHelper.Fatal("看看Fatal");
            LogHelper.Error("看看Error");
            LogHelper.Warn("看看Warn");
            LogHelper.Info("看看Info");
            LogHelper.Debug("看看Debug");
            LogHelper.Trace("看看Trace");
        }

        private static void TestLanguage()
        {
            var req = new LoginReq();
            req.LoginName = "t";
            req.Password = "1";

            var bll = new AccountBLL();
            var ret = bll.LoginAsync(req, "127.0.0.1");
        }

        private static void TestCache()
        {
            var user = new LoginInfo();
            user.NickName = "NickName";
            user.FamilyName = "FamilyName";

            CacheHelper.Set("test", user);
            var t1 = CacheHelper.Get<LoginInfo>("test1");
            var t = CacheHelper.Get<LoginInfo>("test");
            var tt = CacheHelper.Get<LoginInfo>("test");

            Thread.Sleep(1000);

            var ttt = CacheHelper.Get<LoginInfo>("test");
        }

        #region await test

        private static void TestAsync()
        {
            Console.WriteLine("TestAsync 当前线程1：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
            //Async1();

            Task.WaitAll(Async1(), Async2(), Async3(), Async4());

            Console.WriteLine("TestAsync 当前线程2：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        private static async Task Async1()
        {
            Console.WriteLine("Async1 当前线程1：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
            //await Async2();
            //await Task.Delay(1000);
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    var temp = 11111.11111 * 999999999.99999;
                }
            });
            Console.WriteLine("Async1 当前线程2：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        private static async Task Async2()
        {
            Console.WriteLine("Async2 当前线程1：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
            //await Async3();
            //await Task.Delay(2000);
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    var temp = 11111.11111 * 999999999.99999;
                }
            });
            Console.WriteLine("Async2 当前线程2：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        private static async Task Async3()
        {
            Console.WriteLine("Async3 当前线程1：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
            //await Async4();
            //await Task.Delay(3000);
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    var temp = 11111.11111 * 999999999.99999;
                }
            });
            Console.WriteLine("Async3 当前线程2：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        private static async Task Async4()
        {
            Console.WriteLine("Async4 当前线程1：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
            //await Task.Delay(4000);
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    var temp = 11111.11111 * 999999999.99999;
                }
            });
            Console.WriteLine("Async4 当前线程2：" + Thread.CurrentThread.ManagedThreadId.ToString() + " 时间：" + DateTime.Now.ToString("HH:mm:ss.fff"));
        }

        #endregion
    }

    class TestLockNum
    {
        public int Num { get; set; }
    }
    class TestLock
    {
        private static readonly object _lock = new object();

        public void Test()
        {
            RecursionLock(5);
        }

        private void RecursionLock(int num)
        {
            if (num < 3)
            {
                return;
            }

            lock (_lock)
            {
                Console.WriteLine($"序号：{num}，线程ID：{Thread.CurrentThread.ManagedThreadId}，开始");
                Thread.Sleep(2000);
                RecursionLock(--num);
                Console.WriteLine($"序号：{num}，线程ID：{Thread.CurrentThread.ManagedThreadId}，结束");
            }
        }


        public void Test2()
        {
            var num = new TestLockNum();
            num.Num = 5;
            RecursionLock2(num);
        }

        private void RecursionLock2(TestLockNum num)
        {
            if (num.Num < 3)
            {
                return;
            }

            lock (_lock)
            {
                Console.WriteLine($"序号：{num.Num}，线程ID：{Thread.CurrentThread.ManagedThreadId}，开始");
                Thread.Sleep(2000);
                RecursionLock(--num.Num);
                Console.WriteLine($"序号：{num.Num}，线程ID：{Thread.CurrentThread.ManagedThreadId}，结束");
            }
        }

        public void Test3()
        {
            Parallel.For(1, 10, (index) =>
            {
                Lock3(index);
            });
        }

        private void Lock3(int index)
        {
            lock (_lock)
            {
                Console.WriteLine($"序号：{index}，线程ID：{Thread.CurrentThread.ManagedThreadId}，开始");
                Thread.Sleep(2000);
                Console.WriteLine($"序号：{index}，线程ID：{Thread.CurrentThread.ManagedThreadId}，结束");
            }
        }
    }
}
