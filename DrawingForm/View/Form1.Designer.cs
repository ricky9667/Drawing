
namespace DrawingForm
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._canvas = new DrawingForm.DoubleBufferedPanel();
            this._selectionLabel = new System.Windows.Forms.Label();
            this._actionsToolStrip = new System.Windows.Forms.ToolStrip();
            this._rectangleButton = new System.Windows.Forms.Button();
            this._ellipseButton = new System.Windows.Forms.Button();
            this._lineButton = new System.Windows.Forms.Button();
            this._clearButton = new System.Windows.Forms.Button();
            this._canvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // _canvas
            // 
            this._canvas.BackColor = System.Drawing.Color.LightYellow;
            this._canvas.Controls.Add(this._selectionLabel);
            this._canvas.Controls.Add(this._actionsToolStrip);
            this._canvas.Controls.Add(this._rectangleButton);
            this._canvas.Controls.Add(this._ellipseButton);
            this._canvas.Controls.Add(this._lineButton);
            this._canvas.Controls.Add(this._clearButton);
            this._canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this._canvas.Location = new System.Drawing.Point(0, 0);
            this._canvas.Name = "_canvas";
            this._canvas.Size = new System.Drawing.Size(1340, 715);
            this._canvas.TabIndex = 3;
            // 
            // _selectionLabel
            // 
            this._selectionLabel.AutoSize = true;
            this._selectionLabel.Location = new System.Drawing.Point(899, 658);
            this._selectionLabel.Name = "_selectionLabel";
            this._selectionLabel.Size = new System.Drawing.Size(98, 24);
            this._selectionLabel.TabIndex = 4;
            this._selectionLabel.Text = "Selected: ";
            // 
            // _actionsToolStrip
            // 
            this._actionsToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this._actionsToolStrip.Location = new System.Drawing.Point(0, 0);
            this._actionsToolStrip.Name = "_actionsToolStrip";
            this._actionsToolStrip.Size = new System.Drawing.Size(1340, 50);
            this._actionsToolStrip.TabIndex = 3;
            this._actionsToolStrip.Text = "toolStrip1";
            // 
            // _rectangleButton
            // 
            this._rectangleButton.Location = new System.Drawing.Point(397, 60);
            this._rectangleButton.Name = "_rectangleButton";
            this._rectangleButton.Size = new System.Drawing.Size(240, 60);
            this._rectangleButton.TabIndex = 1;
            this._rectangleButton.Text = "Rectangle";
            this._rectangleButton.UseVisualStyleBackColor = true;
            // 
            // _ellipseButton
            // 
            this._ellipseButton.Location = new System.Drawing.Point(707, 60);
            this._ellipseButton.Name = "_ellipseButton";
            this._ellipseButton.Size = new System.Drawing.Size(240, 60);
            this._ellipseButton.TabIndex = 2;
            this._ellipseButton.Text = "Ellipse";
            this._ellipseButton.UseVisualStyleBackColor = true;
            // 
            // _lineButton
            // 
            this._lineButton.Location = new System.Drawing.Point(89, 60);
            this._lineButton.Name = "_lineButton";
            this._lineButton.Size = new System.Drawing.Size(240, 60);
            this._lineButton.TabIndex = 0;
            this._lineButton.Text = "Line";
            this._lineButton.UseVisualStyleBackColor = true;
            this._lineButton.Click += new System.EventHandler(this.HandleLineButtonClick);
            // 
            // _clearButton
            // 
            this._clearButton.Location = new System.Drawing.Point(1019, 60);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(240, 60);
            this._clearButton.TabIndex = 0;
            this._clearButton.Text = "Clear";
            this._clearButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 715);
            this.Controls.Add(this._canvas);
            this.Name = "Form1";
            this.Text = "Form1";
            this._canvas.ResumeLayout(false);
            this._canvas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _rectangleButton;
        private System.Windows.Forms.Button _ellipseButton;
        private System.Windows.Forms.Button _lineButton;
        private DoubleBufferedPanel _canvas;
        private System.Windows.Forms.ToolStrip _actionsToolStrip;
        private System.Windows.Forms.Label _selectionLabel;
    }
}

