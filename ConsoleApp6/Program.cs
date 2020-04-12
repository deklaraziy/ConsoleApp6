using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ServerMyGit
{
    class Program
    {
        static void Main(string[] args)
        {
            Exit exit = new Exit();
            exit.GetEnterExit();
        }
    }

    class Exit
    {
        EnterParam enterParam = new EnterParam();
        CheckParam checkParam = new CheckParam();

        List<Users> items = new List<Users>();
        public string enterChoise { get; set; }

        public void GetEnterExit()
        {
            Console.WriteLine("-nU - Новый пользователь, -e - Войти");
            enterChoise = Console.ReadLine();
            switch (enterChoise)
            {
                case "-nU"://регистрация нового пользователя
                    Console.WriteLine("Для регистрации укажите логин и пароль");
                    enterParam.GetLogin();
                    checkParam.CheckEnterLoginReg(items); //условие проверки на наличие логина, если существует вовращаем к ГетЛогин

                    //код дальнейшей проги
                    break;
                case "-e":
                    Console.WriteLine("Для входа укажите логин и пароль");
                    enterParam.GetLogin();
                    checkParam.CheckEnterLoginExit(items);//проверка на существование логина, если не существует возвращаем к регистрации

                    break;
                case "-h":
                //help
                default:
                    Console.WriteLine("Указан не верный флаг");
                    GetEnterExit();
                    break;
            }
        }
    }

    class Users
    {
        public string LoginUser { get; set; }
        public string PassUser { get; set; }
    }
    class EnterParam//класс работающий с внешним источником
    {
        public string EnterLogin { get; set; }
        public string EnterPass { get; set; }
        public static string PathXml = "C://Users/shadd/Desktop/ServerMyGit/ServerMyGit/user.xml";



        public void GetLogin()//ввод пользователем логина
        {
            EnterLogin = Console.ReadLine();
        }
        public void GetPass()//....пароля
        {
            EnterPass = Console.ReadLine();
        }
        public object ReadLoginXml(object items)//чтение логина из документа
        {
            XDocument xdoc = XDocument.Load(PathXml);
            items = from xe in xdoc.Elements("users").Elements("user")
                    where xe.Attribute("login").Value == EnterLogin.ToString()
                    select new Users
                    {
                        LoginUser = xe.Attribute("login").Value
                    };
            return items;
        }
        public void RecLog_PassXml()//запись логина\пароля
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathXml);
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement userElem = xDoc.CreateElement("user");
            XmlAttribute logAttr = xDoc.CreateAttribute("login");
            XmlElement passElem = xDoc.CreateElement("pass");

            XmlText logText = xDoc.CreateTextNode(EnterLogin.ToString());
            XmlText passText = xDoc.CreateTextNode(EnterPass.ToString());

            logAttr.AppendChild(logText);
            passElem.AppendChild(passText);
            userElem.Attributes.Append(logAttr);
            userElem.AppendChild(passElem);
            xRoot.AppendChild(userElem);
            xDoc.Save(PathXml);
        }
    }

    class CheckParam : EnterParam //класс основной логики/разветления
    {
        public void CheckEnterLoginReg(List<Users> items)//метод проверки логина при регистрации
        {
            ReadLoginXml(items);
            foreach (var item in items)
            {
                if (item.LoginUser == EnterLogin)
                {
                    Console.WriteLine("Логин уже занят. Выберете другой");
                    GetLogin();
                }
                if (item.LoginUser != EnterLogin)
                {
                    GetPass();
                    RecLog_PassXml();
                    Console.WriteLine("Добро пожаловать " + EnterLogin);
                    //код дальнейшей проги
                }
            }
        }
        public void CheckEnterLoginExit(List<Users> items)//проверка при входе
        {
            ReadLoginXml(items);
            foreach (var item in items)
            {
                if (item.LoginUser != EnterLogin)
                {
                    Console.WriteLine("Такого логина не существует!\nПопробуйте снова нажав на Enter, или введите -nU для регистрации");
                    if (EnterLogin == "-nU")
                    {
                        Console.WriteLine("Для регистрации укажите логин и пароль");
                        GetLogin();
                        CheckEnterLoginReg(items);//переброс на регистрацию
                        Console.WriteLine("Добро пожаловать " + EnterLogin);
                        //код программы
                    }
                    else
                    {
                        GetLogin();
                    }
                }
                if (item.LoginUser == EnterLogin)
                {
                    GetPass();
                    CheckPassExit();//проверка пароля
                    Console.WriteLine("С возвращением " + EnterLogin);
                }
            }
        }
        public void CheckPassExit()//проверка пороля
        {
            XDocument xdoc = XDocument.Load(PathXml);
            var items = from xe in xdoc.Elements("users").Elements("user")
                        where xe.Element("pass").Value == EnterPass.ToString()
                        select new Users
                        {
                            PassUser = xe.Element("pass").Value
                        };
            foreach (var item in items)
            {
                if (item.PassUser != EnterPass)
                {
                    Console.WriteLine("Не правильно указан пароль");
                    GetPass();
                    CheckPassExit();
                }
                if (item.PassUser == EnterPass)
                {
                    Console.WriteLine("Добро пожаловать " + EnterLogin);
                    //код программы
                }
            }
        }
    }


}