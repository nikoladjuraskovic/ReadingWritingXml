using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ReadingWritingXML
{
    public partial class WriteXml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string path = "~/Files/Students3.xml";

                path = Server.MapPath(path);

                PisiUXmlFajl(path);

            } catch(Exception ex)
            {
                ErrorLabel.Text = "SERVER ERROR";
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        
        //fja ispisuje podatke o studentima iz liste u fajl u jeziku xml
        //s obzirom da PROGRAM sada pise xml fajl, ovo je dinamicko pisanje xml fajla
        void PisiUXmlFajl(string path)
        {
            /*
             XMLWriter i XMLTextWriter - klase za pisanje podataka u fajlove u xml formatu.
            Preporuka je da se koristi XMLWriter umesto XMLTextWriter-a.

            XMLTextWriter Docs: https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmltextwriter?view=netframework-4.8
            XMLWriter Docs: https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?view=netframework-4.8
             */



            Student s1 = new Student("Petrovic", "Petar", 4);
            Student s2 = new Student("Nikolic", "Nikola", 3);
            Student s3 = new Student("Jovanovic", "Jovan", 1);

            List<Student> students = new List<Student>();
            students.Add(s1);
            students.Add(s2);
            students.Add(s3);

            /*Pozeljno je da su podaci u xml fajlu lepo formatirani da bi bili citljivi za coveka.
             Xml je lepo formatiran ako se pravilno vrsi uvlacenje(engleski indent) cvorova koji su u DOM stablu potomci prethodnih cvorova.
             Uvlacenje pisanjem XmlWriter-om radimo preko klase XmlWriterSettings.
            Docs: https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings?view=netframework-4.8
            */

            XmlWriterSettings settings = new XmlWriterSettings(); //instanciramo klasu
            settings.Indent = true; // indent-ovanje(uvlacenje) postavimo na true tj. hocemo da radi
            settings.IndentChars = "\t"; // tagove indentujemo(uvlacimo) tab-om

            //pravimo xmlwriter koji vrsi ispis u fajl na putanji path prema podesavanjima settings
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                /*Oprez!
                 * Metodi WriteWhitespace i WriteString ako se koriste u XmlWriter-u predefinisu indent-ovanje tj. uvlacenje tag-ova
                 * te nece raditi indentovanje iz XmlWriterSettings-a.
                 * 
                 * XmlIndent property docs: https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwritersettings.indent?view=netframework-4.8
                 */

                writer.WriteStartElement("students"); // ispisi pocetni element <students>

                //da bismo ispisali sve studente treba proci kroz listu
                //svaki student ima svoje tagove
                //hocemo da napravimo da year bude atribut tag-a student, a ime i prezime posebni tagovi unutar taga student
                for (int i = 0; i < students.Count; i++)
                {
                    writer.WriteStartElement("student"); // <student>

                    //ispisuje atribut year i njegovu vrednost u tag <student>
                    writer.WriteAttributeString("year", students[i].year.ToString()); // <student year = "godina_tekuceg_studenta">

                    // ispisuje tag lastname i tekst(prezime tekuceg studenta) izmedju tagova
                    writer.WriteElementString("lastname", students[i].lastName); // <lastname>prezime_tekuceg_studenta</lastname>

                    // ispisuje tag firstname i tekst(ime tekuceg studenta) izmedju tagova
                    writer.WriteElementString("firstname", students[i].firstName); // <firstname>ime_tekuceg_studenta</firstname>

                    //ispisuje zatvarajuci element tekuceg cvora(poslednji nezatvoreni tj. neupareni)
                    writer.WriteEndElement(); // </student>

                }

                //ispisuje komentar sa prosledjenim tekstom
                writer.WriteComment("Napisao studente");
            }
        }
    }
}