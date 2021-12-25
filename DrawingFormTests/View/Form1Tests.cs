using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawingForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainFormUITest;
using System.IO;

namespace DrawingForm.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        Robot _robot;
        private string targetAppPath;
        // init
        [TestInitialize]
        public void Initialize()
        {
            var projectName = "DrawingForm";
            string solutionPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            targetAppPath = Path.Combine(solutionPath, projectName, "bin", "Debug", "DrawingForm.exe");
            _robot = new Robot(targetAppPath, "Form1");
        }

        // dispose
        [TestCleanup]
        public void Cleanup()
        {
            _robot.Sleep(2);
            _robot.CleanUp();
        }

        // test
        [TestMethod()]
        public void DrawLine()
        {
            _robot.ClickButton("Line");
            _robot.DragAndDrop(-200, -150, 100, 200);
        }

        // test
        [TestMethod()]
        public void DrawRectangle()
        {
            _robot.ClickButton("Rectangle");
            _robot.DragAndDrop(-100, -100, 100, 100);
            _robot.ClickPosition(0, 0);
        }

        // test
        [TestMethod()]
        public void DrawEllipse()
        {
            _robot.ClickButton("Ellipse");
            _robot.DragAndDrop(-300, -200, 200, 300);
        }

        // test
        [TestMethod()]
        public void DrawSnowman()
        {
            _robot.ClickButton("Ellipse");
            _robot.DragAndDrop(-100, -200, 100, 0);
            _robot.DragAndDrop(-150, 0, 150, 300);
            _robot.DragAndDrop(-50, -140, -20, -110);
            _robot.DragAndDrop(20, -140, 50, -110);
            _robot.DragAndDrop(-25, 50, 25, 100);
            _robot.DragAndDrop(-25, 125, 25, 175);

            _robot.ClickButton("Rectangle");
            _robot.DragAndDrop(-100, -250, 100, -200);
            _robot.DragAndDrop(-160, -200, 160, -180);
            _robot.DragAndDrop(-20, -80, 20, -40);
            _robot.DragAndDrop(-165, -75, -150, 150);
            _robot.DragAndDrop(150, -75, 165, 150);

        }
    }
}