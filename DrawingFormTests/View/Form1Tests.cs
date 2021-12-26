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
            int x1 = 50, y1 = 50, x2 = 200, y2 = 200;
            DrawRectangle(x1, y1, x2, y2);
            _robot.ClickPosition(x1, y1, _center);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1, y1, x2, y2));
        }

        // test
        [TestMethod()]
        public void DrawEllipseTest()
        {
            int x1 = 100, y1 = 200, x2 = 300, y2 = 300;
            DrawEllipse(x1, y1, x2, y2);
            _robot.ClickPosition(x1, y1, _center);
            _robot.AssertText("_selectionLabel", GetSelectedString("Ellipse", x1, y1, x2, y2));
        }

        // test
        [TestMethod()]
        public void DrawLineTest()
        {
            int rx1 = 100, ry1 = 100, rx2 = 300, ry2 = 200;
            int ex1 = 400, ey1 = 300, ex2 = 500, ey2 = 500;
            int lx1 = (rx1 + rx2) / 2, ly1 = (ry1 + ry2) / 2, lx2 = (ex1 + ex2) / 2, ly2 = (ey1 + ey2) / 2;
            
            DrawRectangle(rx1, ry1, rx2, ry2);
            DrawEllipse(ex1, ey1, ex2, ey2);
            DrawLine(lx1, ly1, lx2, ly2);

            _robot.ClickPosition(lx1, ly1, _center);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", rx1, ry1, rx2, ry2));
            _robot.ClickPosition(lx2, ly2, _center);
            _robot.AssertText("_selectionLabel", GetSelectedString("Ellipse", ex1, ey1, ex2, ey2));
            _robot.ClickPosition((lx1 + lx2) / 2, (ly1 + ly2) / 2, _center);
            _robot.AssertText("_selectionLabel", GetSelectedString("Line", lx1, ly1, lx2, ly2));
        }

        [TestMethod()]
        public void ClearTest()
        {
            int rx1 = 100, ry1 = 100, rx2 = 300, ry2 = 200;
            int ex1 = 400, ey1 = 300, ex2 = 500, ey2 = 500;
            int lx1 = (rx1 + rx2) / 2, ly1 = (ry1 + ry2) / 2, lx2 = (ex1 + ex2) / 2, ly2 = (ey1 + ey2) / 2;
            DrawRectangle(rx1, ry1, rx2, ry2);
            DrawEllipse(ex1, ey1, ex2, ey2);
            DrawLine(lx1, ly1, lx2, ly2);
            ClearCanvas();
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

            ClearCanvas();
        }

        // get center position
        private void SetFormCenterPosition()
        {
            DrawRectangle(0, 0, 0, 0);
            _robot.ClickPosition(0, 0);
            string selectedText = _robot.GetElementText("_selectionLabel");
            ClearCanvas();

            string[] strings = selectedText.Split(' ');
            string centerX = strings[5].Remove(strings[5].Length - 1);
            string centerY = strings[4].Remove(strings[4].Length - 1);

            int.TryParse(centerX, out int x);
            int.TryParse(centerY, out int y);
            _center = new Tuple<int, int>(x, y);
        }

        // get selected string
        private string GetSelectedString(string shape, double x1, double y1, double x2, double y2)
        {
            string[] data = new string[] { shape, x1.ToString(), y1.ToString(), x2.ToString(), y2.ToString() };
            return string.Format("Selected : {0} ({1}, {2}, {3}, {4})", data);
            //return "Selected : " + shape + " (" + x1 + "";
        }

        // draw rectangle
        private void DrawRectangle(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Rectangle");
            _robot.DragAndDrop(x1, y1, x2, y2, _center);
        }

        // draw ellipse
        private void DrawEllipse(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Ellipse");
            _robot.DragAndDrop(x1, y1, x2, y2, _center);
        }

        // draw line
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            _robot.ClickButton("Line");
            _robot.DragAndDrop(x1, y1, x2, y2, _center);
        }

        // clear canvas
        private void ClearCanvas()
        {
            _robot.ClickButton("Clear");
        }
    }
}