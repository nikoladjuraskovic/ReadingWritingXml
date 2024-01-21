using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadingWritingXML
{
    public class Student
    {
        public Student() { }

        public Student(string lastName, string firstName, int year)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.year = year;
        }

        public string lastName { get; set; }
        public string firstName { get; set; }
        public int year { get; set; }


        /*
        
        U zadatku se trazi da se studenti sortiraju po godini. Posto je klasa Student nasa klasa,
        a ne ugradjena, mi moramo nekako da programu kazemo i objasnimo kako ce on da sortira Student-e.
        To se radi pisanjem STATICKE metode Compare.

        staticka fja Compare vrsi poredjenje dva objekta tipa Student na osnovu godine opadajuce.
         Ovakve funkcije uvek vracaju -1, 0  i 1, dok redosled zavisi od toga da li sortiramo
        opadajuce ili rastuce.
         */
        public static int Compare(Student s1, Student s2)
        {
            if (s1.year < s2.year)
                return 1;
            else if (s1.year > s2.year)
                return -1;
            else
                return 0;
        }
        
    }
}