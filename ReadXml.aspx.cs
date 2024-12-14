using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ReadingWritingXML
{
    public partial class ReadXml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                string path = "~/Files/Students.xml";
                string path2 = "~/Files/Students2.xml";

                path = Server.MapPath(path);
                path2 = Server.MapPath(path2);

                CitajXmlFajl(path2);
                CitajXmlFajlGridView(path2);


            } catch(Exception ex) {

                ErrorLabel.Text = "SERVER ERROR";

                System.Diagnostics.Debug.WriteLine(ex.Message);
            
            }
             
        }

        /*
         Jezik C# ima posebne klase za citanje XML fajlova koje omogucavaju mnogo lakse citanje
        i obradu podataka u XML formatu nego sto bi bilo obicnim citanjem kao kad smo radili sa .txt fajlovima.

        Klasa XmlTextReader - starija, ne preporucuje se vise za upotrebu
        https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmltextreader?view=net-8.0

        Klasa XmlReader - ovu koristimo
        https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader?view=netframework-4.8
        
         
         
         */


        //funkcija cita xml fajl i vrsi ispis svih xml elemenata(njihov tip, naziv, sadrzaj)
        //u zavisnosti kog tipa je xml element
        void CitajXmlFajl(string path)
        {
            //XmlReader ima resurse koje treba zatvoriti nakon upotrebe, zato ga stavljamo u using blok
            // metod Create pravi objekat tipa XmlReader koji cita sa prosledjene putanje
            using (XmlReader reader = XmlReader.Create(path)) 
            {
                // metod Read cita sledeci Xml node(cvor, element)
                while(reader.Read())
                {
                    /*
                     NodeType property reader-a je tip cvora tj. tip xml elementa
                    XmlNodeType je enumariocni tip podataka koji oznacava sve tipove xml elemenata
                    Enum: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum

                     */
                    //ako je tip belina(whitespace)
                    if (reader.NodeType == XmlNodeType.Whitespace)
                    {
                        //stampaj u visual studiju njegov tip
                        System.Diagnostics.Debug.WriteLine("Type: " + reader.NodeType);
                    } 
                    //ako je tip tekst(npr. tekst izmedju dva xml tag-a) ili ako je komentar
                    else if(reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Comment)
                    {
                        //stampaj u visual studiju njegov tip i tekst
                        System.Diagnostics.Debug.WriteLine("Type: " + reader.NodeType + ", text: " + reader.Value);
                    }
                    else if(reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement)
                        
                    {
                        //stampaj tip cvora i name xml elementa(otvarajuci tag) odnosno xml end elementa(zatvarajuci tag)
                        System.Diagnostics.Debug.WriteLine("Type: " + reader.NodeType + ", name: " + reader.Name);
                    }

                    else
                    {
                        //u ostalim slucajevima stampaj tip cvora i ime cvora
                        System.Diagnostics.Debug.WriteLine("Type: " + reader.NodeType + ", name: " + reader.Name);
                    }

                    // Ovako citamo atribute elemanta:
                    // ako xml element ima atribute
                    if (reader.HasAttributes)
                    {
                        //stampaj ime elementa koji ima atribute
                        System.Diagnostics.Debug.WriteLine("\tAttributes of <" + reader.Name + ">");
                        //metod MoveToNextAttribute() prelazi na sledeci atribut xml elementa dokle kog taj element ima atributa
                        while (reader.MoveToNextAttribute())
                        {
                            //stampamo tip cvora, ime i vrednost
                            System.Diagnostics.Debug.WriteLine("\t\tType: " + reader.NodeType + ", attribute name: " + reader.Name + ", attribute value: " + reader.Value);
                        }
                        // metod MoveToElement() vraca reader na pocetak xml elementa cije atribute smo procitali
                        reader.MoveToElement();
                    }
                    
                    
                }
            }
        }

        //funkcija cita xml fajl i vrsi ispis sadrzaja xml fajla na veb stranicu u GridView.
        //Studenti iz xml fajla se ispisuju u GridView sortirani opadajuce po godini.
        //Studente iz xml-a cemo predstaviti klasom Student
        void CitajXmlFajlGridView(string path)
        {
            //pravimo listu studenata, pogledati klasu Student
            List<Student> students = new List<Student>();
            /*na osnovu podataka iz xml fajla pravimo objekte tipa student koji ima property-je godina, ime i prezime.
             Postavljamo prazne default vrednosti zbog prirode jezika C# i njegovih provera*/
            int year = 0;
            string lastName = "";
            string firstName = "";

            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {

                    //ako je cvor tipa element i u pitanju je tag <student>
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "student")
                    {
                        /*ovde citamo atribute jer unapred znamo da tag <student> ima atribute.
                         * U praksi bi trebalo
                         kod svakog taga vrsiti ovu proveru ako hocemo da nam program radi za bilo kakav xml fajl.*/
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                //ako je tipa atribut i ako je godina
                                if (reader.NodeType == XmlNodeType.Attribute && reader.Name == "year")
                                    //u promenljivu year upisati vrednost atributa tj. godinu
                                    year = int.Parse(reader.Value);
                            }
                            
                            reader.MoveToElement();
                        }

                    }
                    //ako je element i ako se zove prezime, to znaci da je ovo podatak o prezimenu
                    //izmedju tag-ova <lastname> i </lastname> odnosno prezime je teks taga.
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "lastname")
                    {
                        //tekst tag-a je sledeci xml element, i onda moramo metodom Read preci na njega
                        reader.Read();
                        //procitamo tekst i upisemo u lastName
                        lastName = reader.Value;                       
                    }

                    //za prvo ime isti postupak kao za prezime
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "firstname")
                    {
                        reader.Read();
                        firstName = reader.Value;                       
                    }
                    //ako smo stigli do zatvarajuceg taga student tj. do </student> onda je to kraj podataka
                    // o jednom studentu i sada mozemo napraviti objekat tipa Student i ubaciti ga u listu Studenata
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "student")
                    {
                        Student s = new Student(lastName, firstName, year);
                        students.Add(s);
                        
                    }
                }
            }







            //metod Sort sortira listu Studenata na osnovu prosledjenog argumenta(staticka fja Compare).
            //OPREZ! Funkcija je ovde samo data kao argument metoda Sort, i NE poziva se vec se samo navodi njeno ime.
            //Zato nema oblih zagrada () posle Compare.
            //U C-u postoji nesto slicno a to su pokazivaci na funkcije, a u C#-u delegati(delegate).
            students.Sort(Student.Compare);
            //ispis u GridView
            GridView1.DataSource = students;
            GridView1.DataBind();
        }
       
    }
}