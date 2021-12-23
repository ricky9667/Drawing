using System;
using System.Collections.Generic;

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
        private ShapeType _currentShapeType = ShapeType.RECTANGLE;
        private IShape _hint = new Rectangle();
        private readonly List<IShape> _shapes = new List<IShape>();
        private readonly CommandManager _commandManager = new CommandManager();

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
                return _currentShapeType;
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

        // set current drawing shape and hint shape
        public void SetDrawingShape(ShapeType shapeType)
        {
            _currentShapeType = shapeType;
            _hint = ShapeFactory.CreateShape(shapeType);
        }

        // get shape index if coordinates is in a particular shape
        private int GetClickedShapeIndex(double posX, double posY)
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

        // record first point coordinates on pointer pressed
        public void HandlePointerPressed(double posX, double posY)
        {
            if (posX > 0 && posY > 0)
            {
                _selectedShapeIndex = -1;
                _firstClickedShapeIndex = GetClickedShapeIndex(posX, posY);
                if (_currentShapeType != ShapeType.LINE || _firstClickedShapeIndex > -1)
                {
                    _hint.X1 = _firstPointX = posX;
                    _hint.Y1 = _firstPointY = posY;
                    _isPressed = true;
                }
            }
        }

        // record second point coordinates on pointer moved
        public void HandlePointerMoved(double posX, double posY)
        {
            if (_isPressed)
            {
                _hint.X2 = posX;
                _hint.Y2 = posY;
                NotifyModelChanged();
            }
        }

        // add hint to saved shapes on pointer released
        public void HandlePointerReleased(double posX, double posY)
        {
            if (_isPressed)
            {
                _isPressed = false;
                if (_firstPointX == posX && _firstPointY == posY)
                {
                    _selectedShapeIndex = GetClickedShapeIndex(posX, posY);
                    return;
                }

                if (_currentShapeType == ShapeType.LINE)
                    AddNewLine(posX, posY);
                else
                    AddNewShape(posX, posY);

                NotifyModelChanged();
            }
        }

        // add new line to shapes list
        private void AddNewLine(double posX, double posY)
        {
            int secondShapeIndex = GetClickedShapeIndex(posX, posY);
            if (secondShapeIndex == -1)
            {
                return;
            }

            Line hint = new Line();
            hint.FirstShape = _shapes[_firstClickedShapeIndex];
            hint.SecondShape = _shapes[secondShapeIndex];
            hint.LocatePositionByShapes();
            _commandManager.RunCommand(new DrawCommand(this, hint));
        }

        // add new rectangle or ellipse to shapes list
        private void AddNewShape(double posX, double posY)
        {
            IShape hint = ShapeFactory.CreateShape(_currentShapeType);
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
            _commandManager.Undo();
        }

        // redo command
        public void RedoCommand()
        {
            _commandManager.Redo();
        }

        // draw shapes on canvas
        public void Draw(IGraphics graphics)
        {
            graphics.ClearAll();
            
            foreach (IShape shape in _shapes)
                if (shape.ShapeType == ShapeType.LINE)
                    shape.Draw(graphics);

            foreach (IShape shape in _shapes)
                if (shape.ShapeType != ShapeType.LINE)
                    shape.Draw(graphics);
            
            if (_isPressed)
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