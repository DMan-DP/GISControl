using System.Drawing;

namespace GISControl.Model.ColorPalette
{
    public enum Palette { Green, Rainbow, RedBlue }

    public class ColorPalette
    { 
        private struct ImagePalette
        {
            public double value;
            public Color color;
        }

        private ImagePalette[] colors = new ImagePalette[20];

        public ColorPalette(double[] values, Palette palette = Palette.Rainbow)
        {
            for (int i = 0; i < 20; i++)
            {
                colors[i].value = values[i];
                colors[i].color = Color.Black;
            }

            SetPalette(palette);
        }

        public void SetPalette(Palette palette)
        {
            if (palette == Palette.Green)
            {
                colors[0].color = Color.FromArgb(4, 18, 60);
                colors[1].color = Color.FromArgb(255, 255, 255);
                colors[2].color = Color.FromArgb(196, 186, 166);
                colors[3].color = Color.FromArgb(180, 149, 107);
                colors[4].color = Color.FromArgb(164, 130, 76);
                colors[5].color = Color.FromArgb(148, 114, 62);
                colors[6].color = Color.FromArgb(124, 158, 44);
                colors[7].color = Color.FromArgb(148, 182, 20);
                colors[8].color = Color.FromArgb(116, 170, 4);
                colors[9].color = Color.FromArgb(100, 162, 4);
                colors[10].color = Color.FromArgb(84, 150, 4);
                colors[11].color = Color.FromArgb(60, 134, 4);
                colors[12].color = Color.FromArgb(28, 114, 4);
                colors[13].color = Color.FromArgb(4, 96, 4);
                colors[14].color = Color.FromArgb(4, 72, 4);
                colors[15].color = Color.FromArgb(4, 56, 4);
                colors[16].color = Color.FromArgb(4, 40, 4);
                colors[17].color = Color.FromArgb(4, 18, 4);
                colors[18].color = Color.FromArgb(4, 18, 4);
                colors[19].color = Color.FromArgb(4, 18, 4);
            }

            if (palette == Palette.Rainbow)
            {
                colors[0].color = Color.FromArgb(0, 0, 255);
                colors[1].color = Color.FromArgb(25, 0, 230);
                colors[2].color = Color.FromArgb(51, 0, 204);
                colors[3].color = Color.FromArgb(76, 0, 179);
                colors[4].color = Color.FromArgb(102, 0, 153);
                colors[5].color = Color.FromArgb(127, 0, 128);
                colors[6].color = Color.FromArgb(153, 0, 102);
                colors[7].color = Color.FromArgb(178, 0, 77);
                colors[8].color = Color.FromArgb(204, 0, 51);
                colors[9].color = Color.FromArgb(229, 0, 26);
                colors[10].color = Color.FromArgb(255, 51, 0); ;
                colors[11].color = Color.FromArgb(255, 85, 0);
                colors[12].color = Color.FromArgb(255, 117, 0);
                colors[13].color = Color.FromArgb(255, 153, 0);
                colors[14].color = Color.FromArgb(255, 200, 0);
                colors[15].color = Color.FromArgb(221, 249, 0);
                colors[16].color = Color.FromArgb(170, 238, 0);
                colors[17].color = Color.FromArgb(102, 225, 0);
                colors[18].color = Color.FromArgb(51, 215, 0);
                colors[19].color = Color.FromArgb(0, 204, 0);
            }

            if (palette == Palette.RedBlue)
            {
                colors[0].color = Color.FromArgb(88, 36, 217);
                colors[1].color = Color.FromArgb(96, 37, 213);
                colors[2].color = Color.FromArgb(106, 36, 209);
                colors[3].color = Color.FromArgb(111, 36, 201);
                colors[4].color = Color.FromArgb(124, 36, 197);
                colors[5].color = Color.FromArgb(136, 37, 192);
                colors[6].color = Color.FromArgb(142, 38, 181);
                colors[7].color = Color.FromArgb(156, 37, 174);
                colors[8].color = Color.FromArgb(166, 36, 162);
                colors[9].color = Color.FromArgb(175, 34, 152);
                colors[10].color = Color.FromArgb(185, 35, 139);
                colors[11].color = Color.FromArgb(183, 33, 123);
                colors[12].color = Color.FromArgb(192, 32, 113);
                colors[13].color = Color.FromArgb(194, 31, 99);
                colors[14].color = Color.FromArgb(200, 29, 88);
                colors[15].color = Color.FromArgb(207, 29, 78);
                colors[16].color = Color.FromArgb(209, 29, 70);
                colors[17].color = Color.FromArgb(212, 28, 61);
                colors[18].color = Color.FromArgb(217, 26, 23);
            }
        }

        public Color GetColorPallete(double value)
        {
            if (value >= colors[0].value && value < colors[1].value) return colors[0].color;
            else if (value >= colors[1].value && value < colors[2].value) return colors[1].color;
            else if (value >= colors[2].value && value < colors[3].value) return colors[2].color;
            else if (value >= colors[3].value && value < colors[4].value) return colors[3].color;
            else if (value >= colors[4].value && value < colors[5].value) return colors[4].color;
            else if (value >= colors[5].value && value < colors[6].value) return colors[5].color;
            else if (value >= colors[6].value && value < colors[7].value) return colors[6].color;
            else if (value >= colors[7].value && value < colors[8].value) return colors[7].color;
            else if (value >= colors[8].value && value < colors[9].value) return colors[8].color;
            else if (value >= colors[9].value && value < colors[10].value) return colors[9].color;
            else if (value >= colors[10].value && value < colors[11].value) return colors[10].color;
            else if (value >= colors[11].value && value < colors[12].value) return colors[11].color;
            else if (value >= colors[12].value && value < colors[13].value) return colors[12].color;
            else if (value >= colors[13].value && value < colors[14].value) return colors[13].color;
            else if (value >= colors[14].value && value < colors[15].value) return colors[14].color;
            else if (value >= colors[15].value && value < colors[16].value) return colors[15].color;
            else if (value >= colors[16].value && value < colors[17].value) return colors[16].color;
            else if (value >= colors[17].value && value <= colors[18].value) return colors[17].color;
            else return Color.Black;
        }
    }
}

/*
 * 10 минут доклад
 * 15 ответы на вопросы
 * 5 минут Отзыв рецензента
 * 5 минут После научрука
 * 5 минут Я хотел бы ответить на замечения рецензента
 * 5 минут Потом благодарственной речи:
 *      Благодарим комиссию, за то что выслушали и задали интересныей вопросы
 *      Потом рецензента, за рецензию
 *      После Научного руководителя
 *      Благодарности всем остальным, команде, институту и т.д.
 *      НО НЕ ЗАТЯГИВАЕМ
 */
