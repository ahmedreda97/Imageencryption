using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LFSRimageencryption
{
    public class LFSR
    {

        public int tap, seed, NBits;
        public LFSR(string sseed, int tap) { this.seed = Convert.ToInt32(sseed, 2); this.tap = tap; this.NBits = sseed.Length; }

        public int Onestep() //Make a one lfsr step
        {

            int checktapbit = 1 << this.tap;
            int newbit = (checktapbit & seed) >> this.tap;
            int outbit = seed >> NBits - 1;
            newbit = newbit ^ outbit;
            seed = (seed << 1);
            seed = seed ^ newbit;
            seed = seed & (int)(Math.Pow(2, NBits) - 1);

            return seed & 1;

        }
        public int Kstep(int K) //Extract Kbit new generated integer
        {
            for (int i = 0; i < K; i++)
            {
                this.Onestep();
            }

            return seed & (int)(Math.Pow(2, K) - 1);
        }


    }

    class Program
    {
        static bool IncDec(string seed, int tap, string src, string dest) //Encrypt or decrypt the image
        {

            LFSR lf = new LFSR(seed, tap);
            int R, G, B;

            Bitmap img = new Bitmap(src);
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    Color pixel = img.GetPixel(j, i);

                    R = lf.Kstep(8);
                    G = lf.Kstep(8);
                    B = lf.Kstep(8);

                    Color newcolor = Color.FromArgb(pixel.A, pixel.R ^ R, pixel.G ^ G, pixel.B ^ B);



                    img.SetPixel(j, i, newcolor);

                }
            }
            img.Save(dest);
            return true;
        }
        static void Main(string[] args)
        {
        }
    }
}
