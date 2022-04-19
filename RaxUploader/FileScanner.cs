using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaxUploader
{
    public class FileScanner
    {
        public event EventHandler<FixtureFile> OnFoundNewFile;

        public string ScanFolder { get; private set; }
        public int ScanInterval { get; private set; }
        public int FloderData { get; private set; }
        public string Pattern { get; private set; }
        public string FailFolder { get; private set; }
        public string HeightFolder { get; private set; }
        public string ComputerName { get; private set; }


        private CancellationTokenSource source;

        public FileScanner(string scanFolder,string failFolder, int scanInterval , string HeightFolder  , string ComputerName)
        {
            this.ScanFolder = scanFolder.Substring(0, scanFolder.LastIndexOf("\\"));
            this.Pattern = scanFolder.Substring(scanFolder.LastIndexOf("\\") + 1);            
            this.ScanInterval = scanInterval;
            this.HeightFolder = HeightFolder;
            this.FailFolder = failFolder;
            this.ComputerName = ComputerName;
        }

        public void Start()
        {
            source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)                        
                            break;
                    try {
                        string[] files = Directory.GetFiles(this.ScanFolder, this.Pattern);                        

                        if (files.Count() > 0)
                        {
                            foreach (string fileResult in files)
                            {                                                                
                                string foundFile = fileResult;
                                FixtureFile fixture = new FixtureFile(foundFile , this.ComputerName  , this.HeightFolder );                                
                                //fixture.FailFloder = this.FailFolder;
                                this.OnFoundNewFile(this, fixture);
                            }
                        }
                    }
                    catch
                    {

                    }

                    Thread.Sleep(this.ScanInterval);
                }


            }, token);
        }

        public void Stop()
        {
            Console.WriteLine("[{0}] - call Stop Service");
            source.Cancel();
        }
    }
}
