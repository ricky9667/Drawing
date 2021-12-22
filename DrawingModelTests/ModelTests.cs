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

            model.HandlePointerPressed(testX1, testY1);
            model.HandlePointerMoved(testX2, testY2);
            model.HandlePointerReleased(testX2, testY2);
            
            Assert.AreEqual(testX2, model.Hint.X2);
            Assert.AreEqual(testY2, model.Hint.Y2);
            Assert.IsFalse(model.IsPressed);

            Assert.AreEqual(1, model.Shapes.Count);
            Assert.AreEqual(testX1, model.Shapes[0].X1);
            Assert.AreEqual(testY1, model.Shapes[0].Y1);
            Assert.AreEqual(testX2, model.Shapes[0].X2);
            Assert.AreEqual(testY2, model.Shapes[0].Y2);
        }

        // test
        [TestMethod()]
        public void ClearTest()
        {
            Assert.IsFalse(model.IsPressed);
            Assert.AreEqual(0, model.Shapes.Count);

            model.HandlePointerPressed(12, 49);
            DrawShapeFake(ShapeType.LINE, 1, 2, 3, 4);
            DrawShapeFake(ShapeType.RECTANGLE, 5, 6, 7, 8);
            DrawShapeFake(ShapeType.ELLIPSE, 9, 10, 11, 12);
            Assert.AreEqual(3, model.Shapes.Count);

            model.Clear();
            Assert.IsFalse(model.IsPressed);
            Assert.AreEqual(0, model.Shapes.Count);
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