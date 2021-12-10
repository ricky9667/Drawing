using System;
using System.Windows.Forms;
using DrawingModel;

namespace DrawingForm
{
    public partial class Form1 : Form
    {
        private readonly Model _model;
        private readonly PresentationModel.PresentationModel _presentationModel;
        public Form1()
        {
            _model = new Model();
            _presentationModel = new PresentationModel.PresentationModel(_model, _canvas);
            _model._modelChanged += HandleModelChanged;

            InitializeComponent();
            InitializeEvents();
        }

        // add event to controls
        private void InitializeEvents()
        {
            _canvas.MouseDown += HandleCanvasPressed;
            _canvas.MouseUp += HandleCanvasReleased;
            _canvas.MouseMove += HandleCanvasMoved;
            _canvas.Paint += HandleCanvasPaint;

            _lineButton.Click += HandleLineButtonClick;
            _rectangleButton.Click += HandleRectangleButtonClick;
            _ellipseButton.Click += HandleEllipseButtonClick;
            _clearButton.Click += HandleClearButtonClick;
        }

        // switch to draw line mode
        private void HandleLineButtonClick(object sender, EventArgs e)
        {
            _model.SetDrawingShape(ShapeType.LINE);
            _lineButton.Enabled = false;
            _rectangleButton.Enabled = true;
            _ellipseButton.Enabled = true;
        }

        // switch to draw rectangle mode
        public void HandleRectangleButtonClick(object sender, EventArgs e)
        {
            _model.SetDrawingShape(ShapeType.RECTANGLE);
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = false;
            _ellipseButton.Enabled = true;
        }

        // switch to draw ellipse mode
        public void HandleEllipseButtonClick(object sender, EventArgs e)
        {
            _model.SetDrawingShape(ShapeType.ELLIPSE);
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = true;
            _ellipseButton.Enabled = false;
        }

        // clear drawings on canvas
        public void HandleClearButtonClick(object sender, EventArgs e)
        {
            _model.Clear();
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = true;
            _ellipseButton.Enabled = true;
        }

        // event when canvas is pressed
        public void HandleCanvasPressed(object sender, MouseEventArgs e)
        {
            _model.HandlePointerPressed(e.X, e.Y);
        }

        // event when mouse is moving
        public void HandleCanvasMoved(object sender, MouseEventArgs e)
        {
            _model.HandlePointerMoved(e.X, e.Y);
        }

        // event when canvas press on canvas is release
        public void HandleCanvasReleased(object sender, MouseEventArgs e)
        {
            if (_model.IsPressed)
            {
                _lineButton.Enabled = true;
                _rectangleButton.Enabled = true;
                _ellipseButton.Enabled = true;
            }
            _model.HandlePointerReleased(e.X, e.Y);
            
        }

        // paint drawings on canvas
        public void HandleCanvasPaint(object sender, PaintEventArgs e)
        {
            _presentationModel.Draw(e.Graphics);
        }

        // model change
        public void HandleModelChanged()
        {
            Invalidate(true);
        }
    }
}