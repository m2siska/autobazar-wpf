using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace Autobazar2
{
    [XmlRoot(ElementName = "Auto")]
    public class Auto
    {
        [XmlElement(ElementName = "Model")]
        public string Model { get; set; }

        [XmlElement(ElementName = "Datum")]
        public string Datum { get; set; }

        [XmlIgnore]
        public DateTime DatumStruct
        {
            get
            {
                string[] formats = { "d.M.yyyy", "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy" };
                DateTime.TryParseExact(Datum, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum);
                    return datum;

            }
            set
            {
                Datum = value.ToString("dd.MM.yyyy");
            }
        }

        [XmlElement(ElementName = "Cena")]
        public double Cena { get; set; }

        [XmlElement(ElementName = "DPH")]
        public double DPH { get; set; }
    }

    [XmlRoot(ElementName = "autobazar")]
    public class Autobazar
    {
        [XmlElement(ElementName = "Auto")]
        public List<Auto> Auto { get; set; }
    }

    public class Vikend
    {
        public string Model { get; set; }
        public double Cena { get; set; }
        public double DPH { get; set; }
        public double CenaDPH { get; set; }

        public Vikend(string model, double cena, double cenaDPH)
        {
            Model = model;
            Cena = cena;
            CenaDPH = cenaDPH;
        }

    }
}  