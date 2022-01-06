using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using DrawingModel;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DrawingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Model _model;
        private readonly PresentationModel.PresentationModel _presentationModel;
        private readonly double _width = 1366;
        private readonly double _height = 786;
        public MainPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(_width, _height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            InitializeComponent();
            InitializeEvents();

            _model = new Model();
            _presentationModel = new PresentationModel.PresentationModel(_model, _canvas);
            _model._modelChanged += HandleModelChanged;
            _undoButton.IsEnabled = _model.CanUndo;
            _redoButton.IsEnabled = _model.CanRedo;
            _selectionTextBlock.Text = "";
        }

        // setup component events
        private void InitializeEvents()
        {
            _undoButton.Click += HandleUndoButtonClick;
            _redoButton.Click += HandleRedoButtonClick;

            _canvas.PointerPressed += HandleCanvasPressed;
            _canvas.PointerReleased += HandleCanvasReleased;
            _canvas.PointerMoved += HandleCanvasMoved;

            _clearButton.Click += HandleClearButtonClick;
            _lineButton.Click += HandleLineButtonClick;
            _rectangleButton.Click += HandleRectangleButtonClick;
            _ellipseButton.Click += HandleEllipseButtonClick;
            _saveButton.Click += HandleSaveButtonClick;
            _loadButton.Click += HandleLoadButtonClick;
        }

        // undo last command
        private void HandleUndoButtonClick(object sender, RoutedEventArgs e)
        {
            _model.UndoCommand();
            HandleModelChanged();
        }

        // redo last command
        private void HandleRedoButtonClick(object sender, RoutedEventArgs e)
        {
            _model.RedoCommand();
            HandleModelChanged();
        }

        // clear drawings on canvas
        private void HandleClearButtonClick(object sender, RoutedEventArgs e)
        {
            _model.Clear();
            _lineButton.IsEnabled = true;
            _rectangleButton.IsEnabled = true;
            _ellipseButton.IsEnabled = true;
        }

        // switch to draw line mode
        private void HandleLineButtonClick(object sender, RoutedEventArgs e)
        {
            _model.SetDrawingShape(ShapeType.LINE);
            _lineButton.IsEnabled = false;
            _rectangleButton.IsEnabled = true;
            _ellipseButton.IsEnabled = true;
        }

        // switch to draw rectangle mode
        private void HandleRectangleButtonClick(object sender, RoutedEventArgs e)
        {
            _model.SetDrawingShape(ShapeType.RECTANGLE);
            _lineButton.IsEnabled = true;
            _rectangleButton.IsEnabled = false;
            _ellipseButton.IsEnabled = true;
        }

        // switch to draw ellipse mode
        private void HandleEllipseButtonClick(object sender, RoutedEventArgs e)
        {
            _model.SetDrawingShape(ShapeType.ELLIPSE);
            _lineButton.IsEnabled = true;
            _rectangleButton.IsEnabled = true;
            _ellipseButton.IsEnabled = false;
        }

        // save shapes
        private async void HandleSaveButtonClick(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                _model.SaveShapes();
            });
        }

        // load shapes
        private void HandleLoadButtonClick(object sender, RoutedEventArgs e)
        {
            SetScreenEnabled(false);
            _undoButton.IsEnabled = false;
            _redoButton.IsEnabled = false;
            _model.LoadShapes();
            HandleModelChanged();
            SetScreenEnabled(true);
        }

        // set components enabled
        private void SetScreenEnabled(bool flag)
        {
            _canvas.IsTapEnabled = _canvas.IsHoldingEnabled = flag;
            _lineButton.IsEnabled = flag;
            _rectangleButton.IsEnabled = flag;
            _ellipseButton.IsEnabled = flag;
            _clearButton.IsEnabled = flag;
            _saveButton.IsEnabled = flag;
            _loadButton.IsEnabled = flag;
        }

        // event when canvas is pressed
        public void HandleCanvasPressed(object sender, PointerRoutedEventArgs e)
        {
            _model.HandlePointerPressed(e.GetCurrentPoint(_canvas).Position.X, e.GetCurrentPoint(_canvas).Position.Y);
        }

        // event when mouse is moving
        public void HandleCanvasMoved(object sender, PointerRoutedEventArgs e)
        {
            _model.HandlePointerMoved(e.GetCurrentPoint(_canvas).Position.X, e.GetCurrentPoint(_canvas).Position.Y);
        }

        // event when canvas press on canvas is release
        public void HandleCanvasReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_model.IsPressed)
            {
                _lineButton.IsEnabled = true;
                _rectangleButton.IsEnabled = true;
                _ellipseButton.IsEnabled = true;
            }
            _model.HandlePointerReleased(e.GetCurrentPoint(_canvas).Position.X, e.GetCurrentPoint(_canvas).Position.Y);
            HandleModelChanged();
        }

        // paint drawings on canvas
        public void HandleModelChanged()
        {
            _undoButton.IsEnabled = _model.CanUndo;
            _redoButton.IsEnabled = _model.CanRedo;
            _selectionTextBlock.Text = _presentationModel.SelectedShapeInfo;
            _presentationModel.Draw();
        }
    }
}