using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingModel;

namespace DrawingForm
{
    public partial class Form1 : Form
    {
        private readonly Model _model;
        private readonly PresentationModel.PresentationModel _presentationModel;
        readonly ToolStripButton _undoToolStripButton = new ToolStripButton("Undo", null);
        readonly ToolStripButton _redoToolStripButton = new ToolStripButton("Redo", null);
        public Form1()
        {
            _model = new Model();
            _presentationModel = new PresentationModel.PresentationModel(_model, _canvas);
            _model._modelChanged += HandleModelChanged;

            InitializeComponent();
            InitializeEvents();

            _selectionLabel.Text = "";
            _actionsToolStrip.Items.Add(_undoToolStripButton);
            _actionsToolStrip.Items.Add(_redoToolStripButton);
            _undoToolStripButton.Enabled = _model.CanUndo;
            _redoToolStripButton.Enabled = _model.CanRedo;
        }

        // add event to controls
        private void InitializeEvents()
        {
            _undoToolStripButton.Click += HandleUndoButtonClick;
            _redoToolStripButton.Click += HandleRedoButtonClick;

            _canvas.MouseDown += HandleCanvasPressed;
            _canvas.MouseUp += HandleCanvasReleased;
            _canvas.MouseMove += HandleCanvasMoved;
            _canvas.Paint += HandleCanvasPaint;

            _lineButton.Click += HandleLineButtonClick;
            _rectangleButton.Click += HandleRectangleButtonClick;
            _ellipseButton.Click += HandleEllipseButtonClick;
            _clearButton.Click += HandleClearButtonClick;
            _saveButton.Click += HandleSaveButtonClick;
            _loadButton.Click += HandleLoadButtonClick;
        }

        // undo last command
        private void HandleUndoButtonClick(object sender, EventArgs e)
        {
            _model.UndoCommand();
            HandleModelChanged();
        }

        // redo last command
        private void HandleRedoButtonClick(object sender, EventArgs e)
        {
            _model.RedoCommand();
            HandleModelChanged();
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
        private void HandleRectangleButtonClick(object sender, EventArgs e)
        {
            _model.SetDrawingShape(ShapeType.RECTANGLE);
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = false;
            _ellipseButton.Enabled = true;
        }

        // switch to draw ellipse mode
        private void HandleEllipseButtonClick(object sender, EventArgs e)
        {
            _model.SetDrawingShape(ShapeType.ELLIPSE);
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = true;
            _ellipseButton.Enabled = false;
        }

        // clear drawings on canvas
        private void HandleClearButtonClick(object sender, EventArgs e)
        {
            _model.Clear();
            _lineButton.Enabled = true;
            _rectangleButton.Enabled = true;
            _ellipseButton.Enabled = true;
        }

        // save shapes
        async private void HandleSaveButtonClick(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                _model.SaveShapes();
            });
        }

        // load shapes
        private void HandleLoadButtonClick(object sender, EventArgs e)
        {
            SetScreenEnabled(false);
            _model.LoadShapes();
            HandleModelChanged();
            SetScreenEnabled(true);
        }

        // set components enabled
        private void SetScreenEnabled(bool flag)
        {
            _canvas.Enabled = flag;
            _lineButton.Enabled = flag;
            _rectangleButton.Enabled = flag;
            _ellipseButton.Enabled = flag;
            _clearButton.Enabled = flag;
            _saveButton.Enabled = flag;
            _loadButton.Enabled = flag;
            _undoToolStripButton.Enabled = flag;
            _redoToolStripButton.Enabled = flag;
        }

        // event when canvas is pressed
        private void HandleCanvasPressed(object sender, MouseEventArgs e)
        {
            _model.HandlePointerPressed(e.X, e.Y);
        }

        // event when mouse is moving
        private void HandleCanvasMoved(object sender, MouseEventArgs e)
        {
            _model.HandlePointerMoved(e.X, e.Y);
        }

        // event when canvas press on canvas is release
        private void HandleCanvasReleased(object sender, MouseEventArgs e)
        {
            if (_model.IsPressed)
            {
                _lineButton.Enabled = true;
                _rectangleButton.Enabled = true;
                _ellipseButton.Enabled = true;
            }
            _model.HandlePointerReleased(e.X, e.Y);
            HandleModelChanged();
        }

        // paint drawings on canvas
        private void HandleCanvasPaint(object sender, PaintEventArgs e)
        {
            _presentationModel.Draw(e.Graphics);
        }

        // model change
        private void HandleModelChanged()
        {
            _undoToolStripButton.Enabled = _model.CanUndo;
            _redoToolStripButton.Enabled = _model.CanRedo;
            _selectionLabel.Text = _presentationModel.SelectedShapeInfo;
            Invalidate(true);
        }
    }
}