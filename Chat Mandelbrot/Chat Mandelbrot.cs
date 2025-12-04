
//alle systemen die nodig zijn om dit programma te laten draaien.
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

// De Form waar alles wordt opgeplakt.
Form beeldscherm = new Form();
beeldscherm.Text = "Mandelbrot";
beeldscherm.BackColor = Color.LightCyan;
beeldscherm.ClientSize = new Size(820, 430);

// De layout van de bitmap.
int bitmapBreedte = 400;
int bitmapHoogte = 400;
Bitmap mandelbrot = new Bitmap(bitmapBreedte, bitmapHoogte);

//Label waar de bitmap 'opgeplakt' kan worden.

Label achterbitmap  = new Label();
achterbitmap.Location = new Point(390, 10);
achterbitmap.Size = new Size(bitmapBreedte, bitmapHoogte);
achterbitmap.Image = mandelbrot;
achterbitmap.ImageAlign = ContentAlignment.TopLeft;
beeldscherm.Controls.Add(achterbitmap);

// De tekstvelden en labels voor de invoer.
int AfstandRand1 = 10;
Label lbl_mx = new Label() { Text = "X-waarde middelpunt:", Location = new Point(AfstandRand1, 10), AutoSize = true };
Label lbl_my = new Label() { Text = "Y-waarde middelpunt:", Location = new Point(AfstandRand1, 50), AutoSize = true };
Label lbl_schaal = new Label() { Text = "Schaal:", Location = new Point(AfstandRand1, 90), AutoSize = true };
Label lbl_max = new Label() { Text = "Maximale herhalingen:", Location = new Point(AfstandRand1, 130), AutoSize = true };

int AfstandRand2 = 180;
TextBox invoer_mx = new TextBox() { Location = new Point(AfstandRand2, 10), Text = "0" };
TextBox invoer_my = new TextBox() { Location = new Point(AfstandRand2, 50), Text = "0" };
TextBox invoer_schaal = new TextBox() { Location = new Point(AfstandRand2, 90), Text = "1" };
TextBox invoer_max = new TextBox() { Location = new Point(AfstandRand2, 130), Text = "400" };

beeldscherm.Controls.Add(lbl_mx);
beeldscherm.Controls.Add(lbl_my);
beeldscherm.Controls.Add(lbl_schaal);
beeldscherm.Controls.Add(lbl_max);
beeldscherm.Controls.Add(invoer_mx);
beeldscherm.Controls.Add(invoer_my);
beeldscherm.Controls.Add(invoer_schaal);
beeldscherm.Controls.Add(invoer_max);

// De besturing voor de kleurcomponenten.
TrackBar rood = new TrackBar();
TrackBar groen = new TrackBar();
TrackBar blauw = new TrackBar();

rood.Minimum = groen.Minimum = blauw.Minimum = 0;
rood.Maximum = groen.Maximum = blauw.Maximum = 255;

rood.Location = new Point(AfstandRand1, 220);
groen.Location = new Point(AfstandRand1, 290);
blauw.Location = new Point(AfstandRand1, 360);

rood.Size = groen.Size = blauw.Size = new Size(200, 20);

Label lbl_rood = new Label() { Text = "Rood-component:", Location = new Point(AfstandRand1, 200), AutoSize=true};
Label lbl_groen = new Label() { Text = "Groen-component:", Location = new Point(AfstandRand1, 270), AutoSize=true };
Label lbl_blauw = new Label() { Text = "Blauw-component:", Location = new Point(AfstandRand1, 340), AutoSize = true };

beeldscherm.Controls.Add(rood);
beeldscherm.Controls.Add(groen);
beeldscherm.Controls.Add(blauw);
beeldscherm.Controls.Add(lbl_rood);
beeldscherm.Controls.Add(lbl_groen);
beeldscherm.Controls.Add(lbl_blauw);

Button tekenknop = new Button() { Text = "Go!", Location = new Point(195, 160), AutoSize = true };
beeldscherm.Controls.Add(tekenknop);

// De minimale en maximale waarde van de x en de y voor de afbeelding.
double MininmaalX = -2;
double MaximaalX = 2;
double MinimaalY = -2;
double MaximaalY = 2;

// Het kleuren palet.
Color[] kleuren = new Color[]
{
    Color.Blue,
    Color.Green,
    Color.Yellow,
    Color.Orange,
    Color.Red,
    Color.Purple
};

// De functie van het mandel getal zelf.
int MandelGetal(double x, double y, double maxIter)
{
    double a = 0, b = 0;
    int teller = 0;

    while (a * a + b * b < 4 && teller < maxIter)
    {
        double aa = a * a - b * b + x;
        double bb = 2 * a * b + y;

        a = aa;
        b = bb;
        teller++;
    }
    return teller;
}

