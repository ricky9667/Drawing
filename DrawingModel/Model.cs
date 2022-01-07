﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrawingModel
{
    public class Model
    {
        public event ModelChangedEventHandler _modelChanged;
        public delegate void ModelChangedEventHandler();

        //private const char SPACE = ' ';
        //private const string FILE_NAME = "Shapes.txt";
        //private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), FILE_NAME);
        private double _firstPointX;
        private double _firstPointY;
        private int _firstClickedShapeIndex;
        private int _selectedShapeIndex = -1;
        private bool _isPressed = false;
        private IShape _hint = null;
        private IState _currentState = null;
        private readonly List<IShape> _shapes = new List<IShape>();
        private readonly CommandManager _commandManager = new CommandManager();
        private readonly FileHandler _fileHandler;
        public Model()
        {
            _currentState = new PointerState(this);
            _fileHandler = new FileHandler(this);
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
            _currentState = StateFactory.CreateState(this, shapeType);
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
            if (_hint != null)
            {
                _hint.X1 = posX;
                _hint.Y1 = posY;
            }
        }

        // set hint second point coordinates
        public void SetHintSecondPointCoordinates(double posX, double posY)
        {
            if (_hint != null)
            {
                _hint.X2 = posX;
                _hint.Y2 = posY;
            }
        }

        // get shape index if coordinates is in a particular shape
        private int GetClickedShapeIndex(double posX, double posY)
        {
            for (int index = _shapes.Count - 1; index >= 0; index--)
            {
                IShape shape = _shapes[index];
                if (shape.ShapeType == ShapeType.LINE)
                    continue;
                if (shape.IsPositionInShape(posX, posY))
                    return index; 
            }
            for (int index = _shapes.Count - 1; index >= 0; index--)
            {
                IShape shape = _shapes[index];
                if (shape.ShapeType != ShapeType.LINE)
                    continue;
                if (shape.IsPositionInShape(posX, posY))
                    return index;
            }
            return -1;
        }

        // save new selected shape index
        public void UpdateSelectedShapeIndex(double posX, double posY)
        {
            _selectedShapeIndex = GetClickedShapeIndex(posX, posY);
        }

        // move selected shape
        public void UpdateMovingShape(double posX, double posY)
        {
            if (_selectedShapeIndex > -1)
            {
                IShape shape = _shapes[_selectedShapeIndex];
                shape.MoveShapeByOffset(posX - _firstPointX, posY - _firstPointY);
                UpdateLinesPosition();
            }
        }

        // move shape
        public void MoveShape(double posX, double posY)
        {
            if (_selectedShapeIndex > -1)
            {
                IShape shape = _shapes[_selectedShapeIndex];
                _commandManager.RunCommand(new MoveCommand(this, shape, posX - _firstPointX, posY - _firstPointY));
            }
        }

        // locate lines
        public void UpdateLinesPosition()
        {
            foreach (IShape shape in _shapes.Where(shape => shape.ShapeType == ShapeType.LINE).ToList())
                shape.UpdateSavedPosition();
        }

        // add new line to shapes list
        public void AddNewLine(double posX, double posY)
        {
            int shapeIndex = GetClickedShapeIndex(posX, posY);
            if (shapeIndex == -1 || _shapes[shapeIndex].ShapeType == ShapeType.LINE)
                return;

            Line hint = new Line();
            hint.FirstShape = _shapes[_firstClickedShapeIndex];
            hint.SecondShape = _shapes[shapeIndex];
            hint.UpdateSavedPosition();
            _commandManager.RunCommand(new DrawCommand(this, hint));
        }

        // add new rectangle or ellipse to shapes list
        public void AddNewShape(double posX, double posY)
        {
            const double MIN_AREA = 10;
            if (Math.Abs((_firstPointX - posX) * (_firstPointY - posY)) < MIN_AREA) // avoid shapes that are too small
                return;

            IShape hint = ShapeFactory.CreateShape(_currentState.DrawingShape);
            hint.SetShapeCoordinates(_firstPointX, _firstPointY, posX, posY);
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
            List<IShape> shapes = new List<IShape>();
            shapes.AddRange(_shapes.Where(shape => shape.ShapeType == ShapeType.LINE).ToList());
            shapes.AddRange(_shapes.Where(shape => shape.ShapeType != ShapeType.LINE).ToList());

            foreach (IShape shape in shapes)
                shape.Draw(graphics);
            if (_isPressed && _hint != null)
                _hint.Draw(graphics);
            if (_selectedShapeIndex != -1)
                _shapes[_selectedShapeIndex].DrawSelection(graphics);
        }

        // save shapes image and data
        public void SaveShapes()
        {
            _fileHandler.SaveShapesToLocal();
            _fileHandler.UploadShapesFile();
        }

        // load shapes data
        public void LoadShapes()
        {
            _fileHandler.DownloadShapesFile();
            _fileHandler.LoadShapesFromLocal();
            _commandManager.Clear();
            _selectedShapeIndex = -1;
            NotifyModelChanged();
        }

        // notify observers
        public void NotifyModelChanged()
        {
            if (_modelChanged != null)
                _modelChanged();
        }
    }
}