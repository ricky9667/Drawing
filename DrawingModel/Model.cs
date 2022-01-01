using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawingModel
{
    public class Model
    {
        public event ModelChangedEventHandler _modelChanged;
        public delegate void ModelChangedEventHandler();

        private double _firstPointX;
        private double _firstPointY;
        private int _firstClickedShapeIndex;
        private int _selectedShapeIndex = -1;
        private bool _isPressed = false;
        private IShape _hint = null;
        private IState _currentState = null;
        private readonly List<IShape> _shapes = new List<IShape>();
        private readonly CommandManager _commandManager = new CommandManager();

        public Model()
        {
            _currentState = new PointerState(this);
        }

        public double FirstPointX
        {
            get
            {
                return _firstPointX;
            }
        }

        public double FirstPointY
        {
            get
            {
                return _firstPointY;
            }
        }

        public bool IsPressed
        {
            get
            {
                return _isPressed;
            }
        }

        public ShapeType CurrentShapeType
        {
            get
            {
                return _currentState.DrawingShape;
            }
        }

        public IShape Hint
        { 
            get
            {
                return _hint;
            }
        }

        public List<IShape> Shapes
        {
            get
            {
                return _shapes;
            }
        }

        public bool CanUndo
        {
            get
            {
                return _commandManager.CanUndo;
            }
        }

        public bool CanRedo
        {
            get
            {
                return _commandManager.CanRedo;
            }
        }

        public int SelectedShapeIndex
        {
            get
            {
                return _selectedShapeIndex;
            }
        }

        public string SelectedShapeInfo
        {
            get
            {
                const string SELECTED = "Selected : ";
                return (_selectedShapeIndex == -1) ? "" : SELECTED + _shapes[_selectedShapeIndex].ShapeInfo;
            }
        }

        // set current drawing shape and hint shape
        public void SetDrawingShape(ShapeType shapeType)
        {
            _hint = ShapeFactory.CreateShape(shapeType);
            _currentState = CreateStateInstance(shapeType);
        }

        // create state instance
        private IState CreateStateInstance(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.LINE:
                    return new DrawingLineState(this);
                case ShapeType.RECTANGLE:
                    return new DrawingRectangleState(this);
                case ShapeType.ELLIPSE:
                    return new DrawingEllipseState(this);
                default:
                    return new PointerState(this);
            }
        }

        // record first point coordinates on pointer pressed
        public void HandlePointerPressed(double posX, double posY)
        {
            if (posX > 0 && posY > 0)
            {
                _selectedShapeIndex = -1;
                _firstClickedShapeIndex = GetClickedShapeIndex(posX, posY);
                _currentState.HandlePointerPressed(posX, posY);
                _isPressed = (_currentState.DrawingShape != ShapeType.LINE || _firstClickedShapeIndex > -1);
            }
        }

        // record second point coordinates on pointer moved
        public void HandlePointerMoved(double posX, double posY)
        {
            if (_isPressed)
            {
                _currentState.HandlePointerMoved(posX, posY);
                NotifyModelChanged();
            }
        }

        // add hint to saved shapes on pointer released
        public void HandlePointerReleased(double posX, double posY)
        {
            if (_isPressed)
            {
                _isPressed = false;
                _currentState.HandlePointerReleased(posX, posY);
                _hint = null;
                _currentState = new PointerState(this);
                NotifyModelChanged();
            }
        }

        // set first point coordinates
        public void SetFirstPointCoordinates(double posX, double posY)
        {
            _firstPointX = posX;
            _firstPointY = posY;
        }

        // set hint first point coordinates
        public void SetHintFirstPointCoordinates(double posX, double posY)
        {
            if (_hint == null)
                return;

            _hint.X1 = posX;
            _hint.Y1 = posY;
        }

        // set hint second point coordinates
        public void SetHintSecondPointCoordinates(double posX, double posY)
        {
            if (_hint == null)
                return;

            _hint.X2 = posX;
            _hint.Y2 = posY;
        }

        // get shape index if coordinates is in a particular shape
        public int GetClickedShapeIndex(double posX, double posY)
        {
            for (int index = _shapes.Count - 1; index >= 0; index--)
            {
                IShape shape = _shapes[index];
                if (shape.IsPositionInShape(posX, posY))
                {
                    return index;
                }
            }
            return -1;
        }

        // save new selected shape index
        public void UpdateSelectedShapeIndex(double posX, double posY)
        {
            _selectedShapeIndex = GetClickedShapeIndex(posX, posY);
        }

        // add new line to shapes list
        public void AddNewLine(double posX, double posY)
        {
            int secondShapeIndex = GetClickedShapeIndex(posX, posY);
            if (secondShapeIndex == -1)
                return;

            Line hint = new Line();
            hint.FirstShape = _shapes[_firstClickedShapeIndex];
            hint.SecondShape = _shapes[secondShapeIndex];
            hint.LocatePositionByShapes();
            _commandManager.RunCommand(new DrawCommand(this, hint));
        }

        // add new rectangle or ellipse to shapes list
        public void AddNewShape(double posX, double posY)
        {
            IShape hint = ShapeFactory.CreateShape(_currentState.DrawingShape);
            hint.X1 = _firstPointX;
            hint.Y1 = _firstPointY;
            hint.X2 = posX;
            hint.Y2 = posY;
            _commandManager.RunCommand(new DrawCommand(this, hint));
        }

        // clear all saved shapes
        public void Clear()
        {
            _isPressed = false;
            _selectedShapeIndex = -1;
            _commandManager.RunCommand(new ClearCommand(this));
            NotifyModelChanged();
        }

        // add shape
        public void AddShape(IShape shape)
        {
            _shapes.Add(shape);
        }

        // remove shape
        public void RemoveShape()
        {
            _shapes.RemoveAt(_shapes.Count - 1);
        }

        // undo command
        public void UndoCommand()
        {
            _selectedShapeIndex = -1;
            _commandManager.Undo();
        }

        // redo command
        public void RedoCommand()
        {
            _selectedShapeIndex = -1;
            _commandManager.Redo();
        }

        // draw shapes on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.ClearAll();

            foreach (IShape shape in _shapes.Where(shape => shape.ShapeType == ShapeType.LINE).ToList())
                shape.Draw(graphics);

            foreach (IShape shape in _shapes.Where(shape => shape.ShapeType != ShapeType.LINE).ToList())
                shape.Draw(graphics);

            if (_isPressed && _hint != null)
                _hint.Draw(graphics);

            if (_selectedShapeIndex != -1)
                _shapes[_selectedShapeIndex].DrawSelection(graphics);
        }

        // notify observers
        public void NotifyModelChanged()
        {
            if (_modelChanged != null)
                _modelChanged();
        }
    }
}