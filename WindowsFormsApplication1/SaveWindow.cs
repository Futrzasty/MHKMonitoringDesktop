using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MHKITMonitoring1
{
    static class SaveWindow
    {
        public static void GeometryFromString(string thisWindowGeometry, Form formIn)
        {
            if (string.IsNullOrEmpty(thisWindowGeometry) == true)
            {
                return;
            }
            string[] numbers = thisWindowGeometry.Split('|');
            string windowString = numbers[4];
            if (windowString == "Normal")
            {
                Point windowPoint = new Point(int.Parse(numbers[0]),
                    int.Parse(numbers[1]));
                Size windowSize = new Size(int.Parse(numbers[2]),
                    int.Parse(numbers[3]));

                if (ScreensGeometryToString() == numbers[7])
                {
                    formIn.Location = windowPoint;
                    formIn.Size = windowSize;
                    formIn.StartPosition = FormStartPosition.Manual;
                    formIn.WindowState = FormWindowState.Normal;
                }
            }
            else if (windowString == "Maximized")
            {
                formIn.Location = new Point(int.Parse(numbers[5]), int.Parse(numbers[6]));
                formIn.StartPosition = FormStartPosition.Manual;
                formIn.WindowState = FormWindowState.Maximized;
            }
        }

        public static string GeometryToString(Form mainForm)
        {
            return mainForm.Location.X.ToString() + "|" +   //0
                mainForm.Location.Y.ToString() + "|" +      //1
                mainForm.Size.Width.ToString() + "|" +      //2
                mainForm.Size.Height.ToString() + "|" +     //3
                mainForm.WindowState.ToString() + "|" +     //4
                (mainForm.Location.X + mainForm.Size.Width / 2).ToString() + "|" +  //5
                (mainForm.Location.Y + mainForm.Size.Height / 2).ToString() + "|" + //6
                ScreensGeometryToString();                  //7
        }

        public static string ScreensGeometryToString()
        {
            string screensGeometry = "";
            foreach (Screen s in Screen.AllScreens)
                screensGeometry += s.WorkingArea;
            return screensGeometry;
        }
    }
}
