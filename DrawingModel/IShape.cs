using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingModel
{
    public interface IShape
    {
        double X1
        {
            get; set;
        }

        double Y1
        {
            get; set;
        }

        double X2
        {
            get; set;
        }

        double Y2
        {
            get; set;
        }

        // draw shape on canvas
        void Draw(IGraphics graphics);
    }
}