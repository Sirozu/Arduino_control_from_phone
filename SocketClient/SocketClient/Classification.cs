using System;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Xml;


namespace ApplicationGI
{
    class Classification
    {
        public static SerialPort port = new System.IO.Ports.SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);


        private static readonly int NUMCLASS = 3;
        private static readonly int NUMWORD = 60;
        private static string[,] myCollection = new string[NUMCLASS, NUMWORD];
        private static void InputPrerocessing()
        {
            var doc = new XmlDocument();
            //doc.Load("C:/Users/Siroz/OneDrive/Диплом/SocketClient/SocketClient/VariationAnswer.xml");
            doc.Load("C:/Users/HORIZON/OneDrive/Диплом/SocketClient/SocketClient/VariationAnswer.xml");
            var docRoot = doc.DocumentElement;

            //myCollection.Add("окна", docRoot["WindowGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            //myCollection.Add("свет", docRoot["LightGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            //var a = docRoot["WindowGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            //"дела"
            int j = 0;
            var a = (docRoot["BuisnesGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            for (int i = 0; i < a.Length; i++)
            {
                myCollection[j, i] = a[i];
                if (a.Length < NUMWORD && i > a.Length)
                {
                    myCollection[j, i] = "-1";
                }
            }
            //"приветствия"
            j++;
            a = (docRoot["HelloGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            for (int i = 0; i < a.Length; i++)
            {
                myCollection[j, i] = a[i];
                if (a.Length < NUMWORD && i > a.Length)
                {
                    myCollection[j, i] = "-1";
                }
            }
            //("прощания"
            j++;
            a = (docRoot["GoodbyeGroup"].InnerText.ToLower().Split(new char[] { '\n', ',', '.' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            for (int i = 0; i < a.Length; i++)
            {
                myCollection[j, i] = a[i];
                if (a.Length < NUMWORD && i > a.Length)
                {
                    myCollection[j, i] = "-1";
                }
            }
            //foreach (var x in myCollection)
            //{
            //    Console.WriteLine(x);
            //}

        }

        private static string SelectAnswer(string a)
        {
            a.ToLower().Trim().Replace("!,.;:", "");
            string result = "";
            bool HELLOflag = false;
            for (int i = 0; i < NUMWORD; i++)
            {
                if (myCollection[1, i] != "-1" && myCollection[1, i] != null)
                {
                    try
                    {
                        if (a.Contains(myCollection[1, i].Trim().Replace("!", "")))
                        {
                            HELLOflag = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            if (HELLOflag)
            {
                result += "Здравствуйте ";
            }

            bool GOODBYEflag = false;
            for (int i = 0; i < NUMWORD; i++)
            {
                if (myCollection[2, i] != "-1" && myCollection[2, i] != null)
                {
                    try
                    {
                        if (a.Contains(myCollection[2, i].Trim().Replace("!", "")))
                        {
                            HELLOflag = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            if (GOODBYEflag)
            {
                result += "До свидания ";
            }

            if (a.Contains("открой окно") || a.Contains("Открой окно"))
            {
                result += "открываю ";
                try
                {
                    if (!(port.IsOpen))
                        port.Open();
                    port.Write("2");
                    Console.WriteLine(port.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: "
                                    + ex.Message, "Error!");
                }
            }

            if (a.Contains("закрой окно") || a.Contains("Закрой окно"))
            {
                result += "закрываю ";
                try
                {
                    if (!(port.IsOpen))
                        port.Open();
                    port.Write("3");
                    Console.WriteLine(port.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: "
                                    + ex.Message, "Error!");
                }
            }

            if (a.Contains("включи свет") || a.Contains("Включи свет") || a.Contains("Включи") || a.Contains("включи"))
            {
                result += "включаю ";
                try
                {
                    if (!(port.IsOpen))
                        port.Open();
                    port.Write("1");
                    Console.WriteLine(port.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: "
                                    + ex.Message, "Error!");
                }
            }

            if (a.Contains("выключи свет") || a.Contains("Выключи свет") || a.Contains("Выключить свет")
                || a.Contains("Выключи") || a.Contains("Выключить") || a.Contains("выключить") || a.Contains("выключи"))
            {
                result += "выключаю ";
                try
                {
                    if (!(port.IsOpen))
                        port.Open();
                    port.Write("0");
                    Console.WriteLine(port.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening/writing to serial port :: "
                                    + ex.Message, "Error!");
                }
            }

            //Console.WriteLine(result);
            return result;
        }



        public static string SENDRECV(string a)
        {
            InputPrerocessing();





            Console.WriteLine("Send: " + a);
            // if (port.IsOpen)
            //    port.Close();

            return SelectAnswer(a);
        }

        //public Classification()
        //{
        //    InputPrerocessing();
        //    Console.WriteLine(SelectAnswer("привет  открой окно"));
        //}

    }
}
