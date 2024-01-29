using AsignacionesEstudiantiles.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using AsignacionesEstudiantiles.Data;

namespace AsignacionesEstudiantiles.Utils
{
    public class DrawOverImageUtil
    {

        public static void Write(List<ProgramaModel> model, GetData getData)
        {
            string s = string.Empty;

            model = model.OrderBy(x => x.orden).ToList();

            int contador = 1;
            CultureInfo ci = new CultureInfo("es-ES");
            ci = new CultureInfo("es-ES");
            foreach (var item in model)
            {
                Bitmap bitMapImage = new(".\\AsignacionesFiles\\PLANTILLA_ASIGNACIONES.JPG");
                Graphics graphicImage = Graphics.FromImage(bitMapImage);

                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                graphicImage.DrawString(item.nombre,
                  new Font("Arial", 12, FontStyle.Bold),
                  SystemBrushes.WindowText, new Point(89, 57));

                graphicImage.DrawString(item.ayudante,
                  new Font("Arial", 12, FontStyle.Bold),
                  SystemBrushes.WindowText, new Point(103, 88));

                graphicImage.DrawString((DateTime.ParseExact(item.fecha,"dd/MM/yyyy",CultureInfo.InvariantCulture)).ToString("dd MMMM, yyyy",ci),
                  new Font("Arial", 12, FontStyle.Bold),
                  SystemBrushes.WindowText, new Point(77, 120));

                graphicImage.DrawString(item.asignacion,
                  new Font("Arial", 9, FontStyle.Bold),
                  SystemBrushes.WindowText, new Point(169, 150));

                

                string path = $".\\AsignacionesFiles\\{contador}_{item.asignacion}_{(DateTime.ParseExact(item.fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString("dd-MMMM-yyyy",ci)}.JPG";

                bitMapImage.Save(path, ImageFormat.Jpeg);

                getData.InsertPrograma(item, path);

                graphicImage.Dispose();
                bitMapImage.Dispose();

            }
        }

    }
}
