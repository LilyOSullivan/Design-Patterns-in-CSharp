using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SingleResponsibilityPrinciple
{
    class Program
    {
        public class Journal
        {
            private readonly List<string> entries = new List<string>();
            private static int count = 0;

            public int AddEntry(string text)
            {
                entries.Add($"{++count}: {text}");
                return count;
            }

            public override string ToString()
            {
                return String.Join(Environment.NewLine, entries);
            }

            //public void Save(string filename)
            //{
            //    File.WriteAllText("Dear_Diary.txt", this.ToString());
            //}

            //public Journal Load(string filename)
            //{
            //    var journal = new Journal();
            //    foreach(string line in File.ReadAllLines(filename))
            //    {
            //        journal.AddEntry(line);
            //    }
            //    return journal;
            //}
        }

        public class JournalPersistance
        {
            public void SaveToFile(Journal journal,string filename,bool overwrite=false)
            {
                if(overwrite || !File.Exists(filename))
                {
                    File.WriteAllText(filename, journal.ToString());
                }
            }
        }

        static void Main(string[] args)
        {
            var journal = new Journal();
            journal.AddEntry("I pet a cat today!");
            journal.AddEntry("I now want to own a cat");

            var journalPersistance = new JournalPersistance();
            var filename = $@".{Path.DirectorySeparatorChar}journal.txt";
            journalPersistance.SaveToFile(journal, filename);

        }
    }
}
