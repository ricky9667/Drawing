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
        Tuple<int, int> _center;
        private string targetAppPath;
        // init
        [TestInitialize]
        public void Initialize()
        {
            var projectName = "DrawingForm";
            string solutionPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            targetAppPath = Path.Combine(solutionPath, projectName, "bin", "Debug", "DrawingForm.exe");
            _robot = new Robot(targetAppPath, "Form1");

            SetFormCenterPosition();
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
        public void DrawRectangleTest()
        {
            //DrawRectangle(-100, -100, 100, 100);
            //_robot.ClickPosition(0, 0);
        }

        // test
        [TestMethod()]
        public void DrawEllipseTest()
        {
            DrawEllipse(-300, -200, 200, 300);
        }

        // test
        [TestMethod()]
        public void DrawLineTest()
        {
            DrawLine(-200, -150, 100, 200);
        }

        // test
        [TestMethod()]
        public void DrawSnowmanTest()
        {
            DrawEllipse(-100, -200, 100, 0);
            DrawEllipse(-150, 0, 150, 300);
            DrawEllipse(-50, -140, -20, -110);
            DrawEllipse(20, -140, 50, -110);
            DrawEllipse(-25, 50, 25, 100);
            DrawEllipse(-25, 125, 25, 175);

            DrawRectangle(-100, -250, 100, -200);
            DrawRectangle(-160, -200, 160, -180);
            DrawRectangle(-20, -80, 20, -40);
            DrawRectangle(-165, -75, -150, 150);
            DrawRectangle(150, -75, 165, 150);
        }

        // get center position
        private void SetFormCenterPosition()
        {
            DrawRectangle(0, 0, 0, 0);
            _robot.ClickPosition(0, 0);
            string selectedText = _robot.GetElementText("_selectionLabel");

            string[] strings = selectedText.Split(' ');
            string centerX = strings[5].Remove(strings[5].Length - 1);
            string centerY = strings[4].Remove(strings[4].Length - 1);

            int.TryParse(centerX, out int x);
            int.TryParse(centerY, out int y);
            _center = new Tuple<int, int>(x, y);
        }

        // draw rectangle
        private void DrawRectangle(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Rectangle");
            _robot.DragAndDrop(x1, y1, x2, y2);
        }

        // draw ellipse
        private void DrawEllipse(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Ellipse");
            _robot.DragAndDrop(x1, y1, x2, y2);
        }

        // draw line
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Line");
            _robot.DragAndDrop(x1, y1, x2, y2);
        }
    }
}