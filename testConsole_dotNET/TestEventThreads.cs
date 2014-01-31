using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using testConsole_dotNET.Inheritance;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET
{
    class TestEventThreads
    {
        internal void Go()
        {
            testEventThreads();

            testInheritance();

            testMaths();

            testCodeAnalysis();

            testDispose();

            testDateTime();

            testEnums();

            testXmlFileCreation();
        }

        private void testXmlFileCreation()
        {
            Encoding enc = Encoding.Default; //encoding of XmlWriter and of the final output need to match!
            XmlWriterSettings xmlSettings = new XmlWriterSettings
                                            {
                                                Indent = true,
                                                IndentChars = "\t",
                                                OmitXmlDeclaration = false,
                                                NewLineOnAttributes = false,
                                                Encoding = enc
                                            };

            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlTextWriter.Create(sb, xmlSettings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("root");

            xmlWriter.WriteElementString("foo", "bar");

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            if (sb.ToString().IndexOf("<?xml") < 0)
            {
                throw new ApplicationException("No XML header in the generated XML!");
            }
        }

        private void testEnums()
        {
            Output.LogTestStart("testEnums");

            EnumTests tests = new EnumTests();
            tests.Go();
        }

        private void testDateTime()
        {
            Output.LogTestStart("testDateTime");

            DateTimeTests tests = new DateTimeTests();
            tests.Go();
        }

        private void testDispose()
        {
            Output.LogTestStart("testDispose");

            using (var derived = new DisposeTestDerived())
            {
            }
        }

        private void testCodeAnalysis()
        {
            Output.LogTestStart("testCodeAnalysis");

            CodeAnalysisTests tests = new CodeAnalysisTests();
            tests.Go();
        }

        private void testMaths()
        {
            Output.LogTestStart("testMaths");

            MathTests tests = new MathTests();

            tests.TestScientificNotation();
        }

        private void testInheritance()
        {
            Output.LogTestStart("testInheritance");

            Output.Log("Creating a Derived1");
            var derived = new Derived1();

            Output.Log("Creating a Derived1");
            var derivedString = new Derived1("test");
        }

        event SendMessage OnSendMessage;

        delegate void SendMessage(string msg);

        private void testEventThreads()
        {
            Output.LogTestStart("testEventThreads");

            outputThreadId();
            OnSendMessage += new SendMessage(Test_OnSendMessage);

            OnSendMessage("Testing event firing");
        }

        void Test_OnSendMessage(string msg)
        {
            outputThreadId();
        }

        private void outputThreadId()
        {
            Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
        }
    }
}