//De kleur functie die wordt gebruikt in het mandelbrot
Color functieKleur(int iteratie)
{
    double maxIter = double.Parse(invoer_max.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
    double t = iteratie / maxIter;

    int index1 = (int)(t * (kleuren.Length - 1));
    int index2 = Math.Min(index1 + 1, kleuren.Length - 1);
    double fractie = t * (kleuren.Length - 1) - index1;

    int r = Math.Min(255, kleuren[index1].R + (int)(fractie * (kleuren[index2].R - kleuren[index1].R)) + rood.Value);
    int g = Math.Min(255, kleuren[index1].G + (int)(fractie * (kleuren[index2].G - kleuren[index1].G)) + groen.Value);
    int b = Math.Min(255, kleuren[index1].B + (int)(fractie * (kleuren[index2].B - kleuren[index1].B)) + blauw.Value);

    return Color.FromArgb(r, g, b);
}

// De berekening die voor de afbeelding zorgt.
void TekenMandelbrot(object o, EventArgs e)
{
    double mx = double.Parse(invoer_mx.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
    double my = double.Parse(invoer_my.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
    double schaal = double.Parse(invoer_schaal.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
    double maxIter = double.Parse(invoer_max.Text.Replace(',', '.'), CultureInfo.InvariantCulture);


    MininmaalX = mx - 2 * schaal;
    MaximaalX = mx + 2 * schaal;
    MinimaalY = my - 2 * schaal;
    MaximaalY = my + 2 * schaal;

    for (int px = 0; px < bitmapBreedte; px++)
    {
        for (int py = 0; py < bitmapHoogte; py++)
        {
            double x = MininmaalX + px * (MaximaalX - MininmaalX) / bitmapBreedte;
            double y = MinimaalY + py * (MaximaalY - MinimaalY) / bitmapHoogte;

            int iter = MandelGetal(x, y, maxIter);
            mandelbrot.SetPixel(px, py, functieKleur(iter));
        }
    }
    achterbitmap.Invalidate();
}

// De functie om in te zoomen
void ZoomIn(object o, MouseEventArgs e)
{
    if (e.Button != MouseButtons.Left) return;

    double klik_x = MininmaalX + (e.X * (MaximaalX - MininmaalX) / bitmapBreedte);
    double klik_y = MinimaalY + (e.Y * (MaximaalY - MinimaalY) / bitmapHoogte);

    double factor = 0.5;

    double nieuwBereikX = (MaximaalX - MininmaalX) * factor;
    double nieuwBereikY = (MaximaalY - MinimaalY) * factor;

    MininmaalX = klik_x - nieuwBereikX / 2;
    MaximaalX = klik_x + nieuwBereikX / 2;
    MinimaalY = klik_y - nieuwBereikY / 2;
    MaximaalY = klik_y + nieuwBereikY / 2;

    invoer_mx.Text = ((MininmaalX + MaximaalX) / 2).ToString("G17", CultureInfo.InvariantCulture);
    invoer_my.Text = ((MinimaalY + MaximaalY) / 2).ToString("G17", CultureInfo.InvariantCulture);
    invoer_schaal.Text = ((MaximaalX - MininmaalX) / 4).ToString("G17", CultureInfo.InvariantCulture);

    TekenMandelbrot(o, e);
}

// De functie om uit te zoomen
void ZoomOut(object o, MouseEventArgs e)
{
    if (e.Button != MouseButtons.Right) return;

    double klik_x = MininmaalX + (e.X * (MaximaalX - MininmaalX) / bitmapBreedte);
    double klik_y = MinimaalY + (e.Y * (MaximaalY - MinimaalY) / bitmapHoogte);

    double factor = 0.5;

    double nieuwBereikX = (MaximaalX - MininmaalX) / factor;
    double nieuwBereikY = (MaximaalY - MinimaalY) / factor;

    MininmaalX = klik_x - nieuwBereikX / 2;
    MaximaalX = klik_x + nieuwBereikX / 2;
    MinimaalY = klik_y - nieuwBereikY / 2;
    MaximaalY = klik_y + nieuwBereikY / 2;

    invoer_mx.Text = ((MininmaalX + MaximaalX) / 2).ToString("G17", CultureInfo.InvariantCulture);
    invoer_my.Text = ((MinimaalY + MaximaalY) / 2).ToString("G17", CultureInfo.InvariantCulture);
    invoer_schaal.Text = ((MaximaalX - MininmaalX) / 4).ToString("G17", CultureInfo.InvariantCulture);

    TekenMandelbrot(o, e);
}

// Knopjes van de voorbeelden en dan een waarden ingevuld zodat je een mooie mandelbrot ziet
Button voorbeeld1 = new Button()
{
    Text = "Voorbeeld 1.",
    Location = new Point(250, 250),
    Size = new Size(120, 30)
};
beeldscherm.Controls.Add(voorbeeld1);

Button voorbeeld2 = new Button()
{
    Text = "Voorbeeld 2.",
    Location = new Point(250, 320),
    Size = new Size(120, 30)
};
beeldscherm.Controls.Add(voorbeeld2);

void Voorbeeld1Tekenen(object o, EventArgs e)
{
    invoer_mx.Text = "-0.73341406";
    invoer_my.Text = "-0.28823";
    invoer_schaal.Text = "0.00068359";
    invoer_max.Text = "400";

    rood.Value = 29;
    groen.Value = 200;
    blauw.Value = 100;

    TekenMandelbrot(o, e);
}

void Voorbeeld2Tekenen(object o, EventArgs e)
{
    invoer_mx.Text = "0.12538665771484";
    invoer_my.Text = "0.63108703613281";
    invoer_schaal.Text = "6.103515625E-05";
    invoer_max.Text = "400";

    rood.Value = 0;
    groen.Value = 0;
    blauw.Value = 0;

    TekenMandelbrot(o, e);
}


// De Handlers koppelen zodat er ook daadwerkelijk iets gebeurt

voorbeeld1.Click += Voorbeeld1Tekenen;
voorbeeld2.Click += Voorbeeld2Tekenen;

tekenknop.Click += TekenMandelbrot;
achterbitmap.MouseDown += ZoomIn;
achterbitmap.MouseDown += ZoomOut;

TekenMandelbrot(null, EventArgs.Empty);

Application.Run(beeldscherm);
