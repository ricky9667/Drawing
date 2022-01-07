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
            Console.WriteLine(solutionPath);
            targetAppPath = Path.Combine(solutionPath, projectName, "bin", "Debug", "DrawingForm.exe");
            _robot = new Robot(targetAppPath, "Form1");

            SetFormCenterPosition();
        }

        // get center position
        private void SetFormCenterPosition()
        {
            DrawRectangle(0, 0, 10, 10);
            _robot.ClickPosition(0, 0);
            string selectedText = _robot.GetElementText("_selectionLabel");
            ClearCanvas();

            string[] strings = selectedText.Split(' ');
            string centerX = strings[3].Remove(0, 1).Remove(strings[5].Length - 1);
            string centerY = strings[4].Remove(strings[4].Length - 1);

            int.TryParse(centerX, out int x);
            int.TryParse(centerY, out int y);
            _center = new Tuple<int, int>(x, y);
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
            int x1 = 0, y1 = 0, x2 = 200, y2 = 200;
            DrawRectangle(x1, y1, x2, y2);
            _robot.ClickPosition(x1, y1);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1, y1, x2, y2));
        }

        // test
        [TestMethod()]
        public void DrawEllipseTest()
        {
            int x1 = 0, y1 = 0, x2 = 200, y2 = 200;
            DrawEllipse(x1, y1, x2, y2);
            _robot.ClickPosition(x1, y1);
            _robot.AssertText("_selectionLabel", GetSelectedString("Ellipse", x1, y1, x2, y2));
        }

        // test
        [TestMethod()]
        public void DrawLineTest()
        {
            int rx1 = -250, ry1 = -200, rx2 = -50, ry2 = 0;
            int ex1 = 50, ey1 = 0, ex2 = 250, ey2 = 200;
            int lx1 = (rx1 + rx2) / 2, ly1 = (ry1 + ry2) / 2, lx2 = (ex1 + ex2) / 2, ly2 = (ey1 + ey2) / 2;

            DrawRectangle(rx2, ry2, rx1, ry1);
            DrawEllipse(ex1, ey1, ex2, ey2);
            DrawLine(lx1, ly1, lx2, ly2);

            _robot.ClickPosition(lx1, ly1);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", rx1, ry1, rx2, ry2));
            _robot.ClickPosition(lx2, ly2);
            _robot.AssertText("_selectionLabel", GetSelectedString("Ellipse", ex1, ey1, ex2, ey2));
            _robot.ClickPosition((lx1 + lx2) / 2, (ly1 + ly2) / 2);
            _robot.AssertText("_selectionLabel", GetSelectedString("Line", lx1, ly1, lx2, ly2));
        }

        // test
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
        public void MoveShapeTest()
        {
            int x1 = 0, y1 = 0, x2 = 200, y2 = 200;
            DrawRectangle(x1, y1, x2, y2);
            Drag(0, 0, -100, -100);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1 - 100, y1 - 100, x2 - 100, y2 - 100));
        }

        // test
        [TestMethod()]
        public void UndoRedoTest()
        {
            int x1 = 0, y1 = 0, x2 = 200, y2 = 200;
            DrawRectangle(x1, y1, x2, y2);
            Drag(0, 0, -100, -100);
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1 - 100, y1 - 100, x2 - 100, y2 - 100));
            ClickUndo();
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1, y1, x2, y2));
            ClickRedo();
            _robot.AssertText("_selectionLabel", GetSelectedString("Rectangle", x1 - 100, y1 - 100, x2 - 100, y2 - 100));
        }

        // test
        [TestMethod()]
        public void IntegratedTest()
        {
            DrawRectangle(-100, -100, -80, -80);
            DrawRectangle(100, 100, 80, 80);
            DrawRectangle(-100, 100, -80, 80);
            DrawRectangle(100, -100, 80, -80);
            ClickUndo();
            ClickUndo();
            DrawLine(-90, -90, 90, 90);
            DrawEllipse(-100, 100, -80, 80);
            DrawEllipse(100, -100, 80, -80);
            DrawLine(-90, 90, 90, -90);
            ClickUndo();
            ClickRedo();

            ClickSave();
            _robot.Sleep(1);
            ClearCanvas();
            ClickLoad();
            _robot.Sleep(2);
        }

        // get selected string
        private string GetSelectedString(string shape, double x1, double y1, double x2, double y2)
        {
            Console.WriteLine("centerX = " + _center.Item1);
            Console.WriteLine("x1 = " + x1);
            string[] data = new string[] { shape, (x1 + _center.Item1).ToString(), (y1 + _center.Item2).ToString(), (x2 + _center.Item1).ToString(), (y2 + _center.Item2).ToString() };
            return string.Format("Selected : {0} ({1}, {2}, {3}, {4})", data);
        }

        // drag
        private void Drag(double x1, double y1, double x2, double y2)
        {
            _robot.DragAndDrop(x1, y1, x2, y2);
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

        // click undo button
        private void ClickUndo()
        {
            _robot.ClickButton("Undo");
        }

        // click redo button
        private void ClickRedo()
        {
            _robot.ClickButton("Redo");
        }

        // click save button
        private void ClickSave()
        {
            _robot.ClickButton("Save");
        }

        // click load button
        private void ClickLoad()
        {
            _robot.ClickButton("Load");
        }

        // clear canvas
        private void ClearCanvas()
        {
            _robot.ClickButton("Clear");
        }
    }
}