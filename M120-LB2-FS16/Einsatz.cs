using System;
using System.Drawing;

namespace M120_LB2_FS16
{
    public class Einsatz
    {
        public long ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }
        public Projekt Projekt { get; set; }
        public Mitarbeiter Mitarbeiter { get; set; }
        public Color Farbe { get; set; }
        public Einsatz()
        {
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
