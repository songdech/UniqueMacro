using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RaxUploader
{
    public class FixtureFile
    {
        /// <summary>
        /// Fixture Id
        /// </summary>
        public string FixtureId { get; private set; }

        public FileInfo FileInfo { get; private set; }
        public String FailFloder { get; private set; }
        public String ComputerName { get; private set; }

        public FileInfo HeightFileInfo { get; private set; }

        /// <summary>
        /// Raw file content
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Contains Position & Result
        /// </summary>
        public Dictionary<string, List<string>>  Results { get; set; }

        public FixtureFile(string path , string ComputerName , String PathHeight)
        {
            this.FileInfo = new FileInfo(path);
            this.ComputerName = ComputerName;
            this.HeightFileInfo = new FileInfo(PathHeight);
        }

        public void Validate()
        {
            // check file format (expected  XXX_YYY_ZZZ.txt)
            //string[] sections = this.FileInfo.Name.Split('_').ToArray();
            //if (sections.Length != 3)
           //  throw new FormatException(this.FileInfo.FullName + " invalid file pattern (expected  XXX_YYY_ZZZ.txt");
            //CsvFileReader reader = null;

            try
            {
                //this.FixtureId = sections[0];
                this.Content = File.ReadAllText(this.FileInfo.FullName, Encoding.GetEncoding(874));

                //this.FailFloder = 
                /*
                reader = new CsvFileReader(this.FileInfo.FullName);
                List<String> dataColumns = new List<String>();
                string fixtureID = this.FileInfo.Name;
                fixtureID=fixtureID.Substring(0,fixtureID.Length-19);
                //var mydictionary = new Dictionary<string, List<string>>(keysAndValues.Length);
                var mydictionary = new Dictionary<string, List<string>>();

                while (reader.ReadRow(dataColumns))
                {
                    if (dataColumns[0] != "Model") {
                        this.FixtureId = fixtureID;
                   

                        List<string> list = new List<string>();

                        list.Add(dataColumns[0]);
                        list.Add(dataColumns[1]);
                        list.Add(dataColumns[2]);
                        list.Add(dataColumns[3]);
                    
                        list.Add(dataColumns[4]);
                        list.Add(dataColumns[5]);
                        list.Add(dataColumns[6]);
                        list.Add(dataColumns[7]);
                        list.Add(dataColumns[8]);

                        //list.Add(dataColumns[9]);
                        //list.Add(dataColumns[10]);

                        mydictionary.Add(list[2], list);

                        list = null;

                    } 


                }

                reader.Dispose();
                reader = null;

                
                */

                /*   string[] keysAndValues = this.Content.Split('\n');
                   var mydictionary = new Dictionary<string, List<string>>(keysAndValues.Length);
                   foreach (string item in keysAndValues)
                   {
                       if (item != "") { 
                       List<string> list = new List<string>(item.Replace("\r", "").Split(','));
                       if (list.Count!=0 && (list[0]!= "Model"))
                       {
                               //mydictionary.Add(list[2], list); new structure
                              mydictionary.Add(list[0], list);
                           }
                           list.RemoveAt(0);
                       }
                       // remove key from list to match Jon Skeet's implementation

                   }
                   */

                //this.Results = mydictionary;

            }
            catch (Exception ex)
            {
                throw new FormatException(ex.Message, ex);
            }
        }

        public void Pick()
        {
            try
            {

                File.Delete(this.FileInfo.FullName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
