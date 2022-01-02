using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrawingModel.Tests
{
    [TestClass()]
    public class ModelTests
    {
        private Model model;

        // init
        [TestInitialize]
        public void Initialize()
        {
            model = new Model();
        }

        // test
        [TestMethod()]
        public void SetDrawingShapeTest()
        {
            Assert.AreEqual(ShapeType.NULL, model.CurrentShapeType);
            model.SetDrawingShape(ShapeType.RECTANGLE);
            Assert.AreEqual(ShapeType.RECTANGLE, model.CurrentShapeType);
            model.SetDrawingShape(ShapeType.ELLIPSE);
            Assert.AreEqual(ShapeType.ELLIPSE, model.CurrentShapeType);
        }

        // test
        [TestMethod()]
        public void HandlePointerPressedTest()
        {
            double testX = 17;
            double testY = 7;
            model.SetDrawingShape(ShapeType.RECTANGLE);
            model.HandlePointerPressed(testX, testY);
            
            Assert.AreEqual(testX, model.FirstPointX);
            Assert.AreEqual(testY, model.FirstPointY);
            Assert.AreEqual(testX, model.Hint.X1);
            Assert.AreEqual(testY, model.Hint.Y1);
            Assert.IsTrue(model.IsPressed);

            model.HandlePointerPressed(-23, -79);
            
            Assert.AreEqual(testX, model.FirstPointX);
            Assert.AreEqual(testY, model.FirstPointY);
            Assert.AreEqual(testX, model.Hint.X1);
            Assert.AreEqual(testY, model.Hint.Y1);
            Assert.IsTrue(model.IsPressed);
        }

        // test
        [TestMethod()]
        public void HandlePointerMovedTest()
        {
            double testX1 = 17;
            double testY1 = 7;
            double testX2 = 37;
            double testY2 = 64;

            Assert.IsFalse(model.IsPressed);

            model.SetDrawingShape(ShapeType.RECTANGLE);
            model.HandlePointerPressed(testX1, testY1);
            model.HandlePointerMoved(testX2, testY2);
            
            Assert.AreEqual(testX2, model.Hint.X2);
            Assert.AreEqual(testY2, model.Hint.Y2);
            Assert.IsTrue(model.IsPressed);
        }

        // test
        [TestMethod()]
        public void HandlePointerReleasedTest()
        {
            double testX1 = 17;
            double testY1 = 7;
            double testX2 = 37;
            double testY2 = 64;

            Assert.IsFalse(model.IsPressed);

            model.SetDrawingShape(ShapeType.RECTANGLE);
            model.HandlePointerPressed(testX1, testY1);
            model.HandlePointerMoved(testX2, testY2);
            model.HandlePointerReleased(testX2, testY2);

            Assert.IsFalse(model.IsPressed);

            Assert.AreEqual(1, model.Shapes.Count);
            Assert.AreEqual(testX1, model.Shapes[0].X1);
            Assert.AreEqual(testY1, model.Shapes[0].Y1);
            Assert.AreEqual(testX2, model.Shapes[0].X2);
            Assert.AreEqual(testY2, model.Shapes[0].Y2);
        }

        // test
        [TestMethod()]
        public void SetCoordinatesTest()
        {
            model.SetDrawingShape(ShapeType.RECTANGLE);

            model.SetFirstPointCoordinates(365, 111);
            Assert.AreEqual(365, model.FirstPointX);
            Assert.AreEqual(111, model.FirstPointY);

            model.SetHintFirstPointCoordinates(100, 150);
            Assert.AreEqual(100, model.Hint.X1);
            Assert.AreEqual(150, model.Hint.Y1);

            model.SetHintSecondPointCoordinates(300, 250);
            Assert.AreEqual(300, model.Hint.X2);
            Assert.AreEqual(250, model.Hint.Y2);
        }

        // test
        [TestMethod()]
        public void UpdateSelectedShapeIndexTest()
        {
            Assert.AreEqual(-1, model.SelectedShapeIndex);

            DrawShapeFake(ShapeType.RECTANGLE, 100, 100, 400, 400);
            model.UpdateSelectedShapeIndex(300, 300);
            Assert.AreEqual(0, model.SelectedShapeIndex);

            DrawShapeFake(ShapeType.ELLIPSE, 500, 500, 600, 600);
            model.UpdateSelectedShapeIndex(550, 550);
            Assert.AreEqual(1, model.SelectedShapeIndex);
        }

        // test
        [TestMethod()]
        public void SelectShapeTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 10, 10, 100, 100);
            model.HandlePointerPressed(50, 50);
            model.HandlePointerReleased(50, 50);
            Assert.AreEqual(0, model.SelectedShapeIndex);
        }

        // test
        [TestMethod()]
        public void MovingShapeTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 100, 100, 300, 300);
            model.UpdateSelectedShapeIndex(200, 200);

            model.HandlePointerPressed(200, 200);
            model.UpdateMovingShape(300, 300);
            model.UpdateSelectedShapeIndex(350, 350);
            Assert.AreEqual(0, model.SelectedShapeIndex);
            model.UpdateSelectedShapeIndex(150, 150);
            Assert.AreEqual(-1, model.SelectedShapeIndex);
        }

        [TestMethod()]
        public void MoveShapeTest()
        {
            DrawShapeFake(ShapeType.ELLIPSE, 100, 100, 300, 300);
            model.UpdateSelectedShapeIndex(200, 200);

            model.HandlePointerPressed(200, 200);
            model.MoveShape(300, 300);
            model.UpdateSelectedShapeIndex(350, 350);
            Assert.AreEqual(0, model.SelectedShapeIndex);
            model.UpdateSelectedShapeIndex(150, 150);
            Assert.AreEqual(-1, model.SelectedShapeIndex);
        }

        // test
        [TestMethod()]
        public void ClearTest()
        {
            Assert.IsFalse(model.IsPressed);
            Assert.AreEqual(0, model.Shapes.Count);

            DrawShapeFake(ShapeType.RECTANGLE, 5, 105, 37, 137);
            DrawShapeFake(ShapeType.ELLIPSE, 45, 102, 326, 400);
            Assert.AreEqual(2, model.Shapes.Count);

            model.Clear();
            Assert.IsFalse(model.IsPressed);
            Assert.AreEqual(0, model.Shapes.Count);
        }

        // test
        [TestMethod()]
        public void DrawLineTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 10, 10, 30, 30);
            DrawShapeFake(ShapeType.ELLIPSE, 110, 140, 150, 200);
            Assert.AreEqual(2, model.Shapes.Count);

            DrawShapeFake(ShapeType.LINE, 15, 27, 100, 120);
            Assert.AreEqual(2, model.Shapes.Count);

            DrawShapeFake(ShapeType.LINE, 15, 27, 130, 199);
            Assert.AreEqual(3, model.Shapes.Count);
        }

        // test
        [TestMethod()]
        public void UpdateLinesPositionTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 100, 100, 300, 300);
            DrawShapeFake(ShapeType.ELLIPSE, 500, 100, 700, 300);
            DrawShapeFake(ShapeType.LINE, 200, 200, 600, 200);

            model.UpdateSelectedShapeIndex(600, 200);
            model.MoveShape(600, 600);
            model.UpdateLinesPosition();
            model.UpdateSelectedShapeIndex(400, 400);
            Assert.AreEqual(ShapeType.LINE, model.Shapes[model.SelectedShapeIndex].ShapeType);
        }

        // test
        [TestMethod()]
        public void AddShapeTest()
        {
            Assert.AreEqual(0, model.Shapes.Count);
            
            model.Shapes.Add(new Rectangle());
            model.Shapes.Add(new Ellipse());
            Assert.AreEqual(2, model.Shapes.Count);
            Assert.AreEqual(ShapeType.RECTANGLE, model.Shapes[0].ShapeType);
            Assert.AreEqual(ShapeType.ELLIPSE, model.Shapes[1].ShapeType);
        }

        // test
        [TestMethod()]
        public void RemoveShapeTest()
        {
            Assert.AreEqual(0, model.Shapes.Count);
            
            model.Shapes.Add(new Rectangle());
            model.Shapes.Add(new Ellipse());
            Assert.AreEqual(2, model.Shapes.Count);
            
            model.Shapes.RemoveAt(model.Shapes.Count - 1);
            Assert.AreEqual(1,  model.Shapes.Count);
            Assert.AreEqual(ShapeType.RECTANGLE, model.Shapes[0].ShapeType);
        }

        // test
        [TestMethod()]
        public void UndoCommandTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 10, 10, 30, 30);
            DrawShapeFake(ShapeType.ELLIPSE, 110, 140, 150, 200);
            Assert.AreEqual(2, model.Shapes.Count);
            Assert.AreEqual(ShapeType.ELLIPSE, model.Shapes[1].ShapeType);

            model.UndoCommand();
            Assert.AreEqual(1, model.Shapes.Count);
            Assert.AreEqual(ShapeType.RECTANGLE, model.Shapes[0].ShapeType);
        }

        // test
        [TestMethod()]
        public void RedoCommandTest()
        {
            DrawShapeFake(ShapeType.RECTANGLE, 10, 10, 30, 30);
            DrawShapeFake(ShapeType.ELLIPSE, 110, 140, 150, 200);
            DrawShapeFake(ShapeType.RECTANGLE, 70, 50, 120, 90);
            Assert.AreEqual(3, model.Shapes.Count);
            Assert.AreEqual(ShapeType.RECTANGLE, model.Shapes[2].ShapeType);

            model.UndoCommand();
            model.UndoCommand();
            Assert.AreEqual(1, model.Shapes.Count);
            Assert.AreEqual(ShapeType.RECTANGLE, model.Shapes[0].ShapeType);

            model.RedoCommand();
            Assert.AreEqual(2, model.Shapes.Count);
            Assert.AreEqual(ShapeType.ELLIPSE, model.Shapes[1].ShapeType);
        }

        // test
        [TestMethod()]
        public void NotifyModelChangedTest()
        {
            bool isChanged = false;
            model._modelChanged += () =>
            {
                isChanged = true;
            };

            model.SetDrawingShape(ShapeType.RECTANGLE);
            model.HandlePointerPressed(17, 7);
            model.HandlePointerMoved(37, 64);
            Assert.IsTrue(isChanged);
        }

        // draw shape to model
        public void DrawShapeFake(ShapeType shapeType, double x1, double y1, double x2, double y2)
        {
            model.SetDrawingShape(shapeType);
            model.HandlePointerPressed(x1, y1);
            model.HandlePointerMoved(x2, y2);
            model.HandlePointerReleased(x2, y2);
        }
    }
}